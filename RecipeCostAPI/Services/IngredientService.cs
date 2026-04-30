using Microsoft.EntityFrameworkCore;
using RecipeCost.Shared;
using RecipeCostAPI.Data;
using RecipeCostAPI.Mappers;
using RecipeCostAPI.Models;
using RecipeCostAPI.Services.Interfaces;

namespace RecipeCostAPI.Services
{
    public class IngredientService : IIngredientService
    {
        private readonly AppDbContext _context; 
        private readonly IConverterService _converterService;

        public IngredientService(AppDbContext context, IConverterService converterService)
        {
            _context = context;
            _converterService = converterService;
        }

        public async Task<IEnumerable<IngredientDto>> GetIngredientsAsync()
        {
            var ingredients = await _context.Ingredients.ToListAsync();
            return ingredients.Select(i => i.ToDto());
        }

        public async Task<IngredientDto?> GetIngredientByIdAsync(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            return ingredient?.ToDto();
        }

        public async Task<IngredientDto> CreateIngredientAsync(IngredientDto dto)
        {
            var ingredient = new Ingredient
            {
                Name = dto.Name,
                Description = dto.Description,
                UserUnit = dto.UserUnit,
                CostPerUserUnit = dto.CostPerUserUnit,
                // Map the density
                DensityGramsPerMl = dto.DensityGramsPerMl
            };

            // Convert unit to base unit if needed
            if(dto.UserUnit == UnitType.Gram || dto.UserUnit == UnitType.Milliliter)
            {
                ingredient.BaseUnit = dto.UserUnit;
                ingredient.CostPerBaseUnit = dto.CostPerUserUnit;
            }
            else
            {
                ingredient.BaseUnit = _converterService.GetBaseUnit(dto.UserUnit);
                ingredient.CostPerBaseUnit = _converterService.CalculateBaseUnitCost(dto.CostPerUserUnit, dto.UserUnit);
            }
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();

            return dto with { Id = ingredient.Id };
        }

        public async Task<bool> UpdateIngredientAsync(int id, IngredientDto dto)
        {
            if (id != dto.Id) return false;

            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null) return false;

            ingredient.Name = dto.Name;
            ingredient.CostPerBaseUnit = dto.CostPerBaseUnit;
            ingredient.BaseUnit = dto.BaseUnit;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteIngredientAsync(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null) return false;

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
