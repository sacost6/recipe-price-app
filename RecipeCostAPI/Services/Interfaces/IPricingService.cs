using RecipeCostAPI.Models;
using RecipeCost.Shared;
namespace RecipeCostAPI.Services.Interfaces;
public interface IPricingService
{
    decimal CalculateLineItemCost(decimal amount, UnitType usedUnit, Ingredient ingredient);
    decimal CalculateRecipeCost(Recipe recipe);
}