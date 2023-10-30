using Ms.Api.Utilities.Models;
using PlatformConfiguration.Api.Contracts.DTOs;

namespace PlatformConfiguration.Api.Contracts.Repositories
{
    public interface IPartnerRepository
    {
        Task<QueryPage<IEnumerable<Partner>>> List(int page = 0, int pageSize = 10, bool? status = null, string? q = null);
        Task<Partner> Get(string id);
        Task Create(Partner obj);
        Task<bool> Update(Partner obj);
        Task<bool> Delete(string id);
    }
}