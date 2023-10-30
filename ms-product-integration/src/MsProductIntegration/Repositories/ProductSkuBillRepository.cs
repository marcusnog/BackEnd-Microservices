using MongoDB.Bson;
using MongoDB.Driver;
using MsProductIntegration.Contracts.Data;
using MsProductIntegration.Contracts.DTOs;
using MsProductIntegration.Contracts.Repositories;
using MsProductIntegration.Extensions;

namespace MsProductIntegration.Repositories
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

        public async Task SetSkuBill(string productId, SkuBill sku, bool insert = true)
        {
            var filter = Builders<Product<SkuBill>>.Filter.Eq(x => x.Id, productId);

            if (insert)
            {
                var update = Builders<Product<SkuBill>>.Update.Push("Skus", sku);
                await _context.ProductsBill.UpdateOneAsync(filter, update);
            }
            else
            {
                var update = Builders<Product<SkuBill>>.Update.Set("Skus.$[f]", sku);

                var arrayFilters = new[]
                {
                    new BsonDocumentArrayFilterDefinition<BsonDocument>(
                        new BsonDocument("f.StoreItemCode",
                                new BsonDocument("$in", new BsonArray(new [] { sku.StoreItemCode })))),
                };

                await _context.ProductsBill.UpdateOneAsync(filter, update, new UpdateOptions { ArrayFilters = arrayFilters, IsUpsert = insert });

            }
        }
    }
}
