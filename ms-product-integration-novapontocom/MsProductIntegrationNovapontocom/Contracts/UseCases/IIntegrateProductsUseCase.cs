using MsProductIntegrationNovapontocom.Contracts.Enums;

namespace MsProductIntegrationNovapontocom.Contracts.UseCases
{
    public interface IIntegrateProductsUseCase
    {
        Task IntegrateProducts(IntegrationType integrationType);
        Task IntegrateAvailability();
        Task IntegratePrices();
    }
}
