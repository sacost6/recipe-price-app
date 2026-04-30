using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RecipeCost.Shared; 
namespace RecipeCostAPI.Models;

/// <summary>
/// Represents the join entity for a many-to-many relationship 
/// between Recipes and Ingredients.
/// </summary>
public class RecipeIngredient
{
    [Key]
    public int Id { get; init; }

    [Required]
    public int RecipeId { get; set; }

    [ForeignKey(nameof(RecipeId))]
    public virtual Recipe Recipe { get; set; } = null!;

    [Required]
    public int IngredientId { get; set; }

    [ForeignKey(nameof(IngredientId))]
    public virtual Ingredient Ingredient { get; set; } = null!;

    [Required]  
    public decimal Quantity { get; set; }

    [Required] 
    public UnitType Unit { get; set; }

    [MaxLength(200)]
    public string? PreparationNote { get; set; }
}