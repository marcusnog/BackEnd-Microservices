using Catalog.Api.Contracts.DTOs;

namespace Catalog.Api.Contracts.Repositories
{
    public interface IDbVersionRepository
    {
        Task<DbVersion> GetCurrentVersion();
        Task SetVersion(string version);
    }
}
