using RecipeCost.Shared;
using RecipeCostAPI.Models;
using RecipeCostAPI.Services;
using Xunit;

namespace RecipeCostAPI.Tests.Services
{
    public class RecipeUnitIntegrationTests
    {
        private readonly ConverterService _converterService;
        private readonly PricingService _pricingService;

        public RecipeUnitIntegrationTests()
        {
            _converterService = new ConverterService();
            _pricingService = new PricingService(_converterService);
        }

        #region Integration Scenario Tests

          

        #endregion

        #region Density Conversion Integration Tests

        #endregion

        #region Edge Cases and Validation Tests

        #endregion
    }
}
