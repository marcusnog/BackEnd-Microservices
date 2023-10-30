using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Ms.Api.Utilities.Models;
using MsPointsApi.Contracts.Data;
using MsPointsApi.Contracts.DTOs;
using MsPointsApi.Contracts.Repositories;
using System.Collections.Generic;
using static Ms.Api.Utilities.Enum.Enums;

namespace MsPointsApi.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly IAuthContext _context;

        public AccountRepository(IAuthContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Account> Get(string id)
        {
            return await _context
                           .Account
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<Account> GetAccountMoviments(string id)
        {
            var query = await _context
                           .Account
                           .Find(p => p.UserId == id)
                           .FirstOrDefaultAsync();

            var moviments = await _context.Account_Moviment.Find(x => x.AccountId == query.Id).ToListAsync();

            List<string> types = new List<string>{ "C", "D" };

            var result = new Account()
                          {
                              Id = query.Id,
                              Balance = query.Balance,
                              Active = query.Active,
                              UserId = query.UserId,
                              CampaignId = query.CampaignId,
                              CreatedAt = query.CreatedAt,
                              CreationUserId = query.CreationUserId,
                              DeletedAt = query.DeletedAt,
                              DeletionUserId = query.DeletionUserId,
                              UpdateddAt = query.UpdateddAt,
                              UpdatedUserId = query.UpdatedUserId,
                              AccountMoviment = moviments.Where(x => string.IsNullOrEmpty(x.DeletedAt.ToString()) || !string.IsNullOrEmpty(x.OrderNumber) && types.Contains(x.Type)).OrderByDescending(x => x.CreatedAt).ToList(),
                          };

            return result;

            //result = (Account)query.GroupJoin(moviments,
            //       (x) => x.Id,
            //       (y) => y.AccountId,
            //       (x, y) =>
            //       {
            //           x.AccountMoviment = y.ToList();
            //           return x;
            //       });

            //return result;

        }

        public async Task<Account> GetByUser(string id)
        {
            return await _context
                           .Account
                           .Find(p => p.UserId == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<Account> GetByUserCpf(string cpf)
        {
            return await _context
                            .Account
                            .Find(p => p.Cpf == cpf)
                            .FirstOrDefaultAsync();
        }

        public async Task Create(Account obj)
        {
            await _context.Account.InsertOneAsync(obj, new InsertOneOptions() { });
        }

        public async Task<bool> Update(Account obj)
        {
            var updateResult = await _context
                                        .Account
                                        .ReplaceOneAsync(filter: g => g.Id == obj.Id, options: new ReplaceOptions { IsUpsert = true }, replacement: obj);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Account> filter = Builders<Account>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Account
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<Account>> List()
        {
            return await _context
                           .Account
                            .Find(p => true)
                            .ToListAsync();
        }
    }
}
