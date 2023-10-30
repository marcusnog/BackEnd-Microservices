namespace Catalog.Api.Contracts.DTOs.Response
{
    public class GetProductHashResponse
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Hash { get; set; }
        public Dictionary<string, GetProductHashSku>? Skus { get; set; }
    }

    public class GetProductHashSku
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Hash { get; set; }
    }
}
