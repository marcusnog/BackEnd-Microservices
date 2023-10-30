using MongoDB.Driver;
using Ms.Api.Utilities.Contracts.DTOs;
using MsOrderApi.Contracts.Data;
using MsOrderApi.Contracts.DTOs;

namespace MsOrderApi.Data
{
    public class CatalogContext : ICatalogContext
    {
        private readonly IMongoDatabase _db;
        //public IMongoCollection<DbVersion> Versions => _db.GetCollection<DbVersion>("Versions");

        public IMongoCollection<Order> Orders => _db.GetCollection<Order>("Orders");

        public CatalogContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            _db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        }
    }
}
