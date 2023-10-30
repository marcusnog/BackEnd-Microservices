namespace Platform.Events.Core.Contracts.Product
{
    public class UpdateSkuRangePriceEventV1 : BaseSku, EventBase
    {
        public string ApiVersion => "UpdateSkuRangePrice/v1";
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
