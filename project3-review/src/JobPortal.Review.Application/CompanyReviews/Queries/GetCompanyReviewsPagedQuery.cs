using JobPortal.Review.Application.Common.Models;
using JobPortal.Review.Application.CompanyReviews.DTOs;
using MediatR;

namespace JobPortal.Review.Application.CompanyReviews.Queries;

public record GetCompanyReviewsPagedQuery(
    int Page,
    int PageSize,
    int? CompanyId = null,
    string? SortBy = null
) : IRequest<PagedResult<CompanyReviewDto>>;
