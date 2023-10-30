using MongoDB.Driver;
using MsPointsApi.Contracts.DTOs;

namespace MsPointsApi.Contracts.Data
{
    public interface IAuthContext
    {
        public IMongoCollection<Account> Account { get; }
        public IMongoCollection<Account_Moviment> Account_Moviment { get; }
    }
}
