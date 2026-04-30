using Moq;
using RecipeCost.Shared;
using RecipeCostAPI.Models;
using RecipeCostAPI.Services;
using RecipeCostAPI.Services.Interfaces;
using Xunit;

namespace RecipeCostAPI.Tests.Services
{
    public class PricingServiceTests
    {
        #region CalculateLineItemCost Tests

        [Fact]
        public void CalculateLineItemCost_WithConvertedQuantity_MultipliesByCostPerBaseUnit()
        {
            var converterService = new Mock<IConverterService>(MockBehavior.Strict);
            var pricingService = new PricingService(converterService.Object);
            var ingredient = CreateIngredient(UnitType.Gram, 0.0199m);

            converterService
                .Setup(service => service.Convert(2.5m, UnitType.Kilogram, UnitType.Gram, null))
                .Returns(2500m);

            var result = pricingService.CalculateLineItemCost(2.5m, UnitType.Kilogram, ingredient);

            Assert.Equal(49.7500m, result);
            converterService.VerifyAll();
        }

        [Fact]
        public void CalculateLineItemCost_WithDecimalInputs_DoesNotRoundBeforeReturningCost()
        {
            var converterService = new Mock<IConverterService>(MockBehavior.Strict);
            var pricingService = new PricingService(converterService.Object);
            var ingredient = CreateIngredient(UnitType.Gram, 0.0199m);

            converterService
                .Setup(service => service.Convert(1.234567m, UnitType.Gram, UnitType.Gram, null))
                .Returns(123.4567m);

            var result = pricingService.CalculateLineItemCost(1.234567m, UnitType.Gram, ingredient);

            Assert.Equal(2.45678833m, result);
            converterService.VerifyAll();
        }

        [Fact]
        public void CalculateLineItemCost_PassesIngredientBaseUnitAndDensityToConverter()
        {
            var converterService = new Mock<IConverterService>(MockBehavior.Strict);
            var pricingService = new PricingService(converterService.Object);
            var ingredient = CreateIngredient(UnitType.Gram, 0.25m, densityGramsPerMl: 1.2m);

            converterService
                .Setup(service => service.Convert(1m, UnitType.Cup, UnitType.Gram, 1.2m))
                .Returns(283.9058838m);

            var result = pricingService.CalculateLineItemCost(1m, UnitType.Cup, ingredient);

            Assert.Equal(70.976470950m, result);
            converterService.VerifyAll();
        }

        [Fact]
        public void CalculateLineItemCost_WithCupQuantityAndIngredientDensity_UsesDensityForGramWeight()
        {
            var pricingService = new PricingService(new ConverterService());
            var ingredient = CreateIngredient(UnitType.Gram, 1m, densityGramsPerMl: 1.2m);

            var result = pricingService.CalculateLineItemCost(1m, UnitType.Cup, ingredient);

            AssertApproximatelyEqual(283.9058838m, result, 0.000001m);
        }

