using RecipeCost.Shared;

namespace RecipeCostAPI.Services.Interfaces
{
    // Used to handle unit conversions and cost calculations, but we can abstract it for now
    public interface IConverterService
    { 
        decimal Convert(decimal quantity, UnitType fromUnit, UnitType toUnit, decimal? densityGramsPerMl = null);
        UnitType GetBaseUnit(UnitType unit); 
        decimal CalculateBaseUnitCost(decimal costPerUserUnit, UnitType userUnit);
        bool IsVolumeUnit(UnitType unit);
        bool IsMassUnit(UnitType unit);

    }
}

