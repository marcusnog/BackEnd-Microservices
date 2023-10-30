using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Ms.Api.Utilities.Contracts.DTOs;

namespace MsProductsSearch.Contracts.DTOs
{
    [BsonIgnoreExtraElements]
    public class Sku
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string StoreItemCode { get; set; }
        public double CreationDate { get; set; }
        public double ModificationDate { get; set; }
        public double? DeletionDate { get; set; }
        public bool Active { get; set; }
        public List<Atribute_Generic>? Attributes { get; set; }
        public string[] Tags { get; set; }
        public string Hash { get; set; }
        public bool Availability { get; set; }
        public decimal ValuePoints { get; set; }
        public string? Model { get; set; }
    }
}
