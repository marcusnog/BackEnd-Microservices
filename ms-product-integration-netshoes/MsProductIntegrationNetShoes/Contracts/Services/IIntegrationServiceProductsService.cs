using MsProductIntegrationNetShoes.Contracts.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MsProductIntegrationNetShoes.Contracts.Services
{
    public interface IIntegrationServiceProductsService
    {
        Task<Dictionary<string, ProductsDB>> GetProductsDB();
        Task<IEnumerable<CategoryDB>> GetCategoriesDB();
    }
}
