using Auth.Api.Contracts.DTOs;
using Auth.Api.Contracts.UserCases;
using Ms.Api.Utilities.Models;
using Newtonsoft.Json;
using System.Text;

namespace Auth.Api.UserCases
{
    public class CampaignConnectorLoginUseCase : ICampaignConnectorLoginUseCase
    {
        readonly string _MS_CAMPAIGN_CONNECTOR_URL;
        public CampaignConnectorLoginUseCase(IConfiguration configuration)
        {
            _MS_CAMPAIGN_CONNECTOR_URL = configuration.GetValue<string>("MS_CAMPAIGN_CONNECTOR_URL");
        }
        public async Task<CampaignInfo?> Login(string environment, string campaign, string token)
        {
            var campaignInfo = new DefaultResponse<CampaignInfo?>();
            using HttpClient client = new ();
            var response = await client.PostAsync($"{_MS_CAMPAIGN_CONNECTOR_URL}", new StringContent(JsonConvert.SerializeObject(new { environment, campaign, token }), Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                campaignInfo = JsonConvert.DeserializeObject<DefaultResponse<CampaignInfo?>?>(content);
            }
            else if (!response.IsSuccessStatusCode || campaignInfo?.Success != true)
            {
                var content = await response.Content.ReadAsStringAsync();
                var obj = JsonConvert.DeserializeObject<dynamic>(content);
                throw new Exception(obj?.message.ToString());
            }

            return campaignInfo?.Data;
        }
    }
}
