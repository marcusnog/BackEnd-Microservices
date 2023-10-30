using Auth.Api.Contracts.DTOs;
using IdentityServer4.Models;

namespace Auth.Api.Contracts.Services
{
    public interface IUserAuthorizationService
    {
        Task<UserInfo?> GetUser(string login, string system);
    }
}
