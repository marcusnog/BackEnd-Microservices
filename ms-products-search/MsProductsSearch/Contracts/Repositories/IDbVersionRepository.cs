using MsProductsSearch.Contracts.DTOs;

namespace MsProductsSearch.Contracts.Repositories
{
    public interface IDbVersionRepository
    {
        Task<DbVersion> GetCurrentVersion();
        Task SetVersion(string version);
    }
}
