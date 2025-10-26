namespace JobPortal.Application.Bll.DTOs;

public class InterviewDto
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public string InterviewType { get; set; } = string.Empty;
    public int RoundNumber { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string Status { get; set; } = "Scheduled";
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateInterviewDto
{
    public int ApplicationId { get; set; }
    public string InterviewType { get; set; } = string.Empty;
    public int RoundNumber { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string Status { get; set; } = "Scheduled";
    public string? Notes { get; set; }
}

public class UpdateInterviewDto
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public string InterviewType { get; set; } = string.Empty;
    public int RoundNumber { get; set; }
    public DateTime ScheduledDate { get; set; }
    public string Status { get; set; } = "Scheduled";
    public string? Notes { get; set; }
}
