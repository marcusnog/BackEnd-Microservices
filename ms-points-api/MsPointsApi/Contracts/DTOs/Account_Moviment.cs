using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsPointsApi.Contracts.DTOs
{
    public class Account_Moviment
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string AccountId { get; set; }
        public string Type { get; set; }
        public decimal Value { get; set; }
        public bool Active { get; set; }
        public DateTime CreatedAt { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? CreationUserId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? UpdatedUserId { get; set; }
        public DateTime? DeletedAt { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string? DeletionUserId { get; set; }
        public string? Description { get; set; }
        public string? OrderNumber { get; set; }
        public string? Status { get; set; }
    }
}
