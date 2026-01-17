namespace JobPortal.Review.Application.Common.Interfaces;

public interface IMongoIndexCreationService
{
    Task CreateIndexesAsync(CancellationToken cancellationToken = default);
}
