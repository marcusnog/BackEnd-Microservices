using MongoDB.Driver;
using MsProductsSearch.Contracts.DTOs;

namespace MsProductsSearch.Contracts.Data
{
    public interface IPlataformConfigurationContext
    {
        public IMongoCollection<Store> Stores { get; }
    }
}
