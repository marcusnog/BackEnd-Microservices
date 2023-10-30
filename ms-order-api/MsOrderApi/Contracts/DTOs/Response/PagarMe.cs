namespace MsOrderApi.Contracts.DTOs.Response
{
    public class PagarMeReverseOrderResponse
    {
        public string id { get; set; }
        public string code { get; set; }
        public int amount { get; set; }
        public int canceled_amount { get; set; }
        public int paid_amount { get; set; }
        public string status { get; set; }
        public string currency { get; set; }
        public string payment_method { get; set; }
        public string paid_at { get; set; }
        public string canceled_at { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        //public PagarMeCustomer customer { get; set; }
        public PagarMeLastTransactionResponse last_transaction { get; set; }

    }

    public class PagarMeCreateOrderResponse
    {
        public string id { get; set; }
        public string code { get; set; }
        public int amount { get; set; }
        public string currency { get; set; }
        public bool closed { get; set; }
        public List<PagarMeProductsResponse> items { get; set; }
        public PagarMeCustomerResponse customer { get; set; }
        public string status { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string closed_at { get; set; }
        public List<PagarMeChargesResponse> charges { get; set; }

        public string message { get; set; } //Caso Erro

        //public PagarMeCreateOrderResponse_Erros errors { get; set; }
        public object errors { get; set; }

    }

    //public class PagarMeCreateOrderResponse_Erros
    //{
    //    public List<KeyValuePair<string, ArrayList>> lista { get; set; }
    //}

    public class PagarMeProductsResponse
    {
        public string id { get; set; }
        public string type { get; set; }
        public string description { get; set; }
        public int amount { get; set; }
        public int quantity { get; set; }
        public string status { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
    }

    public class PagarMeCustomerResponse
    {
        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string code { get; set; }
        public string document { get; set; }
        public string document_type { get; set; }
        public string type { get; set; }
        public bool delinquent { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public string birthdate { get; set; }
    }

    public class PagarMeHomePhoneResponse
    {
        public string country_code { get; set; }
        public string area_code { get; set; }
        public string number { get; set; }
    }

    public class PagarMeMobilePhoneResponse
    {
        public string country_code { get; set; }
        public string area_code { get; set; }
        public string number { get; set; }
    }

    public class PagarMeMetadata
    {
        public string company { get; set; }
        public string code { get; set; }
    }

    public class PagarMeChargesResponse
    {
        public string id { get; set; }
        public string code { get; set; }
        public int amount { get; set; }
        public string status { get; set; }
        public string currency { get; set; }

        public string payment_method { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }

        public PagarMeCustomerResponse customer { get; set; }
        public PagarMeLastTransactionResponse last_transaction { get; set; }

    }

    public class PagarMeLastTransactionResponse
    {
        public string id { get; set; }
        public string transaction_type { get; set; }
        public int amount { get; set; }
        public string status { get; set; }
        public bool success { get; set; }
        public string operation_type { get; set; }

        public string created_at { get; set; }
        public string updated_at { get; set; }

        public PagarMeGatewayResponse gateway_response { get; set; }
        public PagarMeAntiFraudResponse antifraud_response { get; set; }

    }

    public class PagarMeCard
    {
        public string id { get; set; }
        public string first_six_digits { get; set; }
        public string last_four_digits { get; set; }
        public string brand { get; set; }
        public string holder_name { get; set; }
        public string exp_month { get; set; }
        public string exp_year { get; set; }
        public string status { get; set; }
        public string type { get; set; }
        public string created_at { get; set; }
        public string updated_at { get; set; }
        public PagarMeBillingAddress billing_address { get; set; }
    }

    public class PagarMeBillingAddress
    {
        public string zip_code { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string line_1 { get; set; }
    }

    public class PagarMeGatewayResponse
    {
        public string code { get; set; }
        public List<PagarMeGatewayErrorResponse> errors { get; set; }
    }

    public class PagarMeGatewayErrorResponse
    {
        public string message { get; set; }
    }

    //public class PagarMeAntifraudeResponse
    //{

    //}

    public class PagarMeAntiFraudResponse
    {
        public string status { get; set; }
        public string score { get; set; }
        public string provider_name { get; set; }

    }
}
