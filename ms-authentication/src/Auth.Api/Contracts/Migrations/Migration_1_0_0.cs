using Auth.Api.Contracts.DTOs;
using Auth.Api.Contracts.Repositories;

namespace Auth.Api.Contracts.Migrations
{
    public static class Migration_1_0_0
    {
        public static async Task Apply(IDbVersionRepository dbVersionRepository,
            IIdentityRepository clientRepository)
        {
            var clients = await clientRepository.List();
            foreach (var x in clients)
            {
                await clientRepository.Delete(x.Id);
            }
            await clientRepository.Create(new IdentityClient()
            {
                ClientId = "plataform-admin",
                ClientSecrets = new[] { "uplBXYYRO/tRUYt6tfV4NeXeyU6Fls7nb9d0aRCZcdY=" },
                AccessTokenType = 0,
                AllowedGrantTypes = new[] { "password" },
                AllowedScopes = new[] { "admin", "openid" },
            });
            await clientRepository.Create(new IdentityClient()
            {
                ClientId = "plataform-catalog",
                ClientSecrets = new[] { "uplBXYYRO/tRUYt6tfV4NeXeyU6Fls7nb9d0aRCZcdY=" },
                AccessTokenType = 0,
                AllowedGrantTypes = new[] { "password", "token" },
                AllowedScopes = new[] { "catalog", "openid" },
            });
            await clientRepository.Create(new IdentityClient()
            {
                ClientId = "ms-authentication",
                ClientSecrets = new[] { "Ppl7Q7frfBqmSuECBZ3Iv1gbeaKkn0FdMzVE2wBELyc=" },
                AccessTokenType = 0,
                AllowedGrantTypes = new[] { "client_credentials", "token" },
                AllowedScopes = new[] { "ms-authorization", "openid" },
            });
            await dbVersionRepository.SetVersion("1.0.1");
        }
    }
}
