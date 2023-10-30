using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace Auth.Api.Services
{
    public class ResourceStore : IResourceStore
    {
        public Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            return Task.FromResult(
                  new ApiResource[]
                   {
                   }.AsEnumerable());
        }

        public Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return Task.FromResult(
                  new ApiResource[]
                   {
                   }.AsEnumerable());
        }

        public Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            return Task.FromResult(
                  new ApiScope[]
                   {
                       new ApiScope("admin", "Admin"),
                       new ApiScope("catalog", "Catalog"),
                       new ApiScope("ms-authorization", "ms-authorization"),
                   }.AsEnumerable());
        }

        public Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return Task.FromResult(
                  new IdentityResource[]
                  {
                      new IdentityResources.OpenId(),
                      new IdentityResources.Profile(),
                      new IdentityResources.Address(),
                      new IdentityResources.Email()
                   }.AsEnumerable());
        }

        public Task<Resources> GetAllResourcesAsync()
        {
            return Task.FromResult(new Resources());
        }
    }
}
