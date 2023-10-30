using MsPointsPurchaseApi.Contracts.DTOs;

namespace MsPointsPurchaseApi.Contracts.Repositories
{
    public interface IPointsPurchaseRepository
    {
        Task<PointsPurchase> Get(string id);
        Task<PointsPurchase> GetByUser(string userId);
        Task Create(PointsPurchase obj);
        Task<bool> Update(PointsPurchase obj);
        Task<bool> Delete(string id);
        Task<IEnumerable<PointsPurchase>> List();
    }
}
