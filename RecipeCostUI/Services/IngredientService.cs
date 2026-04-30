using RecipeCost.Shared;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

public class IngredientService
{
    private readonly HttpClient _http;
    private static readonly JsonSerializerOptions Options = new()
    {
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };
    static JsonSerializerOptions options = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public IngredientService(HttpClient http)
    {
        _http = http;
    }

    public async Task<List<IngredientDto>> GetIngredientsAsync()
    {
        // Hits IngredientsController.GetIngredientsAsync() and returns the list of ingredients as JSON, which is deserialized into a List<IngredientDto>.
        var result = await _http.GetFromJsonAsync<List<IngredientDto>>("api/ingredients", Options);
        return result ?? new List<IngredientDto>();
    }

    public async Task CreateIngredientAsync(IngredientDto ingredient)
    {
        await _http.PostAsJsonAsync("api/ingredients", ingredient);
    }

    public async Task DeleteIngredientAsync(int id)
    {
        await _http.DeleteAsync($"api/ingredients/{id}");
    }

    public async Task UpdateIngredientAsync(IngredientDto ingredient)
    {
        // In REST, PUT usually targets the specific ID in the URL
        var response = await _http.PutAsJsonAsync($"api/ingredients/{ingredient.Id}", ingredient);

        // Check if it failed so we don't just fail silently
        response.EnsureSuccessStatusCode();
    }
}