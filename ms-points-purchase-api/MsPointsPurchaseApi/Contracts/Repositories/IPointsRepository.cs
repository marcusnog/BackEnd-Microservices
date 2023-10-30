using MsPointsPurchaseApi.Contracts.DTOs;

namespace MsPointsPurchaseApi.Contracts.Repositories
{
    public interface IPointsRepository
    {
        Task<Points> Get(string id);
        Task<Points> GetByValue(decimal value);
        Task Create(Points obj);
        Task<bool> Update(Points obj);
        Task<bool> Delete(string id);
        Task<IEnumerable<Points>> List();
    }
}
