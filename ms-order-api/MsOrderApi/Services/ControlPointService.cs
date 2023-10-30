using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.Exceptions;
using Ms.Api.Utilities.Extensions;
using Ms.Api.Utilities.Models;
using MsOrderApi.Contracts.Services;
using System.Text;

namespace MsOrderApi.Services
{
    public class ControlPointInternalService : IControlPointInternalService
    {
        readonly IConfiguration _configuration;
        readonly string _URL;

        public ControlPointInternalService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _URL = _configuration.GetValue<string>("MS_POINTS_URL");
        }

        public async Task<DefaultResponse<string>> EffetiveDebit(EffectDebitRequest request)
        {
            using HttpClient client = new HttpClient();

            var response = await client.PostAsync($"{_URL}/EffetiveDebit",
                                                  new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<DefaultResponse<string>> Booking(CreateReserveMovimentRequest request)
        {
            using HttpClient client = new HttpClient();

            var response = await client.PostAsync($"{_URL}/Booking",
                                                  new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<DefaultResponse<bool>> ReleasePoints(ReleasePointsRequest request)
        {
            using HttpClient client = new HttpClient();

            var response = await client.PostAsync($"{_URL}/ReleasePoints",
                                                  new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<bool>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<DefaultResponse<string>> ReversePoints(ReversePointsRequest request)
        {
            using HttpClient client = new HttpClient();

            var response = await client.PostAsync($"{_URL}/ReversePoints",
                                                  new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<string>>(await response.Content.ReadAsStringAsync());
        }

    }

    public class ControlPointExternalService : IControlPointExternalService
    {
        readonly IConfiguration _configuration;
        readonly string _URL;

        public ControlPointExternalService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _URL = _configuration.GetValue<string>("MS_CAMPAIGN_CONNECTOR_URL");
        }

        public async Task<DefaultResponse<string>> BookPoints(CampaignUserRequest request)
        {
            using HttpClient client = new HttpClient();

            var response = await client.PostAsync($"{_URL}/BookPoints",
                                                  new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<DefaultResponse<string>> EffectDebitPoints(CampaignUserRequest request)
        {
            using HttpClient client = new HttpClient();

            var response = await client.PostAsync($"{_URL}/EffectDebitPoints",
                                                  new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<string>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<DefaultResponse<bool>> ReleasePoints(CampaignUserRequest request)
        {
            using HttpClient client = new HttpClient();

            var response = await client.PostAsync($"{_URL}/ReleasePoints",
                                                  new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<bool>>(await response.Content.ReadAsStringAsync());
        }

        public async Task<DefaultResponse<string>> ReversePoints(CampaignUserRequest request)
        {
            using HttpClient client = new HttpClient();

            var response = await client.PostAsync($"{_URL}/ReversePoints",
                                                  new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<string>>(await response.Content.ReadAsStringAsync());
        }

    }

}
