using Moq;
using RecipeCost.Shared;
using RecipeCostAPI.Data;
using RecipeCostAPI.Models;
using RecipeCostAPI.Services;
using RecipeCostAPI.Services.Interfaces;
using Xunit;

namespace RecipeCostAPI.Tests.Services
{
    public class RecipeServiceTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly Mock<IPricingService> _mockPricingService;
        private readonly RecipeService _recipeService;

        public RecipeServiceTests()
        {
            _mockContext = new Mock<AppDbContext>();
            _mockPricingService = new Mock<IPricingService>();
            _recipeService = new RecipeService(_mockContext.Object, _mockPricingService.Object);
        }

        #region CreateRecipeAsync Tests
 

        #endregion

        #region GetRecipesAsync Tests

        #endregion

        #region GetRecipeByIdAsync Tests

        #endregion

        #region UpdateRecipeAsync Tests

        #endregion

        #region Unit Persistence Tests

        #endregion
    }
}
