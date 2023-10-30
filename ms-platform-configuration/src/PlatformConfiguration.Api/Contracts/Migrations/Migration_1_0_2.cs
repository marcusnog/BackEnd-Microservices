using Ms.Api.Utilities.Extensions;
using PlatformConfiguration.Api.Contracts.DTOs;
using PlatformConfiguration.Api.Contracts.Repositories;
using System.Reflection.Metadata;

namespace PlatformConfiguration.Api.Contracts.Migrations
{
    public class Migration_1_0_2
    {
        static int _CLIENT = 0;

        static Dictionary<string, string> _ids = new()
        {
            { "CLIENT_INTERNO", (++_CLIENT).ToHex() },
            { "CLIENT_ARCOR", (++_CLIENT).ToHex() },
            { "CLIENT_NESTLE", (++_CLIENT).ToHex() },
        };

        public static async Task Apply(IConfiguration configuration,
            IClientRepository clientRepository,
            IDbVersionRepository dbVersionRepository)
        {
            var clients = await clientRepository.List();
            foreach (var x in clients)
            {
                await clientRepository.Delete(x.Id);
            }

            Client client;

            client = new Client()
            {
                Id = _ids["CLIENT_INTERNO"],
                Active = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                Name = "Interno",
                Documents = new DTOs.Document[]
                {
                    new ()
                    {
                        Type = "CNPJ",
                        Value = "71509837000109",
                    }
                }
            };
            await clientRepository.Update(client);

            // client
            client = new Client()
            {
                Id = _ids["CLIENT_ARCOR"],
                Active = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                Name = "ARCOR",
                Documents = new DTOs.Document[]
                {
                    new ()
                    {
                        Type = "CNPJ",
                        Value = "54360656001701",
                    }
                }
            };
            await clientRepository.Update(client);

            client = new Client()
            {
                Id = _ids["CLIENT_NESTLE"],
                Active = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                Name = "NESTLÉ",
                Documents = new DTOs.Document[]
                {
                    new ()
                    {
                        Type = "CNPJ",
                        Value = "60409075000152",
                    }
                }
            };
            await clientRepository.Update(client);


            await dbVersionRepository.SetVersion("1.0.2");
        }
    }
}
