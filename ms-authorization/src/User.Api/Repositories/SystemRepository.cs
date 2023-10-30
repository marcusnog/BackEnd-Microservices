using User.Api.Contracts.DTOs;
using User.Api.Contracts.Repositories;
using MongoDB.Driver;
using System.Linq.Expressions;
using Ms.Api.Utilities.Models;

namespace User.Api.Repositories
{
    public class SystemRepository : ISystemRepository
    {
        private readonly IAuthContext _context;

        public SystemRepository(IAuthContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<SelectItem<string>>> ListAsOptions()
        {
            var items = await _context
                            .Systems
                            .Find(p => true)
                            .ToListAsync();

            return items.Select(x => new SelectItem<string>()
            {
                Value = x.Id,
                Label = x.Name
            });
        }
        public async Task<Contracts.DTOs.System> Get(string id)
        {
            return await _context
                           .Systems
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<Contracts.DTOs.System> GetByName(string name)
        {
            return await _context
                           .Systems
                           .Find(p => p.Name == name)
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Contracts.DTOs.System>> List()
        {
            return await _context
                            .Systems
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Contracts.DTOs.System> Find(Expression<Func<Contracts.DTOs.System, bool>> filter)
        {
            return await _context
                           .Systems
                           .Find(p => filter.Compile()(p))
                           .FirstOrDefaultAsync();
        }
        public async Task<Contracts.DTOs.System> FindByName(string name)
        {
            return await _context
                           .Systems
                           .Find(p => p.Name.ToLower() == name)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Contracts.DTOs.System System)
        {
            await _context.Systems.InsertOneAsync(System);
        }

        public async Task<bool> UpdateSystem(Contracts.DTOs.System System)
        {
            var updateResult = await _context
                                        .Systems
                                        .ReplaceOneAsync(filter: g => g.Id == System.Id, replacement: System);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Contracts.DTOs.System> filter = Builders<Contracts.DTOs.System>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Systems
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
