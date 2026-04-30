namespace RecipeCost.Shared;

public record IngredientDto
{
    public int Id { get; init; } // 'init' allows setting only during creation
    public string Name { get; init; } = string.Empty;

    public UnitType UserUnit { get; init; }  // Store the user's chosen unit 
    public UnitType BaseUnit { get; init; } // The base unit for calculations 
    public decimal CostPerBaseUnit { get; init; } // Cost per base unit (e.g., cost per gram or milliliter)
    public decimal CostPerUserUnit { get; init; } // Cost per the user's chosen unit for display purposes
    public string Description { get; init; } = string.Empty; // Optional description of the ingredient

    public decimal? DensityGramsPerMl { get; init; } // Optional density for unit conversions (e.g., grams per milliliter)
}