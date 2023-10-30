namespace Platform.Events.Core.Contracts.Product
{
    public class CreateSkuPreDefinedPriceEventV1 : BaseSku, EventBase
    {
        public string ApiVersion => "CreateSkuPreDefinedPrice/v1";
        public decimal ListPrice { get; set; }
        public decimal SalePrice { get; set; }
    }
}
