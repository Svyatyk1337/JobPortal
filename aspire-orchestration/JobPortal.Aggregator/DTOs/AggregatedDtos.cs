namespace JobPortal.Aggregator.DTOs;

public class CandidateProfileDto
{
    public CandidateDto Candidate { get; set; } = null!;
    public List<JobApplicationDto> Applications { get; set; } = new();
    public List<InterviewDto> Interviews { get; set; } = new();
}

public class JobDetailDto
{
    public JobDto Job { get; set; } = null!;
    public CompanyDto Company { get; set; } = null!;
    public List<CompanyReviewDto> CompanyReviews { get; set; } = new();
    public int TotalApplications { get; set; }
}

public class ApplicationDetailDto
{
    public JobApplicationDto Application { get; set; } = null!;
    public CandidateDto Candidate { get; set; } = null!;
    public JobDto Job { get; set; } = null!;
    public CompanyDto Company { get; set; } = null!;
    public List<InterviewDto> Interviews { get; set; } = new();
}

public class DashboardDto
{
    public int TotalCandidates { get; set; }
    public int TotalJobs { get; set; }
    public int TotalApplications { get; set; }
    public int TotalCompanies { get; set; }
    public List<JobApplicationDto> RecentApplications { get; set; } = new();
    public List<InterviewDto> UpcomingInterviews { get; set; } = new();
}
