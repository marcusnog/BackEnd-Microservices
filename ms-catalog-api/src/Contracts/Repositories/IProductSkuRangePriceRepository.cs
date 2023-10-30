using Catalog.Api.Contracts.DTOs;

namespace Catalog.Api.Contracts.Repositories
{
    public interface IProductSkuRangePriceRepository
    {
        Task<IEnumerable<Product<SkuRangePrice>>> List();
        Task<Product<SkuRangePrice>> Get(string id);
        Task Create(Product<SkuRangePrice> Product);
        Task<bool> Update(Product<SkuRangePrice> Product);
        Task<bool> Delete(string id);
        Task<IEnumerable<Product<SkuRangePrice>>> Find(object filter);
    }
}
