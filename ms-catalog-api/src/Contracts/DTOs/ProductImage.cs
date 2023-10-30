using Catalog.Api.Contracts.Enums;

namespace Catalog.Api.Contracts.DTOs
{
    public class ProductImage
    {
        public ProductImageSize Size { get; set; }
        public string Url { get; set; }
    }
}
