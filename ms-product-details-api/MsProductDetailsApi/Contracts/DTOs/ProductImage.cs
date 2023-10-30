using MsProductDetailsApi.Contracts.Enum;

namespace MsProductDetailsApi.Contracts.DTOs
{
    public class ProductImage
    {
        public ProductImageSize Size { get; set; }
        public string Url { get; set; }
    }
}
