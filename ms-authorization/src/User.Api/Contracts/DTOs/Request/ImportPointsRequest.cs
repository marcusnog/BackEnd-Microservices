namespace User.Api.Contracts.DTOs.Request
{
    public class ImportPointsRequest
    {
        public IFormFile File { get; set; }
        public string CampaignId { get; set; }
    }
}
