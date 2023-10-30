using MsProductDetailsApi.Contracts.DTOs;

namespace MsProductDetailsApi.Contracts.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product<Sku>>> List(ProductFilter filter);
        //Task<List<Product>> Find(object filter);
    }
}
