using Ms.Api.Utilities.Models;

namespace PlatformConfiguration.Api.Contracts.DTOs.Response
{
    public class GetCampaignInfoResponse
    {
        public string Id { get; set; }
        public string ClientId { get; set; }
        public string ClientName { get; set; }
        public string Name { get; set; }
        public decimal CoinConversionFactor { get; set; }
        public bool AllowCardPayment { get; set; }
        public decimal AllowedCardPaymentPercentage { get; set; }
        public bool AllowPointAmountSelection { get; set; }
        public double CreationDate { get; set; }
        public string CreationUserId { get; set; }
        public double? DeletionDate { get; set; }
        public string? DeletionUserId { get; set; }
        public SelectItem<string>[]? Stores { get; set; }
        public bool Active { get; set; }
    }
}
