namespace User.Api.Contracts.DTOs
{
    public class ConfigIdentity
    {
        public ApiResource[] ApiResources { get; set; }
    }
    public class ApiResource
    {
        public string Name { get; set; }

        public string[] ApiSecrets { get; set; }
    }
    public class ApiScope
    {
        public string Name { get; set; }

        public string DisplayName { get; set; }
    }
}
