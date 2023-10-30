using Catalog.Api.Contracts.Data;
using Catalog.Api.Contracts.DTOs;
using Catalog.Api.Contracts.Repositories;
using Catalog.Api.Extensions;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using System.Linq.Expressions;

namespace Catalog.Api.Repositories
{
    public class ProductSkuRepository : IProductSkuRepository
    {
        private readonly ICatalogContext _context;

        public ProductSkuRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product<Sku>>> List()
        {
            return await _context
                            .Products
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Product<Sku>> Get(string id)
        {
            return await _context
                           .Products
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Product<Sku> Product)
        {
            await _context.Products.InsertOneAsync(Product);
        }

        public async Task<bool> Update(Product<Sku> Product)
        {
            var updateResult = await _context
                                        .Products
                                        .ReplaceOneAsync(filter: g => g.Id == Product.Id, replacement: Product);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product<Sku>> filter = Builders<Product<Sku>>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Products
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
        public async Task<IEnumerable<Product<Sku>>> Find(object filter)
        {
            var filterDefinition = MongoExtension.GenerateFilter<Product<Sku>>(filter);
            return await _context.Products.Find(filterDefinition).ToListAsync();
        }
    }
}
