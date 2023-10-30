using MongoDB.Driver;
using MsPaymentIntegrationCelcoin.Contracts.Repository;
using MsPaymentIntegrationTransfeera.Api.Contracts.Data;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO;
using System.Linq.Expressions;

namespace MsPaymentIntegrationCelcoin.Repository
{
    public class BilletRepository : IBilletRepository
    {
        private readonly IBilletContext _context;

        public BilletRepository(IBilletContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Billet> Get(string id)
        {
            return await _context
                           .Billet
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public IEnumerable<Billet> Find(Expression<Func<Billet, bool>> filter)
        {
            return _context
                           .Billet
                           .AsQueryable()
                           .Where(filter.Compile())
                           .ToList();
        }
        public async Task Create(Billet obj)
        {
            await _context.Billet.InsertOneAsync(obj);
        }

        public async Task<bool> Update(Billet obj)
        {
            var updateResult = await _context
                                        .Billet
                                        .ReplaceOneAsync(filter: g => g.Id == obj.Id, options: new ReplaceOptions { IsUpsert = true }, replacement: obj);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Billet> filter = Builders<Billet>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Billet
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
