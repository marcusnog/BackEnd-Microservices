namespace Platform.Events.Core.Contracts.Product
{
    public class CreateSkuRangePriceEventV1 : BaseSku, EventBase
    {
        public string ApiVersion => "CreateSkuRangePrice/v1";
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
    }
}
