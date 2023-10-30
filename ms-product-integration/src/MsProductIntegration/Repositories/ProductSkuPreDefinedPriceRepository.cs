using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MsProductIntegration.Contracts.Data;
using MsProductIntegration.Contracts.DTOs;
using MsProductIntegration.Contracts.Repositories;
using MsProductIntegration.Extensions;

namespace MsProductIntegration.Repositories
{
    public class ProductSkuPreDefinedPriceRepository : IProductSkuPreDefinedPriceRepository
    {
        private readonly ICatalogContext _context;

        public ProductSkuPreDefinedPriceRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product<SkuPreDefinedPrice>>> List()
        {
            return await _context
                            .ProductsPredefinedPrice
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Product<SkuPreDefinedPrice>> Get(string id)
        {
            return await _context
                           .ProductsPredefinedPrice
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Product<SkuPreDefinedPrice> Product)
        {
            await _context.ProductsPredefinedPrice.InsertOneAsync(Product);
        }

        public async Task<bool> Update(Product<SkuPreDefinedPrice> Product)
        {
            var updateResult = await _context
                                        .ProductsPredefinedPrice
                                        .ReplaceOneAsync(filter: g => g.Id == Product.Id, replacement: Product);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product<SkuPreDefinedPrice>> filter = Builders<Product<SkuPreDefinedPrice>>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .ProductsPredefinedPrice
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
        public async Task<IEnumerable<Product<SkuPreDefinedPrice>>> Find(object filter)
        {
            var filterDefinition = MongoExtension.GenerateFilter<Product<SkuPreDefinedPrice>>(filter);
            return await _context.ProductsPredefinedPrice.Find(filterDefinition).ToListAsync();
        }

        public async Task SetSkuPreDefinedPrice(string productId, SkuPreDefinedPrice sku, bool insert = true)
        {
            var filter = Builders<Product<SkuPreDefinedPrice>>.Filter.Eq(x => x.Id, productId);

            if (insert)
            {
                var update = Builders<Product<SkuPreDefinedPrice>>.Update.Push("Skus", sku);
                await _context.ProductsPredefinedPrice.UpdateOneAsync(filter, update);
            }
            else
            {
                var update = Builders<Product<SkuPreDefinedPrice>>.Update.Set("Skus.$[f]", sku);

                var arrayFilters = new[]
                {
                    new BsonDocumentArrayFilterDefinition<BsonDocument>(
                        new BsonDocument("f.StoreItemCode",
                                new BsonDocument("$in", new BsonArray(new [] { sku.StoreItemCode })))),
                };

                await _context.ProductsPredefinedPrice.UpdateOneAsync(filter, update, new UpdateOptions { ArrayFilters = arrayFilters, IsUpsert = insert });

            }
        }
    }
}
