using FluentValidation;
using JobPortal.Review.Application.CompanyReviews.Commands;

namespace JobPortal.Review.Application.CompanyReviews.Validators;

public class CreateCompanyReviewCommandValidator : AbstractValidator<CreateCompanyReviewCommand>
{
    public CreateCompanyReviewCommandValidator()
    {
        RuleFor(x => x.CompanyId)
            .GreaterThan(0)
            .WithMessage("CompanyId must be greater than 0");

        RuleFor(x => x.UserId)
            .GreaterThan(0)
            .WithMessage("UserId must be greater than 0");

        RuleFor(x => x.OverallRating)
            .InclusiveBetween(0, 5)
            .WithMessage("OverallRating must be between 0 and 5");

        RuleFor(x => x.ReviewText)
            .NotEmpty()
            .WithMessage("ReviewText is required")
            .MaximumLength(5000)
            .WithMessage("ReviewText must not exceed 5000 characters");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(200)
            .WithMessage("Title must not exceed 200 characters");
    }
}
