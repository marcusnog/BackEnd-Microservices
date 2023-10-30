using MsProductsSearch.Contracts.Data;
using MsProductsSearch.Contracts.DTOs;
using MsProductsSearch.Contracts.Repositories;
using MsProductsSearch.Extensions;
using MongoDB.Driver;

namespace MsProductsSearch.Repositories
{
    public class DbVersionRepository : IDbVersionRepository
    {
        private readonly ICatalogContext _context;

        public DbVersionRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<DbVersion> GetCurrentVersion()
        {
            return await _context
                            .Versions
                            .Find(p => true)
                            .SortByDescending(x => x.CreatedAt)
                            .FirstOrDefaultAsync();
        }

        public async Task SetVersion(string version)
        {
            await _context.Versions.InsertOneAsync(new DbVersion()
            {
                Name = version,
                CreatedAt = DateTime.UtcNow.ToUnixTimestamp()
            });
        }
    }
}
