using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Api.Contracts.DTOs
{
    [BsonIgnoreExtraElements]
    public class SkuPreDefinedPrice : Sku
    {
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal ListPrice { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal SalePrice { get; set; }
    }

    public class SkuFilterPrice
    {
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal Price { get; set; }
    }
}
