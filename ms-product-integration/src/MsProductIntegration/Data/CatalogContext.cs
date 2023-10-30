using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using MsProductIntegration.Contracts.Data;
using MsProductIntegration.Contracts.DTOs;

namespace MsProductIntegration.Data
{
    public class CatalogContext : ICatalogContext
    {
        private readonly IMongoDatabase _db;
        public IMongoCollection<Category> Categories => _db.GetCollection<Category>("Categories");
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
