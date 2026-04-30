using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RecipeCost.Shared;
using RecipeCostAPI.Services.Interfaces;
using Xunit;

namespace RecipeCostAPI.Tests.Integration;

public sealed class ExceptionHandlingIntegrationTests
{
    [Fact]
    public async Task UnhandledException_InProduction_ReturnsProblemDetailsWithoutStackTrace()
    {
        await using var factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Production");
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<IRecipeService>();
                    services.AddScoped<IRecipeService, ThrowingRecipeService>();
                });
            });

        using var client = factory.CreateClient();

        var response = await client.GetAsync("/api/recipes");

        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.Equal("application/problem+json", response.Content.Headers.ContentType?.MediaType);

        var body = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(body);
        var problem = document.RootElement;

        Assert.Equal("https://tools.ietf.org/html/rfc9110#section-15.6.1", problem.GetProperty("type").GetString());
        Assert.Equal("An unexpected error occurred.", problem.GetProperty("title").GetString());
        Assert.Equal(500, problem.GetProperty("status").GetInt32());
        Assert.Equal("An unexpected error occurred while processing the request.", problem.GetProperty("detail").GetString());
        Assert.Equal("/api/recipes", problem.GetProperty("instance").GetString());
        Assert.True(problem.TryGetProperty("traceId", out _));

        Assert.DoesNotContain(nameof(InvalidOperationException), body);
        Assert.DoesNotContain(nameof(ThrowingRecipeService), body);
        Assert.DoesNotContain("simulated production failure", body);
        Assert.DoesNotContain(" at ", body);
    }

    private sealed class ThrowingRecipeService : IRecipeService
    {
        public Task<IEnumerable<RecipeDto>> GetRecipesAsync(int pageNumber, int pageSize)
        {
            throw new InvalidOperationException("simulated production failure with sensitive implementation details");
        }

        public Task<RecipeDto> CreateRecipeAsync(RecipeDto recipeDto)
        {
            throw new NotImplementedException();
        }

        public Task<RecipeDto?> GetRecipeByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateRecipeAsync(int id, RecipeDto dto)
        {
            throw new NotImplementedException();
        }
    }
}
