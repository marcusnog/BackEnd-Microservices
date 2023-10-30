using Ms.Api.Utilities.Models;
using MsProductsSearch.Contracts.DTOs;

namespace MsProductsSearch.Contracts.Repositories
{
    public interface IProductSkuPreDefinedPriceRepository
    {
        Task<QueryPage<IEnumerable<GetProductShowCase>>> List(ProductFilter oProductFilter, int page = 0, int pageSize = 10);
        Task<Product<SkuPreDefinedPrice>> Get(string id);
        Task<IEnumerable<Product<SkuPreDefinedPrice>>> Find(object filter);
    }
}
