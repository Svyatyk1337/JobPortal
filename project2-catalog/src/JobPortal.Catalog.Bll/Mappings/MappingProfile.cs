using AutoMapper;
using JobPortal.Catalog.Bll.DTOs;
using JobPortal.Catalog.Domain.Entities;

namespace JobPortal.Catalog.Bll.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Company mappings
        CreateMap<Company, CompanyDto>();
        CreateMap<Company, CompanyWithContactDto>();
        CreateMap<CreateCompanyDto, Company>();
        CreateMap<UpdateCompanyDto, Company>();

        // CompanyContact mappings
        CreateMap<CompanyContact, CompanyContactDto>();
        CreateMap<CreateCompanyContactDto, CompanyContact>();
        CreateMap<UpdateCompanyContactDto, CompanyContact>();

        // Job mappings
        CreateMap<Job, JobDto>();
        CreateMap<Job, JobWithDetailsDto>()
            .ForMember(dest => dest.Skills, opt => opt.MapFrom(src => src.SkillRequirements));
        CreateMap<CreateJobDto, Job>();
        CreateMap<UpdateJobDto, Job>();

        // JobCategory mappings
        CreateMap<JobCategory, JobCategoryDto>();
        CreateMap<CreateJobCategoryDto, JobCategory>();

        // SkillTag mappings
        CreateMap<SkillTag, SkillTagDto>();
        CreateMap<CreateSkillTagDto, SkillTag>();

        // JobSkillRequirement mappings
        CreateMap<JobSkillRequirement, SkillRequirementDto>()
            .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.SkillTag.Name));
    }
}
