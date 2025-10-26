namespace JobPortal.Application.Bll.DTOs;

public class JobApplicationDto
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public DateTime SubmittedDate { get; set; }
    public decimal? ExpectedSalary { get; set; }
    public DateTime CreatedAt { get; set; }

    public CandidateDto? Candidate { get; set; }
    public ApplicationDetailsDto? ApplicationDetails { get; set; }
    public List<InterviewDto>? Interviews { get; set; }
}

public class CreateJobApplicationDto
{
    public int CandidateId { get; set; }
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public DateTime SubmittedDate { get; set; }
    public decimal? ExpectedSalary { get; set; }

    public CreateApplicationDetailsDto? ApplicationDetails { get; set; }
}

public class UpdateJobApplicationDto
{
    public int Id { get; set; }
    public int CandidateId { get; set; }
    public int JobId { get; set; }
    public string JobTitle { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    public string Status { get; set; } = "Pending";
    public DateTime SubmittedDate { get; set; }
    public decimal? ExpectedSalary { get; set; }
}

public class ApplicationDetailsDto
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public string? ResumeUrl { get; set; }
    public string? CoverLetter { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateApplicationDetailsDto
{
    public string? ResumeUrl { get; set; }
    public string? CoverLetter { get; set; }
}
