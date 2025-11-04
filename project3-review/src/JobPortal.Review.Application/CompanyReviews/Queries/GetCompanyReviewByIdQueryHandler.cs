using AutoMapper;
using JobPortal.Review.Application.Common.Interfaces;
using JobPortal.Review.Application.CompanyReviews.DTOs;
using JobPortal.Review.Domain.Entities;
using MediatR;

namespace JobPortal.Review.Application.CompanyReviews.Queries;

public class GetCompanyReviewByIdQueryHandler : IRequestHandler<GetCompanyReviewByIdQuery, CompanyReviewDto?>
{
    private readonly IRepository<CompanyReview> _repository;
    private readonly IMapper _mapper;

    public GetCompanyReviewByIdQueryHandler(IRepository<CompanyReview> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<CompanyReviewDto?> Handle(GetCompanyReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var review = await _repository.GetByIdAsync(request.Id, cancellationToken);
        return review == null ? null : _mapper.Map<CompanyReviewDto>(review);
    }
}
