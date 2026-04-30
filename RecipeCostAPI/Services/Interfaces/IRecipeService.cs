using RecipeCost.Shared;

namespace RecipeCostAPI.Services.Interfaces;

public interface IRecipeService
{
    Task<IEnumerable<RecipeDto>> GetRecipesAsync(int pageNumber, int pageSize);
    Task<RecipeDto> CreateRecipeAsync(RecipeDto recipeDto);
    Task<RecipeDto?> GetRecipeByIdAsync(int id);
    Task<bool> UpdateRecipeAsync(int id, RecipeDto dto);
}
