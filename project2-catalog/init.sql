-- ============================================
-- Project 2: Catalog Service (MySQL)
-- ============================================

-- Drop tables if exist
DROP TABLE IF EXISTS job_skill_requirements;
DROP TABLE IF EXISTS skill_tags;
DROP TABLE IF EXISTS jobs;
DROP TABLE IF EXISTS job_categories;
DROP TABLE IF EXISTS company_contacts;
DROP TABLE IF EXISTS companies;

-- ============================================
-- Table: companies
-- ============================================
CREATE TABLE companies (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(200) NOT NULL,
    Description TEXT,
    Industry VARCHAR(100),
    EmployeeCount INT NOT NULL DEFAULT 0,
    Website VARCHAR(255),
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ============================================
-- Table: company_contacts (1:1 with companies)
-- ============================================
CREATE TABLE company_contacts (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CompanyId INT UNIQUE NOT NULL,
    Email VARCHAR(255) NOT NULL,
    Phone VARCHAR(20),
    Address VARCHAR(500),
    City VARCHAR(100),
    Country VARCHAR(100),
    FOREIGN KEY (CompanyId) REFERENCES companies(Id) ON DELETE CASCADE
);

-- ============================================
-- Table: job_categories
-- ============================================
CREATE TABLE job_categories (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL UNIQUE,
    Description TEXT
);

-- ============================================
-- Table: jobs
-- ============================================
CREATE TABLE jobs (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    CompanyId INT NOT NULL,
    CategoryId INT,
    Title VARCHAR(200) NOT NULL,
    Description TEXT,
    SalaryMin DECIMAL(10, 2),
    SalaryMax DECIMAL(10, 2),
    Location VARCHAR(200),
    EmploymentType VARCHAR(50),
    ExperienceYears INT NOT NULL DEFAULT 0,
    IsActive BOOLEAN DEFAULT TRUE,
    PostedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (CompanyId) REFERENCES companies(Id) ON DELETE CASCADE,
    FOREIGN KEY (CategoryId) REFERENCES job_categories(Id) ON DELETE SET NULL,
    CHECK (SalaryMin > 0),
    CHECK (SalaryMax >= SalaryMin)
);

-- ============================================
-- Table: skill_tags
-- ============================================
CREATE TABLE skill_tags (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Name VARCHAR(100) NOT NULL UNIQUE,
    Category VARCHAR(100)
);

-- ============================================
-- Table: job_skill_requirements (M:N relationship)
-- ============================================
CREATE TABLE job_skill_requirements (
    JobId INT NOT NULL,
    SkillTagId INT NOT NULL,
    RequiredLevel VARCHAR(50) DEFAULT 'Required',
    IsRequired BOOLEAN DEFAULT TRUE,
    FOREIGN KEY (JobId) REFERENCES jobs(Id) ON DELETE CASCADE,
    FOREIGN KEY (SkillTagId) REFERENCES skill_tags(Id) ON DELETE CASCADE,
    PRIMARY KEY (JobId, SkillTagId)
);

-- ============================================
-- Insert Test Data
-- ============================================

-- Insert 3 companies
INSERT INTO companies (Name, Description, Industry, EmployeeCount, Website) VALUES
('TechCorp', 'Leading technology company specializing in cloud solutions and enterprise software', 'Information Technology', 750, 'https://techcorp.example.com'),
('InnoSoft', 'Innovative software development company focused on mobile and web applications', 'Software Development', 300, 'https://innosoft.example.com'),
('DataVision', 'Data analytics and business intelligence solutions provider', 'Data Analytics', 75, 'https://datavision.example.com');

-- Insert 3 company_contacts (1:1 relationship)
INSERT INTO company_contacts (CompanyId, Email, Phone, Address, City, Country) VALUES
(1, 'careers@techcorp.example.com', '+380443456789', '123 Tech Street', 'Kyiv', 'Ukraine'),
(2, 'hr@innosoft.example.com', '+380445678901', '456 Innovation Ave', 'Lviv', 'Ukraine'),
(3, 'jobs@datavision.example.com', '+380447890123', '789 Data Boulevard', 'Kharkiv', 'Ukraine');

-- Insert 5 job_categories
INSERT INTO job_categories (Name, Description) VALUES
('IT', 'Information Technology and Software Development positions'),
('Marketing', 'Marketing, PR, and Communications roles'),
('Sales', 'Sales and Business Development opportunities'),
('Design', 'UI/UX Design and Creative positions'),
('HR', 'Human Resources and Talent Management roles');

-- Insert 10 jobs
INSERT INTO jobs (CompanyId, CategoryId, Title, Description, SalaryMin, SalaryMax, Location, EmploymentType, ExperienceYears, IsActive) VALUES
(1, 1, 'Senior Full-Stack Developer', 'We are looking for an experienced Full-Stack Developer to join our team. You will work on cutting-edge cloud solutions.', 4000.00, 6000.00, 'Kyiv', 'Full-time', 5, TRUE),
(1, 1, 'DevOps Engineer', 'Join our infrastructure team to build and maintain scalable cloud infrastructure.', 4500.00, 6500.00, 'Kyiv', 'Full-time', 4, TRUE),
(2, 1, 'Frontend Developer', 'Looking for a talented Frontend Developer with strong React skills.', 3000.00, 4500.00, 'Lviv', 'Full-time', 3, TRUE),
(2, 1, 'Backend Developer', 'Backend Developer position for building scalable microservices.', 3500.00, 5000.00, 'Lviv', 'Full-time', 3, TRUE),
(2, 1, 'Junior Full-Stack Developer', 'Great opportunity for junior developers to grow and learn.', 2000.00, 3000.00, 'Lviv', 'Full-time', 1, TRUE),
(3, 1, 'Data Engineer', 'Build data pipelines and analytics infrastructure.', 4000.00, 5500.00, 'Kharkiv', 'Full-time', 5, TRUE),
(3, 1, 'Python Developer', 'Python developer for data processing and automation.', 3500.00, 4800.00, 'Kharkiv', 'Remote', 3, TRUE),
(1, 2, 'Marketing Manager', 'Lead our marketing efforts and brand strategy.', 3000.00, 4000.00, 'Kyiv', 'Full-time', 5, TRUE),
(2, 4, 'UI/UX Designer', 'Create beautiful and intuitive user interfaces.', 2500.00, 4000.00, 'Lviv', 'Remote', 3, TRUE),
(3, 3, 'Sales Representative', 'Expand our client base and drive revenue growth.', 2000.00, 3500.00, 'Kharkiv', 'Full-time', 1, TRUE);

-- Insert 15 skill_tags
INSERT INTO skill_tags (Name, Category) VALUES
('C#', 'Programming Language'),
('.NET Core', 'Framework'),
('JavaScript', 'Programming Language'),
('TypeScript', 'Programming Language'),
('React', 'Framework'),
('Node.js', 'Runtime'),
('Python', 'Programming Language'),
('PostgreSQL', 'Database'),
('MySQL', 'Database'),
('MongoDB', 'Database'),
('Docker', 'DevOps'),
('Kubernetes', 'DevOps'),
('AWS', 'Cloud'),
('Azure', 'Cloud'),
('Git', 'Version Control');

-- Insert 20 job_skill_requirements (M:N relationship)
INSERT INTO job_skill_requirements (JobId, SkillTagId, RequiredLevel, IsRequired) VALUES
-- Senior Full-Stack Developer (job_id: 1)
(1, 1, 'Expert', TRUE),  -- C#
(1, 2, 'Expert', TRUE),  -- .NET Core
(1, 5, 'Advanced', TRUE),  -- React
(1, 8, 'Intermediate', FALSE), -- PostgreSQL
-- DevOps Engineer (job_id: 2)
(2, 11, 'Expert', TRUE), -- Docker
(2, 12, 'Advanced', TRUE), -- Kubernetes
(2, 13, 'Advanced', TRUE), -- AWS
(2, 15, 'Intermediate', TRUE), -- Git
-- Frontend Developer (job_id: 3)
(3, 3, 'Expert', TRUE),  -- JavaScript
(3, 4, 'Advanced', TRUE),  -- TypeScript
(3, 5, 'Expert', TRUE),  -- React
-- Backend Developer (job_id: 4)
(4, 6, 'Expert', TRUE),  -- Node.js
(4, 4, 'Intermediate', FALSE), -- TypeScript
(4, 10, 'Advanced', TRUE), -- MongoDB
-- Junior Full-Stack Developer (job_id: 5)
(5, 3, 'Intermediate', TRUE),  -- JavaScript
(5, 5, 'Beginner', FALSE), -- React
-- Data Engineer (job_id: 6)
(6, 7, 'Expert', TRUE),  -- Python
(6, 8, 'Advanced', TRUE),  -- PostgreSQL
(6, 11, 'Intermediate', FALSE), -- Docker
-- Python Developer (job_id: 7)
(7, 7, 'Expert', TRUE);  -- Python

-- ============================================
-- Create Indexes
-- ============================================
CREATE INDEX idx_jobs_company_id ON jobs(CompanyId);
CREATE INDEX idx_jobs_category_id ON jobs(CategoryId);
CREATE INDEX idx_jobs_is_active ON jobs(IsActive);
CREATE INDEX idx_jobs_posted_at ON jobs(PostedAt);

-- ============================================
-- Verification Queries
-- ============================================
SELECT 'Companies created:' as info, COUNT(*) as count FROM companies;
SELECT 'Company contacts created:' as info, COUNT(*) as count FROM company_contacts;
SELECT 'Job categories created:' as info, COUNT(*) as count FROM job_categories;
SELECT 'Jobs created:' as info, COUNT(*) as count FROM jobs;
SELECT 'Skill tags created:' as info, COUNT(*) as count FROM skill_tags;
SELECT 'Job skill requirements created:' as info, COUNT(*) as count FROM job_skill_requirements;
