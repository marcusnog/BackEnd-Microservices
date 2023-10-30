using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Ms.Api.Utilities.Contracts.DTOs;

namespace MsProductDetailsApi.Contracts.DTOs
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

        public IEnumerable<T> Skus { get; set; }
        public List<Atribute_Generic>? Attributes { get; set; }

        public string Hash { get; set; }
    }

    public class ProductFilter
    {
        public string IdProduct { get; set; }
        public string? SkuId { get; set; }
        //public string DisplayName { get; set; }
        public decimal? FatorConversao { get; set; }
        public string? CEP { get; set; }
        public int? Quantity { get; set; }
        public string? StoreName { get; set; }
        public decimal? ValueInReals { get; set; }
    }
}
