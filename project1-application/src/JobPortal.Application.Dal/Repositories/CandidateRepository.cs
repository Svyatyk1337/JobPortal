using System.Data;
using JobPortal.Application.Dal.Interfaces;
using JobPortal.Application.Domain.Models;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace JobPortal.Application.Dal.Repositories;

/// <summary>
/// Candidate Repository implemented using pure ADO.NET
/// Demonstrates connection management, parameterization, and transaction handling
/// </summary>
public class CandidateRepository : ICandidateRepository
{
    private readonly string _connectionString;
    private readonly ILogger<CandidateRepository> _logger;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;

    public CandidateRepository(string connectionString, ILogger<CandidateRepository> logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public void SetTransaction(IDbConnection connection, IDbTransaction transaction)
    {
        _connection = connection;
        _transaction = transaction;
    }

    public async Task<Candidate?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new NpgsqlCommand(
            @"SELECT id, first_name, last_name, email, phone, years_of_experience, created_at
              FROM candidates
              WHERE id = @id",
            connection);

        command.Parameters.AddWithValue("@id", id);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
        {
            return MapToCandidate(reader);
        }

        return null;
    }

    public async Task<IEnumerable<Candidate>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var candidates = new List<Candidate>();

        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new NpgsqlCommand(
            @"SELECT id, first_name, last_name, email, phone, years_of_experience, created_at
              FROM candidates
              ORDER BY created_at DESC",
            connection);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            candidates.Add(MapToCandidate(reader));
        }

        return candidates;
    }

    public async Task<Candidate?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new NpgsqlCommand(
            @"SELECT id, first_name, last_name, email, phone, years_of_experience, created_at
              FROM candidates
              WHERE email = @email",
            connection);

        command.Parameters.AddWithValue("@email", email);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);

        if (await reader.ReadAsync(cancellationToken))
        {
            return MapToCandidate(reader);
        }

        return null;
    }

    public async Task<int> CreateAsync(Candidate candidate, CancellationToken cancellationToken = default)
    {
        var connection = _connection ?? new NpgsqlConnection(_connectionString);
        var shouldCloseConnection = _connection == null;

        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await ((NpgsqlConnection)connection).OpenAsync(cancellationToken);
            }

            await using var command = connection.CreateCommand();
            command.Transaction = _transaction;
            command.CommandText = @"
                INSERT INTO candidates (first_name, last_name, email, phone, years_of_experience, created_at)
                VALUES (@firstName, @lastName, @email, @phone, @yearsOfExperience, @createdAt)
                RETURNING id";

            AddParameter(command, "@firstName", candidate.FirstName);
            AddParameter(command, "@lastName", candidate.LastName);
            AddParameter(command, "@email", candidate.Email);
            AddParameter(command, "@phone", (object?)candidate.Phone ?? DBNull.Value);
            AddParameter(command, "@yearsOfExperience", candidate.YearsOfExperience);
            AddParameter(command, "@createdAt", DateTime.UtcNow);

            var result = await command.ExecuteScalarAsync(cancellationToken);
            return Convert.ToInt32(result);
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

    public async Task<bool> UpdateAsync(Candidate candidate, CancellationToken cancellationToken = default)
    {
        var connection = _connection ?? new NpgsqlConnection(_connectionString);
        var shouldCloseConnection = _connection == null;

        try
        {
            if (connection.State != ConnectionState.Open)
            {
                await ((NpgsqlConnection)connection).OpenAsync(cancellationToken);
            }

            await using var command = connection.CreateCommand();
            command.Transaction = _transaction;
            command.CommandText = @"
                UPDATE candidates
                SET first_name = @firstName,
                    last_name = @lastName,
                    email = @email,
                    phone = @phone,
                    years_of_experience = @yearsOfExperience
                WHERE id = @id";

            AddParameter(command, "@id", candidate.Id);
            AddParameter(command, "@firstName", candidate.FirstName);
            AddParameter(command, "@lastName", candidate.LastName);
            AddParameter(command, "@email", candidate.Email);
            AddParameter(command, "@phone", (object?)candidate.Phone ?? DBNull.Value);
            AddParameter(command, "@yearsOfExperience", candidate.YearsOfExperience);

            var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
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
        await connection.OpenAsync(cancellationToken);

        await using var command = new NpgsqlCommand(
            "DELETE FROM candidates WHERE id = @id",
            connection);

        command.Parameters.AddWithValue("@id", id);

        var rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
        return rowsAffected > 0;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        await using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        await using var command = new NpgsqlCommand(
            "SELECT EXISTS(SELECT 1 FROM candidates WHERE id = @id)",
            connection);

        command.Parameters.AddWithValue("@id", id);

        var result = await command.ExecuteScalarAsync(cancellationToken);
        return result != null && (bool)result;
    }

    private static Candidate MapToCandidate(NpgsqlDataReader reader)
    {
        return new Candidate
        {
            Id = reader.GetInt32(0),
            FirstName = reader.GetString(1),
            LastName = reader.GetString(2),
            Email = reader.GetString(3),
            Phone = reader.IsDBNull(4) ? null : reader.GetString(4),
            YearsOfExperience = reader.GetInt32(5),
            CreatedAt = reader.GetDateTime(6)
        };
    }

    private static void AddParameter(IDbCommand command, string name, object value)
    {
        var parameter = command.CreateParameter();
        parameter.ParameterName = name;
        parameter.Value = value;
        command.Parameters.Add(parameter);
    }
}
