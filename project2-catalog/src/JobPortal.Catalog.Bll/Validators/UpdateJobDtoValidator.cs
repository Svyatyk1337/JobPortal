using FluentValidation;
using JobPortal.Catalog.Bll.DTOs;

namespace JobPortal.Catalog.Bll.Validators;

public class UpdateJobDtoValidator : AbstractValidator<UpdateJobDto>
{
    public UpdateJobDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Job title is required")
            .MaximumLength(200).WithMessage("Job title must not exceed 200 characters");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .MaximumLength(5000).WithMessage("Description must not exceed 5000 characters");

        RuleFor(x => x.CategoryId)
            .GreaterThan(0).WithMessage("Valid category ID is required");

        RuleFor(x => x.SalaryMin)
            .GreaterThanOrEqualTo(0).WithMessage("Minimum salary must be greater than or equal to 0");

        RuleFor(x => x.SalaryMax)
            .GreaterThanOrEqualTo(0).WithMessage("Maximum salary must be greater than or equal to 0")
            .GreaterThanOrEqualTo(x => x.SalaryMin).WithMessage("Maximum salary must be greater than or equal to minimum salary");

        RuleFor(x => x.Location)
            .NotEmpty().WithMessage("Location is required")
            .MaximumLength(200).WithMessage("Location must not exceed 200 characters");

        RuleFor(x => x.EmploymentType)
            .NotEmpty().WithMessage("Employment type is required");

        RuleFor(x => x.ExperienceYears)
            .GreaterThanOrEqualTo(0).WithMessage("Experience years must be greater than or equal to 0");
    }
}
