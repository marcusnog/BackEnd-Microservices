using User.Api.Contracts.DTOs;
using MongoDB.Driver;
using User.Api.Contracts.DTOs.Request;
using User.Api.Contracts.DTOs.Response;

namespace User.Api.Contracts.Repositories
{
    public interface IAuthContext
    {
        public IMongoCollection<Contracts.DTOs.UserAdministrator> UserAdministrator { get; }
        public IMongoCollection<Contracts.DTOs.UserParticipant> UserParticipant { get; }
        public IMongoCollection<Profile> Profiles { get; }
        public IMongoCollection<Role> Roles { get; }
        public IMongoCollection<DTOs.System> Systems { get; }
        public IMongoCollection<DbVersion> Versions { get; }
        public IMongoCollection<PasswordRecovery> PasswordRecovery { get; }
        public IMongoCollection<Account> Account { get; }
        public IMongoCollection<Address> Addresses { get; }
    }
}
