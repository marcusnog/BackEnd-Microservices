using Platform.Events.Core.Contracts.Enums;

namespace Platform.Events.Core.Contracts.Product
{
    public class UpdateProductEventV1 : EventBase
    {
        public string ApiVersion => "UpdateProduct/v1";
        public string Id { get; set; }
        public string Code { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
        public string CategoryId { get; set; }
        public string StoreId { get; set; }
        public bool Active { get; set; }
        public UpdateProductEventV1Image[] Images { get; set; }
    }

    public class UpdateProductEventV1Image
    {
        public ProductImageSize Size { get; set; }
        public string Url { get; set; }
    }
}