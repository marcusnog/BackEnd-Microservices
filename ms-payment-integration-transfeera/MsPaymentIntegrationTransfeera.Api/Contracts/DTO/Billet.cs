using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO
{
    public class Billet
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public string ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public string Email { get; set; }
        public string CampaignId { get; set; }
        public decimal BilletValue { get; set; }
        public decimal BilletFeeValue { get; set; }
        public decimal BilletPointsValue { get; set; }
        public string? TransfeeraTransactionId { get; set; }
        public string? StatusTransfeera { get; set; }
        public double? CreationDate { get; set; }
        public string? ErrorMessage { get; set; }
        public BilletDetails BilletDetails { get; set; }
        public BilletPayment BilletPaymentInfos { get; set; }
    }
}
