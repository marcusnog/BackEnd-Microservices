using User.Api.Contracts.DTOs;
using User.Api.Contracts.Repositories;
using User.Api.Extensions;
using User.Api.Utils;

namespace User.Api.Contracts.Migrations
{
    public class Migration_1_0_12
    {
        public static async Task Apply(ISystemRepository systemRepository,
           IUserParticipantRepository userParticipantRepository,
           IProfileRepository profileRepository,
           IDbVersionRepository dbVersionRepository,
           IConfiguration configuration)
        {
            var users = await userParticipantRepository.List();
            foreach (var x in users)
            {
                await userParticipantRepository.Delete(x.Id);
            }

            var catalog = new DTOs.System()
            {
                Name = "Catalog",
                ContactEmail = "contato@reconhece.vc",
                RecoveryExpiration = 1,
                RecoveryUrl = "{0}/#/forgot-password/recovery?tokenId={1}",
                Url = "http://hmlreconhece.z15.web.core.windows.net",
                Logo = "{0}/static/media/logo_reconhece.png"
            };
            await systemRepository.Create(catalog);

            var profile = new Profile()
            {
                IdSystem = catalog.Id,
                Name = "Participant",
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
                await userParticipantRepository.Create(new DTOs.UserParticipant()
                {
                    Active = true,
                    CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                    Email = email,
                    IdProfile = profile.Id,
                    IdSystem = catalog.Id,
                    Login = email,
                    Password = "SZDPl8G1NUYby4fxnst3Yw==",
                    Nickname = $"{name[0].ToString().ToUpper()}{name[1..]}",
                    UserType = (int)Enums.UserType.Participant,
                    CampaignId = "000000000000000000000001"
                });
            }

            await dbVersionRepository.SetVersion("1.0.12");
        }
    }
}
