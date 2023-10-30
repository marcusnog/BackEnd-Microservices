using IdentityServer4.Models;
using IdentityServer4.Services;

namespace Auth.Api.Services
{
    public class ProfileService : IProfileService
    {

        public ProfileService()
        {
        }

        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            try
            {
                context.IssuedClaims = context.Subject.Claims.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{GetType().Name}: Error {ex}");
            }
            return Task.FromResult(0);
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.FromResult(0);
        }
    }
}
