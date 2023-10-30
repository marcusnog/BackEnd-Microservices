using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Catalog.Api.Contracts.DTOs
{
    [BsonIgnoreExtraElements]
    public class Sku
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string StoreItemCode { get; set; }
        //public string StoreCampaignCode { get; set; }
        public double CreationDate { get; set; }
        public double ModificationDate { get; set; }
        public double? DeletionDate { get; set; }
        public bool Active { get; set; }
        public Dictionary<string, string> Attributes { get; set; } = new();
        public string[] Tags { get; set; }
        public string Hash { get; set; }
        public bool Availability { get; set; }
    }
}
