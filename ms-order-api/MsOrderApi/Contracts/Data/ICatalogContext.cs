using MongoDB.Driver;
using Ms.Api.Utilities.Contracts.DTOs;
using Ms.Api.Utilities.DTO;
using MsOrderApi.Contracts.DTOs;

namespace MsOrderApi.Contracts.Data
{
    public interface ICatalogContext
    {
        //public IMongoCollection<DbVersion> Versions { get; }
        public IMongoCollection<Order> Orders { get; }
    }

    //public interface IBilletContext
    //{
    //    public IMongoCollection<Billet> Billet { get; }
    //}
}
