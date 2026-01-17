using JobPortal.Review.Domain.ValueObjects;
using MongoDB.Bson.Serialization;

namespace JobPortal.Review.Infrastructure.Persistence.Serializers;

public static class MoneyClassMap
{
    public static void Register()
    {
        if (!BsonClassMap.IsClassMapRegistered(typeof(Money)))
        {
            BsonClassMap.RegisterClassMap<Money>(cm =>
            {
                cm.AutoMap();
                cm.MapMember(m => m.Amount);
                cm.MapMember(m => m.Currency);
                cm.MapCreator(m => Money.Create(m.Amount, m.Currency));
            });
        }
    }
}
