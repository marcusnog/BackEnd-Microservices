namespace MsPointsPurchaseApi.Contracts.DTOs
{
    public class EffectDebitAdminRequest
    {
        public string AccountId { get; set; }
        public string CampaignId { get; set; }
        public decimal Value { get; set; }
    }
}
