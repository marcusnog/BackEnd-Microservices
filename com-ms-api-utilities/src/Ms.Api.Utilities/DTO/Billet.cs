using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Ms.Api.Utilities.DTO
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

    public class BilletDetails
    {
        public string? BankCode { get; set; }
        public string BankName { get; set; }
        public string Barcode { get; set; }
        public string DigitableLine { get; set; }
        public string? DueDate { get; set; }
        public decimal? Value { get; set; }
        public string Type { get; set; }
    }

    public class BilletPayment
    {
        public string? RecipientDocument { get; set; }
        public string? RecipientName { get; set; }
        public string? PayerDocument { get; set; }
        public string? PayerName { get; set; }
        public string? DueDate { get; set; }
        public string? LimitDate { get; set; }
        public decimal? MinValue { get; set; }
        public decimal? MaxValue { get; set; }
        public decimal? FineValue { get; set; }
        public decimal? InterestValue { get; set; }
        public decimal OriginalValue { get; set; }
        public decimal TotalUpdatedValue { get; set; }
        public decimal? TotalDiscountValue { get; set; }
        public decimal? TotalAdditionalValue { get; set; }
    }

}
