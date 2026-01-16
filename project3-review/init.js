// ============================================
// Project 3: Review Service (MongoDB)
// ============================================

// Switch to review_db database
db = db.getSiblingDB('review_db');

// ============================================
// Drop collections if exist
// ============================================
db.company_reviews.drop();
db.interview_experiences.drop();
db.salary_reports.drop();

// ============================================
// Collection: company_reviews
// ============================================
db.company_reviews.insertMany([
    {
        company_id: 1,
        user_id: 101,
        job_title: "Senior Developer",
        overall_rating: 4.5,
        work_life_balance_rating: 5.0,
        compensation_rating: 4.0,
        management_rating: 4.5,
        culture_rating: 5.0,
        title: "Great place to work!",
        review_text: "I've been working here for 2 years and it's been an amazing experience.",
        pros: "Great team, modern tech stack, excellent work-life balance",
        cons: "Sometimes tight deadlines on major releases",
        is_current_employee: true,
        created_at: new Date("2025-01-15")
    },
    {
        company_id: 2,
        user_id: 102,
        job_title: "Frontend Developer",
        overall_rating: 4.0,
        work_life_balance_rating: 4.0,
        compensation_rating: 4.0,
        management_rating: 4.0,
        culture_rating: 4.0,
        title: "Good company for learning",
        review_text: "Lots of opportunities to grow your skills and learn new technologies.",
        pros: "Lots of learning opportunities, friendly atmosphere, modern office",
        cons: "Could offer better compensation packages",
        is_current_employee: true,
        created_at: new Date("2025-01-10")
    },
    {
        company_id: 3,
        user_id: 103,
        job_title: "Data Engineer",
        overall_rating: 4.8,
        work_life_balance_rating: 5.0,
        compensation_rating: 5.0,
        management_rating: 4.5,
        culture_rating: 5.0,
        title: "Excellent company with great projects",
        review_text: "Working on cutting-edge data engineering projects with modern tools.",
        pros: "Excellent compensation, cutting-edge projects, great leadership",
        cons: "Small team, limited social events",
        is_current_employee: false,
        created_at: new Date("2025-01-18")
    },
    {
        company_id: 1,
        user_id: 104,
        job_title: "DevOps Engineer",
        overall_rating: 4.3,
        work_life_balance_rating: 4.0,
        compensation_rating: 5.0,
        management_rating: 4.0,
        culture_rating: 4.5,
        title: "Challenging DevOps role",
        review_text: "Great infrastructure setup with plenty of challenges to solve.",
        pros: "Great infrastructure, good pay, interesting challenges",
        cons: "On-call rotation can be demanding",
        is_current_employee: true,
        created_at: new Date("2025-01-20")
    },
    {
        company_id: 2,
        user_id: 105,
        job_title: "Backend Developer",
        overall_rating: 3.8,
        work_life_balance_rating: 3.0,
        compensation_rating: 4.0,
        management_rating: 4.0,
        culture_rating: 4.0,
        title: "Good tech stack but work-life balance needs improvement",
        review_text: "The technology is modern but sometimes we need to work overtime.",
        pros: "Good tech stack, supportive colleagues",
        cons: "Sometimes overtime is required, could improve work-life balance",
        is_current_employee: true,
        created_at: new Date("2025-01-12")
    }
]);

// ============================================
// Collection: interview_experiences
// ============================================
db.interview_experiences.insertMany([
    {
        company_id: 1,
        company_name: "TechCorp",
        job_title: "Senior Full-Stack Developer",
        interview_process: [
            {
                round: 1,
                type: "HR Screening",
                duration: 30,
                difficulty: "Easy",
                description: "General questions about experience and motivation"
            },
            {
                round: 2,
                type: "Technical Interview",
                duration: 90,
                difficulty: "Medium",
                topics: ["C#", ".NET Core", "React", "System Design"],
                description: "Coding tasks and architecture discussion"
            },
            {
                round: 3,
                type: "Final Interview",
                duration: 45,
                difficulty: "Easy",
                description: "Meeting with team lead and product manager"
            }
        ],
        outcome: "Offer Extended",
        overall_rating: 4.5,
        feedback: "Professional and well-organized process. Clear communication throughout.",
        created_at: new Date("2025-01-16")
    },
    {
        company_id: 2,
        company_name: "InnoSoft",
        job_title: "Frontend Developer",
        interview_process: [
            {
                round: 1,
                type: "HR Screening",
                duration: 20,
                difficulty: "Easy",
                description: "Brief intro call"
            },
            {
                round: 2,
                type: "Technical Interview",
                duration: 60,
                difficulty: "Medium",
                topics: ["JavaScript", "React", "TypeScript"],
                description: "Live coding session with React components"
            }
        ],
        outcome: "Offer Extended",
        overall_rating: 4.0,
        feedback: "Friendly interviewers, realistic coding tasks",
        created_at: new Date("2025-01-11")
    },
    {
        company_id: 3,
        company_name: "DataVision",
        job_title: "Data Engineer",
        interview_process: [
            {
                round: 1,
                type: "Technical Phone Screen",
                duration: 45,
                difficulty: "Medium",
                topics: ["Python", "SQL", "Data Pipelines"],
                description: "SQL queries and Python coding"
            },
            {
                round: 2,
                type: "Technical Interview",
                duration: 90,
                difficulty: "Hard",
                topics: ["System Design", "ETL", "PostgreSQL"],
                description: "Design a data warehouse solution"
            },
            {
                round: 3,
                type: "Cultural Fit",
                duration: 30,
                difficulty: "Easy",
                description: "Team culture and values discussion"
            }
        ],
        outcome: "Offer Extended",
        overall_rating: 4.7,
        feedback: "Challenging but fair. Great questions that test real skills.",
        created_at: new Date("2025-01-19")
    },
    {
        company_id: 1,
        company_name: "TechCorp",
        job_title: "DevOps Engineer",
        interview_process: [
            {
                round: 1,
                type: "Technical Interview",
                duration: 75,
                difficulty: "Hard",
                topics: ["Docker", "Kubernetes", "AWS", "CI/CD"],
                description: "Deep dive into infrastructure and DevOps practices"
            },
            {
                round: 2,
                type: "Practical Task",
                duration: 120,
                difficulty: "Hard",
                description: "Take-home assignment to set up CI/CD pipeline"
            }
        ],
        outcome: "No Offer",
        overall_rating: 3.5,
        feedback: "Very challenging technical assessment. Could provide better feedback.",
        created_at: new Date("2025-01-13")
    }
]);

