using Microsoft.EntityFrameworkCore;
using RecipeCost.Shared;
using RecipeCostAPI.Data;
using RecipeCostAPI.Mappers; 
using RecipeCostAPI.Models;
using RecipeCostAPI.Services.Interfaces;
namespace RecipeCostAPI.Services
{
    public class RecipeService : IRecipeService
    {
        private readonly AppDbContext _context;
        private readonly IPricingService _pricingService; 

        public RecipeService(AppDbContext context, IPricingService pricingService)
        {
            _context = context;
            _pricingService = pricingService;
        }

        public async Task<IEnumerable<RecipeDto>> GetRecipesAsync()
        {
            var recipes = await _context.Recipes
                .Include(r => r.RecipeIngredients) // Eager load RecipeIngredients
                    .ThenInclude(ri => ri.Ingredient) // Eager load the related Ingredient for each RecipeIngredient
                .ToListAsync();
            return recipes.Select(r => r.ToDto(_pricingService));
        }

        public async Task<RecipeDto?> GetRecipeByIdAsync(int id)
        {
            var recipe = await _context.Recipes
                .Include(r => r.RecipeIngredients) // Eager load RecipeIngredients
                    .ThenInclude(ri => ri.Ingredient) // Eager load the related Ingredient for each RecipeIngredient
                .FirstOrDefaultAsync(r => r.Id == id); // Find the recipe by ID
            if (recipe == null) return null;
            return recipe.ToDto(_pricingService);
        }

        public async Task<RecipeDto> CreateRecipeAsync(RecipeDto recipeDto)
        {
            var recipe = new Recipe
            {
                Name = recipeDto.Name,
                Description = recipeDto.Description,
                RecipeIngredients = recipeDto.Ingredients.Select(i => new RecipeIngredient
                {
                    IngredientId = i.IngredientId,
                    Quantity = i.Quantity,
                    Unit = i.BaseUnit   // Enum handles type safety here
                }).ToList()
            };

            _context.Recipes.Add(recipe);
            await _context.SaveChangesAsync();

            // If your DTO is an immutable record (init), return a new instance with the DB-generated ID.
            // If it's a class with { get; set; }, you could just update recipeDto.Id and return it.
            return recipeDto with { Id = recipe.Id };
        }

        public async Task<bool> UpdateRecipeAsync(int id, RecipeDto dto)
        {
            if (id != dto.Id) return false; 

            var recipe = await _context.Recipes
                .Include(r => r.RecipeIngredients) // Eager load RecipeIngredients
                .FirstOrDefaultAsync(r => r.Id == id); // Find the recipe by ID
            
            if(recipe == null) return false;

            recipe.Name = dto.Name;
            recipe.Description = dto.Description;
            recipe.RecipeIngredients = dto.Ingredients.Select(i => new RecipeIngredient
            {
                IngredientId = i.IngredientId,
                Quantity = i.Quantity,
                Unit = i.BaseUnit   // Enum handles type safety here
            }).ToList();
            
            recipe.Servings = dto.Servings;
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
