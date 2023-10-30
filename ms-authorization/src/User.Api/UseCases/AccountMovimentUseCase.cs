using Ms.Api.Utilities.DTO;
using Ms.Api.Utilities.Models;
using System.Text;
using User.Api.Contracts.DTOs.Request;
using User.Api.Contracts.UseCases;

namespace User.Api.UseCases
{
    public class AccountMovimentUseCase : IAccountMovimentUseCase
    {
        readonly string _MS_POINTS_URL;
        readonly IConfiguration _configuration;
        public AccountMovimentUseCase(IConfiguration configuration)
        {
            _MS_POINTS_URL = configuration.GetValue<string>("MS_POINTS_URL");
        }

        public async Task<string> CreditPoints(CreditPointsRequest request, string token)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);

            var response = await client.PostAsync($"{_MS_POINTS_URL}/AccountMoviment/CreditPoints", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<string>>(content);

            if (!response.IsSuccessStatusCode || !string.IsNullOrEmpty(result.Message))
                throw new Exception($"{result.Message}");

            return Convert.ToString(result);
        }

        public async Task<bool> DistributePoints(EffectDebitAdminRequest request, string token)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);

            var response = await client.PostAsync($"{_MS_POINTS_URL}/AccountMoviment/DistributePointsAdmin", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<bool>>(content);

            if (!response.IsSuccessStatusCode || !string.IsNullOrEmpty(result.Message))
                throw new Exception($"{result.Message}");

            return result.Data;
        }
    }
}
