using PlatformConfiguration.Api.Contracts.Data;
using PlatformConfiguration.Api.Contracts.DTOs;
using PlatformConfiguration.Api.Contracts.Repositories;
using MongoDB.Driver;
using Ms.Api.Utilities.Models;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace PlatformConfiguration.Api.Repositories
{
    public class PartnerRepository : BaseRepository, IPartnerRepository
    {
        private readonly IPlataformConfigurationContext _context;

        public PartnerRepository(IPlataformConfigurationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<QueryPage<IEnumerable<Partner>>> List(int page = 0, int pageSize = 10, bool? status = null, string? q = null)
        {
            var filter = Builders<Partner>.Filter.Empty;
            if(status != null)
                filter &= Builders<Partner>.Filter.Eq(x => x.Active, status);
            if(q != null)
                filter &= Builders<Partner>.Filter.Regex(x => x.Name, new BsonRegularExpression(new Regex(q, RegexOptions.IgnoreCase)));
            
            return await GetPagerResultAsync(page, pageSize, _context.Partners, filter);
        }
        public async Task<Partner> Get(string id)
        {
            return await _context
                           .Partners
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Partner obj)
        {
            await _context.Partners.InsertOneAsync(obj);
        }

        public async Task<bool> Update(Partner obj)
        {
            var updateResult = await _context
                                        .Partners
                                        .ReplaceOneAsync(filter: g => g.Id == obj.Id, options: new ReplaceOptions { IsUpsert = true }, replacement: obj);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Partner> filter = Builders<Partner>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Partners
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
