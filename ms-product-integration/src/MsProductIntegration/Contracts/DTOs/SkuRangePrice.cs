using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsProductIntegration.Contracts.DTOs
{
    [BsonIgnoreExtraElements]
    public class SkuRangePrice : Sku
    {
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal MinPrice { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal MaxPrice { get; set; }
    }
}
