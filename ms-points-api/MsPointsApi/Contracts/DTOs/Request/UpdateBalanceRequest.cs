using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MsPointsApi.Contracts.DTOs.Request
{
    public class UpdateBalanceRequest
    {
        public string UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
