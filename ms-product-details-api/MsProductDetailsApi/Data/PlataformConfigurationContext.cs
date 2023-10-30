using MongoDB.Driver;
using MsProductDetailsApi.Contracts.Data;
using MsProductDetailsApi.Contracts.DTOs;

namespace MsProductDetailsApi.Data
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
