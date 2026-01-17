using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JobPortal.Catalog.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Industry = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmployeeCount = table.Column<int>(type: "int", nullable: false),
                    Website = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "job_categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_categories", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "skill_tags",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Category = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_skill_tags", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "company_contacts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Phone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Address = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    City = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Country = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_company_contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_company_contacts_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "jobs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Description = table.Column<string>(type: "varchar(5000)", maxLength: 5000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CategoryId = table.Column<int>(type: "int", nullable: false),
                    SalaryMin = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SalaryMax = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Location = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmploymentType = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ExperienceYears = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    PostedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_jobs", x => x.Id);
                    table.CheckConstraint("CK_Jobs_Salary", "salary_max >= salary_min");
                    table.ForeignKey(
                        name: "FK_jobs_companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_jobs_job_categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "job_categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "job_skill_requirements",
                columns: table => new
                {
                    JobId = table.Column<int>(type: "int", nullable: false),
                    SkillTagId = table.Column<int>(type: "int", nullable: false),
                    RequiredLevel = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    IsRequired = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_job_skill_requirements", x => new { x.JobId, x.SkillTagId });
                    table.ForeignKey(
                        name: "FK_job_skill_requirements_jobs_JobId",
                        column: x => x.JobId,
                        principalTable: "jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_job_skill_requirements_skill_tags_SkillTagId",
                        column: x => x.SkillTagId,
                        principalTable: "skill_tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "companies",
                columns: new[] { "Id", "CreatedAt", "Description", "EmployeeCount", "Industry", "Name", "Website" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc).AddMonths(-12), "Leading software development company", 500, "Information Technology", "TechCorp Solutions", "https://techcorp.example.com" },
                    { 2, new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc).AddMonths(-8), "Data science and analytics consulting", 150, "Consulting", "DataMinds Inc", "https://dataminds.example.com" },
                    { 3, new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc).AddMonths(-6), "Cloud infrastructure and DevOps services", 300, "Cloud Services", "CloudNative Systems", "https://cloudnative.example.com" }
                });

            migrationBuilder.InsertData(
                table: "job_categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { 1, "Software engineering and development roles", "Software Development" },
                    { 2, "Data analysis and machine learning roles", "Data Science" },
                    { 3, "DevOps and infrastructure roles", "DevOps" },
                    { 4, "UI/UX and graphic design roles", "Design" }
                });

            migrationBuilder.InsertData(
                table: "skill_tags",
                columns: new[] { "Id", "Category", "Name" },
                values: new object[,]
                {
                    { 1, "Programming Language", "C#" },
                    { 2, "Framework", ".NET" },
                    { 3, "Programming Language", "Python" },
                    { 4, "Programming Language", "JavaScript" },
                    { 5, "Framework", "React" },
                    { 6, "Database", "SQL" },
                    { 7, "DevOps", "Docker" },
                    { 8, "DevOps", "Kubernetes" }
                });

            migrationBuilder.InsertData(
                table: "company_contacts",
                columns: new[] { "Id", "Address", "City", "CompanyId", "Country", "Email", "Phone" },
                values: new object[,]
                {
                    { 1, "123 Tech Street", "Kyiv", 1, "Ukraine", "hr@techcorp.example.com", "+380441234567" },
                    { 2, "456 Data Avenue", "Lviv", 2, "Ukraine", "careers@dataminds.example.com", "+380442345678" },
                    { 3, "789 Cloud Boulevard", "Kharkiv", 3, "Ukraine", "jobs@cloudnative.example.com", "+380443456789" }
                });

            migrationBuilder.InsertData(
                table: "jobs",
                columns: new[] { "Id", "CategoryId", "CompanyId", "Description", "EmploymentType", "ExperienceYears", "IsActive", "Location", "PostedAt", "SalaryMax", "SalaryMin", "Title" },
                values: new object[,]
                {
                    { 1, 1, 1, "We are looking for an experienced .NET developer to join our team.", "Full-time", 5, true, "Kyiv, Ukraine", new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc).AddDays(-10), 5000m, 3000m, "Senior .NET Developer" },
                    { 2, 2, 2, "Join our data science team to build ML models.", "Full-time", 3, true, "Lviv, Ukraine", new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc).AddDays(-7), 4500m, 2500m, "Data Scientist" },
                    { 3, 3, 3, "Looking for a DevOps engineer with Kubernetes experience.", "Full-time", 4, true, "Kharkiv, Ukraine", new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc).AddDays(-5), 4800m, 2800m, "DevOps Engineer" },
                    { 4, 1, 1, "Frontend developer position with React expertise.", "Full-time", 2, true, "Remote", new DateTime(2025, 1, 17, 0, 0, 0, 0, DateTimeKind.Utc).AddDays(-3), 3500m, 2000m, "React Developer" }
                });

            migrationBuilder.InsertData(
                table: "job_skill_requirements",
                columns: new[] { "JobId", "SkillTagId", "IsRequired", "RequiredLevel" },
                values: new object[,]
                {
                    { 1, 1, true, "Expert" },
                    { 1, 2, true, "Expert" },
                    { 1, 6, true, "Advanced" },
                    { 2, 3, true, "Expert" },
                    { 2, 6, true, "Advanced" },
                    { 3, 7, true, "Expert" },
                    { 3, 8, true, "Advanced" },
                    { 4, 4, true, "Expert" },
                    { 4, 5, true, "Expert" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_companies_Name",
                table: "companies",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_company_contacts_CompanyId",
                table: "company_contacts",
                column: "CompanyId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_job_categories_Name",
                table: "job_categories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_job_skill_requirements_SkillTagId",
                table: "job_skill_requirements",
                column: "SkillTagId");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_CategoryId",
                table: "jobs",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_CompanyId",
                table: "jobs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_IsActive",
                table: "jobs",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_jobs_PostedAt",
                table: "jobs",
                column: "PostedAt");

            migrationBuilder.CreateIndex(
                name: "IX_skill_tags_Name",
                table: "skill_tags",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "company_contacts");

            migrationBuilder.DropTable(
                name: "job_skill_requirements");

            migrationBuilder.DropTable(
                name: "jobs");

            migrationBuilder.DropTable(
                name: "skill_tags");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "job_categories");
        }
    }
}
