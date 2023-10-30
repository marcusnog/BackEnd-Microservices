using User.Api.Contracts.DTOs;
using User.Api.Contracts.Repositories;
using MongoDB.Driver;

namespace User.Api.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly IAuthContext _context;

        public RoleRepository(IAuthContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Role>> ListRoles()
        {
            return await _context
                            .Roles
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Role> GetRole(RoleFilter filter)
        {
            return await _context
                           .Roles
                           .Find(p => p.Id == filter.Id)
                           .FirstOrDefaultAsync();
        }

        public async Task CreateRole(Role Role)
        {
            await _context.Roles.InsertOneAsync(Role);
        }

        public async Task<bool> UpdateRole(Role Role)
        {
            var updateResult = await _context
                                        .Roles
                                        .ReplaceOneAsync(filter: g => g.Id == Role.Id, replacement: Role);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> DeleteRole(RoleFilter Role)
        {
            FilterDefinition<Role> filter = Builders<Role>.Filter.Eq(p => p.Id, Role.Id);

            DeleteResult deleteResult = await _context
                                                .Roles
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
