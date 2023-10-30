using Auth.Api.Contracts.Repositories;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Auth.Api.Services
{
    public class UserClientStore : IClientStore
    {
        readonly IIdentityRepository _identityRepository;
        public UserClientStore(IIdentityRepository identityRepository)
        {
            _identityRepository = identityRepository;
        }

        public async Task<Client?> FindClientByIdAsync(string clientId)
        {
            var client = await _identityRepository.FindClientById(clientId);
            if (client == null) return null;

            return new Client()
            {
                ClientId = client.ClientId,
                ClientSecrets = client.ClientSecrets?.Select(x => new Secret(x))?.ToArray(),
                AccessTokenType = (AccessTokenType)client.AccessTokenType,
                AllowedGrantTypes = client.AllowedGrantTypes,
                AllowedScopes = client.AllowedScopes,
            };
        }
    }
}
