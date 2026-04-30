using RecipeCost.Shared; 
using RecipeCostAPI.Models; 
using RecipeCostAPI.Services.Interfaces;

namespace RecipeCostAPI.Mappers;
public static class RecipeMapper
{
     
    // Map RecipeIngredient -> RecipeIngredientDto
    public static RecipeIngredientDto ToDto(this RecipeIngredient ri, IPricingService pricingService) =>
        new RecipeIngredientDto
        {
            IngredientId = ri.IngredientId,
            IngredientName = ri.Ingredient?.Name ?? "Unknown",
            Quantity = (decimal)ri.Quantity, // Cast double to decimal for the DTO
            BaseUnit = ri.Unit,
            CalculatedCost = pricingService.CalculateLineItemCost(ri.Quantity, ri.Unit, ri.Ingredient!)
        };

    // Map Recipe -> RecipeDto (The "Big" One)
    public static RecipeDto ToDto(this Recipe recipe, IPricingService pricingService)
    {
        var ingredientDtos = recipe.RecipeIngredients
            .Select(ri => ri.ToDto(pricingService))
            .ToList();

        var totalCost = ingredientDtos.Sum(i => i.CalculatedCost);

        return new RecipeDto
        {
            Id = recipe.Id,
            Name = recipe.Name,
            Servings = recipe.Servings,
            Description = recipe.Description != null ? recipe.Description : string.Empty,
            TotalCost = totalCost,
            CostPerServing = recipe.Servings > 0 ? totalCost / recipe.Servings : 0,
            Ingredients = ingredientDtos
        };
    }
}

public static class RecipeMappingExtensions
{
    /// <summary>
    /// Converts a CreateRecipeDto (or RecipeDto) into a Recipe Entity.
    /// Note: This does NOT fetch from the DB, it just prepares the object graph.
    /// </summary>
    public static Recipe ToEntity(this RecipeDto dto)
    {
        return new Recipe
        {
            Name = dto.Name,
            Servings = dto.Servings,
            Description = dto.Description,
            // Map the child collection
            RecipeIngredients = dto.Ingredients.Select(i => new RecipeIngredient
            {
                IngredientId = i.IngredientId,
                Quantity = i.Quantity, 
                Unit = i.BaseUnit // Enum handles type safety here
            }).ToList()
        };
    }
}