using MongoDB.Driver;
using MsPointsPurchaseApi.Contracts.Data;
using MsPointsPurchaseApi.Contracts.DTOs;
using MsPointsPurchaseApi.Contracts.Repositories;

namespace MsPointsPurchaseApi.Repositories
{
    public class PointsRepository : IPointsRepository
    {
        private readonly IPlatformConfigurationContext _context;

        public PointsRepository(IPlatformConfigurationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Points> Get(string id)
        {
            return await _context
                           .Points
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<Points> GetByValue(decimal value)
        {
            var filter = Builders<Points>.Filter.Eq("Active", true);

            if (value > 0)
            {
                filter = filter & Builders<Points>.Filter.Gt("MaxPointsValue", value);
                filter = filter & Builders<Points>.Filter.Lte("MinPointsValue", value);
            }

            return await _context.Points.Find(filter)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Points obj)
        {
            await _context.Points.InsertOneAsync(obj, new InsertOneOptions() { });
        }

        public async Task<bool> Update(Points obj)
        {
            var updateResult = await _context
                                        .Points
                                        .ReplaceOneAsync(filter: g => g.Id == obj.Id, options: new ReplaceOptions { IsUpsert = true }, replacement: obj);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Points> filter = Builders<Points>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Points
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Points>> List()
        {
            return await _context
                           .Points
                            .Find(p => true)
                            .ToListAsync();
        }
    }
}
