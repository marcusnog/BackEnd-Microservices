namespace PlatformConfiguration.Api.Contracts.DTOs.Request
{
    public class CreateClientRequest
    {
        public string Name { get; set; }
        public Document[] Documents { get; set; }
    }
}
