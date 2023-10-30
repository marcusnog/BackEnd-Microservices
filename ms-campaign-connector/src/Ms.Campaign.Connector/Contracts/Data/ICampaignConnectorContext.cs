using MongoDB.Driver;
using Ms.Campaign.Connector.Contracts.DTO;

namespace Ms.Campaign.Connector.Contracts.Data
{
    public interface ICampaignConnectorContext
    {
        public IMongoCollection<CampaignConnector> CampaignConnectors { get; }
        public IMongoCollection<DbVersion> Versions { get; }
    }
}
