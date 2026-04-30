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
        private readonly Mock<IConverterService> _mockConverterService;
        private readonly PricingService _pricingService;

        public PricingServiceTests()
        {
            _mockConverterService = new Mock<IConverterService>();
            _pricingService = new PricingService(_mockConverterService.Object);
        }

        #region CalculateLineItemCost Tests

        [Fact]
        public void CalculateLineItemCost_WithCupQuantityAndIngredientDensity_UsesDensityForGramWeight()
        {
            var pricingService = new PricingService(new ConverterService());
            var ingredient = new Ingredient
            {
                Name = "Dense Ingredient",
                BaseUnit = UnitType.Gram,
                CostPerBaseUnit = 1m,
                DensityGramsPerMl = 1.2m,
            };

            var result = pricingService.CalculateLineItemCost(1m, UnitType.Cup, ingredient);

            AssertApproximatelyEqual(283.9058838m, result, 0.000001m);
        }

        #endregion

        #region CalculateRecipeCost Tests

        #endregion

        private static void AssertApproximatelyEqual(decimal expected, decimal actual, decimal tolerance)
        {
            var difference = Math.Abs(expected - actual);
            Assert.True(
                difference <= tolerance,
                $"Expected {expected} +/- {tolerance}, but got {actual}. Difference was {difference}.");
        }
    }
}
