using MongoDB.Driver;
using MsProductIntegration.Contracts.DTOs;

namespace MsProductIntegration.Contracts.Data
{
    public interface ICatalogContext
    {
        public IMongoCollection<Product<Sku>> Products { get; }
        public IMongoCollection<Product<SkuBill>> ProductsBill { get; }
        public IMongoCollection<Product<SkuPreDefinedPrice>> ProductsPredefinedPrice { get; }
        public IMongoCollection<Product<SkuRangePrice>> ProductsRangePrice { get; }
        public IMongoCollection<Category> Categories { get; }
    }
}
