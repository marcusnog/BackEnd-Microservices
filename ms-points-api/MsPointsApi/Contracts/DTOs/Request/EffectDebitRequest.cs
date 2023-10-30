using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsPointsApi.Contracts.DTOs.Request
{
    public class EffectDebitRequest
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string? ReleaseCode { get; set; }
        public string OrderNumber { get; set; }
    }

    public class EffectDebitAdminRequest
    {
        public string AccountId { get; set; }
        public string CampaignId { get; set; }
        public decimal Value { get; set; }
    }
}
