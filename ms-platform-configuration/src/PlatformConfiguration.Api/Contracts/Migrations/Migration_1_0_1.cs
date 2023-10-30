using Ms.Api.Utilities.Extensions;
using PlatformConfiguration.Api.Contracts.DTOs;
using PlatformConfiguration.Api.Contracts.Repositories;

namespace PlatformConfiguration.Api.Contracts.Migrations
{
    public static class Migration_1_0_1
    {
        static int _CAMPAIGN = 1;
        static int _PARTNER = 6;
        static int _STORE = 85;
        static Dictionary<string, string> _ids = new()
        {
            { "CAMPAIGN_FATOR1_CARTAO_PONTOS", (_CAMPAIGN).ToHex() },
            { "PARTNER_TODO", (++_PARTNER).ToHex() },
            { "STORE_TODO_TODO", (++_STORE).ToHex() },
        };
        public static async Task Apply(IConfiguration configuration, 
            ICampaignRepository campaignRepository, 
            IClientRepository clientRepository, 
            IPartnerRepository partnerRepository, 
            IStoreRepository storeRepository,  
            IDbVersionRepository dbVersionRepository)
        {
            Partner partner;
            Campaign campaign;
            Store store;

            campaign = await campaignRepository.Get(_ids["CAMPAIGN_FATOR1_CARTAO_PONTOS"]);

            // create partner & stores
            #region TODO

            partner = new Partner()
            {
                Id = _ids["PARTNER_TODO"],
                Name = "TODO Cartões",
                Active = true,
                AcceptCardPayment = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp()

            };
            await partnerRepository.Update(partner);

            // TODO -> TODO Cartões
            store = CreateStore(_ids["STORE_TODO_TODO"], "TODO Cartões", partner.Id, true, true, campaign.Id, null);
            await storeRepository.Update(store);
           

            #endregion

            await dbVersionRepository.SetVersion("1.0.1");
        }
        static Store CreateStore(string id, string name, string partnerId, bool acceptCardPayment, bool haveProducts, string campaignId, string storeCode)
        {
            return new ()
            {
                Id = id,
                Name = name,
                PartnerId = partnerId,
                AcceptCardPayment = acceptCardPayment,
                HaveProducts = haveProducts,
                CampaignConfiguration = storeCode == null ? new StoreCampaignConfiguration[] { } : new StoreCampaignConfiguration[]
                {
                    new StoreCampaignConfiguration()
                    {
                        CampaignId = campaignId,
                        StoreCode = storeCode
                    }
                },
                Active = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),

            };
        }
    }
}
