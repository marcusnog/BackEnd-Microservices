using Amazon.Runtime.Internal;
using Ms.Api.Utilities.Models;
using System.Text;
using User.Api.Contracts.DTOs.Request;
using User.Api.Contracts.UseCases;

namespace User.Api.UseCases
{
    public class AccountUseCase : IAccountUseCase
    {
        readonly string _MS_POINTS_URL;
        readonly IConfiguration _configuration;
        public AccountUseCase(IConfiguration configuration)
        {
            _MS_POINTS_URL = configuration.GetValue<string>("MS_POINTS_URL");
        }

        public async Task<Account> GetAccount(string userId, string token)
        {
            using var client = new HttpClient();

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Add("Authorization", token);

            var response = await client.GetAsync($"{_MS_POINTS_URL}/Account/GetByUserId/{userId}");
            var content = await response.Content.ReadAsStringAsync();

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<Account>>(content);

            if (!response.IsSuccessStatusCode || !string.IsNullOrEmpty(result.Message))
                throw new Exception($"{result.Message}");

            return result.Data;
        }

        public async Task<Account> CreateAccount(CreateAccountRequest request, string token)
        {
            using var client = new HttpClient();

            if (!string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Add("Authorization", token);

            var response = await client.PostAsync($"{_MS_POINTS_URL}/Account/Create", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<Account>>(content);

            if (!response.IsSuccessStatusCode || !string.IsNullOrEmpty(result.Message))
                throw new Exception($"{result.Message}");

            return result.Data;
        }

        public async Task<decimal> GetBalancePoints(string cpf, string token)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);

            var response = await client.GetAsync($"{_MS_POINTS_URL}/Account/GetPoints/{cpf}");
            var content = await response.Content.ReadAsStringAsync();

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<decimal>(content);

            return result;
        }

        public async Task<DefaultResponse<bool>> UpdateBalance(UpdateBalanceRequest request, string token)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);

            var response = await client.PostAsync($"{_MS_POINTS_URL}/Account/UpdateBalance", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<bool>>(content);

            return result;
        }
    }
}
