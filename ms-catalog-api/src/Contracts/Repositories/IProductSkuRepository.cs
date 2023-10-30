using Catalog.Api.Contracts.DTOs;
using System.Linq.Expressions;

namespace Catalog.Api.Contracts.Repositories
{
    public interface IProductSkuRepository
    {
        Task<IEnumerable<Product<Sku>>> List();
        Task<Product<Sku>> Get(string id);
        Task Create(Product<Sku> Product);
        Task<bool> Update(Product<Sku> Product);
        Task<bool> Delete(string id);
        Task<IEnumerable<Product<Sku>>> Find(object filter);
    }
}
