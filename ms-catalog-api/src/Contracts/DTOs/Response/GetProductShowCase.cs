namespace Catalog.Api.Contracts.DTOs.Response
{
    public class GetProductShowCase
    {
        public string ProductId { get; set; }
        public string Id { get; set; }
        public string DisplayName { get; set; }
        public decimal ListPrice { get; set; }
        public decimal? SalePrice { get; set; }
        public string StoreId { get; set; }
        public bool Available { get; set; }
        public string Image { get; set; }

    }
}
