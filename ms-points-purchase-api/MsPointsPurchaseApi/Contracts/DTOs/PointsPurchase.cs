using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MsPointsPurchaseApi.Contracts.DTOs
{
    public class PointsPurchase
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PointsId { get; set; }
        public string AccountId { get; set; }
        public decimal PointsValue { get; set; }
        public bool Active { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreationUserId { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public string? UpdatedUserId { get; set; }
    }
}
