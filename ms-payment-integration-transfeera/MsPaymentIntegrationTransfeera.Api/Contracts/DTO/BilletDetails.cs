using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO
{
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
}
