using AutoMapper;
using JobPortal.Review.Application.Common.Interfaces;
using JobPortal.Review.Application.CompanyReviews.DTOs;
using JobPortal.Review.Domain.Entities;
using MediatR;

namespace JobPortal.Review.Application.CompanyReviews.Queries;

public class GetAllCompanyReviewsQueryHandler : IRequestHandler<GetAllCompanyReviewsQuery, IEnumerable<CompanyReviewDto>>
{
    private readonly IRepository<CompanyReview> _repository;
    private readonly IMapper _mapper;

    public GetAllCompanyReviewsQueryHandler(IRepository<CompanyReview> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompanyReviewDto>> Handle(GetAllCompanyReviewsQuery request, CancellationToken cancellationToken)
    {
        var reviews = await _repository.GetAllAsync(cancellationToken);
        return _mapper.Map<IEnumerable<CompanyReviewDto>>(reviews);
    }
}
