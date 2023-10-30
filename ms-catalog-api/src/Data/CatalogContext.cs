using Catalog.Api.Contracts.Data;
using Catalog.Api.Contracts.DTOs;
using MongoDB.Driver;

namespace Catalog.Api.Data
{
    public class CatalogContext : ICatalogContext
    {
        private readonly IMongoDatabase _db;
        public IMongoCollection<DbVersion> Versions => _db.GetCollection<DbVersion>("Versions");
        public IMongoCollection<Category> Categories => _db.GetCollection<Category>("Categories");
        public IMongoCollection<MainCategory> MainCategories => _db.GetCollection<MainCategory>("MainCategories");
        public IMongoCollection<Product<Sku>> Products => _db.GetCollection<Product<Sku>>("Products");
        public IMongoCollection<Product<SkuBill>> ProductsBill => _db.GetCollection<Product<SkuBill>>("ProductsSkuBill");
        public IMongoCollection<Product<SkuPreDefinedPrice>> ProductsPredefinedPrice => _db.GetCollection<Product<SkuPreDefinedPrice>>("ProductsSkuPreDefinedPrice");
        public IMongoCollection<Product<SkuRangePrice>> ProductsRangePrice => _db.GetCollection<Product<SkuRangePrice>>("ProductsSkuRangePrice");

        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            _db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        }
    }
}
