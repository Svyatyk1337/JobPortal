using JobPortal.Review.Domain.ValueObjects;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace JobPortal.Review.Infrastructure.Persistence.Serializers;

public class RatingSerializer : SerializerBase<Rating>
{
    public override Rating Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var type = context.Reader.GetCurrentBsonType();

        if (type == BsonType.Double)
        {
            var value = context.Reader.ReadDouble();
            return Rating.Create(value);
        }

        if (type == BsonType.Int32)
        {
            var value = context.Reader.ReadInt32();
            return Rating.Create(value);
        }

        throw new BsonSerializationException($"Cannot deserialize Rating from {type}");
    }

    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, Rating value)
    {
        context.Writer.WriteDouble(value.Value);
    }
}
