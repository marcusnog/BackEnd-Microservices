using User.Api.Contracts.DTOs;
using User.Api.Contracts.Repositories;
using MongoDB.Driver;
using Ms.Api.Utilities.Models;
using Ms.Api.Utilities.Extensions;

namespace User.Api.Repositories
{
    public class ProfileRepository : IProfileRepository
    {
        private readonly IAuthContext _context;

        public ProfileRepository(IAuthContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<SelectItem<string>>> ListAsOptions(string systemId)
        {
            if(!systemId.IsValidMongoID()) return Enumerable.Empty<SelectItem<string>>();

            var items = await _context
                            .Profiles
                            .Find(p => p.IdSystem == systemId)
                            .ToListAsync();
            return items.Select(x => new SelectItem<string>()
            {
                Value = x.Id,
                Label = x.Name
            });
        }
        public async Task<IEnumerable<Profile>> List()
        {
            return await _context 
                            .Profiles
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Profile> Get(string id)
        {
            return await _context
                           .Profiles
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<Profile> GetByName(string name)
        {
            return await _context
                           .Profiles
                           .Find(p => p.Name == name)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Profile Profile)
        {
            await _context.Profiles.InsertOneAsync(Profile);
        }

        public async Task<bool> UpdateProfile(Profile Profile)
        {
            var updateResult = await _context
                                        .Profiles
                                        .ReplaceOneAsync(filter: g => g.Id == Profile.Id, replacement: Profile);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Profile> filter = Builders<Profile>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .Profiles
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
    }
}
