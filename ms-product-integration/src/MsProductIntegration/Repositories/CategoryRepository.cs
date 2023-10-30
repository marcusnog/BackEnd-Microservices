using MongoDB.Driver;
using MsProductIntegration.Contracts.Data;
using MsProductIntegration.Contracts.DTOs;
using MsProductIntegration.Contracts.Repositories;

namespace MsProductIntegration.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ICatalogContext _context;

        public CategoryRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Category>> List()
        {
            return await _context
                            .Categories
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Category> Get(string id)
        {
            return await _context
                           .Categories
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Category Category)
        {
            await _context.Categories.InsertOneAsync(Category);
        }

        public async Task<bool> Update(Category Category)
        {
            var updateResult = await _context
                                        .Categories
                                        .ReplaceOneAsync(filter: g => g.Id == Category.Id, replacement: Category);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Category> filter = Builders<Category>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Categories
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