// ============================================
// Collection: salary_reports
// ============================================
db.salary_reports.insertMany([
    {
        job_title: "Senior Full-Stack Developer",
        company_name: "TechCorp",
        salary: {
            amount: 4500,
            currency: "USD",
            period: "monthly"
        },
        location: {
            city: "Kyiv",
            country: "Ukraine"
        },
        total_experience: 6,
        skills: ["C#", ".NET Core", "React", "PostgreSQL", "Docker"],
        employment_type: "Full-time",
        benefits: ["Health Insurance", "Remote Work", "Annual Bonus"],
        created_at: new Date("2025-01-16")
    },
    {
        job_title: "Frontend Developer",
        company_name: "InnoSoft",
        salary: {
            amount: 3500,
            currency: "USD",
            period: "monthly"
        },
        location: {
            city: "Lviv",
            country: "Ukraine"
        },
        total_experience: 3,
        skills: ["JavaScript", "React", "TypeScript", "HTML/CSS"],
        employment_type: "Full-time",
        benefits: ["Health Insurance", "English Courses"],
        created_at: new Date("2025-01-11")
    },
    {
        job_title: "Data Engineer",
        company_name: "DataVision",
        salary: {
            amount: 5000,
            currency: "USD",
            period: "monthly"
        },
        location: {
            city: "Kharkiv",
            country: "Ukraine"
        },
        total_experience: 5,
        skills: ["Python", "PostgreSQL", "Apache Airflow", "AWS"],
        employment_type: "Full-time",
        benefits: ["Health Insurance", "Remote Work", "Stock Options"],
        created_at: new Date("2025-01-19")
    },
    {
        job_title: "DevOps Engineer",
        company_name: "Anonymous",
        salary: {
            amount: 4800,
            currency: "USD",
            period: "monthly"
        },
        location: {
            city: "Kyiv",
            country: "Ukraine"
        },
        total_experience: 7,
        skills: ["Docker", "Kubernetes", "AWS", "Terraform", "Jenkins"],
        employment_type: "Full-time",
        benefits: ["Health Insurance", "Remote Work", "Annual Bonus", "Conference Budget"],
        created_at: new Date("2025-01-14")
    },
    {
        job_title: "Backend Developer",
        company_name: "Anonymous",
        salary: {
            amount: 4000,
            currency: "USD",
            period: "monthly"
        },
        location: {
            city: "Lviv",
            country: "Ukraine"
        },
        total_experience: 4,
        skills: ["Node.js", "TypeScript", "MongoDB", "Redis"],
        employment_type: "Full-time",
        benefits: ["Health Insurance", "Remote Work"],
        created_at: new Date("2025-01-12")
    },
    {
        job_title: "Junior Full-Stack Developer",
        company_name: "Anonymous",
        salary: {
            amount: 2500,
            currency: "USD",
            period: "monthly"
        },
        location: {
            city: "Kyiv",
            country: "Ukraine"
        },
        total_experience: 1,
        skills: ["JavaScript", "React", "Node.js"],
        employment_type: "Full-time",
        benefits: ["Health Insurance", "Mentorship Program"],
        created_at: new Date("2025-01-08")
    }
]);

// ============================================
// Create Indexes
// ============================================
db.company_reviews.createIndex({ company_id: 1, overall_rating: -1 });
db.company_reviews.createIndex({ company_name: 1 });
db.company_reviews.createIndex({ created_at: -1 });

db.interview_experiences.createIndex({ company_id: 1, created_at: -1 });
db.interview_experiences.createIndex({ company_name: 1 });
db.interview_experiences.createIndex({ job_title: 1 });

db.salary_reports.createIndex({ job_title: 1, "location.city": 1 });
db.salary_reports.createIndex({ "salary.amount": -1 });
db.salary_reports.createIndex({ created_at: -1 });

// ============================================
// Verification
// ============================================
print("=== Database Initialization Complete ===");
print("Company reviews:", db.company_reviews.countDocuments());
print("Interview experiences:", db.interview_experiences.countDocuments());
print("Salary reports:", db.salary_reports.countDocuments());
print("");
print("Sample company review:");
printjson(db.company_reviews.findOne());
