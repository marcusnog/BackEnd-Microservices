namespace MsProductIntegrationGiffty.Api.Contracts.DTOs
{
    public class Store
    {
        public string Id { get; set; }
        public string PartnerId { get; set; }
        public string Name { get; set; }
        public double CreationDate { get; set; }
        public double? DeletionDate { get; set; }
        public bool Active { get; set; }
        public bool AcceptCardPayment { get; set; }
        public bool HaveProducts { get; set; }
        public StoreCampaignConfiguration[] CampaignConfiguration { get; set; }
    }
}
