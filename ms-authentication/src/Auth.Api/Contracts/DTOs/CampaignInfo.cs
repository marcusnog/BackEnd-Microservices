namespace Auth.Api.Contracts.DTOs
{
    public class CampaignInfo
    {
        public string ClientId { get; set; }
        public string[] ClientSecrets { get; set; }
        public List<CampaignClaim> Claims { get; set; } = new();
    }
    public class CampaignClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; } = "http://www.w3.org/2001/XMLSchema#string";

    }
}
