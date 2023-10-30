using MongoDB.Driver;
using MsPointsPurchaseApi.Contracts.Data;
using MsPointsPurchaseApi.Contracts.DTOs;
using System;

namespace MsPointsPurchaseApi.Data
{
    public class PlatformConfigurationContext : IPlatformConfigurationContext
    {
        private readonly IMongoDatabase _db;
        public IMongoCollection<Points> Points => _db.GetCollection<Points>("Points");
        public IMongoCollection<PointsPurchase> PointsPurchase => _db.GetCollection<PointsPurchase>("PointsPurchase");
        public IMongoCollection<DbVersion> VersionsPointsPurchase => _db.GetCollection<DbVersion>("VersionsPointsPurchase");

        public PlatformConfigurationContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            _db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        }
    }
}
