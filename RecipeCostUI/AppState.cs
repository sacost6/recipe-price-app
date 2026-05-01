using RecipeCost.Shared;

namespace RecipeCostUI;

public class AppState
{
    public List<IngredientDto> Ingredients { get; private set; } = new();
    public List<RecipeDto> Recipes { get; private set; } = new();

    public event Action? OnChange;

    public void SetIngredients(IEnumerable<IngredientDto> ingredients)
    {
        Ingredients = ingredients.ToList();
        NotifyStateChanged();
    }

    public void SetRecipes(IEnumerable<RecipeDto> recipes)
    {
        Recipes = recipes.ToList();
        NotifyStateChanged();
    }

    public void SetData(IEnumerable<IngredientDto> ingredients, IEnumerable<RecipeDto> recipes)
    {
        Ingredients = ingredients.ToList();
        Recipes = recipes.ToList();
        NotifyStateChanged();
    }

    public void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}
