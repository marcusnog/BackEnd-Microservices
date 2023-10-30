using PlatformConfiguration.Api.Contracts.DTOs.Response;

namespace PlatformConfiguration.Api.Contracts.UseCases
{
    public interface IGetPartnerConfigurationUseCase
    {
        Task<GetPartnerConfigurationResponse> Get(string partnerId, string storeId);
    }
}
