using FluentValidation;
using RecipeCost.Shared;

namespace RecipeCostAPI.Validation;

public sealed class RecipeDtoValidator : AbstractValidator<RecipeDto>
{
    public RecipeDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(dto => dto.Description)
            .MaximumLength(500);

        RuleFor(dto => dto.Servings)
            .InclusiveBetween(1, 100);

        RuleFor(dto => dto.Ingredients)
            .NotNull();

        RuleForEach(dto => dto.Ingredients)
            .SetValidator(new RecipeIngredientDtoValidator());
    }
}
