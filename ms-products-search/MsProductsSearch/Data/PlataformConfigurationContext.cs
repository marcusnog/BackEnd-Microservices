using MongoDB.Driver;
using MsProductsSearch.Contracts.Data;
using MsProductsSearch.Contracts.DTOs;

namespace MsProductsSearch.Data
{
    public class PlataformConfigurationContext : IPlataformConfigurationContext
    {
        private readonly IMongoDatabase _db;

        public IMongoCollection<Store> Stores => _db.GetCollection<Store>("Stores");

        public PlataformConfigurationContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            _db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabasePlatform"));
        }
    }
}
