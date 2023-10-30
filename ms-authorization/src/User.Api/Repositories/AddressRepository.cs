using MongoDB.Driver;
using User.Api.Contracts.DTOs.Response;
using User.Api.Contracts.Repositories;
using User.Contracts.Repositories;

namespace User.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly IAuthContext _context;

        public AddressRepository(IAuthContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Address> Get(string id)
        {
            return await _context
                            .Addresses
                            .Find(p => p.Id == id)
                            .FirstOrDefaultAsync();
        }

        public async Task<List<Address>> List(string userId)
        {
            return await _context
                            .Addresses
                            .Find(p => p.UserId == userId)
                            .ToListAsync();
        }
    }
}
