using Auth.Api.Contracts.DTOs;
using Auth.Api.Contracts.Repositories;
using Auth.Api.Contracts.Services;
using System.Net.Http.Headers;
using System.Text;

namespace Auth.Api.Services
{
    public class UserAuthorizationService : IUserAuthorizationService
    {
        readonly IIdentityRepository _identityRepository;
        readonly IConfiguration _configuration;
        readonly string _GET_USERINFO_URL;
        readonly string _GET_PARTICIPANTINFO_URL;
        readonly string _IDENTITYSERVER_URL;
        readonly string _MS_AUTHENTICATION_SECRET;
        DateTime? _lastTokenDate = null;
        string? _lastToken = null;
        public UserAuthorizationService(IConfiguration configuration, IIdentityRepository identityRepository)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _identityRepository = identityRepository ?? throw new ArgumentNullException(nameof(identityRepository));
            _GET_USERINFO_URL = _configuration.GetValue<string>("GET_USERINFO_URL");
            _GET_PARTICIPANTINFO_URL = _configuration.GetValue<string>("GET_PARTICIPANTINFO_URL");
            _IDENTITYSERVER_URL = _configuration.GetValue<string>("IDENTITYSERVER_URL");
            _MS_AUTHENTICATION_SECRET = _configuration.GetValue<string>("MS_AUTHENTICATION_SECRET");
        }
        public async Task<UserInfo?> GetUser(string login, string system)
        {
            HttpResponseMessage response = new();
            using HttpClient client = await getClient();

            if (system != "catalog")
                response = await client.PostAsync($"{_GET_USERINFO_URL}", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { login, system }), Encoding.UTF8, "application/json"));
            else
                response = await client.PostAsync($"{_GET_PARTICIPANTINFO_URL}", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new { login, system }), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                return null;

            return Newtonsoft.Json.JsonConvert.DeserializeObject<UserInfo>(await response.Content.ReadAsStringAsync());
        }

        async Task<HttpClient> getClient()
        {
            var client = new HttpClient();

            if (_lastTokenDate == null || _lastTokenDate.Value.AddHours(1) > DateTime.Now)
            {
                var identity = await _identityRepository.FindClientById("ms-authentication");
                if (identity == null) return client;

                var nvc = new List<KeyValuePair<string, string>>();
                nvc.Add(new KeyValuePair<string, string>("grant_type", identity.AllowedGrantTypes.FirstOrDefault() ?? ""));
                nvc.Add(new KeyValuePair<string, string>("client_id", identity.ClientId));
                nvc.Add(new KeyValuePair<string, string>("client_secret", _MS_AUTHENTICATION_SECRET));
                nvc.Add(new KeyValuePair<string, string>("scope", identity.AllowedScopes.FirstOrDefault() ?? ""));
                var req = new HttpRequestMessage(HttpMethod.Post, $"{_IDENTITYSERVER_URL}/connect/token") { Content = new FormUrlEncodedContent(nvc) };
                var res = await client.SendAsync(req);
                if (!res.IsSuccessStatusCode) return client;

                var token = Newtonsoft.Json.JsonConvert.DeserializeObject<Contracts.DTOs.Token>(await res.Content.ReadAsStringAsync());
                _lastToken = token.access_token;
                _lastTokenDate = DateTime.Now;
            }

            client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", _lastToken);
            return client;
        }
    }
}
