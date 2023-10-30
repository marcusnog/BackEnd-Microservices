using Catalog.Api.Contracts.Data;
using Catalog.Api.Contracts.DTOs;
using Catalog.Api.Contracts.Repositories;
using MongoDB.Driver;
using Ms.Api.Utilities.Models;
using System.Linq.Expressions;

namespace Catalog.Api.Repositories
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

        public async Task<IEnumerable<SelectItem<string>>> ListAsOptions()
        {
            var items = await _context
                            .Categories
                            .Find(p => true)
                            .SortBy(e => e.Name)
                            .ToListAsync();

            return items.Select(x => new SelectItem<string>()
            {
                Value = x.Id,
                Label = x.Name
            });
        }
        public async Task<Category> Get(string id)
        {
            return await _context
                           .Categories
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<Category> GetByName(string name)
        {
            return await _context
                           .Categories
                           .Find(p => p.Name == name)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Category Category)
        {
            await _context.Categories.InsertOneAsync(Category);
        }

        public async Task Create(IEnumerable<Category> categories)
        {
            if (categories?.Any() != true) return;
            await _context.Categories.InsertManyAsync(categories);
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
        public IEnumerable<Category> Find(Expression<Func<Category, bool>> filter)
        {
            return _context
                           .Categories
                           .AsQueryable()
                           .Where(filter.Compile())
                           .ToList();
        }
    }
}
