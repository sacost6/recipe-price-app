using RecipeCost.Shared;

namespace RecipeCostUI;

public class AppState
{
    public List<IngredientDto> Ingredients { get; private set; } = new();
    public List<RecipeDto> Recipes { get; private set; } = new();
    public bool HasIngredients { get; private set; }
    public bool HasRecipes { get; private set; }

    public event Action? OnChange;

    public void SetIngredients(IEnumerable<IngredientDto> ingredients)
    {
        Ingredients = ingredients.ToList();
        HasIngredients = true;
        NotifyStateChanged();
    }

    public void SetRecipes(IEnumerable<RecipeDto> recipes)
    {
        Recipes = recipes.ToList();
        HasRecipes = true;
        NotifyStateChanged();
    }

    public void SetData(IEnumerable<IngredientDto> ingredients, IEnumerable<RecipeDto> recipes)
    {
        Ingredients = ingredients.ToList();
        Recipes = recipes.ToList();
        HasIngredients = true;
        HasRecipes = true;
        NotifyStateChanged();
    }

    public void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}
