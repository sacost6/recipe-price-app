using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RecipeCost.Shared;
using RecipeCostAPI.Data;
using Testcontainers.PostgreSql;
using Xunit;

namespace RecipeCostAPI.Tests.Integration;

public sealed class IngredientPostgresIntegrationTests : IAsyncLifetime
{
    private static readonly JsonSerializerOptions JsonOptions = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() },
    };

    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:16-alpine")
        .WithDatabase("recipe_cost_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    private WebApplicationFactory<Program> _factory = null!;
    private HttpClient _client = null!;

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();

        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.UseEnvironment("Testing");
                builder.ConfigureServices(services =>
                {
                    services.RemoveAll<DbContextOptions<AppDbContext>>();
                    services.AddDbContext<AppDbContext>(options =>
                        options.UseNpgsql(_postgres.GetConnectionString()));
                });
            });

        _client = _factory.CreateClient();
    }

    public async Task DisposeAsync()
    {
        _client.Dispose();
        await _factory.DisposeAsync();
        await _postgres.DisposeAsync();
    }

    public static TheoryData<UnitType, decimal, UnitType, decimal> UserUnitCostCases => new()
    {
        { UnitType.Kilogram, 5.00m, UnitType.Gram, 0.0050m },
        { UnitType.Liter, 2.50m, UnitType.Milliliter, 0.0025m },
    };

    [Theory]
    [MemberData(nameof(UserUnitCostCases))]
    public async Task PostIngredient_WithUserUnitAndCostPerUserUnit_StoresCalculatedCostPerBaseUnitInPostgres(
        UnitType userUnit,
        decimal costPerUserUnit,
        UnitType expectedBaseUnit,
        decimal expectedCostPerBaseUnit)
    {
        var request = new IngredientDto
        {
            Name = $"Integration {userUnit} {Guid.NewGuid():N}",
            Description = "Created through the API integration test.",
            UserUnit = userUnit,
            CostPerUserUnit = costPerUserUnit,
            BaseUnit = UnitType.Each,
            CostPerBaseUnit = 123.4567m,
        };

        var response = await _client.PostAsJsonAsync("/api/ingredients", request, JsonOptions);

        response.EnsureSuccessStatusCode();
        var created = await response.Content.ReadFromJsonAsync<IngredientDto>(JsonOptions);
        Assert.NotNull(created);

        await using var scope = _factory.Services.CreateAsyncScope();
        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var stored = await context.Ingredients.SingleAsync(ingredient => ingredient.Id == created.Id);

        Assert.Equal(userUnit, stored.UserUnit);
        Assert.Equal(costPerUserUnit, stored.CostPerUserUnit);
        Assert.Equal(expectedBaseUnit, stored.BaseUnit);
        Assert.Equal(expectedCostPerBaseUnit, stored.CostPerBaseUnit);
    }
}
