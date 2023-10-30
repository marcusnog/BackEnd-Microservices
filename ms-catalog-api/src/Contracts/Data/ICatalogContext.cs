using Catalog.Api.Contracts.DTOs;
using MongoDB.Driver;

namespace Catalog.Api.Contracts.Data
{
    public interface ICatalogContext
    {
        public IMongoCollection<DbVersion> Versions { get; }
        public IMongoCollection<Product<Sku>> Products { get; }
        public IMongoCollection<Product<SkuBill>> ProductsBill { get; }
        public IMongoCollection<Product<SkuPreDefinedPrice>> ProductsPredefinedPrice { get; }
        public IMongoCollection<Product<SkuRangePrice>> ProductsRangePrice { get; }
        public IMongoCollection<Category> Categories { get; }
        public IMongoCollection<MainCategory> MainCategories { get; }
    }
}
