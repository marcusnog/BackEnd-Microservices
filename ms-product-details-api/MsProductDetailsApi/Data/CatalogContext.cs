﻿using MongoDB.Driver;
using MsProductDetailsApi.Contracts.Data;
using MsProductDetailsApi.Contracts.DTOs;

namespace MsProductDetailsApi.Data
{
    public class CatalogContext : ICatalogContext
    {
        private readonly IMongoDatabase _db;
        public IMongoCollection<DbVersion> Versions => _db.GetCollection<DbVersion>("Versions");

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
