using RecipeCost.Shared;

namespace RecipeCostUI.Models;

public class RecipeFormModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Servings { get; set; } = 1;
    public List<RecipeIngredientFormModel> Ingredients { get; set; } = new();

    public RecipeDto ToDto() => new()
    {
        Id = Id,
        Name = Name,
        Description = Description,
        Servings = Servings,
        Ingredients = Ingredients.Select(i => i.ToDto()).ToList()
    };
}
