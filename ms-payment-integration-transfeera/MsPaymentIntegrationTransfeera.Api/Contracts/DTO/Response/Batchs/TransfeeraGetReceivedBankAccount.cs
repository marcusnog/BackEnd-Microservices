namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Response.Batchs
{
    public class TransfeeraGetReceivedBankAccount
    {
        public int id { get; set; }
        public string agency { get; set; }
        public string account { get; set; }
        public string operation_type { get; set; }
        public TransfeeraGetBank Bank { get; set; }
    }
}
