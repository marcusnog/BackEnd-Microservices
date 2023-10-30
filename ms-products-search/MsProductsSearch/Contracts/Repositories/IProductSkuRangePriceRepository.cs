using MsProductsSearch.Contracts.DTOs;

namespace MsProductsSearch.Contracts.Repositories
{
    public interface IProductSkuRangePriceRepository
    {
        Task<IEnumerable<Product<SkuRangePrice>>> List();
        Task<Product<SkuRangePrice>> Get(string id);
        Task<IEnumerable<Product<SkuRangePrice>>> Find(object filter);
    }
}
