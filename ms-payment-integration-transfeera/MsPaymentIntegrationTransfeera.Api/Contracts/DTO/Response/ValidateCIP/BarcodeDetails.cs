namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Response.ValidateCIP
{
    public class BarcodeDetails
    {
        public string bank_code { get; set; }
        public string bank_name { get; set; }
        public string barcode { get; set; }
        public string digitable_line { get; set; }
        public string due_date { get; set; }
        public decimal value { get; set; }
        public string type { get; set; }
    }
}
