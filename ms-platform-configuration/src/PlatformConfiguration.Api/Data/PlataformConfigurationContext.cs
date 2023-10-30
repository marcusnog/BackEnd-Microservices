using MongoDB.Driver;
using PlatformConfiguration.Api.Contracts.Data;
using PlatformConfiguration.Api.Contracts.DTOs;

namespace PlatformConfiguration.Api.Data
{
    public class PlataformConfigurationContext : IPlataformConfigurationContext
    {
        private readonly IMongoDatabase _db;
        public IMongoCollection<DbVersion> Versions => _db.GetCollection<DbVersion>("Versions");
        public IMongoCollection<Campaign> Campaigns => _db.GetCollection<Campaign>("Campaigns");
        public IMongoCollection<Client> Clients => _db.GetCollection<Client>("Clients");
        public IMongoCollection<Partner> Partners => _db.GetCollection<Partner>("Partners");
        public IMongoCollection<Store> Stores => _db.GetCollection<Store>("Stores");

        public PlataformConfigurationContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            _db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        }
    }
}
