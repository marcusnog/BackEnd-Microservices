using Microsoft.Extensions.Configuration;

namespace MsProductIntegrationGiffty.Contracts.UseCases
{
    public interface IIntegrateProductsUseCase
    {
        Task IntegrateProducts();
        //Task IntegratePrices();
        Task IntegrateAvailability();
        Task IntegrateStores();
    }
}
