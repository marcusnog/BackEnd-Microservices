using MsProductIntegrationGiffty.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationGiffty.Contracts.Services
{
    public interface IIntegrationService
    {
        Task<IEnumerable<ProductPartner>> GetProducts();
        Task<IEnumerable<ProductPartner>> GetAvailabilities();
        IEnumerable<CategoriesPartner> GetCategories();
        Task<IEnumerable<StoreParceiro>> GetStores();
    }
}
