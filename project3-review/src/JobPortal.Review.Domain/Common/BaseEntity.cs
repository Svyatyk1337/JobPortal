using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace JobPortal.Review.Domain.Common;

public abstract class BaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; protected set; } = ObjectId.GenerateNewId().ToString();

    [BsonElement("created_at")]
    public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;

    [BsonElement("updated_at")]
    public DateTime? UpdatedAt { get; protected set; }

    protected void MarkAsUpdated()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}
