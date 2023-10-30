using User.Api.Contracts.DTOs;

namespace User.Api.Contracts.Repositories
{
    public interface IPasswordRecoveryRepository
    {
        Task Create(PasswordRecovery obj);
        Task<bool> Validate(string id);
        Task<bool> Validate(string id, string code);
        Task<bool> Delete(string id);
        Task<PasswordRecovery> Get(string id);
        Task<PasswordRecovery> GetByUser(string userId);
    }
}
