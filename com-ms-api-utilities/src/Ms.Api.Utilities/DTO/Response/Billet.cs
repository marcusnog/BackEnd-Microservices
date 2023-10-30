namespace Ms.Api.Utilities.DTO.Response
{
    public class TransfeeraGetBank
    {
        public string id { get; set; }
        public string name { get; set; }
        public string image { get; set; }
    }

    public class TransfeeraGetBatchResponse
    {
        public int id { get; set; }
        public decimal value { get; set; }
        public string status { get; set; }
        public string name { get; set; }
        public string payer_name { get; set; }
        public string payer_cpf_cnpj { get; set; }
        public string received_date { get; set; }
        public string returned_date { get; set; }
        public string finish_date { get; set; }
        public string created_at { get; set; }
        public bool reconciled { get; set; }
        public TransfeeraGetCreatedBy created_by { get; set; }
        public string type { get; set; }
        public TransfeeraGetReceivedBankAccount received_bank_account { get; set; }
    }

    public class TransfeeraGetCreatedBy
    {
        public string name { get; set; }
        public string email { get; set; }
    }

    public class TransfeeraGetReceivedBankAccount
    {
        public int id { get; set; }
        public string agency { get; set; }
        public string account { get; set; }
        public string operation_type { get; set; }
        public TransfeeraGetBank Bank { get; set; }
    }

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

    public class TransfeeraGetBilletsResponse
    {
        public List<TransfeeraGetBilletResponse> data { get; set; }
        public TransfeeraGetBilletsResponseMetadata metadata { get; set; }
    }

    public class TransfeeraGetBilletsResponseMetadata
    {
        public TransfeeraGetBilletsResponseMetadataPagination pagination { get; set; }
    }

    public class TransfeeraGetBilletsResponseMetadataPagination
    {
        public int itemsPerPage { get; set; }
        public int totalItems { get; set; }
    }

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

    public class ValidateCIPResponse
    {
        public bool isSuccess => barcode_details != null;
        public string status { get; set; }
        public string message { get; set; }
        public BarcodeDetails barcode_details { get; set; }
        public PaymentInfo payment_info { get; set; }
    }

    public class TransfeeraAuthResponse
    {
        public string access_token { get; set; }
        public int? expires_in { get; set; }
    }

    public class TransfeeraAuthErrorResponse
    {
        public int? statusCode { get; set; }
        public string error { get; set; }
        public string message { get; set; }
    }

    public class TransfeeraCheckBalanceResponse
    {
        public decimal value { get; set; }
        public decimal waiting_value { get; set; }
    }

    public class TransfeeraCloseBatchResponse
    {
        public string name { get; set; }
        public string agency { get; set; }
        public string account { get; set; }
        public string type { get; set; }
        public string company { get; set; }
        public string cnpj { get; set; }
        public bool is_batch_paid { get; set; }
    }

    public class TransfeeraCreateBilletBatchErrorResponse
    {
        public int statusCode { get; set; }
        public string error { get; set; }
        public string message { get; set; }
        public string field { get; set; }
        public string errorCode { get; set; }
    }

    public class TransfeeraCreateBilletBatchResponse
    {
        public int id { get; set; }
    }


}
