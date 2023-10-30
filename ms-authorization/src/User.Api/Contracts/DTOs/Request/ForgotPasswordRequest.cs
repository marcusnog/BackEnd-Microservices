namespace User.Api.Contracts.DTOs.Request
{
    public class ForgotPasswordRequest
    {
        public string Username { get; set; }
        public string Scope { get; set; }
        public string Lang { get; set; }
    }
}
