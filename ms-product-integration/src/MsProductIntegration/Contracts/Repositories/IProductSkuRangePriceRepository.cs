using Catalog.Api.Contracts.DTOs;
using MsProductIntegration.Contracts.DTOs;

namespace MsProductIntegration.Contracts.Repositories
{
    public interface IProductSkuRangePriceRepository
    {
        Task<IEnumerable<Product<SkuRangePrice>>> List();
        Task<Product<SkuRangePrice>> Get(string id);
        Task Create(Product<SkuRangePrice> Product);
        Task<bool> Update(Product<SkuRangePrice> Product);
        Task<bool> Delete(string id);
        Task<IEnumerable<Product<SkuRangePrice>>> Find(object filter);
        Task SetSkuRangePrice(string productId, SkuRangePrice sku, bool insert = true);
    }
}
