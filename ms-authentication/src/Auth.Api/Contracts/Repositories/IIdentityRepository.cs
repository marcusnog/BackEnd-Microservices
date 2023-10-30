using Auth.Api.Contracts.DTOs;

namespace Auth.Api.Contracts.Repositories
{
    public interface IIdentityRepository
    {
        Task<IdentityClient> FindClientById(string clientId);
        Task<IdentityClient> Get(string id);
        Task<IEnumerable<IdentityClient>> List();
        Task Create(IdentityClient Client);
        Task<bool> Update(IdentityClient Client);
        Task<bool> Delete(string id);
    }
}
