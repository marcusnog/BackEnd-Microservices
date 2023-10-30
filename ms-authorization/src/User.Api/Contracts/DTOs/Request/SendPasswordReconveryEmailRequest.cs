namespace User.Api.Contracts.DTOs.Request
{
    public class SendPasswordReconveryEmailRequest
    {
        public string imageUrl { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string code { get; set; }
        public string link { get; set; }
        public string expiration { get; set; }
        public string contactEmail { get; set; }
        public string accountType { get; set; }
    }
}
