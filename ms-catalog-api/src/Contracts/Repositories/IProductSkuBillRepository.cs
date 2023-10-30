using Catalog.Api.Contracts.DTOs;

namespace Catalog.Api.Contracts.Repositories
{
    public interface IProductSkuBillRepository
    {
        Task<IEnumerable<Product<SkuBill>>> List();
        Task<Product<SkuBill>> Get(string id);
        Task Create(Product<SkuBill> Product);
        Task<bool> Update(Product<SkuBill> Product);
        Task<bool> Delete(string id);
        Task<IEnumerable<Product<SkuBill>>> Find(object filter);
    }
}
