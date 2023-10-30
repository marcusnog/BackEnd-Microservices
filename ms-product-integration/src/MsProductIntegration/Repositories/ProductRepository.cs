using MongoDB.Bson;
using MongoDB.Driver;
using MsProductIntegration.Contracts.Data;
using MsProductIntegration.Contracts.DTOs;
using MsProductIntegration.Contracts.Repositories;
using MsProductIntegration.Extensions;
using System.Linq.Expressions;

namespace MsProductIntegration.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
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
        public async Task SetSku(string productId, Sku sku, bool insert = true)
        {
            var filter = Builders<Product<Sku>>.Filter.Eq(x => x.Id, productId);

            if (insert)
            {
                var update = Builders<Product<Sku>>.Update.Push("Skus", sku);
                await _context.Products.UpdateOneAsync(filter, update);
            }
            else
            {
                var update = Builders<Product<Sku>>.Update.Set("Skus.$[f]", sku);

                var arrayFilters = new[]
                {
                    new BsonDocumentArrayFilterDefinition<BsonDocument>(
                        new BsonDocument("f.StoreItemCode",
                                new BsonDocument("$in", new BsonArray(new [] { sku.StoreItemCode })))),
                };

                await _context.Products.UpdateOneAsync(filter, update, new UpdateOptions { ArrayFilters = arrayFilters, IsUpsert = insert });

            }
        }
    }
}
