using MsProductDetailsApi.Contracts.DTOs;

namespace MsProductDetailsApi.Contracts.Repositories
{
    public interface IProductSkuRangePriceRepository
    {
        Task<IEnumerable<Product<SkuRangePrice>>> List();
        Task<Product<SkuRangePrice>> Get(string id);
        Task<IEnumerable<Product<SkuRangePrice>>> Find(object filter);
    }
}
