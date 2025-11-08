using System.Data;
using JobPortal.Application.Dal.Interfaces;
using JobPortal.Application.Dal.Repositories;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace JobPortal.Application.Dal.UnitOfWork;

/// <summary>
/// Unit of Work pattern implementation
/// Manages database connection, transaction lifecycle, and coordinates multiple repositories
/// Ensures atomicity of operations across multiple repositories
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly string _connectionString;
    private readonly ILogger<UnitOfWork> _logger;
    private IDbConnection? _connection;
    private IDbTransaction? _transaction;
    private bool _disposed;

    private ICandidateRepository? _candidates;
    private IJobApplicationRepository? _jobApplications;
    private IInterviewRepository? _interviews;

    public UnitOfWork(string connectionString, ILogger<UnitOfWork> logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public ICandidateRepository Candidates
    {
        get
        {
            if (_candidates == null)
            {
                var logger = CreateLogger<CandidateRepository>();
                _candidates = new CandidateRepository(_connectionString, logger);

                if (_connection != null && _transaction != null)
                {
                    ((CandidateRepository)_candidates).SetTransaction(_connection, _transaction);
                }
            }
            return _candidates;
        }
    }

    public IJobApplicationRepository JobApplications
    {
        get
        {
            if (_jobApplications == null)
            {
                var logger = CreateLogger<JobApplicationRepository>();
                _jobApplications = new JobApplicationRepository(_connectionString, logger);

                if (_connection != null && _transaction != null)
                {
                    ((JobApplicationRepository)_jobApplications).SetTransaction(_connection, _transaction);
                }
            }
            return _jobApplications;
        }
    }

    public IInterviewRepository Interviews
    {
        get
        {
            if (_interviews == null)
            {
                var logger = CreateLogger<InterviewRepository>();
                _interviews = new InterviewRepository(_connectionString, logger);

                if (_connection != null && _transaction != null)
                {
                    ((InterviewRepository)_interviews).SetTransaction(_connection, _transaction);
                }
            }
            return _interviews;
        }
    }

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction != null)
        {
            throw new InvalidOperationException("Transaction is already started.");
        }

        _connection = new NpgsqlConnection(_connectionString);
        await ((NpgsqlConnection)_connection).OpenAsync(cancellationToken);

        // Use ReadCommitted isolation level by default
        // Can be changed to Serializable for stricter consistency requirements
        _transaction = _connection.BeginTransaction(IsolationLevel.ReadCommitted);

        _logger.LogDebug("Transaction started with isolation level: {IsolationLevel}",
            _transaction.IsolationLevel);

        // Set transaction for all existing repositories
        if (_candidates is CandidateRepository candidateRepo)
        {
            candidateRepo.SetTransaction(_connection, _transaction);
        }

        if (_jobApplications is JobApplicationRepository jobAppRepo)
        {
            jobAppRepo.SetTransaction(_connection, _transaction);
        }

        if (_interviews is InterviewRepository interviewRepo)
        {
            interviewRepo.SetTransaction(_connection, _transaction);
        }
    }

    public async Task CommitAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction to commit.");
        }

        try
        {
            _transaction.Commit();
            _logger.LogDebug("Transaction committed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error committing transaction");
            throw;
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;

            if (_connection != null)
            {
                _connection.Close();
                await ((NpgsqlConnection)_connection).DisposeAsync();
                _connection = null;
            }
        }
    }

    public async Task RollbackAsync(CancellationToken cancellationToken = default)
    {
        if (_transaction == null)
        {
            throw new InvalidOperationException("No transaction to rollback.");
        }

        try
        {
            _transaction.Rollback();
            _logger.LogWarning("Transaction rolled back");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rolling back transaction");
            throw;
        }
        finally
        {
            _transaction.Dispose();
            _transaction = null;

            if (_connection != null)
            {
                _connection.Close();
                await ((NpgsqlConnection)_connection).DisposeAsync();
                _connection = null;
            }
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _transaction?.Dispose();
                _connection?.Dispose();
            }

            _disposed = true;
        }
    }

    private ILogger<T> CreateLogger<T>()
    {
        // In a real application, this would use ILoggerFactory
        // For simplicity, we're creating a dummy logger
        return (ILogger<T>)new DummyLogger<T>();
    }

    // Simple logger implementation for repositories
    private class DummyLogger<T> : ILogger<T>
    {
        public IDisposable BeginScope<TState>(TState state) => null!;
        public bool IsEnabled(LogLevel logLevel) => false;
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state,
            Exception? exception, Func<TState, Exception?, string> formatter)
        {
        }
    }
}
