using MongoDB.Driver;
using MsPointsApi.Contracts.Data;
using MsPointsApi.Contracts.DTOs;
using MsPointsApi.Contracts.Repositories;

namespace MsPointsApi.Repositories
{
    public class Account_MovimentRepository : IAccount_MovimentRepository
    {
        private readonly IAuthContext _context;

        public Account_MovimentRepository(IAuthContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Account_Moviment> Get(string id)
        {
            return await _context
                           .Account_Moviment
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Account_Moviment obj)
        {
            await _context.Account_Moviment.InsertOneAsync(obj, new InsertOneOptions() { });
        }

        public async Task<bool> Update(Account_Moviment obj)
        {
            var updateResult = await _context
                                        .Account_Moviment
                                        .ReplaceOneAsync(filter: g => g.Id == obj.Id, options: new ReplaceOptions { IsUpsert = true }, replacement: obj);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Account_Moviment> filter = Builders<Account_Moviment>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Account_Moviment
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Account_Moviment>> List()
        {
            return await _context
                           .Account_Moviment
                            .Find(p => true)
                            .ToListAsync();
        }
    }
}
