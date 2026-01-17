using JobPortal.Review.Application.CompanyReviews.DTOs;
using MediatR;

namespace JobPortal.Review.Application.CompanyReviews.Queries;

public record GetAllCompanyReviewsQuery() : IRequest<IEnumerable<CompanyReviewDto>>;
