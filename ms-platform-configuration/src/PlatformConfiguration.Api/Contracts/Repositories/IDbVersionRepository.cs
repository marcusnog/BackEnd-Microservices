using PlatformConfiguration.Api.Contracts.DTOs;

namespace PlatformConfiguration.Api.Contracts.Repositories
{
    public interface IDbVersionRepository
    {
        Task<DbVersion> GetCurrentVersion();
        Task SetVersion(string version);
    }
}
