using User.Api.Contracts.DTOs;
using User.Api.Contracts.Repositories;
using MongoDB.Driver;
using User.Api.Extensions;

namespace User.Api.Repositories
{
    public class PasswordRecoveryRepository : IPasswordRecoveryRepository
    {
        private readonly IAuthContext _context;

        public PasswordRecoveryRepository(IAuthContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task Create(PasswordRecovery obj)
        {
            await _context.PasswordRecovery.InsertOneAsync(obj);
        }
        public async Task<bool> Validate(string id)
        {
            if (!id.IsValidMongoID())
                return false;

            var now = DateTime.UtcNow.ToUnixTimestamp();
            var token = await _context
                           .PasswordRecovery
                           .Find(p => p.Id == id && now <= p.ExpiresIn && p.Active)
                           .FirstOrDefaultAsync();
            return token != null;
        }
        public async Task<bool> Validate(string id, string code)
        {
            var now = DateTime.UtcNow.ToUnixTimestamp();
            var token = await _context
                           .PasswordRecovery
                           .Find(p => p.Id == id && p.Code == code && now <= p.ExpiresIn && p.Active)
                           .FirstOrDefaultAsync();
            return token != null;
        }
        public async Task<bool> Delete(string id)
        {
            FilterDefinition<PasswordRecovery> filter = Builders<PasswordRecovery>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .PasswordRecovery
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
        public async Task<PasswordRecovery> Get(string id)
        {
            return await _context
                           .PasswordRecovery
                           .Find(p => p.Id == id && p.Active)
                           .FirstOrDefaultAsync();
        }
        public async Task<PasswordRecovery> GetByUser(string userId)
        {
            return await _context
                           .PasswordRecovery
                           .Find(p => p.UserId == userId && p.Active)
                           .FirstOrDefaultAsync();
        }
    }
}
