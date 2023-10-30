namespace PlatformConfiguration.Api.Contracts.DTOs.Response
{
    public class GetPartnerConfigurationResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool AcceptCardPayment { get; set; }
        public bool Active { get; set; }
        public GetPartnerConfigurationStore Stores { get; set; }
    }
    public class GetPartnerConfigurationStore
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public bool AcceptCardPayment { get; set; }
        public StoreCampaignConfiguration[] CampaignConfiguration { get; set; }
    }
}
