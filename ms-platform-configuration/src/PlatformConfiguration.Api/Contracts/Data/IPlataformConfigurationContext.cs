using MongoDB.Driver;
using PlatformConfiguration.Api.Contracts.DTOs;

namespace PlatformConfiguration.Api.Contracts.Data
{
    public interface IPlataformConfigurationContext
    {
        public IMongoCollection<DbVersion> Versions { get; }
        public IMongoCollection<Campaign> Campaigns { get; }
        public IMongoCollection<Client> Clients { get; }
        public IMongoCollection<Partner> Partners { get; }
        public IMongoCollection<Store> Stores { get; }
    }
}
