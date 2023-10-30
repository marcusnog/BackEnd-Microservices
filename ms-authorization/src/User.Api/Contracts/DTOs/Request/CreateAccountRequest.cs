using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace User.Api.Contracts.DTOs.Request
{
    public class CreateAccountRequest
    {
        public string UserId { get; set; }
        public string Cpf { get; set; }
        public string CampaignId { get; set; }
        public decimal Balance { get; set; }
    }
}
