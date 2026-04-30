using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using RecipeCostAPI.Models; 
using RecipeCostAPI.Services.Interfaces;
using RecipeCost.Shared;
namespace RecipeCostAPI.Services; 

public class PricingService : IPricingService
{ 

    private readonly IConverterService _converterService;
    private readonly ILogger<PricingService> _logger;

    public PricingService(IConverterService converterService, ILogger<PricingService>? logger = null)
    {
        _converterService = converterService;
        _logger = logger ?? NullLogger<PricingService>.Instance;
    }

    public decimal CalculateLineItemCost(decimal amount, UnitType usedUnit, Ingredient ingredient)
    {
        if (ingredient == null)
        {
            _logger.LogWarning(
                "Skipping line item cost calculation because ingredient was null. Amount: {Amount}, UsedUnit: {UsedUnit}",
                amount,
                usedUnit);

            return 0;
        }

        if (amount <= 0)
        {
            _logger.LogWarning(
                "Skipping line item cost calculation for IngredientId {IngredientId} because amount was not positive. Amount: {Amount}, UsedUnit: {UsedUnit}",
                ingredient.Id,
                amount,
                usedUnit);

            return 0;
        }

        try
        {
            // Convert the used unit to the ingredient's base unit
            decimal convertedQuantity = _converterService.Convert(amount, usedUnit, ingredient.BaseUnit, ingredient.DensityGramsPerMl);

            // Final calculation: converted quantity multiplied by the cost per base unit
            var lineItemCost = convertedQuantity * ingredient.CostPerBaseUnit;

            _logger.LogInformation(
                "Calculated line item cost for IngredientId {IngredientId}. Amount: {Amount}, UsedUnit: {UsedUnit}, BaseUnit: {BaseUnit}, ConvertedQuantity: {ConvertedQuantity}, CostPerBaseUnit: {CostPerBaseUnit}, LineItemCost: {LineItemCost}",
                ingredient.Id,
                amount,
                usedUnit,
                ingredient.BaseUnit,
                convertedQuantity,
                ingredient.CostPerBaseUnit,
                lineItemCost);

            return lineItemCost;
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(
                ex,
                "Failed to calculate line item cost for IngredientId {IngredientId}. Amount: {Amount}, UsedUnit: {UsedUnit}, BaseUnit: {BaseUnit}, DensityGramsPerMl: {DensityGramsPerMl}",
                ingredient.Id,
                amount,
                usedUnit,
                ingredient.BaseUnit,
                ingredient.DensityGramsPerMl);

            return 0; // Return 0 cost if conversion fails
        }
    }

    // Calculate the total cost of a recipe by summing the costs of its line items
    public decimal CalculateRecipeCost(Recipe recipe)
	{
		if (recipe == null)
        {
            _logger.LogWarning("Skipping recipe cost calculation because recipe was null.");
            return 0;
        }

        if (recipe.RecipeIngredients == null)
        {
            _logger.LogWarning(
                "Skipping recipe cost calculation for RecipeId {RecipeId} because recipe ingredients were null.",
                recipe.Id);

            return 0;
        }

        decimal totalCost = 0;
		foreach (var lineItem in recipe.RecipeIngredients)
		{
			totalCost += CalculateLineItemCost(lineItem.Quantity, lineItem.Unit, lineItem.Ingredient);
		}

        _logger.LogInformation(
            "Calculated recipe cost for RecipeId {RecipeId}. IngredientCount: {IngredientCount}, TotalCost: {TotalCost}",
            recipe.Id,
            recipe.RecipeIngredients.Count,
            totalCost);

		return totalCost;
    }
}
