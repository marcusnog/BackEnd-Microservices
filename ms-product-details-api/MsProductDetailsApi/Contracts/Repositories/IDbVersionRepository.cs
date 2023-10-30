using MsProductDetailsApi.Contracts.DTOs;

namespace MsProductDetailsApi.Contracts.Repositories
{
    public interface IDbVersionRepository
    {
        Task<DbVersion> GetCurrentVersion();
        Task SetVersion(string version);
    }
}
