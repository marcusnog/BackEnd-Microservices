using MsPointsPurchaseApi.Contracts.DTOs;

namespace MsPointsPurchaseApi.Contracts.Repositories
{
    public interface IDbVersionRepository
    {
        Task<DbVersion> GetCurrentVersion();
        Task SetVersion(string version);
    }
}
