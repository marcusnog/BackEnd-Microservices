using User.Api.Contracts.DTOs;

namespace User.Api.Contracts.Repositories
{
    public interface IDbVersionRepository
    {
        Task<DbVersion> GetCurrentVersion();
        Task SetVersion(string version);
    }
}
