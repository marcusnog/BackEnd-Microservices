using MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Response.Batchs;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Response
{
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
}
