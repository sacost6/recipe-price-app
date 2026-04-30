using RecipeCost.Shared;
using RecipeCostAPI.Services;
using Xunit;

namespace RecipeCostAPI.Tests.Services;

public class ConverterServiceTests
{
    private readonly ConverterService _sut = new();

    public static TheoryData<decimal, UnitType, UnitType, decimal, decimal> SameCategoryConversionCases => new()
    {
        { 1000m, UnitType.Gram, UnitType.Kilogram, 1m, 0.000001m },
        { 1m, UnitType.Kilogram, UnitType.Gram, 1000m, 0.000001m },
        { 1m, UnitType.Ounce, UnitType.Gram, 28.349523125m, 0.000001m },
        { 28.349523125m, UnitType.Gram, UnitType.Ounce, 1m, 0.000001m },
        { 1m, UnitType.Pound, UnitType.Gram, 453.59237m, 0.000001m },
        { 453.59237m, UnitType.Gram, UnitType.Pound, 1m, 0.000001m },
        { 16m, UnitType.Ounce, UnitType.Pound, 1m, 0.000001m },
        { 1m, UnitType.Pound, UnitType.Ounce, 16m, 0.000001m },
        { 1000m, UnitType.Milliliter, UnitType.Liter, 1m, 0.000001m },
        { 1m, UnitType.Liter, UnitType.Milliliter, 1000m, 0.000001m },
        { 1m, UnitType.FluidOunce, UnitType.Milliliter, 29.5735295625m, 0.000001m },
        { 29.5735295625m, UnitType.Milliliter, UnitType.FluidOunce, 1m, 0.000001m },
        { 1m, UnitType.Cup, UnitType.Milliliter, 236.5882365m, 0.000001m },
        { 236.5882365m, UnitType.Milliliter, UnitType.Cup, 1m, 0.000001m },
        { 1m, UnitType.Teaspoon, UnitType.Milliliter, 4.92892159375m, 0.000001m },
        { 4.92892159375m, UnitType.Milliliter, UnitType.Teaspoon, 1m, 0.000001m },
        { 1m, UnitType.Tablespoon, UnitType.Milliliter, 14.78676478125m, 0.000001m },
        { 14.78676478125m, UnitType.Milliliter, UnitType.Tablespoon, 1m, 0.000001m },
        { 1m, UnitType.Quart, UnitType.Milliliter, 946.352946m, 0.000001m },
        { 946.352946m, UnitType.Milliliter, UnitType.Quart, 1m, 0.000001m },
        { 1m, UnitType.Gallon, UnitType.Milliliter, 3785.411784m, 0.000001m },
        { 3785.411784m, UnitType.Milliliter, UnitType.Gallon, 1m, 0.000001m },
    };

    public static TheoryData<decimal, UnitType, UnitType, decimal?> DensityConversionCases => new()
    {
        { 250m, UnitType.Milliliter, UnitType.Gram, 1m },
        { 500m, UnitType.Gram, UnitType.Milliliter, 1m },
        { 1m, UnitType.Cup, UnitType.Gram, 0.8m },
        { 100m, UnitType.Gram, UnitType.Tablespoon, 0.5m },
    };

    public static TheoryData<decimal, UnitType, decimal> CalculateBaseUnitCostCases => new()
    {
        { 1m, UnitType.Gram, 1m },
        { 3.99m, UnitType.Kilogram, 1000m },
        { 0.75m, UnitType.Ounce, 28.349523125m },
        { 6m, UnitType.Pound, 453.59237m },
        { 1m, UnitType.Milliliter, 1m },
        { 2.25m, UnitType.Liter, 1000m },
        { 0.5m, UnitType.FluidOunce, 29.5735295625m },
        { 3m, UnitType.Cup, 236.5882365m },
        { 1.25m, UnitType.Teaspoon, 4.92892159375m },
        { 1.75m, UnitType.Tablespoon, 14.78676478125m },
        { 4.25m, UnitType.Quart, 946.352946m },
        { 7.5m, UnitType.Gallon, 3785.411784m },
    };

    [Theory]
    [MemberData(nameof(SameCategoryConversionCases))]
    public void Convert_SameCategoryUnits_ReturnsExpectedValue(
        decimal quantity,
        UnitType fromUnit,
        UnitType toUnit,
        decimal expected,
        decimal tolerance)
    {
        var result = _sut.Convert(quantity, fromUnit, toUnit);

        AssertApproximatelyEqual(expected, result, tolerance);
    }

    [Theory]
    [InlineData(42.5, UnitType.Gram)]
    [InlineData(9.25, UnitType.Cup)]
    [InlineData(3, UnitType.Piece)]
    public void Convert_SameUnit_ReturnsOriginalQuantity(decimal quantity, UnitType unit)
    {
        var result = _sut.Convert(quantity, unit, unit);

        Assert.Equal(quantity, result);
    }

