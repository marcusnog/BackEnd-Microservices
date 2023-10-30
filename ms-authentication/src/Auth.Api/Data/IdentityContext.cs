using Auth.Api.Contracts.Data;
using Auth.Api.Contracts.DTOs;
using MongoDB.Driver;

namespace Auth.Api.Data
{
    public class IdentityContext : IIdentityContext
    {
        private readonly IMongoDatabase _db;
        public IMongoCollection<IdentityClient> IdentityClients => _db.GetCollection<IdentityClient>("IdentityClient");
        public IMongoCollection<DbVersion> Versions => _db.GetCollection<DbVersion>("Versions");
        public IdentityContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            _db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        }
    }
}
