using Catalog.Api.Contracts.DTOs;
using System.Linq.Expressions;

namespace Catalog.Api.Contracts.Repositories
{
    public interface IProductRepository
    {
        Task<IEnumerable<Product<SkuPreDefinedPrice>>> List();
        Task<Product<SkuPreDefinedPrice>> Get(string id);
        Task Create(Product<SkuPreDefinedPrice> Product);
        Task<bool> Update(Product<SkuPreDefinedPrice> Product);
        Task<bool> Delete(string id);
        Task<IEnumerable<Product<SkuPreDefinedPrice>>> Find(object filter);
    }
}
