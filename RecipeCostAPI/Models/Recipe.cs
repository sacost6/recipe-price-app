using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeCostAPI.Models;
// Data model for Recipe
public class Recipe
{ 
	public int Id {  get; init; }

	[Required]
	[MaxLength(100)]
	public string Name { get; set; } = string.Empty;

	[Range(1,100)]
	public int Servings { get; set; }

	[MaxLength(500)]
	public string? Description { get; set; } 

	// This links to our join table, which contains the 'Amount' and 'Unit'
	public virtual ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();

	// Calculated property - usually not stored in the DB to avoid data staleness
	[NotMapped]
	public decimal TotalCost { get; set; }

	[NotMapped]
	public decimal CostPerServing => Servings > 0 ? TotalCost / Servings : 0;
 
}
