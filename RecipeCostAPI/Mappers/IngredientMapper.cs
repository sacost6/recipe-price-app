using RecipeCost.Shared; 
using RecipeCostAPI.Models; 
using RecipeCostAPI.Services.Interfaces;

namespace RecipeCostAPI.Mappers;
public static class IngredientMapper
{
    // Map Ingredient -> IngredientDto
    public static IngredientDto ToDto(this Ingredient ingredient) =>
        new IngredientDto
        {
            Id = ingredient.Id,
            Name = ingredient.Name,
            BaseUnit = ingredient.BaseUnit,
            CostPerBaseUnit = ingredient.CostPerBaseUnit,
            UserUnit = ingredient.UserUnit,
            CostPerUserUnit = ingredient.CostPerUserUnit,
            Description = ingredient.Description,
            DensityGramsPerMl = ingredient.DensityGramsPerMl
        };
}