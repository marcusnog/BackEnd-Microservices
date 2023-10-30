using Ms.Api.Utilities.Models;
using MsPaymentIntegrationTransfeera.Contracts.DTO.Request;
using MsPaymentIntegrationTransfeera.Contracts.UseCases;
using System.Text;

namespace MsPaymentIntegrationTransfeera.UseCases
{
    public class CampaignConnectorUseCases : ICampaignConnectorUseCases
    {
        readonly string _MS_CAMPAIGN_CONNECTOR_URL;
        public CampaignConnectorUseCases(IConfiguration configuration)
        {
            _MS_CAMPAIGN_CONNECTOR_URL = configuration.GetValue<string>("MS_CAMPAIGN_CONNECTOR_URL");
        }

        //Obter-Pontos
        public async Task<string?> GetPoints(CampaignUserRequest request)
        {
            using HttpClient client = new();
            var response = await client.PostAsync($"{_MS_CAMPAIGN_CONNECTOR_URL}/GetPoints", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            var userPoints = Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<string>>(await response.Content.ReadAsStringAsync());

            if (!response.IsSuccessStatusCode)
                throw new Exception(userPoints?.Message);

            return userPoints.Data;
        }

        //Reservar-Pontos
        public async Task<string?> BookPoints(CampaignUserRequest request)
        {
            using HttpClient client = new();
            var response = await client.PostAsync($"{_MS_CAMPAIGN_CONNECTOR_URL}/BookPoints", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            var userExtract = Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<string>>(await response.Content.ReadAsStringAsync());

            if (!response.IsSuccessStatusCode || !string.IsNullOrEmpty(userExtract.Message))
                throw new Exception($"Api Campanha - {userExtract.Message}");

            return userExtract.Data;
        }

        //Debitar-Pontos
        public async void EffectDebitPoints(CampaignUserRequest request)
        {
            using HttpClient client = new();

            var response = await client.PostAsync($"{_MS_CAMPAIGN_CONNECTOR_URL}/EffectDebitPoints", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new Exception("Não foi possível efetuar o debito dos pontos.");
        }

        //Liberar-Pontos
        public async void ReleasePoints(CampaignUserRequest request)
        {
            using HttpClient client = new();

            var response = await client.PostAsync($"{_MS_CAMPAIGN_CONNECTOR_URL}/ReleasePoints", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new Exception("Não foi possível liberar os pontos.");
        }

        //Estornar-Pontos
        public async void ReversePoints(CampaignUserRequest request)
        {
            using HttpClient client = new();

            var response = await client.PostAsync($"{_MS_CAMPAIGN_CONNECTOR_URL}/ReversePoints", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new Exception("Não foi possível estornar os pontos.");
        }
    }
}
