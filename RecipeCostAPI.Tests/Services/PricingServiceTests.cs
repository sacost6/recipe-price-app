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
 
        #endregion

        #region CalculateRecipeCost Tests

        #endregion
    }
}
