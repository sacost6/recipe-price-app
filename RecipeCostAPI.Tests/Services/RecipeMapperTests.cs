using RecipeCost.Shared;
using RecipeCostAPI.Mappers;
using RecipeCostAPI.Models;
using RecipeCostAPI.Services.Interfaces;
using Xunit;
using Moq;

namespace RecipeCostAPI.Tests.Services
{
    public class RecipeMapperTests
    {
        private readonly Mock<IPricingService> _mockPricingService;

        public RecipeMapperTests()
        {
            _mockPricingService = new Mock<IPricingService>();
        }

        #region RecipeIngredientDto Mapping Tests
 
        #endregion

        #region RecipeDto Mapping Tests
 

        #endregion

        #region Entity Conversion Tests
 

        #endregion

        #region Ingredient Mapping Tests

     
        #endregion
    }
}
