using AutoMapper;
using JobPortal.Review.Application.Common.Interfaces;
using JobPortal.Review.Application.Common.Models;
using JobPortal.Review.Application.CompanyReviews.DTOs;
using JobPortal.Review.Domain.Entities;
using MediatR;
using MongoDB.Driver;

namespace JobPortal.Review.Application.CompanyReviews.Queries;

public class GetCompanyReviewsPagedQueryHandler : IRequestHandler<GetCompanyReviewsPagedQuery, PagedResult<CompanyReviewDto>>
{
    private readonly IRepository<CompanyReview> _repository;
    private readonly IMapper _mapper;

    public GetCompanyReviewsPagedQueryHandler(IRepository<CompanyReview> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PagedResult<CompanyReviewDto>> Handle(GetCompanyReviewsPagedQuery request, CancellationToken cancellationToken)
    {
        // Build filter
        var filterBuilder = Builders<CompanyReview>.Filter;
        var filter = filterBuilder.Empty;

        if (request.CompanyId.HasValue)
        {
            filter = filterBuilder.Eq(r => r.CompanyId, request.CompanyId.Value);
        }

        // Build sort
        var sortBuilder = Builders<CompanyReview>.Sort;
        var sort = request.SortBy?.ToLower() switch
        {
            "createdat" => sortBuilder.Descending(r => r.CreatedAt),
            "createdatasc" => sortBuilder.Ascending(r => r.CreatedAt),
            "overallrating" => sortBuilder.Descending(r => r.OverallRating),
            "overallratingasc" => sortBuilder.Ascending(r => r.OverallRating),
            _ => sortBuilder.Descending(r => r.CreatedAt) // Default sort
        };

        // Calculate pagination
        var skip = (request.Page - 1) * request.PageSize;

        // Get total count and paged data
        var totalCount = await _repository.CountAsync(filter, cancellationToken);
        var reviews = await _repository.GetPagedAsync(filter, sort, skip, request.PageSize, cancellationToken);

        // Map to DTOs
        var reviewDtos = _mapper.Map<IEnumerable<CompanyReviewDto>>(reviews);

        return new PagedResult<CompanyReviewDto>(reviewDtos, request.Page, request.PageSize, totalCount);
    }
}
