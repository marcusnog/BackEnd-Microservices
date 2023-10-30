using Ms.Campaign.Connector.Contracts.DTO;

namespace Ms.Campaign.Connector.Contracts.Repositories
{
    public interface ICampaignConnectorRepository
    {
        Task<CampaignConnector?> Find(string environment, string campaign);
        Task<CampaignConnector> Get(string id);
        Task<IEnumerable<CampaignConnector>> List();
        Task Create(CampaignConnector Client);
        Task<bool> Update(CampaignConnector Client);
        Task<bool> Delete(string id);
    }
}
