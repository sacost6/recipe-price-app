using System.ComponentModel;

namespace RecipeCost.Shared;

public enum UnitType
{
    // --- MASS (Base Unit: Gram) ---
    [Category("Mass")] Gram,
    [Category("Mass")] Kilogram,
    [Category("Mass")] Ounce,
    [Category("Mass")] Pound,


    // --- VOLUME (Base Unit: Milliliter) ---
    [Category("Volume")] Milliliter,
    [Category("Volume")] Liter,
    [Category("Volume")] FluidOunce,
    [Category("Volume")] Cup,
    [Category("Volume")] Teaspoon,
    [Category("Volume")] Tablespoon,
    [Category("Volume")] Quart,
    [Category("Volume")] Gallon,

    // --- DISCRETE (Base Unit: Each) ---
    [Category("Discrete")] Piece,
    [Category("Discrete")] Each,
    [Category("Discrete")] Clove, // Useful for garlic!
    [Category("Discrete")] Large, // e.g., "1 Large Egg"
}