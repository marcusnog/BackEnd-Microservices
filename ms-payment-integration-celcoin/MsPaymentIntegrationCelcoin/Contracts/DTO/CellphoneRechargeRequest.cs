namespace MsPaymentIntegrationCelcoin.Contracts.DTO
{
    public class CellphoneRechargeRequest
    {
        public string ParticipantId { get; set; }
        public string ParticipantName { get; set; }
        public string ParticipantDocument { get; set; }
        public string CampaignId { get; set; }
        public int CelcoinTransactionId { get; set; }
        public string CellphoneOperator { get; set; }
        public decimal RechargeValue { get; set; }
        public decimal RechargeFeeValue { get; set; }
        public decimal RechargePointsValue { get; set; }
        public int StateCode { get; set; }
        public string PhoneNumber { get; set; }
        public int ProviderId { get; set; }
        public string? token { get; set; }
        public string? environment { get; set; }
        public string? campaign { get; set; }
    }
}
