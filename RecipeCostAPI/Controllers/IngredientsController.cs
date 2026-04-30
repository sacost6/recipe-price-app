using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeCostAPI.Data; 
using RecipeCostAPI.Mappers;
using RecipeCostAPI.Models;
using RecipeCost.Shared;
using RecipeCostAPI.Services.Interfaces;

namespace RecipeCostAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class IngredientsController : ControllerBase
{
    private readonly IIngredientService _ingredientService; 

    public IngredientsController(IIngredientService ingredientService)
    {
        _ingredientService = ingredientService;
    }

    // GET: api/ingredients
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IngredientDto>>> GetIngredients()
    {
        var ingredients = await _ingredientService.GetIngredientsAsync();
        return Ok(ingredients);

    }

    // GET: api/ingredients/1
    [HttpGet("{id}")]
    public async Task<ActionResult<IngredientDto>> GetIngredient(int id)
    {
        var ingredient = await _ingredientService.GetIngredientByIdAsync(id);
        if (ingredient == null) return NotFound();

        return Ok(ingredient);
    }

    [HttpPost]
    public async Task<ActionResult<IngredientDto>> CreateIngredient(IngredientDto ingredientDto)
    {
        var created = await _ingredientService.CreateIngredientAsync(ingredientDto);
        return CreatedAtAction(nameof(GetIngredient), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateIngredient(int id, IngredientDto dto)
    {
        if (id != dto.Id) return BadRequest();

        var success = await _ingredientService.UpdateIngredientAsync(id, dto);
        if (!success) return NotFound();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteIngredient(int id)
    {
        var success = await _ingredientService.DeleteIngredientAsync(id);
        if (!success) return NotFound();

        return NoContent();
    }
      
}