    [Theory]
    [InlineData(UnitType.Gram, UnitType.Liter)]
    [InlineData(UnitType.Piece, UnitType.Gallon)]
    public void Convert_ZeroQuantity_ReturnsZero(UnitType fromUnit, UnitType toUnit)
    {
        var result = _sut.Convert(0m, fromUnit, toUnit);

        Assert.Equal(0m, result);
    }

    [Theory]
    [InlineData(100, UnitType.Gram, UnitType.Milliliter)]
    [InlineData(2, UnitType.Cup, UnitType.Ounce)]
    public void Convert_CrossCategoryWithoutDensity_ThrowsArgumentException(
        decimal quantity,
        UnitType fromUnit,
        UnitType toUnit)
    {
        var exception = Assert.Throws<ArgumentException>(() => _sut.Convert(quantity, fromUnit, toUnit));

        Assert.Contains("Density is required", exception.Message);
    }

    [Theory]
    [MemberData(nameof(DensityConversionCases))]
    public void Convert_CrossCategoryWithDensity_ReturnsExpectedValue(
        decimal quantity,
        UnitType fromUnit,
        UnitType toUnit,
        decimal? densityGramsPerMl)
    {
        var expected = ExpectedDensityConversion(quantity, fromUnit, toUnit, densityGramsPerMl!.Value);

        var result = _sut.Convert(quantity, fromUnit, toUnit, densityGramsPerMl);

        AssertApproximatelyEqual(expected, result, 0.000001m);
    }

    [Theory]
    [InlineData(UnitType.Piece, UnitType.Gram)]
    [InlineData(UnitType.Clove, UnitType.Liter)]
    public void Convert_UnsupportedCategories_ThrowsArgumentException(UnitType fromUnit, UnitType toUnit)
    {
        var exception = Assert.Throws<ArgumentException>(() => _sut.Convert(1m, fromUnit, toUnit));

        Assert.Contains("Cannot convert", exception.Message);
    }

    [Theory]
    [InlineData(UnitType.Gram, UnitType.Gram)]
    [InlineData(UnitType.Kilogram, UnitType.Gram)]
    [InlineData(UnitType.Ounce, UnitType.Gram)]
    [InlineData(UnitType.Pound, UnitType.Gram)]
    [InlineData(UnitType.Milliliter, UnitType.Milliliter)]
    [InlineData(UnitType.Liter, UnitType.Milliliter)]
    [InlineData(UnitType.FluidOunce, UnitType.Milliliter)]
    [InlineData(UnitType.Cup, UnitType.Milliliter)]
    [InlineData(UnitType.Teaspoon, UnitType.Milliliter)]
    [InlineData(UnitType.Tablespoon, UnitType.Milliliter)]
    [InlineData(UnitType.Quart, UnitType.Milliliter)]
    [InlineData(UnitType.Gallon, UnitType.Milliliter)]
    public void GetBaseUnit_SupportedUnits_ReturnsExpectedBaseUnit(UnitType unit, UnitType expectedBaseUnit)
    {
        var result = _sut.GetBaseUnit(unit);

        Assert.Equal(expectedBaseUnit, result);
    }

    [Theory]
    [InlineData(UnitType.Piece)]
    [InlineData(UnitType.Each)]
    [InlineData(UnitType.Clove)]
    [InlineData(UnitType.Large)]
    public void GetBaseUnit_UnsupportedUnits_ThrowsArgumentException(UnitType unit)
    {
        var exception = Assert.Throws<ArgumentException>(() => _sut.GetBaseUnit(unit));

        Assert.Contains("category not recognized", exception.Message);
    }

    [Theory]
    [MemberData(nameof(CalculateBaseUnitCostCases))]
    public void CalculateBaseUnitCost_SupportedUnits_ReturnsMathematicallyCorrectBaseCost(
        decimal costPerUserUnit,
        UnitType userUnit,
        decimal baseUnitsPerUserUnit)
    {
        var expected = costPerUserUnit / baseUnitsPerUserUnit;

        var result = _sut.CalculateBaseUnitCost(costPerUserUnit, userUnit);

        AssertApproximatelyEqual(expected, result, 0.000001m);
    }

    [Theory]
    [InlineData(UnitType.Piece)]
    [InlineData(UnitType.Each)]
    public void CalculateBaseUnitCost_PieceUnits_ReturnsOriginalValue(UnitType userUnit)
    {
        var result = _sut.CalculateBaseUnitCost(4.25m, userUnit);

        Assert.Equal(4.25m, result);
    }

