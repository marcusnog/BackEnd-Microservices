using PlatformConfiguration.Api.Contracts.Data;
using PlatformConfiguration.Api.Contracts.DTOs;
using PlatformConfiguration.Api.Contracts.Repositories;
using MongoDB.Driver;
using System.Linq.Expressions;
using Ms.Api.Utilities.Models;
using MongoDB.Bson;

namespace PlatformConfiguration.Api.Repositories
{
    public class StoreRepository : IStoreRepository
    {
        private readonly IPlataformConfigurationContext _context;

        public StoreRepository(IPlataformConfigurationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Store> Get(string id)
        {
            return await _context
                           .Stores
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public IEnumerable<Store> Find(Expression<Func<Store, bool>> filter)
        {
            //var filterDefinition = Builders<Store>.Filter.Where(f => filter.Compile()(f));
            return _context
                           .Stores
                           .AsQueryable()
                           .Where(filter.Compile())
                           .ToList();
        }
        public async Task Create(Store obj)
        {
            await _context.Stores.InsertOneAsync(obj);
        }

        public async Task<bool> Update(Store obj)
        {
            var updateResult = await _context
                                        .Stores
                                        .ReplaceOneAsync(filter: g => g.Id == obj.Id, options: new ReplaceOptions { IsUpsert = true }, replacement: obj);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Store> filter = Builders<Store>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Stores
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<IEnumerable<SelectItem<string>>> ListAsOptions()
        {
            var items = await _context
                            .Stores
                            .Find(p => true && p.HaveProducts == true)
                            .ToListAsync();

            return items.Select(x => new SelectItem<string>()
            {
                Value = x.Id,
                Label = x.Name
            });
        }

        public async Task<IEnumerable<Store>> ListStoresGiftty()
        {
            var stores = from s in _context.Stores.AsQueryable()
                          join p in _context.Partners.AsQueryable()
                          on s.PartnerId equals p.Id
                          where s.Active == true && p.Name == "GIFTTY"
                         select new Store()
                          {
                              Id = s.Id,
                              AcceptCardPayment = s.AcceptCardPayment,
                              Active = s.Active,
                              Name = s.Name,
                              PartnerId = p.Id,
                              CreationDate = s.CreationDate,
                              DeletionDate = s.DeletionDate,
                              HaveProducts = s.HaveProducts,
                              CampaignConfiguration = s.CampaignConfiguration,
                          };

            return stores.AsEnumerable();
        }
    }
}
