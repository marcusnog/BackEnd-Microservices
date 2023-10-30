using Ms.Api.Utilities.DTO;
using Ms.Api.Utilities.DTO.Response;
using Ms.Api.Utilities.Exceptions;
using Ms.Api.Utilities.Extensions;
using Ms.Api.Utilities.Models;
using MsOrderApi.Contracts.Services;
using System.Text;

namespace MsOrderApi.Services
{
    public class IntegrationRechargeService : IIntegrationRechargeService
    {
        readonly IConfiguration _configuration;
        readonly string _MS_INTEGRATION_RECHARGE_URL;

        public IntegrationRechargeService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _MS_INTEGRATION_RECHARGE_URL = _configuration.GetValue<string>("MS_INTEGRATION_RECHARGE_URL");
        }

        public async Task<DefaultResponse<CelcoinReserveBalanceResponse>> ConfirmPaymentRecharge(CellphoneRechargeRequest oRequest)
        {
            using HttpClient client = new HttpClient();

            var response = await client.PostAsync($"{_MS_INTEGRATION_RECHARGE_URL}/ConfirmPaymentRecharge",
                                                  new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(oRequest), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<CelcoinReserveBalanceResponse>>(await response.Content.ReadAsStringAsync());
        }
    }
}
