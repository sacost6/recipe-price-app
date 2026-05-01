using RecipeCost.Shared;
using System.Net.Http.Json;
using System.Text.Json;

public class RecipeService
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions;

    public RecipeService(HttpClient http, JsonSerializerOptions jsonOptions)
    {
        _http = http;
        _jsonOptions = jsonOptions;
    }

    public async Task<List<RecipeDto>> GetRecipesAsync()
    {
        var result = await _http.GetFromJsonAsync<List<RecipeDto>>("api/recipes", _jsonOptions);
        return result ?? new List<RecipeDto>();
    }

    public async Task CreateRecipeAsync(RecipeDto recipe)
    {
        var response = await _http.PostAsJsonAsync("api/recipes", recipe, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    public async Task UpdateRecipeAsync(RecipeDto recipe)
    {
        var response = await _http.PutAsJsonAsync($"api/recipes/{recipe.Id}", recipe, _jsonOptions);
        await EnsureSuccessAsync(response);
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode)
        {
            return;
        }

        var error = await response.Content.ReadAsStringAsync();
        var message = string.IsNullOrWhiteSpace(error)
            ? $"Request failed with status code {(int)response.StatusCode} ({response.ReasonPhrase})."
            : error;

        throw new HttpRequestException(message, null, response.StatusCode);
    }
}
