using Platform.Events.Core.Contracts.Enums;

namespace Platform.Events.Core.Contracts.Product
{
    public class CreateProductEventV1 : EventBase
    {
        public string ApiVersion => "CreateProduct/v1";
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string StoreId { get; set; }
        public CreateProductEventV1Image[] Images { get; set; }
    }
    public class CreateProductEventV1Image
    {
        public ProductImageSize Size { get; set; }
        public string Url { get; set; }
    }
}
