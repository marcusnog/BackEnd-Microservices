using Ms.Campaign.Connector.Contracts.DTO;

namespace Ms.Campaign.Connector.Contracts.Repositories
{
    public interface IDbVersionRepository
    {
        Task<DbVersion> GetCurrentVersion();
        Task SetVersion(string version);
    }
}
