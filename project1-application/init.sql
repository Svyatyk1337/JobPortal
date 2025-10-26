-- ============================================
-- Project 1: Application Service (PostgreSQL)
-- ============================================

-- Drop tables if exist
DROP TABLE IF EXISTS application_skills CASCADE;
DROP TABLE IF EXISTS interviews CASCADE;
DROP TABLE IF EXISTS application_details CASCADE;
DROP TABLE IF EXISTS job_applications CASCADE;
DROP TABLE IF EXISTS candidates CASCADE;

-- ============================================
-- Table: candidates
-- ============================================
CREATE TABLE candidates (
    id SERIAL PRIMARY KEY,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    email VARCHAR(255) UNIQUE NOT NULL,
    phone VARCHAR(20),
    years_of_experience INTEGER DEFAULT 0,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- ============================================
-- Table: job_applications
-- ============================================
CREATE TABLE job_applications (
    id SERIAL PRIMARY KEY,
    candidate_id INTEGER NOT NULL,
    job_id INTEGER NOT NULL,
    job_title VARCHAR(200) NOT NULL,
    company_name VARCHAR(200) NOT NULL,
    status VARCHAR(50) DEFAULT 'Pending',
    submitted_date DATE NOT NULL,
    expected_salary DECIMAL(10, 2),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (candidate_id) REFERENCES candidates(id) ON DELETE CASCADE
);

-- ============================================
-- Table: application_details (1:1 with job_applications)
-- ============================================
CREATE TABLE application_details (
    id SERIAL PRIMARY KEY,
    application_id INTEGER UNIQUE NOT NULL,
    resume_url VARCHAR(500),
    cover_letter TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (application_id) REFERENCES job_applications(id) ON DELETE CASCADE
);

-- ============================================
-- Table: interviews
-- ============================================
CREATE TABLE interviews (
    id SERIAL PRIMARY KEY,
    application_id INTEGER NOT NULL,
    interview_type VARCHAR(100) NOT NULL,
    round_number INTEGER DEFAULT 1,
    scheduled_date TIMESTAMP NOT NULL,
    status VARCHAR(50) DEFAULT 'Scheduled',
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (application_id) REFERENCES job_applications(id) ON DELETE CASCADE
);

-- ============================================
-- Table: application_skills (M:N relationship)
-- ============================================
CREATE TABLE application_skills (
    id SERIAL PRIMARY KEY,
    application_id INTEGER NOT NULL,
    skill_name VARCHAR(100) NOT NULL,
    proficiency_level VARCHAR(50) NOT NULL,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (application_id) REFERENCES job_applications(id) ON DELETE CASCADE
);

-- ============================================
-- Insert Test Data
-- ============================================

-- Insert 4 candidates
INSERT INTO candidates (first_name, last_name, email, phone, years_of_experience) VALUES
('Ivan', 'Kovalenko', 'ivan.kovalenko@example.com', '+380501234567', 5),
('Olena', 'Shevchenko', 'olena.shevchenko@example.com', '+380507654321', 3),
('Dmytro', 'Bondarenko', 'dmytro.bondarenko@example.com', '+380502345678', 7),
('Kateryna', 'Moroz', 'kateryna.moroz@example.com', '+380508765432', 2);

-- Insert 5 job applications
INSERT INTO job_applications (candidate_id, job_id, job_title, company_name, status, submitted_date, expected_salary) VALUES
(1, 101, 'Senior Full-Stack Developer', 'TechCorp', 'Interview', '2025-01-15', 4500.00),
(1, 102, 'Backend Developer', 'InnoSoft', 'Pending', '2025-01-18', 4000.00),
(2, 103, 'Frontend Developer', 'DataVision', 'Offer', '2025-01-10', 3500.00),
(3, 104, 'DevOps Engineer', 'TechCorp', 'Interview', '2025-01-12', 5000.00),
(4, 105, 'Junior Full-Stack Developer', 'InnoSoft', 'Rejected', '2025-01-08', 2500.00);

-- Insert 5 application_details (1:1 relationship)
INSERT INTO application_details (application_id, resume_url, cover_letter) VALUES
(1, 'https://storage.example.com/resumes/ivan_kovalenko.pdf', 'I am excited to apply for the Senior Full-Stack Developer position at TechCorp...'),
(2, 'https://storage.example.com/resumes/ivan_kovalenko.pdf', 'I believe my skills align perfectly with the Backend Developer role...'),
(3, 'https://storage.example.com/resumes/olena_shevchenko.pdf', 'As a passionate Frontend Developer with 3 years of experience...'),
(4, 'https://storage.example.com/resumes/dmytro_bondarenko.pdf', 'With 7 years in DevOps and cloud infrastructure...'),
(5, 'https://storage.example.com/resumes/kateryna_moroz.pdf', 'I am eager to start my career as a Junior Developer...');

-- Insert 6 interviews
INSERT INTO interviews (application_id, interview_type, round_number, scheduled_date, status, notes) VALUES
(1, 'HR Screening', 1, '2025-01-20 10:00:00', 'Completed', 'Candidate performed well, moving to technical round'),
(1, 'Technical Interview', 2, '2025-01-25 14:00:00', 'Scheduled', NULL),
(3, 'HR Screening', 1, '2025-01-12 11:00:00', 'Completed', 'Excellent communication skills'),
(3, 'Technical Interview', 2, '2025-01-16 15:00:00', 'Completed', 'Strong React and TypeScript knowledge'),
(3, 'Final Interview', 3, '2025-01-22 10:00:00', 'Completed', 'Offer extended'),
(4, 'Technical Interview', 1, '2025-01-18 13:00:00', 'Scheduled', NULL);

-- Insert 10 application_skills
INSERT INTO application_skills (application_id, skill_name, proficiency_level) VALUES
(1, 'C#', 'Expert'),
(1, 'JavaScript', 'Advanced'),
(1, 'React', 'Advanced'),
(1, 'PostgreSQL', 'Advanced'),
(2, 'Node.js', 'Advanced'),
(2, 'MongoDB', 'Intermediate'),
(3, 'React', 'Expert'),
(3, 'TypeScript', 'Advanced'),
(3, 'HTML/CSS', 'Expert'),
(4, 'Docker', 'Expert'),
(4, 'Kubernetes', 'Advanced'),
(4, 'AWS', 'Advanced'),
(5, 'JavaScript', 'Intermediate'),
(5, 'React', 'Beginner');

-- ============================================
-- Create Indexes
-- ============================================
CREATE INDEX idx_job_applications_candidate_id ON job_applications(candidate_id);
CREATE INDEX idx_job_applications_status ON job_applications(status);
CREATE INDEX idx_interviews_application_id ON interviews(application_id);
CREATE INDEX idx_application_skills_application_id ON application_skills(application_id);

-- ============================================
-- Verification Queries
-- ============================================
SELECT 'Candidates created:' as info, COUNT(*) as count FROM candidates;
SELECT 'Job applications created:' as info, COUNT(*) as count FROM job_applications;
SELECT 'Application details created:' as info, COUNT(*) as count FROM application_details;
SELECT 'Interviews created:' as info, COUNT(*) as count FROM interviews;
SELECT 'Application skills created:' as info, COUNT(*) as count FROM application_skills;
