using MsPointsApi.Contracts.DTOs;

namespace MsPointsApi.Contracts.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> Get(string id);
        Task<Account> GetByUser(string id);
        Task<Account> GetAccountMoviments(string id);
        Task<Account> GetByUserCpf(string cpf);
        Task Create(Account obj);
        Task<bool> Update(Account obj);
        Task<bool> Delete(string id);
    }
}
