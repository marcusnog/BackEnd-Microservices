using MsProductIntegrationNovapontocom.Contracts.DTOs;

namespace MsProductIntegrationNovapontocom.Contracts.Service
{
    public interface IIntegrationService
    {
        Task<IEnumerable<CategoryPartner>> GetCategories();
        Task<IEnumerable<ProductPartner>> GetProductsPartial();
        Task<IEnumerable<ProductPartner>> GetProducts();
        Task<IEnumerable<AvailabilityPartner>> GetAvailabilities();
    }
}
