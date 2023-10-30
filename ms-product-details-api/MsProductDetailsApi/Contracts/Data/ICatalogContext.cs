using MongoDB.Driver;
using MsProductDetailsApi.Contracts.DTOs;

namespace MsProductDetailsApi.Contracts.Data
{
    public interface ICatalogContext
    {
        public IMongoCollection<DbVersion> Versions { get; }
        public IMongoCollection<Product<Sku>> Products { get; }
        public IMongoCollection<Product<SkuBill>> ProductsBill { get; }
        public IMongoCollection<Product<SkuPreDefinedPrice>> ProductsPredefinedPrice { get; }
        public IMongoCollection<Product<SkuRangePrice>> ProductsRangePrice { get; }
    }
}
