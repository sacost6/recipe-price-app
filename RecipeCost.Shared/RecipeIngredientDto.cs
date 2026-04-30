namespace RecipeCost.Shared;

public record RecipeIngredientDto
{
    public int IngredientId { get; init; }
    public string IngredientName { get; init; } = string.Empty;
    public decimal Quantity { get; init; }
    public UnitType BaseUnit { get; init; } = UnitType.Gram;

    // Calculated dynamically: (Quantity * CostPerBaseUnit)
    public decimal CalculatedCost { get; init; }
}