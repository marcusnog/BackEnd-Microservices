using Catalog.Api.Contracts.DTOs;
using Catalog.Api.Contracts.DTOs.Response;

namespace Catalog.Api.Contracts.Repositories
{
    public interface IProductSkuPreDefinedPriceRepository
    {
        Task<IEnumerable<Product<SkuPreDefinedPrice>>> List();
        Task<Product<SkuPreDefinedPrice>> Get(string id);
        Task Create(Product<SkuPreDefinedPrice> Product);
        Task<bool> Update(Product<SkuPreDefinedPrice> Product);
        Task<bool> Delete(string id);
        Task<IEnumerable<Product<SkuPreDefinedPrice>>> Find(object filter);
        Task<IEnumerable<GetProductShowCase>> ListBestSellers(int page = 1, int pageSize = 10);
        Task<IEnumerable<GetProductShowCase>> ListOnSale(int page = 1, int pageSize = 10);
        Task<IEnumerable<GetProductShowCase>> ListProductsWithinPrice(SkuFilterPrice model, int page = 1, int pageSize = 10);
        Task<IEnumerable<Product<SkuPreDefinedPrice>>> FindGifttyProducts(List<Store> stores);
    }
}