    [Theory]
    [InlineData(UnitType.Clove)]
    [InlineData(UnitType.Large)]
    public void CalculateBaseUnitCost_UnsupportedDiscreteUnits_ThrowsArgumentException(UnitType userUnit)
    {
        var exception = Assert.Throws<ArgumentException>(() => _sut.CalculateBaseUnitCost(2m, userUnit));

        Assert.Contains("not a valid mass or volume unit", exception.Message);
    }

    [Fact]
    public void IsMassUnit_ClassifiesUnitsCorrectly()
    {
        var massUnits = new HashSet<UnitType>
        {
            UnitType.Gram,
            UnitType.Kilogram,
            UnitType.Ounce,
            UnitType.Pound,
        };

        foreach (var unit in Enum.GetValues<UnitType>())
        {
            Assert.Equal(massUnits.Contains(unit), _sut.IsMassUnit(unit));
        }
    }

    [Fact]
    public void IsVolumeUnit_ClassifiesUnitsCorrectly()
    {
        var volumeUnits = new HashSet<UnitType>
        {
            UnitType.Milliliter,
            UnitType.Liter,
            UnitType.FluidOunce,
            UnitType.Cup,
            UnitType.Teaspoon,
            UnitType.Tablespoon,
            UnitType.Quart,
            UnitType.Gallon,
        };

        foreach (var unit in Enum.GetValues<UnitType>())
        {
            Assert.Equal(volumeUnits.Contains(unit), _sut.IsVolumeUnit(unit));
        }
    }

    [Fact]
    public void IsPieceUnit_ClassifiesUnitsCorrectly()
    {
        var pieceUnits = new HashSet<UnitType>
        {
            UnitType.Piece,
            UnitType.Each,
        };

        foreach (var unit in Enum.GetValues<UnitType>())
        {
            Assert.Equal(pieceUnits.Contains(unit), _sut.IsPieceUnit(unit));
        }
    }

    private static decimal ExpectedDensityConversion(decimal quantity, UnitType fromUnit, UnitType toUnit, decimal densityGramsPerMl)
    {
        var quantityInBaseUnit = fromUnit switch
        {
            UnitType.Gram => quantity,
            UnitType.Kilogram => quantity * 1000m,
            UnitType.Ounce => quantity * 28.349523125m,
            UnitType.Pound => quantity * 453.59237m,
            UnitType.Milliliter => quantity,
            UnitType.Liter => quantity * 1000m,
            UnitType.FluidOunce => quantity * 29.5735295625m,
            UnitType.Cup => quantity * 236.5882365m,
            UnitType.Teaspoon => quantity * 4.92892159375m,
            UnitType.Tablespoon => quantity * 14.78676478125m,
            UnitType.Quart => quantity * 946.352946m,
            UnitType.Gallon => quantity * 3785.411784m,
            _ => throw new ArgumentOutOfRangeException(nameof(fromUnit), fromUnit, null),
        };

        var convertedBaseQuantity = IsVolumeUnit(fromUnit)
            ? quantityInBaseUnit * densityGramsPerMl
            : quantityInBaseUnit / densityGramsPerMl;

        return toUnit switch
        {
            UnitType.Gram => convertedBaseQuantity,
            UnitType.Kilogram => convertedBaseQuantity / 1000m,
            UnitType.Ounce => convertedBaseQuantity / 28.349523125m,
            UnitType.Pound => convertedBaseQuantity / 453.59237m,
            UnitType.Milliliter => convertedBaseQuantity,
            UnitType.Liter => convertedBaseQuantity / 1000m,
            UnitType.FluidOunce => convertedBaseQuantity / 29.5735295625m,
            UnitType.Cup => convertedBaseQuantity / 236.5882365m,
            UnitType.Teaspoon => convertedBaseQuantity / 4.92892159375m,
            UnitType.Tablespoon => convertedBaseQuantity / 14.78676478125m,
            UnitType.Quart => convertedBaseQuantity / 946.352946m,
            UnitType.Gallon => convertedBaseQuantity / 3785.411784m,
            _ => throw new ArgumentOutOfRangeException(nameof(toUnit), toUnit, null),
        };
    }

    private static bool IsVolumeUnit(UnitType unit) => unit is
        UnitType.Milliliter or
        UnitType.Liter or
        UnitType.FluidOunce or
        UnitType.Cup or
        UnitType.Teaspoon or
        UnitType.Tablespoon or
        UnitType.Quart or
        UnitType.Gallon;

    private static void AssertApproximatelyEqual(decimal expected, decimal actual, decimal tolerance)
    {
        var difference = Math.Abs(expected - actual);
        Assert.True(
            difference <= tolerance,
            $"Expected {expected} +/- {tolerance}, but got {actual}. Difference was {difference}.");
    }
}
