using RecipeCostAPI.Models; 
using RecipeCostAPI.Services.Interfaces;
using RecipeCost.Shared;
namespace RecipeCostAPI.Services; 

public class PricingService : IPricingService
{ 

    private readonly IConverterService _converterService;

    public PricingService(IConverterService converterService)
    {
        _converterService = converterService;
    }

    public decimal CalculateLineItemCost(decimal amount, UnitType usedUnit, Ingredient ingredient)
    {
        if(ingredient == null || amount <= 0) return 0;

        try
        {
            // Convert the used unit to the ingredient's base unit
            decimal convertedQuanity = _converterService.Convert(amount, usedUnit, ingredient.BaseUnit, ingredient.DensityGramsPerMl);

            // Final calculation: converted quantity multiplied by the cost per base unit
            return convertedQuanity * ingredient.CostPerBaseUnit;
        }
        catch (ArgumentException ex)
        {
            // Log the error (not implemented here)
            Console.WriteLine($"Conversion error: {ex.Message}");
            return 0; // Return 0 cost if conversion fails
        }
    }

    // Calculate the total cost of a recipe by summing the costs of its line items
    public decimal CalculateRecipeCost(Recipe recipe)
	{
		if(recipe == null || recipe.RecipeIngredients == null) return 0;
		decimal totalCost = 0;
		foreach (var lineItem in recipe.RecipeIngredients)
		{
			totalCost += CalculateLineItemCost(lineItem.Quantity, lineItem.Unit, lineItem.Ingredient);
		}
		return totalCost;
    }
}