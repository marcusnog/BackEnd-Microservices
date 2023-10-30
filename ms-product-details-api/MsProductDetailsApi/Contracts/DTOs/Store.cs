using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsProductDetailsApi.Contracts.DTOs
{
    [BsonIgnoreExtraElements]
    public class Store
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string PartnerId { get; set; }
        public string Name { get; set; }
        public double CreationDate { get; set; }
        public double? DeletionDate { get; set; }
        public bool Active { get; set; }
        public bool AcceptCardPayment { get; set; }
    }
}
