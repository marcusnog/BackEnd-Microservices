namespace PlatformConfiguration.Api.Contracts.DTOs.Request
{
    public class CreateCampaignRequest
    {
        public string ClientId { get; set; }
        public string Name { get; set; }
        public decimal CoinConversionFactor { get; set; }
        public bool AllowCardPayment { get; set; }
        public byte[]? CampaignLogo { get; set; }
        public byte[]? CampaignBanner { get; set; }
        public string? PrimaryColor { get; set; }
        public string? SecondaryColor { get; set; }
        public decimal AllowedCardPaymentPercentage { get; set; }
        public bool AllowPointAmountSelection { get; set; }
        public string[]? Stores { get; set; }
    }
}
