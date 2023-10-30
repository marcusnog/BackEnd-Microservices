using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsPointsApi.Contracts.DTOs.Request
{
    public class CreateAccountRequest
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string UserId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string CampaignId { get; set; }
        public decimal Balance { get; set; }
    }
}
