using User.Api.Contracts.DTOs;
using User.Api.Contracts.Repositories;
using MongoDB.Driver;
using User.Api.Contracts.DTOs.Request;
using User.Api.Contracts.DTOs.Response;

namespace User.Api.Repositories
{
    public class AuthContext : IAuthContext
    {
        private readonly IMongoDatabase _db;
        public IMongoCollection<Contracts.DTOs.UserAdministrator> UserAdministrator => _db.GetCollection<Contracts.DTOs.UserAdministrator>("UserAdministrator");
        public IMongoCollection<Contracts.DTOs.UserParticipant> UserParticipant => _db.GetCollection<Contracts.DTOs.UserParticipant>("UserParticipant");
        public IMongoCollection<Profile> Profiles => _db.GetCollection<Profile>("Profiles");
        public IMongoCollection<Role> Roles => _db.GetCollection<Role>("Roles");
        public IMongoCollection<Contracts.DTOs.System> Systems => _db.GetCollection<Contracts.DTOs.System>("Systems");
        public IMongoCollection<DbVersion> Versions => _db.GetCollection<DbVersion>("Versions");
        public IMongoCollection<PasswordRecovery> PasswordRecovery => _db.GetCollection<PasswordRecovery>("PasswordRecovery");
        public IMongoCollection<Account> Account => _db.GetCollection<Account>("Account");
        public IMongoCollection<Address> Addresses => _db.GetCollection<Address>("Addresses");

        public AuthContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            _db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        }
    }
}
