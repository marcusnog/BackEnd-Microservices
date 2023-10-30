using MongoDB.Driver;
using MsPaymentIntegrationCelcoin.Contracts.Data;
using MsPaymentIntegrationCelcoin.Contracts.DTO;
using MsPaymentIntegrationCelcoin.Contracts.Repository;
using System.Linq.Expressions;

namespace MsPaymentIntegrationCelcoin.Repository
{
    public class CellphoneRechargeRepository : ICellphoneRechargeRepository
    {
        private readonly ICellphoneRechargeContext _context;

        public CellphoneRechargeRepository(ICellphoneRechargeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<CellphoneRecharge> Get(string id)
        {
            return await _context
                           .CellphoneRecharge
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public IEnumerable<CellphoneRecharge> Find(Expression<Func<CellphoneRecharge, bool>> filter)
        {
            return _context
                           .CellphoneRecharge
                           .AsQueryable()
                           .Where(filter.Compile())
                           .ToList();
        }
        public async Task Create(CellphoneRecharge obj)
        {
            await _context.CellphoneRecharge.InsertOneAsync(obj);
        }

        public async Task<bool> Update(CellphoneRecharge obj)
        {
            var updateResult = await _context
                                        .CellphoneRecharge
                                        .ReplaceOneAsync(filter: g => g.Id == obj.Id, options: new ReplaceOptions { IsUpsert = true }, replacement: obj);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<CellphoneRecharge> filter = Builders<CellphoneRecharge>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .CellphoneRecharge
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
