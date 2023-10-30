namespace User.Api.Contracts.DTOs.Request
{
    public class CreateParticipantRequest
    {
        public string? Nickname { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; }
        public string? Cpf { get; set; }
        public string CampaignId  { get; set; }
    }
}
