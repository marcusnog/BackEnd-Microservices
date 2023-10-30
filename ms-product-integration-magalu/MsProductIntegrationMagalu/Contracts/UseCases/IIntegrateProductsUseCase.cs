using Microsoft.Extensions.Configuration;

namespace MsProductIntegrationMagalu.Contracts.UseCases
{
    public interface IIntegrateProductsUseCase
    {
        Task IntegrateProducts();
        Task IntegratePrices();
        Task IntegrateAvailability();
    }
}
