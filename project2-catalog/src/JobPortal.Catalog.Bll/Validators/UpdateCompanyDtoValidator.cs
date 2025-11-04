using FluentValidation;
using JobPortal.Catalog.Bll.DTOs;

namespace JobPortal.Catalog.Bll.Validators;

public class UpdateCompanyDtoValidator : AbstractValidator<UpdateCompanyDto>
{
    public UpdateCompanyDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Company name is required")
            .MaximumLength(200).WithMessage("Company name must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters");

        RuleFor(x => x.Industry)
            .NotEmpty().WithMessage("Industry is required")
            .MaximumLength(100).WithMessage("Industry must not exceed 100 characters");

        RuleFor(x => x.EmployeeCount)
            .GreaterThan(0).WithMessage("Employee count must be greater than 0");

        RuleFor(x => x.Website)
            .MaximumLength(255).WithMessage("Website must not exceed 255 characters");
    }
}
