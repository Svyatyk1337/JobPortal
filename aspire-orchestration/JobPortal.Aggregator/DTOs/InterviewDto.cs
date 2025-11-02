namespace JobPortal.Aggregator.DTOs;

public class InterviewDto
{
    public int Id { get; set; }
    public int ApplicationId { get; set; }
    public DateTime ScheduledAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Feedback { get; set; }
}
