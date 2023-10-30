using Platform.Events.Core.Contracts.Enums;

namespace Platform.Events.Core.Contracts.Product
{
    public class UpdateProductSkuPreDefinedPriceEventV1 : EventBase
    {
        public string ApiVersion => "UpdateProductSkuBill/v1";
        public string Id { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string StoreId { get; set; }
        public bool Active { get; set; }
        public CreateProductSkuPreDefinedPriceEventV1Image[] Images { get; set; }
    }
}