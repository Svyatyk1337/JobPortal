using System.Data;
using Dapper;
using JobPortal.Application.Dal.Interfaces;
using JobPortal.Application.Domain.Models;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace JobPortal.Application.Dal.Repositories;

/// <summary>
/// Interview Repository implemented using Dapper
/// Demonstrates async operations and query composition
/// </summary>
public class InterviewRepository : IInterviewRepository
{
    private readonly string _connectionString;
    private readonly ILogger<InterviewRepository> _logger;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    public InterviewRepository(string connectionString, ILogger<InterviewRepository> logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void SetTransaction(IDbConnection connection, IDbTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public async Task<Interview?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        const string sql = @"
            SELECT id, application_id, interview_type, round_number, scheduled_date,
                   status, notes, created_at
            FROM interviews
            WHERE id = @Id";

        return await connection.QuerySingleOrDefaultAsync<Interview>(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<Interview>> GetByApplicationIdAsync(int applicationId, CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        const string sql = @"
            SELECT id, application_id, interview_type, round_number, scheduled_date,
                   status, notes, created_at
            FROM interviews
            WHERE application_id = @ApplicationId
            ORDER BY round_number ASC";

        return await connection.QueryAsync<Interview>(
            new CommandDefinition(sql, new { ApplicationId = applicationId }, cancellationToken: cancellationToken));
    }

    public async Task<IEnumerable<Interview>> GetUpcomingInterviewsAsync(CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        const string sql = @"
            SELECT id, application_id, interview_type, round_number, scheduled_date,
                   status, notes, created_at
            FROM interviews
            WHERE scheduled_date >= @CurrentDate
              AND status IN ('Scheduled', 'Confirmed')
            ORDER BY scheduled_date ASC";

        return await connection.QueryAsync<Interview>(
            new CommandDefinition(sql, new { CurrentDate = DateTime.UtcNow }, cancellationToken: cancellationToken));
    }

    public async Task<int> CreateAsync(Interview interview, CancellationToken cancellationToken = default)
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
                INSERT INTO interviews
                    (application_id, interview_type, round_number, scheduled_date, status, notes, created_at)
                VALUES
                    (@ApplicationId, @InterviewType, @RoundNumber, @ScheduledDate, @Status, @Notes, @CreatedAt)
                RETURNING id";

            var parameters = new
            {
                interview.ApplicationId,
                interview.InterviewType,
                interview.RoundNumber,
                interview.ScheduledDate,
                Status = interview.Status ?? "Scheduled",
                interview.Notes,
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
                await ((NpgsqlConnection)connection).DisposeAsync();
            }
        }
    }

    public async Task<bool> UpdateAsync(Interview interview, CancellationToken cancellationToken = default)
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
                UPDATE interviews
                SET application_id = @ApplicationId,
                    interview_type = @InterviewType,
                    round_number = @RoundNumber,
                    scheduled_date = @ScheduledDate,
                    status = @Status,
                    notes = @Notes
                WHERE id = @Id";

            var rowsAffected = await connection.ExecuteAsync(
                new CommandDefinition(sql, interview, _transaction, cancellationToken: cancellationToken));

            return rowsAffected > 0;
        }
        finally
        {
            if (shouldCloseConnection && connection.State == ConnectionState.Open)
            {
                connection.Close();
                await ((NpgsqlConnection)connection).DisposeAsync();
            }
        }
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(_connectionString);

        const string sql = "DELETE FROM interviews WHERE id = @Id";

        var rowsAffected = await connection.ExecuteAsync(
            new CommandDefinition(sql, new { Id = id }, cancellationToken: cancellationToken));

        return rowsAffected > 0;
    }
}
