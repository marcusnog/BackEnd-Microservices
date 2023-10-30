using MongoDB.Driver;
using Ms.Api.Utilities.Extensions;
using Ms.Campaign.Connector.Contracts.Data;
using Ms.Campaign.Connector.Contracts.DTO;
using Ms.Campaign.Connector.Contracts.Repositories;

namespace Ms.Campaign.Connector.Repositories
{
    public class DbVersionRepository : IDbVersionRepository
    {
        private readonly ICampaignConnectorContext _context;

        public DbVersionRepository(ICampaignConnectorContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<DbVersion> GetCurrentVersion()
        {
            return await _context
                            .Versions
                            .Find(p => true)
                            .SortByDescending(x => x.CreatedAt)
                            .FirstOrDefaultAsync();
        }

        public async Task SetVersion(string version)
        {
            await _context.Versions.InsertOneAsync(new DbVersion()
            {
                Name = version,
                CreatedAt = DateTime.UtcNow.ToUnixTimestamp()
            });
        }
    }
}
