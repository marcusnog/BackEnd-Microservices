using User.Api.Contracts.DTOs.Response;

namespace User.Contracts.Repositories
{
    public interface IAddressRepository
    {
        Task<Address> Get(string id);
        Task<List<Address>> List(string userId);
    }
}
