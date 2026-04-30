using FluentValidation;
using RecipeCost.Shared;

namespace RecipeCostAPI.Validation;

public sealed class IngredientDtoValidator : AbstractValidator<IngredientDto>
{
    public IngredientDtoValidator()
    {
        RuleFor(dto => dto.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(dto => dto.UserUnit)
            .IsInEnum();

        RuleFor(dto => dto.BaseUnit)
            .IsInEnum();

        RuleFor(dto => dto.CostPerUserUnit)
            .GreaterThan(0);

        RuleFor(dto => dto.CostPerBaseUnit)
            .GreaterThanOrEqualTo(0);

        RuleFor(dto => dto.DensityGramsPerMl)
            .GreaterThan(0)
            .When(dto => dto.DensityGramsPerMl.HasValue);
    }
}
