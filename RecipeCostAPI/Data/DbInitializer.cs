using RecipeCostAPI.Models;
using Microsoft.EntityFrameworkCore;
using RecipeCost.Shared;
namespace RecipeCostAPI.Data;

public static class DbInitializer
{
    public static async Task SeedData(AppDbContext context)
    {
        // 1. Ensure the database exists and has the latest schema
        await context.Database.MigrateAsync();

        // 2. Check if we already have data (avoid duplicates)
        if (await context.Ingredients.AnyAsync()) return;

        // 3. Create initial Ingredients
        var flour = new Ingredient
        {
            Name = "All-Purpose Flour",
            BaseUnit = UnitType.Gram,
            CostPerBaseUnit = 0.002m // $2.00 per kg
        };

        var sugar = new Ingredient
        {
            Name = "White Sugar",
            BaseUnit = UnitType.Gram,
            CostPerBaseUnit = 0.0015m // $1.50 per kg
        };

        var eggs = new Ingredient
        {
            Name = "Large Eggs",
            BaseUnit = UnitType.Piece,
            CostPerBaseUnit = 0.25m // $3.00 per dozen
        };

        await context.Ingredients.AddRangeAsync(flour, sugar, eggs);
        await context.SaveChangesAsync();

        // 4. Create a Sample Recipe (Simple Cake)
        if (!await context.Recipes.AnyAsync())
        {
            var cake = new Recipe
            {
                Name = "Basic Sponge Cake",
                Servings = 8,
                Description = "A simple starter cake.",
                RecipeIngredients = new List<RecipeIngredient>
                {
                    new() { Ingredient = flour, Quantity = 250, Unit = UnitType.Gram },
                    new() { Ingredient = sugar, Quantity = 200, Unit = UnitType.Gram },
                    new() { Ingredient = eggs, Quantity = 4, Unit = UnitType.Piece }
                }
            };

            await context.Recipes.AddAsync(cake);
            await context.SaveChangesAsync();
        }
    }
}