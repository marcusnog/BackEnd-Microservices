namespace User.Api.Contracts.DTOs
{
    public class UserInfo
    {
        public string ClientId { get; set; }
        public string UserId { get; set; }
        public string[] ClientSecrets { get; set; }
        public List<ClientClaim> Claims { get; set; } = new();
    }
    public class ClientClaim
    {
        public string Type { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; } = "http://www.w3.org/2001/XMLSchema#string";

    }
}
