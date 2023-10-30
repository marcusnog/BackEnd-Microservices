using MongoDB.Driver;
using MsPaymentIntegrationCelcoin.Contracts.Data;
using MsPaymentIntegrationCelcoin.Contracts.DTO;

namespace MsPaymentIntegrationCelcoin.Data
{
    public class CellphoneRechargeContext : ICellphoneRechargeContext
    {
        private readonly IMongoDatabase _db;
        public IMongoCollection<CellphoneRecharge> CellphoneRecharge => _db.GetCollection<CellphoneRecharge>("CellphoneRecharge");

        public CellphoneRechargeContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            _db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        }
    }
}
