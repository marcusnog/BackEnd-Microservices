using User.Api.Contracts.DTOs;
using User.Api.Contracts.Repositories;
using User.Api.Extensions;
using User.Api.Utils;

namespace User.Api.Contracts.Migrations
{
    public class Migration_1_0_14
    {
        public static async Task Apply(ISystemRepository systemRepository,
            IUserAdministratorRepository userAdministratorRepository,
            IProfileRepository profileRepository,
            IDbVersionRepository dbVersionRepository,
            IConfiguration configuration)
        {
            var systems = await systemRepository.List();

            var profile = new Profile()
            {
                IdSystem = systems.First(x => x.Name == "Admin").Id,
                Name = "ReadOnly",
                Abilities = new ProfileAbility[]
                {
                    new ProfileAbility()
                    {
                        action = "Relatorio",
                        subject = "Campanhas"
                    },
                    new ProfileAbility()
                    {
                        action = "Relatorio",
                        subject = "Participantes"
                    },
                    new ProfileAbility()
                    {
                        action = "Relatorio",
                        subject = "Pontos"
                    },
                    new ProfileAbility()
                    {
                        action = "Relatorio",
                        subject = "EstornoPontos"
                    },
                    new ProfileAbility()
                    {
                        action = "Relatorio",
                        subject = "Recargas"
                    },
                    new ProfileAbility()
                    {
                        action = "Relatorio",
                        subject = "Pagamentos"
                    },
                    new ProfileAbility()
                    {
                        action = "Relatorio",
                        subject = "Financeiro"
                    },
                    new ProfileAbility()
                    {
                        action = "Relatorio",
                        subject = "Pedidos"
                    }
                }
            };
            await profileRepository.Create(profile);

            await dbVersionRepository.SetVersion("1.0.14");
        }
    }
}
