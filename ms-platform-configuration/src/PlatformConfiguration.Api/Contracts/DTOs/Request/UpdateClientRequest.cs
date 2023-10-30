namespace PlatformConfiguration.Api.Contracts.DTOs.Request
{
    public class UpdateClientRequest
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public Document[] Documents { get; set; }
}
}
