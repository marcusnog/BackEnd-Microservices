using MongoDB.Driver;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.Data
{
    public interface IBilletContext
    {
        public IMongoCollection<Billet> Billet { get; }
    }
}
