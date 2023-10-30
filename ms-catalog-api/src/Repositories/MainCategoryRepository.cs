using Catalog.Api.Contracts.Data;
using Catalog.Api.Contracts.DTOs;
using Catalog.Api.Contracts.Repositories;
using MongoDB.Driver;
using Ms.Api.Utilities.Models;
using System.Linq.Expressions;

namespace Catalog.Api.Repositories
{
    public class MainCategoryRepository : IMainCategoryRepository
    {
        private readonly ICatalogContext _context;

        public MainCategoryRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<MainCategory>> List()
        {
            return await _context
                            .MainCategories
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<IEnumerable<SelectItem<string>>> ListAsOptions()
        {
            var items = await _context
                            .MainCategories
                            .Find(p => true)
                            .SortBy(e => e.Name)
                            .ToListAsync();

            return items.Select(x => new SelectItem<string>()
            {
                Value = x.Id,
                Label = x.Name
            });
        }
        public async Task<MainCategory> Get(string id)
        {
            return await _context
                           .MainCategories
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<MainCategory> GetByName(string name)
        {
            return await _context
                           .MainCategories
                           .Find(p => p.Name == name)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(MainCategory Category)
        {
            await _context.MainCategories.InsertOneAsync(Category);
        }

        public async Task Create(IEnumerable<MainCategory> categories)
        {
            if (categories?.Any() != true) return;
            await _context.MainCategories.InsertManyAsync(categories);
        }

        public async Task<bool> Update(MainCategory Category)
        {
            var updateResult = await _context
                                        .MainCategories
                                        .ReplaceOneAsync(filter: g => g.Id == Category.Id, replacement: Category);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<MainCategory> filter = Builders<MainCategory>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .MainCategories
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
        public IEnumerable<MainCategory> Find(Expression<Func<MainCategory, bool>> filter)
        {
            return _context
                           .MainCategories
                           .AsQueryable()
                           .Where(filter.Compile())
                           .ToList();
        }
    }
}
