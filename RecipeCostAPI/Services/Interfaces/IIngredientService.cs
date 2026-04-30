using RecipeCost.Shared;

namespace RecipeCostAPI.Services.Interfaces
{
    public interface IIngredientService
    {
        Task<IEnumerable<IngredientDto>> GetIngredientsAsync();
        Task<IngredientDto?> GetIngredientByIdAsync(int id);
        Task<IngredientDto> CreateIngredientAsync(IngredientDto dto);
        Task<bool> UpdateIngredientAsync(int id, IngredientDto dto);
        Task<bool> DeleteIngredientAsync(int id);
    }
}
