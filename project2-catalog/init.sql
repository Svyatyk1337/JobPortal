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
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(200) NOT NULL,
    description TEXT,
    industry VARCHAR(100),
    size VARCHAR(50),
    website VARCHAR(255),
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ============================================
-- Table: company_contacts (1:1 with companies)
-- ============================================
CREATE TABLE company_contacts (
    id INT AUTO_INCREMENT PRIMARY KEY,
    company_id INT UNIQUE NOT NULL,
    email VARCHAR(255) NOT NULL,
    phone VARCHAR(20),
    city VARCHAR(100),
    country VARCHAR(100),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (company_id) REFERENCES companies(id) ON DELETE CASCADE
);

-- ============================================
-- Table: job_categories
-- ============================================
CREATE TABLE job_categories (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ============================================
-- Table: jobs
-- ============================================
CREATE TABLE jobs (
    id INT AUTO_INCREMENT PRIMARY KEY,
    company_id INT NOT NULL,
    category_id INT,
    title VARCHAR(200) NOT NULL,
    description TEXT,
    salary_from DECIMAL(10, 2),
    salary_to DECIMAL(10, 2),
    location VARCHAR(200),
    is_remote BOOLEAN DEFAULT FALSE,
    experience_level VARCHAR(50),
    is_active BOOLEAN DEFAULT TRUE,
    posted_date DATE NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (company_id) REFERENCES companies(id) ON DELETE CASCADE,
    FOREIGN KEY (category_id) REFERENCES job_categories(id) ON DELETE SET NULL,
    CHECK (salary_from > 0),
    CHECK (salary_to >= salary_from)
);

-- ============================================
-- Table: skill_tags
-- ============================================
CREATE TABLE skill_tags (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(100) NOT NULL UNIQUE,
    category VARCHAR(100),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ============================================
-- Table: job_skill_requirements (M:N relationship)
-- ============================================
CREATE TABLE job_skill_requirements (
    id INT AUTO_INCREMENT PRIMARY KEY,
    job_id INT NOT NULL,
    skill_tag_id INT NOT NULL,
    level ENUM('Required', 'Preferred') DEFAULT 'Required',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (job_id) REFERENCES jobs(id) ON DELETE CASCADE,
    FOREIGN KEY (skill_tag_id) REFERENCES skill_tags(id) ON DELETE CASCADE,
    UNIQUE KEY unique_job_skill (job_id, skill_tag_id)
);

-- ============================================
-- Insert Test Data
-- ============================================

-- Insert 3 companies
INSERT INTO companies (name, description, industry, size, website, is_active) VALUES
('TechCorp', 'Leading technology company specializing in cloud solutions and enterprise software', 'Information Technology', '500-1000', 'https://techcorp.example.com', TRUE),
('InnoSoft', 'Innovative software development company focused on mobile and web applications', 'Software Development', '100-500', 'https://innosoft.example.com', TRUE),
('DataVision', 'Data analytics and business intelligence solutions provider', 'Data Analytics', '50-100', 'https://datavision.example.com', TRUE);

-- Insert 3 company_contacts (1:1 relationship)
INSERT INTO company_contacts (company_id, email, phone, city, country) VALUES
(1, 'careers@techcorp.example.com', '+380443456789', 'Kyiv', 'Ukraine'),
(2, 'hr@innosoft.example.com', '+380445678901', 'Lviv', 'Ukraine'),
(3, 'jobs@datavision.example.com', '+380447890123', 'Kharkiv', 'Ukraine');

-- Insert 5 job_categories
INSERT INTO job_categories (name, description) VALUES
('IT', 'Information Technology and Software Development positions'),
('Marketing', 'Marketing, PR, and Communications roles'),
('Sales', 'Sales and Business Development opportunities'),
('Design', 'UI/UX Design and Creative positions'),
('HR', 'Human Resources and Talent Management roles');

-- Insert 10 jobs
INSERT INTO jobs (company_id, category_id, title, description, salary_from, salary_to, location, is_remote, experience_level, is_active, posted_date) VALUES
(1, 1, 'Senior Full-Stack Developer', 'We are looking for an experienced Full-Stack Developer to join our team. You will work on cutting-edge cloud solutions.', 4000.00, 6000.00, 'Kyiv', TRUE, 'Senior', TRUE, '2025-01-10'),
(1, 1, 'DevOps Engineer', 'Join our infrastructure team to build and maintain scalable cloud infrastructure.', 4500.00, 6500.00, 'Kyiv', TRUE, 'Middle', TRUE, '2025-01-12'),
(2, 1, 'Frontend Developer', 'Looking for a talented Frontend Developer with strong React skills.', 3000.00, 4500.00, 'Lviv', FALSE, 'Middle', TRUE, '2025-01-08'),
(2, 1, 'Backend Developer', 'Backend Developer position for building scalable microservices.', 3500.00, 5000.00, 'Lviv', TRUE, 'Middle', TRUE, '2025-01-15'),
(2, 1, 'Junior Full-Stack Developer', 'Great opportunity for junior developers to grow and learn.', 2000.00, 3000.00, 'Lviv', FALSE, 'Junior', TRUE, '2025-01-05'),
(3, 1, 'Data Engineer', 'Build data pipelines and analytics infrastructure.', 4000.00, 5500.00, 'Kharkiv', TRUE, 'Senior', TRUE, '2025-01-18'),
(3, 1, 'Python Developer', 'Python developer for data processing and automation.', 3500.00, 4800.00, 'Kharkiv', TRUE, 'Middle', TRUE, '2025-01-20'),
(1, 2, 'Marketing Manager', 'Lead our marketing efforts and brand strategy.', 3000.00, 4000.00, 'Kyiv', FALSE, 'Senior', TRUE, '2025-01-14'),
(2, 4, 'UI/UX Designer', 'Create beautiful and intuitive user interfaces.', 2500.00, 4000.00, 'Lviv', TRUE, 'Middle', TRUE, '2025-01-16'),
(3, 3, 'Sales Representative', 'Expand our client base and drive revenue growth.', 2000.00, 3500.00, 'Kharkiv', FALSE, 'Junior', TRUE, '2025-01-22');

-- Insert 15 skill_tags
INSERT INTO skill_tags (name, category) VALUES
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
INSERT INTO job_skill_requirements (job_id, skill_tag_id, level) VALUES
-- Senior Full-Stack Developer (job_id: 1)
(1, 1, 'Required'),  -- C#
(1, 2, 'Required'),  -- .NET Core
(1, 5, 'Required'),  -- React
(1, 8, 'Preferred'), -- PostgreSQL
-- DevOps Engineer (job_id: 2)
(2, 11, 'Required'), -- Docker
(2, 12, 'Required'), -- Kubernetes
(2, 13, 'Required'), -- AWS
(2, 15, 'Required'), -- Git
-- Frontend Developer (job_id: 3)
(3, 3, 'Required'),  -- JavaScript
(3, 4, 'Required'),  -- TypeScript
(3, 5, 'Required'),  -- React
-- Backend Developer (job_id: 4)
(4, 6, 'Required'),  -- Node.js
(4, 4, 'Preferred'), -- TypeScript
(4, 10, 'Required'), -- MongoDB
-- Junior Full-Stack Developer (job_id: 5)
(5, 3, 'Required'),  -- JavaScript
(5, 5, 'Preferred'), -- React
-- Data Engineer (job_id: 6)
(6, 7, 'Required'),  -- Python
(6, 8, 'Required'),  -- PostgreSQL
(6, 11, 'Preferred'), -- Docker
-- Python Developer (job_id: 7)
(7, 7, 'Required');  -- Python

-- ============================================
-- Create Indexes
-- ============================================
CREATE INDEX idx_jobs_company_id ON jobs(company_id);
CREATE INDEX idx_jobs_category_id ON jobs(category_id);
CREATE INDEX idx_jobs_is_active ON jobs(is_active);
CREATE INDEX idx_jobs_posted_date ON jobs(posted_date);
CREATE INDEX idx_job_skill_requirements_job_id ON job_skill_requirements(job_id);
CREATE INDEX idx_job_skill_requirements_skill_tag_id ON job_skill_requirements(skill_tag_id);

-- ============================================
-- Verification Queries
-- ============================================
SELECT 'Companies created:' as info, COUNT(*) as count FROM companies;
SELECT 'Company contacts created:' as info, COUNT(*) as count FROM company_contacts;
SELECT 'Job categories created:' as info, COUNT(*) as count FROM job_categories;
SELECT 'Jobs created:' as info, COUNT(*) as count FROM jobs;
SELECT 'Skill tags created:' as info, COUNT(*) as count FROM skill_tags;
SELECT 'Job skill requirements created:' as info, COUNT(*) as count FROM job_skill_requirements;
