namespace User.Api.Contracts.DTOs.Request
{
    public class UpdateUserRequest
    {
        public string Id { get; set; }
        public string? Nickname { get; set; }
        public string? Phone { get; set; }
        public string IdProfile { get; set; }
        public bool? AcceptedConsentTerm { get; set; }

    }
}
