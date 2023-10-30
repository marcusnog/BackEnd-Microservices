using Ms.Api.Utilities.DTO;
using System.Linq.Expressions;

namespace MsPaymentIntegrationCelcoin.Contracts.Repository
{
    public interface ICellphoneRechargeRepository
    {
        Task<CellphoneRecharge> Get(string id);
        IEnumerable<CellphoneRecharge> Find(Expression<Func<CellphoneRecharge, bool>> filter);
        Task Create(CellphoneRecharge obj);
        Task<bool> Update(CellphoneRecharge obj);
        Task<bool> Delete(string id);
    }
}
