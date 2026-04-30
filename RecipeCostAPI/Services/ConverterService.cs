using RecipeCost.Shared;
using RecipeCostAPI.Services.Interfaces;
using UnitsNet;
using UnitsNet.Units;

namespace RecipeCostAPI.Services
{
    public class ConverterService : IConverterService
    { 
        public decimal Convert(decimal quantity, UnitType fromUnit, UnitType toUnit, decimal? densityGramsPerMl = null)
        {
            if (quantity == 0) return 0;
            if (fromUnit == toUnit) return quantity;

            // Handle Mass to Mass
            if(IsMassUnit(fromUnit) && IsMassUnit(toUnit))
            {
                var fromUnitsNet = MapToMass(fromUnit);
                var toUnitsNet = MapToMass(toUnit);
                return (decimal)UnitConverter.Convert(quantity, fromUnitsNet, toUnitsNet);
            }

            // Handle Volume to Volume
            if(IsVolumeUnit(fromUnit) && IsVolumeUnit(toUnit))
            {
                var fromUnitsNet = MapToVolume(fromUnit);
                var toUnitsNet = MapToVolume(toUnit);
                return (decimal)UnitConverter.Convert(quantity, fromUnitsNet, toUnitsNet);
            }

            // Handle Volume to Mass (Teaspoons to grams)
            if(IsVolumeUnit(fromUnit) && IsMassUnit(toUnit))
            {
                if(!densityGramsPerMl.HasValue)
                    throw new ArgumentException($"Density is required to convert from volume unit '{fromUnit}' to mass unit '{toUnit}'.");

                var volumeInMilliliters = (decimal)UnitConverter.Convert((double)quantity, MapToVolume(fromUnit), VolumeUnit.Milliliter);
                var massInGrams = volumeInMilliliters * densityGramsPerMl.Value;

                return (decimal)UnitConverter.Convert((double)massInGrams, MassUnit.Gram, MapToMass(toUnit));
            }

            // Handle Mass to Volume (grams to teaspoons)
            if(IsMassUnit(fromUnit) && IsVolumeUnit(toUnit))
            {
                if(!densityGramsPerMl.HasValue)
                    throw new ArgumentException($"Density is required to convert from mass unit '{fromUnit}' to volume unit '{toUnit}'.");

                var massInGrams = (decimal)UnitConverter.Convert((double)quantity, MapToMass(fromUnit), MassUnit.Gram);
                var volumeInMilliliters = massInGrams / densityGramsPerMl.Value;

                return (decimal)UnitConverter.Convert((double)volumeInMilliliters, VolumeUnit.Milliliter, MapToVolume(toUnit)); 
            }

            throw new ArgumentException($"Cannot convert from '{fromUnit}' to '{toUnit}'. Units must be of the same category (both mass or both volume).");
        }

        public UnitType GetBaseUnit(UnitType unit)
        {
            if (IsMassUnit(unit)) return UnitType.Gram;
            if (IsVolumeUnit(unit)) return UnitType.Milliliter;
            throw new ArgumentException($"Unit {unit} category not recognized.");
        }


        public decimal CalculateBaseUnitCost(decimal costPerUserUnit, UnitType userUnit)
        {
            // Convert the price for exactly one user unit into a price per base unit.
            if (IsVolumeUnit(userUnit))
            {
                var baseUnitsPerUserUnit = UnitConverter.Convert(1d, MapToVolume(userUnit), VolumeUnit.Milliliter);
                var baseUnitCost = costPerUserUnit / (decimal)baseUnitsPerUserUnit;
                return baseUnitCost;
            } 

            if (IsMassUnit(userUnit))
            {
                var baseUnitsPerUserUnit = UnitConverter.Convert(1d, MapToMass(userUnit), MassUnit.Gram);
                var baseUnitCost = costPerUserUnit / (decimal)baseUnitsPerUserUnit;
                return baseUnitCost;
            }

            if(IsPieceUnit(userUnit))
            {
                return costPerUserUnit;
            }

            throw new ArgumentException($"Unit {userUnit} is not a valid mass or volume unit.");
        }

        public bool IsMassUnit(UnitType unit) => unit switch
        {
            UnitType.Gram or UnitType.Kilogram or UnitType.Ounce or UnitType.Pound => true,
            _ => false
        };

        public bool IsVolumeUnit(UnitType unit) => unit switch
        {
            UnitType.Milliliter or UnitType.Liter or UnitType.FluidOunce or UnitType.Cup or UnitType.Gallon 
            or UnitType.Quart or UnitType.Teaspoon or UnitType.Tablespoon => true,
            _ => false
        };

        public bool IsPieceUnit(UnitType unit) => unit switch
        {
            UnitType.Piece or UnitType.Each => true,
            _ => false
        };

        private MassUnit MapToMass(UnitType unit) => unit switch
        {
            UnitType.Gram => MassUnit.Gram,
            UnitType.Kilogram => MassUnit.Kilogram,
            UnitType.Ounce => MassUnit.Ounce,
            UnitType.Pound => MassUnit.Pound,
            _ => throw new ArgumentException($"{unit} is not a mass unit.")
        };

        private VolumeUnit MapToVolume(UnitType unit) => unit switch
        {
            UnitType.Milliliter => VolumeUnit.Milliliter,
            UnitType.Liter => VolumeUnit.Liter,
            UnitType.FluidOunce => VolumeUnit.UsOunce,
            UnitType.Cup => VolumeUnit.UsCustomaryCup,
            UnitType.Teaspoon => VolumeUnit.UsTeaspoon,
            UnitType.Tablespoon => VolumeUnit.UsTablespoon,
            UnitType.Gallon => VolumeUnit.UsGallon,
            UnitType.Quart => VolumeUnit.UsQuart,
            _ => throw new ArgumentException($"{unit} is not a volume unit.")
        }; 
    }
}
