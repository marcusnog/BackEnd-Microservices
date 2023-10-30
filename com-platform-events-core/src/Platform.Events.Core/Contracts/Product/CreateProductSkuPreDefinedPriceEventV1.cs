using Platform.Events.Core.Contracts.Enums;

namespace Platform.Events.Core.Contracts.Product
{
    public class CreateProductSkuPreDefinedPriceEventV1 : EventBase
    {
        public string ApiVersion => "CreateProductSkuPreDefinedPrice/v1";
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string StoreId { get; set; }
        public CreateProductSkuPreDefinedPriceEventV1Image[] Images { get; set; }
    }

    public class CreateProductSkuPreDefinedPriceEventV1Image
    {
        public ProductImageSize Size { get; set; }
        public string Url { get; set; }
    }
}
