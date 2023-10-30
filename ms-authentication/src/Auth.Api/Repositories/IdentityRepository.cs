using Auth.Api.Contracts.Data;
using Auth.Api.Contracts.DTOs;
using Auth.Api.Contracts.Repositories;
using MongoDB.Driver;

namespace Auth.Api.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly IIdentityContext _context;

        public IdentityRepository(IIdentityContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task Create(IdentityClient obj)
        {
            await _context.IdentityClients.InsertOneAsync(obj);
        }

        public async Task<bool> Delete(string id)
        {
            var filter = Builders<IdentityClient>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .IdentityClients
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IdentityClient> Get(string id)
        {
            return await _context
                            .IdentityClients
                            .Find(p => p.Id == id)
                            .FirstOrDefaultAsync();
        }
        public async Task<IdentityClient> FindClientById(string clientId)
        {
            return await _context
                            .IdentityClients
                            .Find(p => p.ClientId == clientId)
                            .FirstOrDefaultAsync();
        }
        
        public async Task<IEnumerable<IdentityClient>> List()
        {
            return await _context
                            .IdentityClients
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<bool> Update(IdentityClient obj)
        {
            var updateResult = await _context
                                        .IdentityClients
                                        .ReplaceOneAsync(filter: g => g.Id == obj.Id, replacement: obj);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
