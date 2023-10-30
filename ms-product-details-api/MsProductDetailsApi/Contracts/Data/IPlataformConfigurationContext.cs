using MongoDB.Driver;
using MsProductDetailsApi.Contracts.DTOs;

namespace MsProductDetailsApi.Contracts.Data
{
    public interface IPlataformConfigurationContext
    {
        public IMongoCollection<Store> Stores { get; }
    }
}
