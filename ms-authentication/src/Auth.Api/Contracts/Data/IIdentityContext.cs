using Auth.Api.Contracts.DTOs;
using MongoDB.Driver;

namespace Auth.Api.Contracts.Data
{
    public interface IIdentityContext
    {
        public IMongoCollection<IdentityClient> IdentityClients { get; }
        public IMongoCollection<DbVersion> Versions { get; }
    }
}
