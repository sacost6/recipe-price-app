using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecipeCost.Shared;
namespace RecipeCostAPI.Models;
// Data model for Ingredient
public class Ingredient
{
    public int Id { get; init; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public UnitType BaseUnit { get; set; }

    [Range(0, 999999)]
    public decimal CostPerBaseUnit { get; set; }

    [Required] 
    public UnitType UserUnit { get; set; }
    
    [Required]
    public decimal CostPerUserUnit { get; set; }

    public string Description { get; set; } = string.Empty;

    // Store density for unit conversions (e.g., grams per milliliter)
    [Range(0, 999999)]
    public decimal? DensityGramsPerMl { get; set; }

    // Links back to any recipes using this ingredient
    public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}