using MsPaymentIntegrationTransfeera.Api.Contracts.DTO;
using System.Linq.Expressions;

namespace MsPaymentIntegrationCelcoin.Contracts.Repository
{
    public interface IBilletRepository
    {
        Task<Billet> Get(string id);
        IEnumerable<Billet> Find(Expression<Func<Billet, bool>> filter);
        Task Create(Billet obj);
        Task<bool> Update(Billet obj);
        Task<bool> Delete(string id);
    }
}
