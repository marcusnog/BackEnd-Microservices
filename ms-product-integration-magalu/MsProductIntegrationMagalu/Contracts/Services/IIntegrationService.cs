using MsProductIntegrationMagalu.Contracts.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationMagalu.Contracts.Services
{
    public interface IIntegrationService
    {
        Task<ProductsAndCategoriesPartner> GetProductsAndCategories(IEnumerable<string> StoreCodes);
        Task<IEnumerable<AvailabilityPartner>> GetAvailabilities(IEnumerable<string> StoreCodes);
    }
}
