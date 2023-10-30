using MongoDB.Driver;
using Ms.Api.Utilities.Extensions;
using PlatformConfiguration.Api.Contracts.Data;
using PlatformConfiguration.Api.Contracts.DTOs;
using PlatformConfiguration.Api.Contracts.Repositories;

namespace PlatformConfiguration.Api.Repositories
{
    public class DbVersionRepository : IDbVersionRepository
    {
        private readonly IPlataformConfigurationContext _context;

        public DbVersionRepository(IPlataformConfigurationContext context)
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
