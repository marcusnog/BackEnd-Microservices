using MongoDB.Driver;
using MsPointsApi.Contracts.Data;
using MsPointsApi.Contracts.DTOs;

namespace MsPointsApi.Data
{
    public class AuthContext : IAuthContext
    {
        private readonly IMongoDatabase _db;
        public IMongoCollection<Account> Account => _db.GetCollection<Account>("Account");
        public IMongoCollection<Account_Moviment> Account_Moviment => _db.GetCollection<Account_Moviment>("Account_Moviment");

        public AuthContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            _db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        }
    }
}
