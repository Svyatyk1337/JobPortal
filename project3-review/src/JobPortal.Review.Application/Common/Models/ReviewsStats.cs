namespace JobPortal.Review.Application.Common.Models;

public class ReviewsStats
{
    public int Count { get; set; }
    public double AverageRating { get; set; }
    public Dictionary<int, int> RatingDistribution { get; set; } = new();
}
