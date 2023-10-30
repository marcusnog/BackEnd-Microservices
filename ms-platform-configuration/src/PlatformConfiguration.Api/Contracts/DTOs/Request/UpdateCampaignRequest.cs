namespace PlatformConfiguration.Api.Contracts.DTOs.Request
{
    public class UpdateCampaignRequest
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string Name { get; set; }
        public decimal CoinConversionFactor { get; set; }
        public bool AllowCardPayment { get; set; }
        public decimal AllowedCardPaymentPercentage { get; set; }
        public bool AllowPointAmountSelection { get; set; }
        public string[] Stores { get; set; }
    }
}
