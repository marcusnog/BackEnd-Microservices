using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace User.Api.Contracts.DTOs
{
    public class PointsPurchase
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PointsId { get; set; }
        public string AccountId { get; set; }
        public decimal PointsValue { get; set; }
        public bool Active { get; set; }
        public bool Enabled { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreationUserId { get; set; }
        public DateTime? DistributedAt { get; set; }
        public string? DistributionUserId { get; set; }
    }
}
