using MongoDB.Driver;
using Ms.Campaign.Connector.Contracts.Data;
using Ms.Campaign.Connector.Contracts.DTO;

namespace Ms.Campaign.Connector.Data
{
    public class CampaignConnectorContext : ICampaignConnectorContext
    {
        private readonly IMongoDatabase _db;
        public IMongoCollection<CampaignConnector> CampaignConnectors => _db.GetCollection<CampaignConnector>("CampaignConnectors");
        public IMongoCollection<DbVersion> Versions => _db.GetCollection<DbVersion>("Versions");
        public CampaignConnectorContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            _db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        }
    }
}
