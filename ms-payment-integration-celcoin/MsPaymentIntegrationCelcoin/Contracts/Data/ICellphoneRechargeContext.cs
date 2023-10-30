using MongoDB.Driver;
using MsPaymentIntegrationCelcoin.Contracts.DTO;

namespace MsPaymentIntegrationCelcoin.Contracts.Data
{
    public interface ICellphoneRechargeContext
    {
        public IMongoCollection<CellphoneRecharge> CellphoneRecharge { get; }
    }
}
