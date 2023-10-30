using MongoDB.Driver;
using MsPointsPurchaseApi.Contracts.DTOs;

namespace MsPointsPurchaseApi.Contracts.Data
{
    public interface IPlatformConfigurationContext
    {
        public IMongoCollection<Points> Points { get; }
        public IMongoCollection<PointsPurchase> PointsPurchase { get; }
        public IMongoCollection<DbVersion> VersionsPointsPurchase { get; }
    }
}
