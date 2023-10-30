using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Api.Contracts.DTOs
{
    [BsonIgnoreExtraElements]
    public class SkuRangePrice : Sku
    {
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
