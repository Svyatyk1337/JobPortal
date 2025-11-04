using AutoMapper;
using JobPortal.Review.Application.CompanyReviews.DTOs;
using JobPortal.Review.Domain.Entities;

namespace JobPortal.Review.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CompanyReview, CompanyReviewDto>()
            .ForMember(dest => dest.OverallRating, opt => opt.MapFrom(src => src.OverallRating.Value))
            .ForMember(dest => dest.WorkLifeBalanceRating, opt => opt.MapFrom(src => src.WorkLifeBalanceRating.Value))
            .ForMember(dest => dest.CultureRating, opt => opt.MapFrom(src => src.CultureRating.Value))
            .ForMember(dest => dest.ManagementRating, opt => opt.MapFrom(src => src.ManagementRating.Value))
            .ForMember(dest => dest.CompensationRating, opt => opt.MapFrom(src => src.CompensationRating.Value));
    }
}
