namespace User.Api.Contracts.DTOs.Request
{
    public class ImportParticipantsRequest
    {
        public IFormFile File { get; set; }
        public string CampaignId { get; set; } 
    }
}
