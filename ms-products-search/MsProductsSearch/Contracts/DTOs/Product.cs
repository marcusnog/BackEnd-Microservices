using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Ms.Api.Utilities.Contracts.DTOs;

namespace MsProductsSearch.Contracts.DTOs
{
    public class Product<T>
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string CategoryId { get; set; }
        public double CreationDate { get; set; }
        public double ModificationDate { get; set; }
        public double? DeletionDate { get; set; }
        public bool Active { get; set; }
        public IEnumerable<ProductImage>? Images { get; set; }

        // external key
        [BsonRepresentation(BsonType.ObjectId)]
        public string StoreId { get; set; }
        public string StoreName { get; set; }
        public string StoreItemCode { get; set; }

        public List<T>? Skus { get; set; }
        public List<Atribute_Generic>? Attributes { get; set; }
        public string Hash { get; set; }
    }

    public class ProductFilter
    {
        public string? ProductName { get; set; }
        public string? SkuCode { get; set; }
        public decimal? StartValue { get; set; }
        public decimal? EndValue { get; set; }
        public string? StoreId { get; set; }
        public string? Category { get; set; }
    }
}
