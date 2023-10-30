using MongoDB.Driver;
using MsPaymentIntegrationTransfeera.Api.Contracts.Data;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO;

namespace MsPaymentIntegrationTransfeera.Api.Data
{
    public class BilletContext : IBilletContext
    {
        private readonly IMongoDatabase _db;
        public IMongoCollection<Billet> Billet => _db.GetCollection<Billet>("Billet");

        public BilletContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("DatabaseSettings:ConnectionString"));
            _db = client.GetDatabase(configuration.GetValue<string>("DatabaseSettings:DatabaseName"));
        }
    }
}
