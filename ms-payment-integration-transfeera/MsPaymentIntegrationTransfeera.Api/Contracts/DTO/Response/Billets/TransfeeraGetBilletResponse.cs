namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Response.Billets
{
    public class TransfeeraGetBilletResponse
    {
        public string id { get; set; }
        public decimal value { get; set; }
        public string barcode { get; set; }
        public string description { get; set; }
        public string due_date { get; set; }
        public string payment_date { get; set; }
        public string status { get; set; }
        public string status_description { get; set; }
        public string created_at { get; set; }
        public string integration_id { get; set; }
        public string batch_id { get; set; }
        public string receipt_url { get; set; }
        public string bank_receipt_url { get; set; }
        public string paid_date { get; set; }
        public string returned_date { get; set; }
        public string finish_date { get; set; }
        public string bank_auth_code { get; set; }
    }
}
