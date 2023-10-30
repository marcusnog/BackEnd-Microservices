using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Request
{
    public class BilletFilterRequest
    {
        public string? BilletId { get; set; }
    }

    public class BilletRequest
    {
        public string? BilletId { get; set; }
        public string ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public string Email { get; set; }
        public string CampaignId { get; set; }
        public decimal BilletValue { get; set; }
        public decimal BilletFeeValue { get; set; }
        public decimal BilletPointsValue { get; set; }
        public BilletDetailsRequest BilletDetails { get; set; }
        public BilletPaymentRequest BilletPaymentInfos { get; set; }
        public string token { get; set; }
        public string environment { get; set; }
        public string campaign { get; set; }
    }

    public class BilletDetailsRequest
    {
        public string? BankCode { get; set; }
        public string BankName { get; set; }
        public string Barcode { get; set; }
        public string DigitableLine { get; set; }
        public string? DueDate { get; set; }
        public decimal? Value { get; set; }
        public string Type { get; set; }
    }

    public class BilletPaymentRequest
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
