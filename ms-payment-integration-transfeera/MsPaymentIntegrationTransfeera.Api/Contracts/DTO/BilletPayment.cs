using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO
{
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
