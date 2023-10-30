using System.Reflection.Metadata;

namespace User.Api.Contracts.DTOs.Request
{
    public class CreateClientRequest
    {
        public string Name { get; set; }
        public Document[] Documents { get; set; }
    }

    public class Document
    {
        public string Type { get; set; }
        public string Value { get; set; }
    }
}
