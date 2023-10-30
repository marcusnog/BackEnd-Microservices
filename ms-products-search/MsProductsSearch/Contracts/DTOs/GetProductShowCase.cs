using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsProductsSearch.Contracts.DTOs
{
    public class GetProductShowCase
    {
        public string ProductId { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }

        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? ListPrice { get; set; }
        [BsonRepresentation(BsonType.Decimal128)]
        public decimal? SalePrice { get; set; }
        public string StoreId { get; set; }
        public string StoreName { get; set; }
        public bool Available { get; set; }
        public string Image { get; set; }

    }
}
