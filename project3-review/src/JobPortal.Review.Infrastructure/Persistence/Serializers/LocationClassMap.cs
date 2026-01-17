using JobPortal.Review.Domain.ValueObjects;
using MongoDB.Bson.Serialization;

namespace JobPortal.Review.Infrastructure.Persistence.Serializers;

public static class LocationClassMap
{
    public static void Register()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Location)))
        {
            BsonClassMap.RegisterClassMap<Location>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(l => l.City);
                cm.MapMember(l => l.Country);
                cm.MapCreator(l => Location.Create(l.City, l.Country));
            });
        }
    }
}
