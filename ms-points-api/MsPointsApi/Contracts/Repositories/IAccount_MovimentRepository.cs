using MsPointsApi.Contracts.DTOs;

namespace MsPointsApi.Contracts.Repositories
{
    public interface IAccount_MovimentRepository
    {
        Task<Account_Moviment> Get(string id);
        Task Create(Account_Moviment obj);
        Task<bool> Update(Account_Moviment obj);
        Task<bool> Delete(string id);
    }
}
