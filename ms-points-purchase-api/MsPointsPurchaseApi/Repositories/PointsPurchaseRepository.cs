using MongoDB.Driver;
using MsPointsPurchaseApi.Contracts.Data;
using MsPointsPurchaseApi.Contracts.DTOs;
using MsPointsPurchaseApi.Contracts.Repositories;

namespace MsPointsPurchaseApi.Repositories
{
    public class PointsPurchaseRepository : IPointsPurchaseRepository
    {
        private readonly IPlatformConfigurationContext _context;

        public PointsPurchaseRepository(IPlatformConfigurationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<PointsPurchase> Get(string id)
        {
            return await _context
                           .PointsPurchase
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<PointsPurchase> GetByUser(string userId)
        {
            return await _context
                           .PointsPurchase
                           .Find(p => p.UserId == userId)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(PointsPurchase obj)
        {
            await _context.PointsPurchase.InsertOneAsync(obj, new InsertOneOptions() { });
        }

        public async Task<bool> Update(PointsPurchase obj)
        {
            var updateResult = await _context
                                        .PointsPurchase
                                        .ReplaceOneAsync(filter: g => g.Id == obj.Id, options: new ReplaceOptions { IsUpsert = true }, replacement: obj);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<PointsPurchase> filter = Builders<PointsPurchase>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .PointsPurchase
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<PointsPurchase>> List()
        {
            return await _context
                           .PointsPurchase
                            .Find(p => true)
                            .ToListAsync();
        }
    }
}
