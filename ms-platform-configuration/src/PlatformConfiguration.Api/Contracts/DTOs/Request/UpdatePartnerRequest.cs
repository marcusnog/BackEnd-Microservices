namespace PlatformConfiguration.Api.Contracts.DTOs.Request
{
    public class UpdatePartnerRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool AcceptCardPayment { get; set; }
    }
}
