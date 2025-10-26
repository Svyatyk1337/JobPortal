using AutoMapper;
using JobPortal.Application.Bll.DTOs;
using JobPortal.Application.Domain.Models;

namespace JobPortal.Application.Bll.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Candidate mappings
        CreateMap<Candidate, CandidateDto>();
        CreateMap<CreateCandidateDto, Candidate>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.JobApplications, opt => opt.Ignore());
        CreateMap<UpdateCandidateDto, Candidate>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.JobApplications, opt => opt.Ignore());

        // JobApplication mappings
        CreateMap<JobApplication, JobApplicationDto>();
        CreateMap<CreateJobApplicationDto, JobApplication>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Candidate, opt => opt.Ignore())
            .ForMember(dest => dest.ApplicationDetails, opt => opt.Ignore())
            .ForMember(dest => dest.Interviews, opt => opt.Ignore())
            .ForMember(dest => dest.ApplicationSkills, opt => opt.Ignore());
        CreateMap<UpdateJobApplicationDto, JobApplication>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.Candidate, opt => opt.Ignore())
            .ForMember(dest => dest.ApplicationDetails, opt => opt.Ignore())
            .ForMember(dest => dest.Interviews, opt => opt.Ignore())
            .ForMember(dest => dest.ApplicationSkills, opt => opt.Ignore());

        // ApplicationDetails mappings
        CreateMap<ApplicationDetails, ApplicationDetailsDto>();
        CreateMap<CreateApplicationDetailsDto, ApplicationDetails>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.ApplicationId, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.JobApplication, opt => opt.Ignore());

        // Interview mappings
        CreateMap<Interview, InterviewDto>();
        CreateMap<CreateInterviewDto, Interview>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.JobApplication, opt => opt.Ignore());
        CreateMap<UpdateInterviewDto, Interview>()
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
            .ForMember(dest => dest.JobApplication, opt => opt.Ignore());
    }
}
