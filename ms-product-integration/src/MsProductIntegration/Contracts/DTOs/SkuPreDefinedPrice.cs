using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsProductIntegration.Contracts.DTOs
{
    [BsonIgnoreExtraElements]
    public class SkuPreDefinedPrice : Sku
    {
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal ListPrice { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal SalePrice { get; set; }
    }
}
