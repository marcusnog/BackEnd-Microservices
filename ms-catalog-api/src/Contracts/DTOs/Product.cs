using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Ms.Api.Utilities.Contracts.DTOs;

namespace Catalog.Api.Contracts.DTOs
{
    [BsonIgnoreExtraElements]
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
        public string StoreItemCode { get; set; }

        public List<T> Skus { get; set; }
        public List<Atribute_Generic>? Attributes { get; set; }
        public string Hash { get; set; }

    }

    public class ProductFilter
    {
        public string? ProductName { get; set; }
        public string? SkuCode { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public decimal StartValue { get; set; }
        public decimal EndValue { get; set; }
        public List<string>? StoreList { get; set; }
        public List<string>? CategoryList { get; set; }
        public TypeSearch typeSearch { get; set; }
        public decimal? Value { get; set; }

        public enum TypeSearch
        {
            WithinPrice,
            OnSale,
            BestSeller
        }
    }


    public class Promotion
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public double StartDate { get; set; }
        public double EndDate { get; set; }
        public string WebBanner { get; set; }
        public string MobileBanner { get; set; }
        public double CreationDate { get; set; }
        public double ModificationDate { get; set; }
        public double? DeletionDate { get; set; }
        public bool Active { get; set; }
        public IEnumerable<Product<SkuPreDefinedPrice>> Products { get; set; }
        public IEnumerable<Campaign> Campaigns { get; set; }
    }

    public class PromotionFilter
    {
        public string Id { get; set; }
        public string SkuId { get; set; }
    }

    public class Campaign
    {
        public string CampaignId { get; set; }

        public bool Active { get; set; }
    }

}
