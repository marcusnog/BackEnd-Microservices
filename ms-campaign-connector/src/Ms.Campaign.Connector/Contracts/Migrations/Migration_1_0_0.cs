using Ms.Campaign.Connector.Contracts.DTO;
using Ms.Campaign.Connector.Contracts.Repositories;
using Ms.Api.Utilities.Extensions;

namespace Ms.Campaign.Connector.Contracts.Migrations
{
    public static class Migration_1_0_0
    {
        static int idCampaignConnector = 0;
        public static async Task Apply(IDbVersionRepository dbVersionRepository,
            ICampaignConnectorRepository campaignConnectorRepository)
        {
            var clients = await campaignConnectorRepository.List();
            foreach (var x in clients)
            {
                await campaignConnectorRepository.Delete(x.Id);
            }
            #region HML
            await campaignConnectorRepository.Create(new CampaignConnector()
            {
                Id = (++idCampaignConnector).ToHex(),
                Environment = "hml",
                Campaign = "arcor",
                UserInfoEndpoint = "/reconhece/usuario",
                UserGetPointsEndpoint = "/reconhece/obter-pontos",
                UserBookPointsEndpoint = "/reconhece/reservar-pontos",
                UserDebitPointsEndpoint = "/reconhece/debitar-pontos",
                UserReleasePointsEndpoint = "/reconhece/liberar-pontos",
                UserReversePointsEndpoint = "/reconhece/estornar-pontos",
                ServiceBusUrl = "https://apihml.arcor.digi.ag",
            });
            await campaignConnectorRepository.Create(new CampaignConnector()
            {
                Id = (++idCampaignConnector).ToHex(),
                Environment = "hml",
                Campaign = "brasileirao",
                UserInfoEndpoint = "/reconhece/usuario",
                UserGetPointsEndpoint = "/reconhece/obter-pontos",
                UserBookPointsEndpoint = "/reconhece/reservar-pontos",
                UserDebitPointsEndpoint = "/reconhece/debitar-pontos",
                UserReleasePointsEndpoint = "/reconhece/liberar-pontos",
                UserReversePointsEndpoint = "/reconhece/estornar-pontos",
                ServiceBusUrl = "https://apihml.brasileirao.digi.ag",
            });
            await campaignConnectorRepository.Create(new CampaignConnector()
            {
                Id = (++idCampaignConnector).ToHex(),
                Environment = "hml",
                Campaign = "purina",
                UserInfoEndpoint = "/reconhece/usuario",
                UserGetPointsEndpoint = "/reconhece/obter-pontos",
                UserBookPointsEndpoint = "/reconhece/reservar-pontos",
                UserDebitPointsEndpoint = "/reconhece/debitar-pontos",
                UserReleasePointsEndpoint = "/reconhece/liberar-pontos",
                UserReversePointsEndpoint = "/reconhece/estornar-pontos",
                ServiceBusUrl = "https://apihml.purina.digi.ag",
            });
            await campaignConnectorRepository.Create(new CampaignConnector()
            {
                Id = (++idCampaignConnector).ToHex(),
                Environment = "hml",
                Campaign = "topodendo",
                UserInfoEndpoint = "/reconhece/usuario",
                UserGetPointsEndpoint = "/reconhece/obter-pontos",
                UserBookPointsEndpoint = "/reconhece/reservar-pontos",
                UserDebitPointsEndpoint = "/reconhece/debitar-pontos",
                UserReleasePointsEndpoint = "/reconhece/liberar-pontos",
                UserReversePointsEndpoint = "/reconhece/estornar-pontos",
                ServiceBusUrl = "https://apihmltopodendo.digi.ag",
            });
            await campaignConnectorRepository.Create(new CampaignConnector()
            {
                Id = (++idCampaignConnector).ToHex(),
                Environment = "hml",
                Campaign = "venca",
                UserInfoEndpoint = "/reconhece/usuario",
                UserGetPointsEndpoint = "/reconhece/obter-pontos",
                UserBookPointsEndpoint = "/reconhece/reservar-pontos",
                UserDebitPointsEndpoint = "/reconhece/debitar-pontos",
                UserReleasePointsEndpoint = "/reconhece/liberar-pontos",
                UserReversePointsEndpoint = "/reconhece/estornar-pontos",
                ServiceBusUrl = "https://apihml.eficienciavenca.digi.ag",
            });
            #endregion


            #region PRD
            await campaignConnectorRepository.Create(new CampaignConnector()
            {
                Id = (++idCampaignConnector).ToHex(),
                Environment = "prd",
                Campaign = "arcor",
                UserInfoEndpoint = "/reconhece/usuario",
                UserGetPointsEndpoint = "/reconhece/obter-pontos",
                UserBookPointsEndpoint = "/reconhece/reservar-pontos",
                UserDebitPointsEndpoint = "/reconhece/debitar-pontos",
                UserReleasePointsEndpoint = "/reconhece/liberar-pontos",
                UserReversePointsEndpoint = "/reconhece/estornar-pontos",
                ServiceBusUrl = "https://apiarcor.azurewebsites.net",
            });
            await campaignConnectorRepository.Create(new CampaignConnector()
            {
                Id = (++idCampaignConnector).ToHex(),
                Environment = "prd",
                Campaign = "brasileirao",
                UserInfoEndpoint = "/reconhece/usuario",
                UserGetPointsEndpoint = "/reconhece/obter-pontos",
                UserBookPointsEndpoint = "/reconhece/reservar-pontos",
                UserDebitPointsEndpoint = "/reconhece/debitar-pontos",
                UserReleasePointsEndpoint = "/reconhece/liberar-pontos",
                UserReversePointsEndpoint = "/reconhece/estornar-pontos",
                ServiceBusUrl = "https://api.brasileiraobrokers.com.br",
            });
            await campaignConnectorRepository.Create(new CampaignConnector()
            {
                Id = (++idCampaignConnector).ToHex(),
                Environment = "prd",
                Campaign = "purina",
                UserInfoEndpoint = "/reconhece/usuario",
                UserGetPointsEndpoint = "/reconhece/obter-pontos",
                UserBookPointsEndpoint = "/reconhece/reservar-pontos",
                UserDebitPointsEndpoint = "/reconhece/debitar-pontos",
                UserReleasePointsEndpoint = "/reconhece/liberar-pontos",
                UserReversePointsEndpoint = "/reconhece/estornar-pontos",
                ServiceBusUrl = "https://apipurinaprod.digi.ag",
            });
            await campaignConnectorRepository.Create(new CampaignConnector()
            {
                Id = (++idCampaignConnector).ToHex(),
                Environment = "prd",
                Campaign = "topodendo",
                UserInfoEndpoint = "/reconhece/usuario",
                UserGetPointsEndpoint = "/reconhece/obter-pontos",
                UserBookPointsEndpoint = "/reconhece/reservar-pontos",
                UserDebitPointsEndpoint = "/reconhece/debitar-pontos",
                UserReleasePointsEndpoint = "/reconhece/liberar-pontos",
                UserReversePointsEndpoint = "/reconhece/estornar-pontos",
                ServiceBusUrl = "https://api.incentivotopodendo.com.br",
            });
            await campaignConnectorRepository.Create(new CampaignConnector()
            {
                Id = (++idCampaignConnector).ToHex(),
                Environment = "prd",
                Campaign = "venca",
                UserInfoEndpoint = "/reconhece/usuario",
                UserGetPointsEndpoint = "/reconhece/obter-pontos",
                UserBookPointsEndpoint = "/reconhece/reservar-pontos",
                UserDebitPointsEndpoint = "/reconhece/debitar-pontos",
                UserReleasePointsEndpoint = "/reconhece/liberar-pontos",
                UserReversePointsEndpoint = "/reconhece/estornar-pontos",
                ServiceBusUrl = "https://api.eficienciavenca.com.br",
            });
            #endregion
            await dbVersionRepository.SetVersion("1.0.0");
        }
    }
}
