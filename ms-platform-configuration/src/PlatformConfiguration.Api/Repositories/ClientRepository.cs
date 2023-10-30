using PlatformConfiguration.Api.Contracts.Data;
using PlatformConfiguration.Api.Contracts.DTOs;
using PlatformConfiguration.Api.Contracts.Repositories;
using MongoDB.Driver;
using Ms.Api.Utilities.Models;
using MongoDB.Bson;
using System.Text.RegularExpressions;

namespace PlatformConfiguration.Api.Repositories
{
    public class ClientRepository : BaseRepository, IClientRepository
    {
        private readonly IPlataformConfigurationContext _context;

        public ClientRepository(IPlataformConfigurationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Client> Get(string id)
        {
            return await _context
                           .Clients
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Client obj)
        {
            await _context.Clients.InsertOneAsync(obj, new InsertOneOptions() { });
        }

        public async Task<bool> Update(Client obj)
        {
            var updateResult = await _context
                                        .Clients
                                        .ReplaceOneAsync(filter: g => g.Id == obj.Id, options: new ReplaceOptions { IsUpsert = true }, replacement: obj);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Client> filter = Builders<Client>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Clients
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<QueryPage<IEnumerable<Client>>> List(int page = 0, int pageSize = 10, bool? status = null, string? q = null)
        {
            var filter = Builders<Client>.Filter.Empty;
            if (status != null)
                filter &= Builders<Client>.Filter.Eq(x => x.Active, status);
            if (q != null)
                filter &= Builders<Client>.Filter.Regex(x => x.Name, new BsonRegularExpression(new Regex(q, RegexOptions.IgnoreCase)));

            return await GetPagerResultAsync(page, pageSize, _context.Clients, filter);
        }

        public async Task<IEnumerable<Client>> List()
        {
            return await _context
                           .Clients
                            .Find(p => true)
                            .ToListAsync();
        }
        public async Task<IEnumerable<SelectItem<string>>> ListAsOptions()
        {
            var items = await _context
                            .Clients
                            .Find(p => true)
                            .ToListAsync();

            return items.Select(x => new SelectItem<string>()
            {
                Value = x.Id,
                Label = x.Name
            });
        }
    }
}