        [Fact]
        public void CalculateLineItemCost_WhenIngredientIsNull_ReturnsZeroWithoutConverting()
        {
            var converterService = new Mock<IConverterService>(MockBehavior.Strict);
            var pricingService = new PricingService(converterService.Object);

            var result = pricingService.CalculateLineItemCost(1m, UnitType.Gram, null!);

            Assert.Equal(0m, result);
            converterService.VerifyNoOtherCalls();
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CalculateLineItemCost_WhenAmountIsNotPositive_ReturnsZeroWithoutConverting(decimal amount)
        {
            var converterService = new Mock<IConverterService>(MockBehavior.Strict);
            var pricingService = new PricingService(converterService.Object);
            var ingredient = CreateIngredient(UnitType.Gram, 0.02m);

            var result = pricingService.CalculateLineItemCost(amount, UnitType.Gram, ingredient);

            Assert.Equal(0m, result);
            converterService.VerifyNoOtherCalls();
        }

        [Fact]
        public void CalculateLineItemCost_WhenConversionFails_ReturnsZero()
        {
            var converterService = new Mock<IConverterService>(MockBehavior.Strict);
            var pricingService = new PricingService(converterService.Object);
            var ingredient = CreateIngredient(UnitType.Gram, 0.02m);

            converterService
                .Setup(service => service.Convert(1m, UnitType.Cup, UnitType.Gram, null))
                .Throws(new ArgumentException("Density is required."));

            var result = pricingService.CalculateLineItemCost(1m, UnitType.Cup, ingredient);

            Assert.Equal(0m, result);
            converterService.VerifyAll();
        }

        #endregion

        #region CalculateRecipeCost Tests

        [Fact]
        public void CalculateRecipeCost_WithMultipleLineItems_ReturnsExactSumOfLineItemCosts()
        {
            var converterService = new Mock<IConverterService>(MockBehavior.Strict);
            var pricingService = new PricingService(converterService.Object);
            var flour = CreateIngredient(UnitType.Gram, 0.004m);
            var sugar = CreateIngredient(UnitType.Gram, 0.006m);
            var milk = CreateIngredient(UnitType.Milliliter, 0.002m);
            var recipe = new Recipe
            {
                Name = "Pancakes",
                RecipeIngredients = new List<RecipeIngredient>
                {
                    CreateRecipeIngredient(250m, UnitType.Gram, flour),
                    CreateRecipeIngredient(0.5m, UnitType.Kilogram, sugar),
                    CreateRecipeIngredient(1.25m, UnitType.Cup, milk),
                },
            };

            converterService
                .Setup(service => service.Convert(250m, UnitType.Gram, UnitType.Gram, null))
                .Returns(250m);
            converterService
                .Setup(service => service.Convert(0.5m, UnitType.Kilogram, UnitType.Gram, null))
                .Returns(500m);
            converterService
                .Setup(service => service.Convert(1.25m, UnitType.Cup, UnitType.Milliliter, null))
                .Returns(295.735295625m);

            var result = pricingService.CalculateRecipeCost(recipe);

            Assert.Equal(4.591470591250m, result);
            converterService.VerifyAll();
        }

        [Fact]
        public void CalculateRecipeCost_WhenOneLineItemCannotConvert_ExcludesThatLineItemFromTotal()
        {
            var converterService = new Mock<IConverterService>(MockBehavior.Strict);
            var pricingService = new PricingService(converterService.Object);
            var flour = CreateIngredient(UnitType.Gram, 0.004m);
            var oil = CreateIngredient(UnitType.Gram, 0.02m);
            var recipe = new Recipe
            {
                Name = "Partial Recipe",
                RecipeIngredients = new List<RecipeIngredient>
                {
                    CreateRecipeIngredient(250m, UnitType.Gram, flour),
                    CreateRecipeIngredient(1m, UnitType.Cup, oil),
                },
            };

            converterService
                .Setup(service => service.Convert(250m, UnitType.Gram, UnitType.Gram, null))
                .Returns(250m);
            converterService
                .Setup(service => service.Convert(1m, UnitType.Cup, UnitType.Gram, null))
                .Throws(new ArgumentException("Density is required."));

            var result = pricingService.CalculateRecipeCost(recipe);

            Assert.Equal(1.000m, result);
            converterService.VerifyAll();
        }

        [Fact]
        public void CalculateRecipeCost_WithEmptyRecipe_ReturnsZero()
        {
            var converterService = new Mock<IConverterService>(MockBehavior.Strict);
            var pricingService = new PricingService(converterService.Object);
            var recipe = new Recipe { Name = "Empty Recipe" };

            var result = pricingService.CalculateRecipeCost(recipe);

            Assert.Equal(0m, result);
            converterService.VerifyNoOtherCalls();
        }

        [Fact]
        public void CalculateRecipeCost_WhenRecipeIsNull_ReturnsZero()
        {
            var converterService = new Mock<IConverterService>(MockBehavior.Strict);
            var pricingService = new PricingService(converterService.Object);

            var result = pricingService.CalculateRecipeCost(null!);

            Assert.Equal(0m, result);
            converterService.VerifyNoOtherCalls();
        }

        [Fact]
        public void CalculateRecipeCost_WhenRecipeIngredientsIsNull_ReturnsZero()
        {
            var converterService = new Mock<IConverterService>(MockBehavior.Strict);
            var pricingService = new PricingService(converterService.Object);
            var recipe = new Recipe
            {
                Name = "Invalid Recipe",
                RecipeIngredients = null!,
            };

            var result = pricingService.CalculateRecipeCost(recipe);

            Assert.Equal(0m, result);
            converterService.VerifyNoOtherCalls();
        }

        #endregion

        private static Ingredient CreateIngredient(
            UnitType baseUnit,
            decimal costPerBaseUnit,
            decimal? densityGramsPerMl = null)
        {
            return new Ingredient
            {
                Name = "Test Ingredient",
                BaseUnit = baseUnit,
                CostPerBaseUnit = costPerBaseUnit,
                DensityGramsPerMl = densityGramsPerMl,
            };
        }

        private static RecipeIngredient CreateRecipeIngredient(decimal quantity, UnitType unit, Ingredient ingredient)
        {
            return new RecipeIngredient
            {
                Quantity = quantity,
                Unit = unit,
                Ingredient = ingredient,
            };
        }

        private static void AssertApproximatelyEqual(decimal expected, decimal actual, decimal tolerance)
        {
            var difference = Math.Abs(expected - actual);
            Assert.True(
                difference <= tolerance,
                $"Expected {expected} +/- {tolerance}, but got {actual}. Difference was {difference}.");
        }
    }
}
