using MsProductDetailsApi.Contracts.DTOs;

namespace MsProductDetailsApi.Contracts.Repositories
{
    public interface IProductSkuPreDefinedPriceRepository
    {
        Task<Product<SkuPreDefinedPrice>> Find(ProductFilter oProductFilter);
        Task<Product<SkuPreDefinedPrice>> Get(string id);
    }
}
