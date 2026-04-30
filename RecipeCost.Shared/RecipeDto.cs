namespace RecipeCost.Shared;

public record RecipeDto
{     
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    // List of ingredients with quantities for this recipe
    public List<RecipeIngredientDto> Ingredients { get; init; } = new();
    // Total cost of the recipe, calculated on the backend
    public decimal TotalCost { get; init; }

    // Cost per serving, calculated on the backend
    public decimal CostPerServing { get; init; }
    // Number of servings the recipe makes
    public int Servings { get; init; }

}