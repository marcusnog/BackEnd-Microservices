using PlatformConfiguration.Api.Contracts.DTOs.Response;
using PlatformConfiguration.Api.Contracts.Repositories;
using PlatformConfiguration.Api.Contracts.UseCases;

namespace PlatformConfiguration.Api.UseCases
{
    public class GetPartnerConfigurationUseCase : IGetPartnerConfigurationUseCase
    {
        readonly IPartnerRepository _partnerRepository;
        readonly IStoreRepository _storeRepository;
        public GetPartnerConfigurationUseCase(IPartnerRepository partnerRepository, IStoreRepository storeRepository)
        {
            _partnerRepository = partnerRepository;
            _storeRepository = storeRepository;
        }
        public async Task<GetPartnerConfigurationResponse> Get(string partnerId, string storeId)
        {
            var partner = await _partnerRepository.Get(partnerId);
            if (partner == null)
                throw new ArgumentException($"Invalid partner ({partnerId})");

            var store = await _storeRepository.Get(storeId);
            if (store == null)
                throw new ArgumentException($"Invalid store ({storeId})");

            return new GetPartnerConfigurationResponse()
            {
                Id = partner.Id,
                Name = partner.Name,
                AcceptCardPayment = partner.AcceptCardPayment,
                Active = partner.Active,
                Stores = new GetPartnerConfigurationStore
                {
                   Id = store.Id,
                   Active = store.Active,
                   AcceptCardPayment = store.AcceptCardPayment,
                   CampaignConfiguration = store.CampaignConfiguration,
                   Name = store.Name
                }
            };
        }
    }
}
