using System.Data;
using Dapper;
using JobPortal.Application.Dal.Interfaces;
using JobPortal.Application.Domain.Models;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace JobPortal.Application.Dal.Repositories;

/// <summary>
/// JobApplication Repository implemented using Dapper (micro-ORM)
/// Demonstrates async operations, multi-mapping, and compact queries
/// </summary>
public class JobApplicationRepository : IJobApplicationRepository
{
    private readonly string _connectionString;
    private readonly ILogger<JobApplicationRepository> _logger;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    public JobApplicationRepository(string connectionString, ILogger<JobApplicationRepository> logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void SetTransaction(IDbConnection connection, IDbTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public async Task<JobApplication?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        const string sql = @"
            SELECT id, candidate_id, job_id, job_title, company_name, status,
                   submitted_date, expected_salary, created_at
            FROM job_applications
            WHERE id = @Id";

        return await connection.QuerySingleOrDefaultAsync<JobApplication>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<JobApplication?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        // Multi-mapping: JobApplication + ApplicationDetails + Candidate
        const string sql = @"
            SELECT
                ja.id, ja.candidate_id, ja.job_id, ja.job_title, ja.company_name,
                ja.status, ja.submitted_date, ja.expected_salary, ja.created_at,
                ad.id, ad.application_id, ad.resume_url, ad.cover_letter, ad.created_at,
                c.id, c.first_name, c.last_name, c.email, c.phone,
                c.years_of_experience, c.created_at
            FROM job_applications ja
            LEFT JOIN application_details ad ON ja.id = ad.application_id
            LEFT JOIN candidates c ON ja.candidate_id = c.id
            WHERE ja.id = @Id";

        var result = await connection.QueryAsync<JobApplication, ApplicationDetails?, Candidate?, JobApplication>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken),
            (jobApp, details, candidate) =>
            {
                jobApp.ApplicationDetails = details;
                jobApp.Candidate = candidate;
                return jobApp;
            },
            splitOn: "id,id");

        return result.FirstOrDefault();
    }

    public async Task<IEnumerable<JobApplication>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        const string sql = @"
            SELECT id, candidate_id, job_id, job_title, company_name, status,
                   submitted_date, expected_salary, created_at
            FROM job_applications
            ORDER BY created_at DESC";

        return await connection.QueryAsync<JobApplication>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<JobApplication>> GetByCandidateIdAsync(int candidateId, CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        const string sql = @"
            SELECT id, candidate_id, job_id, job_title, company_name, status,
                   submitted_date, expected_salary, created_at
            FROM job_applications
            WHERE candidate_id = @CandidateId
            ORDER BY submitted_date DESC";

        return await connection.QueryAsync<JobApplication>(
            new CommandDefinition(sql, new { CandidateId = candidateId }, cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<JobApplication>> GetByStatusAsync(string status, CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        const string sql = @"
            SELECT id, candidate_id, job_id, job_title, company_name, status,
                   submitted_date, expected_salary, created_at
            FROM job_applications
            WHERE status = @Status
            ORDER BY submitted_date DESC";

        return await connection.QueryAsync<JobApplication>(
            new CommandDefinition(sql, new { Status = status }, cancellationToken: cancellationToken));
    }

    public async Task<int> CreateAsync(JobApplication application, CancellationToken cancellationToken = default)
    {
        var connection = _connection ?? new NpgsqlConnection(_connectionString);
        var shouldCloseConnection = _connection == null;

        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await ((NpgsqlConnection)connection).OpenAsync(cancellationToken);
            }

            const string sql = @"
                INSERT INTO job_applications
                    (candidate_id, job_id, job_title, company_name, status, submitted_date, expected_salary, created_at)
                VALUES
                    (@CandidateId, @JobId, @JobTitle, @CompanyName, @Status, @SubmittedDate, @ExpectedSalary, @CreatedAt)
                RETURNING id";

            var parameters = new
            {
                application.CandidateId,
                application.JobId,
                application.JobTitle,
                application.CompanyName,
                Status = application.Status ?? "Pending",
                application.SubmittedDate,
                application.ExpectedSalary,
                CreatedAt = DateTime.UtcNow
            };

            return await connection.ExecuteScalarAsync<int>(
                new CommandDefinition(sql, parameters, _transaction, cancellationToken: cancellationToken));
        }
        finally
        {
            if (shouldCloseConnection && connection.State == ConnectionState.Open)
            {
                connection.Close();
                await connection.DisposeAsync();
            }
        }
    }

    public async Task<bool> UpdateAsync(JobApplication application, CancellationToken cancellationToken = default)
    {
        var connection = _connection ?? new NpgsqlConnection(_connectionString);
        var shouldCloseConnection = _connection == null;

        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await ((NpgsqlConnection)connection).OpenAsync(cancellationToken);
            }

            const string sql = @"
                UPDATE job_applications
                SET candidate_id = @CandidateId,
                    job_id = @JobId,
                    job_title = @JobTitle,
                    company_name = @CompanyName,
                    status = @Status,
                    submitted_date = @SubmittedDate,
                    expected_salary = @ExpectedSalary
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(
                new CommandDefinition(sql, application, _transaction, cancellationToken: cancellationToken));

            return rowsAffected > 0;
        }
        finally
        {
            if (shouldCloseConnection && connection.State == ConnectionState.Open)
            {
                connection.Close();
                await connection.DisposeAsync();
            }
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        const string sql = "DELETE FROM job_applications WHERE id = @Id";

        var rowsAffected = await connection.ExecuteAsync(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));

        return rowsAffected > 0;
    }
}
