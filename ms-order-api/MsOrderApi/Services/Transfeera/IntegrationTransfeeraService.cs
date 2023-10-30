using Ms.Api.Utilities.DTO;
using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.Exceptions;
using Ms.Api.Utilities.Extensions;
using Ms.Api.Utilities.Models;
using MsOrderApi.Contracts.Services;
using System.Text;

namespace MsOrderApi.Services
{
    public class IntegrationTransfeeraService : IIntegrationTransfeeraService
    {
        readonly IConfiguration _configuration;
        readonly string _MS_INTEGRATION_TRANSFEERA_URL;

        public IntegrationTransfeeraService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _MS_INTEGRATION_TRANSFEERA_URL = _configuration.GetValue<string>("MS_INTEGRATION_TRANSFEERA_URL");
        }

        public async Task<Billet> ConfirmPaymentBillet(BilletRequest oRequest)
        {
            using HttpClient client = new HttpClient();

            var response = await client.PostAsync($"{_MS_INTEGRATION_TRANSFEERA_URL}/ConfirmPaymentBillet",
                                                  new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(oRequest), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<Billet>(await response.Content.ReadAsStringAsync());
        }
    }
}
