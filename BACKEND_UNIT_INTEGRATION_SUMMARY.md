# Backend Unit Integration Summary

## Overview
The backend has been successfully updated to handle unit types when saving and calculating recipe ingredient costs. The unit type selected in the recipe UI is now properly stored and used in cost calculations.

## Components Updated

### 1. RecipeService.cs
**Changes:**
- `CreateRecipeAsync()`: Now maps `i.BaseUnit` from the DTO to the `RecipeIngredient.Unit` property
- `UpdateRecipeAsync()`: Now maps `i.BaseUnit` from the DTO to the `RecipeIngredient.Unit` property

**Impact:** When a user selects a unit type for an ingredient in a recipe, it is now saved to the database in the `RecipeIngredient.Unit` column.

### 2. PricingService.cs (Already Correct)
**Current Implementation:**
- `CalculateLineItemCost()`: Takes `UnitType usedUnit` parameter and converts it to the ingredient's base unit before calculating cost
- `CalculateRecipeCost()`: Iterates through recipe ingredients and calls `CalculateLineItemCost()` with `lineItem.Unit`

**How It Works:**
1. User specifies a quantity and unit for an ingredient (e.g., 2 cups of flour)
2. `CalculateLineItemCost()` receives:
   - `amount`: 2
   - `usedUnit`: Cup
   - `ingredient`: Flour (with BaseUnit: Gram, CostPerBaseUnit: $0.50)
3. `ConverterService.Convert()` converts 2 cups to the equivalent grams
4. Cost is calculated: `convertedQuantity * ingredient.CostPerBaseUnit`

### 3. RecipeIngredient Model (Already Has Unit Property)
```csharp
public class RecipeIngredient
{
    public int Id { get; init; }
    public int RecipeId { get; set; }
    public virtual Recipe Recipe { get; set; }
    public int IngredientId { get; set; }
    public virtual Ingredient Ingredient { get; set; }
    public decimal Quantity { get; set; }
    public UnitType Unit { get; set; }  // ? This stores the user-selected unit
    public string? PreparationNote { get; set; }
}
```

### 4. RecipeMapper.cs (Already Correct)
- Correctly maps `ri.Unit` to `BaseUnit` in the DTO
- Correctly maps `i.BaseUnit` (from DTO) to `Unit` in the entity

### 5. Frontend Changes (Already Made)
- `RecipeIngredientFormModel` now includes a `Unit` property
- Unit dropdown added to the recipe form
- Unit is displayed in the ingredients table
- Unit is preserved when editing recipes

## Data Flow

### Creating a Recipe
1. User enters recipe details and selects ingredients with quantities and units
2. Frontend sends `RecipeDto` with `Ingredients` list containing `BaseUnit` (the selected unit)
3. `RecipeService.CreateRecipeAsync()` maps this to `RecipeIngredient.Unit`
4. `PricingService` calculates costs using the unit when needed

### Updating a Recipe
1. User edits ingredient units in the recipe
2. Frontend sends updated `RecipeDto` with modified units
3. `RecipeService.UpdateRecipeAsync()` updates the `RecipeIngredient.Unit` values
4. Costs are recalculated based on the new units

### Cost Calculation
1. For each ingredient in a recipe:
   - User-selected quantity and unit are retrieved from `RecipeIngredient`
   - `ConverterService` converts to the ingredient's base unit
   - Cost = converted quantity × ingredient's cost per base unit
2. Total recipe cost = sum of all ingredient costs
3. Cost per serving = total cost ÷ servings

## Database Schema
The `RecipeIngredient` table now stores:
- `Id`: Unique identifier
- `RecipeId`: Foreign key to Recipe
- `IngredientId`: Foreign key to Ingredient
- `Quantity`: The amount used
- `Unit`: **The unit type of that quantity** ? NEW/UPDATED
- `PreparationNote`: Optional notes

## Example Scenario

**Ingredient Setup:**
- Flour: Base unit = Gram, Cost = $0.50 per 100g

**Recipe Creation:**
1. User adds 2 cups of flour to a recipe
2. Frontend saves: Quantity = 2, Unit = Cup
3. Backend calculation:
   - Convert 2 cups to grams: ? 473 grams
   - Cost = 473 grams × ($0.50 / 100 grams) = $2.365

**Another User Uses Different Unit:**
1. Different user adds 500 grams of flour to their recipe
2. Frontend saves: Quantity = 500, Unit = Gram
3. Backend calculation:
   - No conversion needed (already in base unit)
   - Cost = 500 grams × ($0.50 / 100 grams) = $2.50

## Testing
All existing tests pass with these changes:
- `ConverterServiceTests`: Validates unit conversion logic
- `PricingService`: Works correctly with the unit mapping

## Next Steps (Optional Enhancements)
1. Add database migration to ensure `Unit` column exists (if upgrading from existing DB)
2. Add validation to ensure only compatible units are used with each ingredient
3. Add error logging for invalid unit conversions
4. Add UI notifications when conversion fails
