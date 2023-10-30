using MongoDB.Driver;
using Ms.Campaign.Connector.Contracts.Data;
using Ms.Campaign.Connector.Contracts.DTO;
using Ms.Campaign.Connector.Contracts.Repositories;

namespace Ms.Campaign.Connector.Repositories
{
    public class CampaignConnectorRepository : ICampaignConnectorRepository
    {
        private readonly ICampaignConnectorContext _context;

        public CampaignConnectorRepository(ICampaignConnectorContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task Create(CampaignConnector obj)
        {
            await _context.CampaignConnectors.InsertOneAsync(obj);
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<CampaignConnector>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .CampaignConnectors
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<CampaignConnector> Get(string id)
        {
            return await _context
                            .CampaignConnectors
                            .Find(p => p.Id == id)
                            .FirstOrDefaultAsync();
        }
        public async Task<CampaignConnector?> Find(string environment, string campaign)
        {
            return await _context
                            .CampaignConnectors
                            .Find(p => p.Environment == environment && p.Campaign == campaign)
                            .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<CampaignConnector>> List()
        {
            return await _context
                            .CampaignConnectors
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<bool> Update(CampaignConnector obj)
        {
            var updateResult = await _context
                                        .CampaignConnectors
                                        .ReplaceOneAsync(filter: g => g.Id == obj.Id, replacement: obj);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
