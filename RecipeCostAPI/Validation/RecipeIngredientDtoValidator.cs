using FluentValidation;
using RecipeCost.Shared;

namespace RecipeCostAPI.Validation;

public sealed class RecipeIngredientDtoValidator : AbstractValidator<RecipeIngredientDto>
{
    public RecipeIngredientDtoValidator()
    {
        RuleFor(dto => dto.IngredientId)
            .GreaterThan(0);

        RuleFor(dto => dto.Quantity)
            .GreaterThan(0);

        RuleFor(dto => dto.BaseUnit)
            .IsInEnum();

        RuleFor(dto => dto.CalculatedCost)
            .GreaterThanOrEqualTo(0);
    }
}
