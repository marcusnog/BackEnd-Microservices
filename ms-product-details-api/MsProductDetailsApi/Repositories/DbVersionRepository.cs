using MsProductDetailsApi.Contracts.Data;
using MsProductDetailsApi.Contracts.DTOs;
using MsProductDetailsApi.Contracts.Repositories;
using MsProductDetailsApi.Extensions;
using MongoDB.Driver;

namespace MsProductDetailsApi.Repositories
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
