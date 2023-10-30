namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Response.ValidateCIP
{
    public class PaymentInfo
    {
        public string recipient_document { get; set; }
        public string recipient_name { get; set; }
        public string payer_document { get; set; }
        public string payer_name { get; set; }
        public string due_date { get; set; }
        public string limit_date { get; set; }
        public decimal? min_value { get; set; }
        public decimal? max_value { get; set; }
        public decimal? fine_value { get; set; }
        public decimal? interest_value { get; set; }
        public decimal? original_value { get; set; }
        public decimal total_updated_value { get; set; }
        public decimal? total_discount_value { get; set; }
        public decimal? total_additional_value { get; set; }
    }
}
