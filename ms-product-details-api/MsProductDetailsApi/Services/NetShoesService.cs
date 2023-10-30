using Ms.Api.Utilities.Exceptions;
using Ms.Api.Utilities.Extensions;
using Ms.Api.Utilities.Models;
using MsProductDetailsApi.Contracts.DTOs;
using MsProductDetailsApi.Contracts.Services;
using System.Text;

namespace MsProductDetailsApi.Services
{
    public class NetShoesService : INetShoesService
    {
        //readonly IIdentityRepository _identityRepository;
        readonly IConfiguration _configuration;
        readonly string _MS_ORDER_INTEGRATION_NETSHOES_URL;
        //readonly string _GET_USERINFO_URL;
        //readonly string _IDENTITYSERVER_URL;
        //readonly string _MS_AUTHENTICATION_SECRET;
        //DateTime? _lastTokenDate = null;
        //string? _lastToken = null;

        //public NetShoesService(IConfiguration configuration, IIdentityRepository identityRepository)
        //{
        //    _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        //    _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
        //    _GET_USERINFO_URL = _configuration.GetValue<string>("GET_USERINFO_URL");
        //    _IDENTITYSERVER_URL = _configuration.GetValue<string>("IDENTITYSERVER_URL");
        //    _MS_AUTHENTICATION_SECRET = _configuration.GetValue<string>("MS_AUTHENTICATION_SECRET");
        //}

        public NetShoesService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _MS_ORDER_INTEGRATION_NETSHOES_URL = _configuration.GetValue<string>("MS_ORDER_INTEGRATION_NETSHOES_URL");
        }

        public async Task<DefaultResponse<CalcShippingResponse>> GetShippingValue(CalcShipping request)
        {
            using HttpClient client = new HttpClient();

            var response = await client.PostAsync($"{_MS_ORDER_INTEGRATION_NETSHOES_URL}", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request),
                Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_netshoes}.{1.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<CalcShippingResponse>>(await response.Content.ReadAsStringAsync());
        }

        //async Task<HttpClient> getClient()
        //{
        //    var client = new HttpClient();

        //    if (_lastTokenDate == null || _lastTokenDate.Value.AddHours(1) > DateTime.Now)
        //    {
        //        var identity = await _identityRepository.FindClientById("ms-authentication");
        //        if (identity == null) return client;

        //        var nvc = new List<KeyValuePair<string, string>>();
        //        nvc.Add(new KeyValuePair<string, string>("grant_type", identity.AllowedGrantTypes.FirstOrDefault() ?? ""));
        //        nvc.Add(new KeyValuePair<string, string>("client_id", identity.ClientId));
        //        nvc.Add(new KeyValuePair<string, string>("client_secret", _MS_AUTHENTICATION_SECRET));
        //        nvc.Add(new KeyValuePair<string, string>("scope", identity.AllowedScopes.FirstOrDefault() ?? ""));
        //        var req = new HttpRequestMessage(HttpMethod.Post, $"{_IDENTITYSERVER_URL}/connect/token") { Content = new FormUrlEncodedContent(nvc) };
        //        var res = await client.SendAsync(req);
        //        if (!res.IsSuccessStatusCode) return client;

        //        var token = Newtonsoft.Json.JsonConvert.DeserializeObject<Contracts.DTOs.Token>(await res.Content.ReadAsStringAsync());
        //        _lastToken = token.access_token;
        //        _lastTokenDate = DateTime.Now;
        //    }

        //    client.DefaultRequestHeaders.Authorization =
        //            new AuthenticationHeaderValue("Bearer", _lastToken);
        //    return client;
        //}
        //}
    }
}
