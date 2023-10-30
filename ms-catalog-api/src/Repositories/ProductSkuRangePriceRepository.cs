using Catalog.Api.Contracts.Data;
using Catalog.Api.Contracts.DTOs;
using Catalog.Api.Contracts.Repositories;
using Catalog.Api.Extensions;
using MongoDB.Driver;

namespace Catalog.Api.Repositories
{
    public class ProductSkuRangePriceRepository : IProductSkuRangePriceRepository
    {
        private readonly ICatalogContext _context;

        public ProductSkuRangePriceRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product<SkuRangePrice>>> List()
        {
            return await _context
                            .ProductsRangePrice
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Product<SkuRangePrice>> Get(string id)
        {
            return await _context
                           .ProductsRangePrice
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Product<SkuRangePrice> Product)
        {
            await _context.ProductsRangePrice.InsertOneAsync(Product);
        }

        public async Task<bool> Update(Product<SkuRangePrice> Product)
        {
            var updateResult = await _context
                                        .ProductsRangePrice
                                        .ReplaceOneAsync(filter: g => g.Id == Product.Id, replacement: Product);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product<SkuRangePrice>> filter = Builders<Product<SkuRangePrice>>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .ProductsRangePrice
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
        public async Task<IEnumerable<Product<SkuRangePrice>>> Find(object filter)
        {
            var filterDefinition = MongoExtension.GenerateFilter<Product<SkuRangePrice>>(filter);
            return await _context.ProductsRangePrice.Find(filterDefinition).ToListAsync();
        }
    }
}
