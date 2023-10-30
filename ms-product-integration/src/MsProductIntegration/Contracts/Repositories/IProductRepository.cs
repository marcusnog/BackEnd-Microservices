using MsProductIntegration.Contracts.DTOs;
using System.Linq.Expressions;

namespace MsProductIntegration.Contracts.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product<Sku>>> List();
        Task<Product<Sku>> Get(string id);
        Task Create(Product<Sku> Product);
        Task<bool> Update(Product<Sku> Product);
        Task<bool> Delete(string id);
        Task<IEnumerable<Product<Sku>>> Find(object filter);
        Task SetSku(string productId, Sku sku, bool insert = true);
    }
}
