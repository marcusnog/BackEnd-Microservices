using User.Api.Contracts.DTOs;
using MongoDB.Driver;

namespace User.Api.Contracts.Repositories
{
    public interface IRoleRepository
    {
        Task<IEnumerable<Role>> ListRoles();
        Task<Role> GetRole(RoleFilter filter);
        Task CreateRole(Role Role);
        Task<bool> UpdateRole(Role Role);
        Task<bool> DeleteRole(RoleFilter Role);
    }
}
