using Catalog.Api.Contracts.Data;
using Catalog.Api.Contracts.DTOs;
using Catalog.Api.Contracts.Repositories;
using Catalog.Api.Extensions;
using MongoDB.Driver;
using System.Linq.Expressions;

namespace Catalog.Api.Repositories
{
    public class ProductSkuBillRepository : IProductSkuBillRepository
    {
        private readonly ICatalogContext _context;

        public ProductSkuBillRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product<SkuBill>>> List()
        {
            return await _context
                            .ProductsBill
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Product<SkuBill>> Get(string id)
        {
            return await _context
                           .ProductsBill
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Product<SkuBill> Product)
        {
            await _context.ProductsBill.InsertOneAsync(Product);
        }

        public async Task<bool> Update(Product<SkuBill> Product)
        {
            var updateResult = await _context
                                        .ProductsBill
                                        .ReplaceOneAsync(filter: g => g.Id == Product.Id, replacement: Product);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product<SkuBill>> filter = Builders<Product<SkuBill>>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .ProductsBill
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
        public async Task<IEnumerable<Product<SkuBill>>> Find(object filter)
        {
            var filterDefinition = MongoExtension.GenerateFilter<Product<SkuBill>>(filter);
            return await _context.ProductsBill.Find(filterDefinition).ToListAsync();
        }
    }
}
