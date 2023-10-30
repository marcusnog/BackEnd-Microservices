using MongoDB.Driver;
using MsProductsSearch.Contracts.DTOs;

namespace MsProductsSearch.Contracts.Data
{
    public interface ICatalogContext
    {
        public IMongoCollection<DbVersion> Versions { get; }
        public IMongoCollection<Product<Sku>> Products { get; }
        public IMongoCollection<Product<SkuBill>> ProductsBill { get; }
        public IMongoCollection<Product<SkuPreDefinedPrice>> ProductsPredefinedPrice { get; }
        public IMongoCollection<Product<SkuRangePrice>> ProductsRangePrice { get; }
        public IMongoCollection<Category> Categories { get; }
    }
}
