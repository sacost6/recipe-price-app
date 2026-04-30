using Microsoft.AspNetCore.Mvc;
using RecipeCost.Shared;
using RecipeCostAPI.Data;
using RecipeCostAPI.Services.Interfaces;

namespace RecipeCostAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecipesController : ControllerBase
{
    private readonly IRecipeService _recipeService;
    private readonly AppDbContext _context;

    public RecipesController(IRecipeService recipeService, AppDbContext context)
    {
        _recipeService = recipeService;
        _context = context;
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
    public async Task<IActionResult> UpdateRecipe(int id, RecipeDto dto)
    {
        if (id != dto.Id) return BadRequest();

        var updated = await _recipeService.UpdateRecipeAsync(id, dto);
        if (!updated) return NotFound();


        return NoContent();
    }
    // DELETE: api/recipes/1
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRecipe(int id)
    {
        var recipe = await _context.Recipes.FindAsync(id);
        if (recipe == null) return NotFound();

        _context.Recipes.Remove(recipe);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
