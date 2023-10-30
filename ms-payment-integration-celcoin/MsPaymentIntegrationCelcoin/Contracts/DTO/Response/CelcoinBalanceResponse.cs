namespace MsPaymentIntegrationCelcoin.Contracts.DTO.Response
{
    public class CelcoinBalanceResponse
    {
        public int antecipated { get; set; }
        public string reconcilieExecuting { get; set; }
        public decimal consumed { get; set; }
        public int credit { get; set; }
        public decimal balance { get; set; }
        public string errorCode { get; set; }
        public string message { get; set; }
        public int status { get; set; }
    }
}
