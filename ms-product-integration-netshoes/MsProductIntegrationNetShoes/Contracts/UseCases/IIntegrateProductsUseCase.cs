using Microsoft.Extensions.Configuration;

namespace MsProductIntegrationNetShoes.Contracts.UseCases
{
    public interface IIntegrateProductsUseCase
    {
        Task IntegrateProducts();
        Task IntegratePrices();
        Task IntegrateAvailability();
    }
}
