namespace Platform.Events.Core.Contracts.Product
{
    public class UpdateSkuPreDefinedPriceEventV1 : BaseSku, EventBase
    {
        public string ApiVersion => "UpdateSkuPreDefinedPrice/v1";
        public decimal ListPrice { get; set; }
        public decimal SalePrice { get; set; }
    }
}
