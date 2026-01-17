using JobPortal.Review.Application.Common.Interfaces;
using JobPortal.Review.Domain.Entities;
using MongoDB.Driver;

namespace JobPortal.Review.Infrastructure.Persistence;

public class DataSeeder : IDataSeeder
{
    private readonly MongoDbContext _context;

    public DataSeeder(MongoDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        // Check if collection is empty
        var count = await _context.CompanyReviews.CountDocumentsAsync(
            FilterDefinition<CompanyReview>.Empty,
            cancellationToken: cancellationToken);

        if (count > 0)
        {
            // Collection is not empty, skip seeding to maintain idempotency
            return;
        }

        // Create test company reviews
        var reviews = new List<CompanyReview>
        {
            CompanyReview.Create(
                companyId: 1,
                userId: 101,
                overallRating: 4.5,
                workLifeBalanceRating: 4.0,
                cultureRating: 4.5,
                managementRating: 4.0,
                compensationRating: 5.0,
                title: "Great place to work!",
                reviewText: "I've been working here for 2 years and it's been an amazing experience. The company culture is fantastic and the benefits are excellent.",
                pros: "Good salary, flexible working hours, great team",
                cons: "Sometimes deadlines can be tight",
                isCurrentEmployee: true,
                jobTitle: "Senior Software Engineer"
            ),
            CompanyReview.Create(
                companyId: 1,
                userId: 102,
                overallRating: 3.5,
                workLifeBalanceRating: 3.0,
                cultureRating: 4.0,
                managementRating: 3.5,
                compensationRating: 4.0,
                title: "Good company with room for improvement",
                reviewText: "Overall a solid place to work, but there are areas that could be improved, especially work-life balance.",
                pros: "Competitive salary, modern tech stack, nice office",
                cons: "Long working hours, limited career growth opportunities",
                isCurrentEmployee: false,
                jobTitle: "Software Developer"
            ),
            CompanyReview.Create(
                companyId: 2,
                userId: 103,
                overallRating: 5.0,
                workLifeBalanceRating: 5.0,
                cultureRating: 5.0,
                managementRating: 4.5,
                compensationRating: 4.5,
                title: "Best company I've ever worked for!",
                reviewText: "The company truly cares about its employees. Great work-life balance, supportive management, and exciting projects.",
                pros: "Amazing culture, flexible schedule, cutting-edge technology",
                cons: "None that I can think of",
                isCurrentEmployee: true,
                jobTitle: "Tech Lead"
            ),
            CompanyReview.Create(
                companyId: 2,
                userId: 104,
                overallRating: 4.0,
                workLifeBalanceRating: 4.5,
                cultureRating: 3.5,
                managementRating: 4.0,
                compensationRating: 4.0,
                title: "Solid workplace with good benefits",
                reviewText: "I enjoy working here. The projects are interesting and the team is professional. Management could be better at communication.",
                pros: "Good benefits, interesting projects, smart colleagues",
                cons: "Communication issues, slow decision making",
                isCurrentEmployee: true,
                jobTitle: "Product Manager"
            ),
            CompanyReview.Create(
                companyId: 3,
                userId: 105,
                overallRating: 3.0,
                workLifeBalanceRating: 2.5,
                cultureRating: 3.0,
                managementRating: 2.5,
                compensationRating: 3.5,
                title: "Not recommended for junior developers",
                reviewText: "The company has potential but lacks proper mentorship programs and work-life balance is poor.",
                pros: "Decent salary for the region",
                cons: "Poor work-life balance, lack of mentorship, outdated processes",
                isCurrentEmployee: false,
                jobTitle: "Junior Developer"
            ),
            CompanyReview.Create(
                companyId: 3,
                userId: 106,
                overallRating: 4.2,
                workLifeBalanceRating: 4.0,
                cultureRating: 4.5,
                managementRating: 4.0,
                compensationRating: 4.0,
                title: "Great for experienced professionals",
                reviewText: "If you're an experienced professional, this company offers great opportunities to work on challenging projects.",
                pros: "Challenging projects, good salary, professional team",
                cons: "Not ideal for beginners",
                isCurrentEmployee: true,
                jobTitle: "Senior DevOps Engineer"
            ),
            CompanyReview.Create(
                companyId: 4,
                userId: 107,
                overallRating: 4.8,
                workLifeBalanceRating: 4.5,
                cultureRating: 5.0,
                managementRating: 4.5,
                compensationRating: 5.0,
                title: "Dream job - highly recommended!",
                reviewText: "Everything you could ask for in a workplace. Great culture, excellent compensation, and meaningful work.",
                pros: "Top-tier salary, amazing benefits, great culture, work-life balance",
                cons: "High expectations, can be stressful at times",
                isCurrentEmployee: true,
                jobTitle: "Staff Engineer"
            ),
            CompanyReview.Create(
                companyId: 4,
                userId: 108,
                overallRating: 4.3,
                workLifeBalanceRating: 4.0,
                cultureRating: 4.5,
                managementRating: 4.0,
                compensationRating: 4.5,
                title: "Excellent company for career growth",
                reviewText: "The company invests heavily in employee development. Great opportunities for learning and advancement.",
                pros: "Career development, good compensation, supportive environment",
                cons: "Fast-paced environment may not suit everyone",
                isCurrentEmployee: true,
                jobTitle: "Engineering Manager"
            ),
            CompanyReview.Create(
                companyId: 5,
                userId: 109,
                overallRating: 3.8,
                workLifeBalanceRating: 4.0,
                cultureRating: 3.5,
                managementRating: 3.5,
                compensationRating: 4.0,
                title: "Average company, nothing special",
                reviewText: "It's an okay place to work. Nothing particularly bad, but nothing exceptional either. Good for gaining experience.",
                pros: "Stable employment, regular salary, normal working hours",
                cons: "Limited innovation, bureaucratic processes",
                isCurrentEmployee: true,
                jobTitle: "QA Engineer"
            ),
            CompanyReview.Create(
                companyId: 5,
                userId: 110,
                overallRating: 4.0,
                workLifeBalanceRating: 4.5,
                cultureRating: 4.0,
                managementRating: 3.5,
                compensationRating: 3.5,
                title: "Good work-life balance",
                reviewText: "If work-life balance is your priority, this is a great place. The pace is reasonable and expectations are realistic.",
                pros: "Excellent work-life balance, friendly team, low stress",
                cons: "Below-market compensation, limited growth opportunities",
                isCurrentEmployee: true,
                jobTitle: "Frontend Developer"
            )
        };

        // Insert all reviews at once
        await _context.CompanyReviews.InsertManyAsync(reviews, cancellationToken: cancellationToken);
    }
}
