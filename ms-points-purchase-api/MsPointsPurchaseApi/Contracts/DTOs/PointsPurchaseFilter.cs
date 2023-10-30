using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MsPointsPurchaseApi.Contracts.DTOs
{
    public class PointsPurchaseFilter
    {
        public string UserId { get; set; }
        public string CampaignId { get; set; }
        public decimal PointsValue { get; set; }
    }
}
