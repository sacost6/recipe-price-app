using RecipeCost.Shared;

namespace RecipeCostUI.Models;

public class RecipeIngredientFormModel
{
    public int IngredientId { get; set; }
    public string IngredientName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public UnitType Unit { get; set; } = UnitType.Gram;

    public RecipeIngredientDto ToDto() => new()
    {
        IngredientId = IngredientId,
        IngredientName = IngredientName,
        Quantity = Quantity,
        BaseUnit = Unit
    };
}
