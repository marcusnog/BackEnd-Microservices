using User.Api.Contracts.DTOs;
using User.Api.Contracts.Repositories;
using User.Api.Extensions;
using User.Api.Utils;

namespace User.Api.Contracts.Migrations
{
    public class Migration_1_0_13
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
                Name = "Master",
                Abilities = new ProfileAbility[]
                {
                    new ProfileAbility()
                    {
                        action = "*",
                        subject = "*"
                    }
                }
            };
            await profileRepository.Create(profile);

            var emails = new List<string>()
            {
                    "andre.araujo@reconhece.vc",
                    "wagner.santos@reconhece.vc",
                    "guilherme.henrique@reconhece.vc",
                    "leon@reconhece.vc",
                    "marcus@reconhece.vc",
                    "rafael.braga@reconhece.vc"
            };
            foreach (var email in emails)
            {
                var name = email.Split(".").First();
                await userAdministratorRepository.Create(new DTOs.UserAdministrator()
                {
                    Active = true,
                    CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                    Email = email,
                    IdProfile = profile.Id,
                    IdSystem = systems.First(x => x.Name == "Admin").Id,
                    Profile = profile,
                    System = systems.First(x => x.Name == "Admin"),
                    Login = email,
                    Password = "SZDPl8G1NUYby4fxnst3Yw==",
                    Nickname = $"{name[0].ToString().ToUpper()}{name[1..]}",
                    UserType = (int)Enums.UserType.Administrator
                });
            }

            await dbVersionRepository.SetVersion("1.0.13");
        }
    }
}
