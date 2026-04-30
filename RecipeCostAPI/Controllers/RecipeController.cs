using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeCost.Shared;
using RecipeCostAPI.Data;
using RecipeCostAPI.Mappers;
using RecipeCostAPI.Services;
using RecipeCostAPI.Services.Interfaces;

namespace RecipeCostAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IRecipeService _recipeService;

    public RecipesController(IRecipeService recipeService)
    {
        _recipeService = recipeService;
    }

    // GET: api/recipes
    [HttpGet]
    public async Task<IActionResult> GetRecipes()
    {
        var recipe = await _recipeService.GetRecipesAsync();
        return Ok(recipe);
    }

    // GET: api/recipes/1
    [HttpGet("{id}")]
    public async Task<IActionResult> GetRecipe(int id)
    {
        var recipe = await _recipeService.GetRecipeByIdAsync(id);
        if (recipe == null) return NotFound();

        return Ok(recipe);
    }
    // POST: api/recipes
    [HttpPost]
    public async Task<IActionResult> CreateRecipe(RecipeDto recipeDto)
    {
        var created = await _recipeService.CreateRecipeAsync(recipeDto);
        return CreatedAtAction(nameof(GetRecipe), new { id = created.Id }, created);
    }
    // PUT: api/recipes/1
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIngredient(int id, RecipeDto dto)
    {
        if (id != dto.Id) return BadRequest();

        var updated = await _recipeService.UpdateRecipeAsync(id, dto);


        return NoContent();
    }
    // DELETE: api/recipes/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        // Implement delete logic if needed
        return NoContent();
    }
}