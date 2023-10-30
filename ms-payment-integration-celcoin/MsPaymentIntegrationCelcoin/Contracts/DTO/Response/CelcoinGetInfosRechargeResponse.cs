namespace MsPaymentIntegrationCelcoin.Contracts.DTO.Response
{
    public class CelcoinGetInfosRechargeResponse
    {
        public CelcoinTransaction transaction { get; set; }
        public int errorCode { get; set; }
    }

    public class CelcoinTransaction
    {
        public int authentication { get; set; }
        public string errorCode { get; set; }
        public string createDate { get; set; }
        public string message { get; set; }
        public int externalNsu { get; set; }
        public string transactionId { get; set; }
        public int status { get; set; }
        public int externalTerminal { get; set; }
    }
}
