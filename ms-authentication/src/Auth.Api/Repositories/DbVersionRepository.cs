using Auth.Api.Contracts.Data;
using Auth.Api.Contracts.DTOs;
using Auth.Api.Contracts.Repositories;
using MongoDB.Driver;
using Ms.Api.Utilities.Extensions;

namespace Auth.Api.Repositories
{
    public class DbVersionRepository : IDbVersionRepository
    {
        private readonly IIdentityContext _context;

        public DbVersionRepository(IIdentityContext context)
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
