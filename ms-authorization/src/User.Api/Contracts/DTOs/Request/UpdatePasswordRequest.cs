namespace User.Api.Contracts.DTOs.Request
{
    public class UpdatePasswordRequest
    {
        public string tokenId { get; set; }
        public string code { get; set; }
        public string password { get; set; }
    }
}
