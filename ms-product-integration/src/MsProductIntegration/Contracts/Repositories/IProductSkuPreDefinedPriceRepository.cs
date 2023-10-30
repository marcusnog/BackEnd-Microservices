using MsProductIntegration.Contracts.DTOs;

namespace MsProductIntegration.Contracts.Repositories
{
    public interface IProductSkuPreDefinedPriceRepository
    {
        Task<IEnumerable<Product<SkuPreDefinedPrice>>> List();
        Task<Product<SkuPreDefinedPrice>> Get(string id);
        Task Create(Product<SkuPreDefinedPrice> Product);
        Task<bool> Update(Product<SkuPreDefinedPrice> Product);
        Task<bool> Delete(string id);
        Task<IEnumerable<Product<SkuPreDefinedPrice>>> Find(object filter);
        Task SetSkuPreDefinedPrice(string productId, SkuPreDefinedPrice sku, bool insert = true);
    }
}
