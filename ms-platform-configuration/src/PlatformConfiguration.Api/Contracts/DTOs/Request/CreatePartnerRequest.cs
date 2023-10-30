namespace PlatformConfiguration.Api.Contracts.DTOs.Request
{
    public class CreatePartnerRequest
    {
        public string Name { get; set; }
        public bool AcceptCardPayment { get; set; }
    }
}
