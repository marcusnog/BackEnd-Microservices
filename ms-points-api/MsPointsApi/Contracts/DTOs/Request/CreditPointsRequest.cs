using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsPointsApi.Contracts.DTOs.Request
{
    public class CreditPointsRequest
    {
        public string AccountId { get; set; }
        public decimal Value { get; set; }
        public string? Description { get; set; }
    }
}
