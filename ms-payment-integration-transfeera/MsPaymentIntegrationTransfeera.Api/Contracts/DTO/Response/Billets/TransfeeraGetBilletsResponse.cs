namespace MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Response.Billets
{
    public class TransfeeraGetBilletsResponse
    {
        public List<TransfeeraGetBilletResponse> data { get; set; }
        public TransfeeraGetBilletsResponseMetadata metadata { get; set; }
    }
}
