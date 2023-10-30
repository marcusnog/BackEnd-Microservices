using PlatformConfiguration.Api.Contracts.Data;
using PlatformConfiguration.Api.Contracts.DTOs;
using PlatformConfiguration.Api.Contracts.Repositories;
using MongoDB.Driver;
using Ms.Api.Utilities.Models;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace PlatformConfiguration.Api.Repositories
{
    public class CampaignRepository : BaseRepository, ICampaignRepository
    {
        private readonly IPlataformConfigurationContext _context;

        public CampaignRepository(IPlataformConfigurationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Campaign> Get(string id)
        {
            return await _context
                           .Campaigns
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<Campaign> GetByName(string name)
        {
            return await _context
                           .Campaigns
                           .Find(p => p.Name == name)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Campaign obj)
        {
            await _context.Campaigns.InsertOneAsync(obj);
        }

        public async Task<bool> Update(Campaign obj)
        {
            var updateResult = await _context
                                        .Campaigns
                                        .ReplaceOneAsync(filter: g => g.Id == obj.Id, options: new ReplaceOptions { IsUpsert = true }, replacement: obj);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Campaign> filter = Builders<Campaign>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Campaigns
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<QueryPage<IEnumerable<Campaign>>> List(int page = 0, int pageSize = 10, bool? status = null, string? q = null)
        {
            var filter = Builders<Campaign>.Filter.Empty;
            if (status != null)
                filter &= Builders<Campaign>.Filter.Eq(x => x.Active, status);
            if (q != null)
                filter &= Builders<Campaign>.Filter.Regex(x => x.Name, new BsonRegularExpression(new Regex(q, RegexOptions.IgnoreCase)));

            return await GetPagerResultAsync(page, pageSize, _context.Campaigns, filter);
        }
        public async Task<IEnumerable<SelectItem<string>>> ListAsOptions(string clientId)
        {
            var items = await _context
                            .Campaigns
                            .Find(p => p.ClientId == clientId)
                            .ToListAsync();

            return items.Select(x => new SelectItem<string>()
            {
                Value = x.Id,
                Label = x.Name
            });
        }
    }
}
