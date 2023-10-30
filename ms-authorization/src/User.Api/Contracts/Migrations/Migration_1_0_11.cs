using User.Api.Contracts.DTOs;
using User.Api.Contracts.Repositories;
using User.Api.Extensions;
using User.Api.Utils;

namespace User.Api.Contracts.Migrations
{
    public class Migration_1_0_11
    {
        public static async Task Apply(ISystemRepository systemRepository,
            IUserAdministratorRepository userAdministratorRepository,
            IProfileRepository profileRepository,
            IDbVersionRepository dbVersionRepository,
            IConfiguration configuration)
        {
            var users = await userAdministratorRepository.List();
            foreach (var x in users)
            {
                await userAdministratorRepository.Delete(x.Id);
            }

            var systems = await systemRepository.List();
            foreach (var x in systems)
            {
                await systemRepository.Delete(x.Id);
            }

            var profiles = await profileRepository.List();
            foreach (var x in profiles)
            {
                await profileRepository.Delete(x.Id);
            }

            var admin = new DTOs.System()
            {
                Name = "Admin",
                ContactEmail = "contato@reconhece.vc",
                RecoveryExpiration = 1,
                RecoveryUrl = "{0}/#/forgot-password/recovery?tokenId={1}",
                Url = "http://hmlreconhece.z15.web.core.windows.net",
                Logo = "{0}/static/media/logo_reconhece.png"
            };
            await systemRepository.Create(admin);

            var profile = new Profile()
            {
                IdSystem = admin.Id,
                Name = "Administrator",
                Abilities = new ProfileAbility[]
                {
                    new ProfileAbility()
                    {
                        action = "Inserir",
                        subject = "Campanhas"
                    },
                    new ProfileAbility()
                    {
                        action = "Alterar",
                        subject = "Campanhas"
                    },
                    new ProfileAbility()
                    {
                        action = "Relatorio",
                        subject = "Campanhas"
                    }, new ProfileAbility()
                    {
                        action = "Inserir",
                        subject = "Participantes"
                    }, 
                    new ProfileAbility()
                    {
                        action = "Alterar",
                        subject = "Participantes"
                    },
                    new ProfileAbility()
                    {
                        action = "Excluir",
                        subject = "Participantes"
                    },
                    new ProfileAbility()
                    {
                        action = "Relatorio",
                        subject = "Participantes"
                    },
                    new ProfileAbility()
                    {
                        action = "Distribuir",
                        subject = "Pontos"
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
                        action = "Comprar",
                        subject = "Financeiro"
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

            //var emails = new List<string>()
            //{
            //    "andre.araujo@reconhece.vc",
            //    "wagner.santos@reconhece.vc",
            //    "guilherme.henrique@reconhece.vc",
            //    "leon@reconhece.vc",
            //    "marcus@reconhece.vc",
            //    "rafael.braga@reconhece.vc"

            //};
            //foreach (var email in emails)
            //{
            //    var name = email.Split(".").First();
            //    await userAdministratorRepository.Create(new DTOs.UserAdministrator()
            //    {
            //        Active = true,
            //        CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
            //        Email = email,
            //        IdProfile = profile.Id,
            //        IdSystem = admin.Id,
            //        Profile = profile,
            //        System = systems,
            //        Login = email,
            //        Password = "SZDPl8G1NUYby4fxnst3Yw==",
            //        Nickname = $"{name[0].ToString().ToUpper()}{name[1..]}",
            //        UserType = (int)Enums.UserType.Administrator
            //    });
            //}

            await dbVersionRepository.SetVersion("1.0.11");
        }
    }
}
