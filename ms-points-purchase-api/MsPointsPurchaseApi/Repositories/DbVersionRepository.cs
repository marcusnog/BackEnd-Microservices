using MongoDB.Driver;
using Ms.Api.Utilities.Extensions;
using MsPointsPurchaseApi.Contracts.Data;
using MsPointsPurchaseApi.Contracts.DTOs;
using MsPointsPurchaseApi.Contracts.Repositories;

namespace MsPointsPurchaseApi.Repositories
{
    public class DbVersionRepository : IDbVersionRepository
    {
        private readonly IPlatformConfigurationContext _context;

        public DbVersionRepository(IPlatformConfigurationContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<DbVersion> GetCurrentVersion()
        {
            return await _context
                            .VersionsPointsPurchase
                            .Find(p => true)
                            .SortByDescending(x => x.CreatedAt)
                            .FirstOrDefaultAsync();
        }

        public async Task SetVersion(string version)
        {
            await _context.VersionsPointsPurchase.InsertOneAsync(new DbVersion()
            {
                Name = version,
                CreatedAt = DateTime.UtcNow.ToUnixTimestamp()
            });
        }
    }
}
