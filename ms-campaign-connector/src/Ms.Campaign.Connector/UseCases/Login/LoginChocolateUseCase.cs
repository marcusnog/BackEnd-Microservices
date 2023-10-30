using Ms.Api.Utilities.Models;
using Ms.Campaign.Connector.Contracts.DTO.Campaign;
using Ms.Campaign.Connector.Contracts.DTO.Request;
using Ms.Campaign.Connector.Contracts.DTO.Response;
using Ms.Campaign.Connector.Contracts.Enums;
using Ms.Campaign.Connector.Contracts.Repositories;
using Ms.Campaign.Connector.Contracts.UseCases.Login;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace Ms.Campaign.Connector.UseCases.Login
{
    public class LoginChocolateUseCase : LoginUseCase
    {
        public string[] campaigns => new string[] { "arcor", "topodendo", "purina", "venca", "brasileirao" };
        readonly ICampaignConnectorRepository _campaignConnectorRepository;
        readonly IRedisRepository _redisRepository;

        public LoginChocolateUseCase(IRedisRepository redisRepository, ICampaignConnectorRepository campaignConnectorRepository)
        {
            _campaignConnectorRepository = campaignConnectorRepository;
            _redisRepository = redisRepository;
        }

        public async Task<CampaignInfo> GetUser(string environment, string campaign, string token)
        {
            var connector = await _campaignConnectorRepository.Find(environment, campaign);
            if (connector == null)
                throw new ArgumentException("Api Campanha - Invalid environment/campaign");

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"{connector.ServiceBusUrl}{connector.UserInfoEndpoint}");
            var content = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                if ((int)response.StatusCode >= 500)
                    throw new Exception("Api Campanha - Campaign serviceBus unavailable");

                var error = JsonConvert.DeserializeObject<ChocolateErrorResponse>(content);
                throw new ArgumentException($"Api Campanha - {error.message}");
            }

            var userData = JsonConvert.DeserializeObject<ChocolateDefaultResponse<ChocolateUserData>>(content);

            if (!userData.sucesso)
                throw new ArgumentException(userData.mensagem);

            var saldo = await this.GetPoints(token, environment, campaign);

            if (saldo != userData.dados.saldo)
                userData.dados.saldo = saldo;

            var userRedis = new UserRedisRequest();
            userRedis.IdUser = userData.dados.id ?? userData.dados.userId;
            //userRedis.IdUser = "624209e86701d540c22585a1";
            userRedis.UserChocolate = userData.dados;
            userRedis.UserChocolate.internCampaign = false;

            var returnRedis = await _redisRepository.InsertUserRedis(userRedis);

            if (!returnRedis.Success)
                throw new Exception($"Api Campanha (Redis) - {returnRedis.Message}");

            return new CampaignInfo()
            {
                ClientId = userData.dados.id ?? userData.dados.userId,
                ClientSecrets = new string[] { userData.dados.userId },
                Claims = new List<CampaignClaim>()
                {
                    new CampaignClaim()
                    {
                        Type = "userId",
                        Value = userData.dados.id ?? userData.dados.userId,
                    },
                    new CampaignClaim()
                    {
                        Type = "campaign",
                        Value = campaign,
                    },
                }
            };

            //return new CampaignUserInformation(){
            //    AddressInfo = new CampaignUserAddress()
            //    {
            //        Address = userData.dados?.endereco,
            //        City = userData.dados?.cidade,
            //        State = userData.dados?.uf,
            //        ZipCode = userData.dados?.cep                    
            //    },
            //    Balance = decimal.Parse(userData?.dados?.saldo ?? "0", CultureInfo.InvariantCulture),
            //    DateOfBirth = userData.dados?.dataNascimento,
            //    Document = userData.dados?.cpf,
            //    DocumentType = (int)DocumentType.CPF,
            //    Email = userData.dados?.email,
            //    Name = userData.dados?.nome,
            //    Contact = new CampaignUserContactPhone[]
            //    {
            //        new CampaignUserContactPhone()
            //        {
            //            Number = userData.dados.telefone,
            //            Type = (int)PhoneType.Personal
            //        },
            //        new CampaignUserContactPhone()
            //        {
            //            Number = userData.dados.celular,
            //            Type = (int)PhoneType.Mobile
            //        }
            //    }.Where(x=> !string.IsNullOrEmpty(x.Number)).ToArray()
            //};
        }

        /// <summary>
        /// Obter Pontos
        /// </summary>
        /// <param name="token"></param>
        /// <param name="environment"></param>
        /// <param name="campaign"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<decimal> GetPoints(string token, string environment, string campaign)
        {
            var connector = await _campaignConnectorRepository.Find(environment, campaign);
            if (connector == null)
                throw new ArgumentException("Invalid environment/campaign");


            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await client.GetAsync($"{connector.ServiceBusUrl}{connector.UserGetPointsEndpoint}");
            var content = await response.Content.ReadAsStringAsync();

            var userData = JsonConvert.DeserializeObject<ChocolateDefaultResponse<ChocolateUserData>>(content);

            if (!userData.sucesso)
                throw new ArgumentException(userData.mensagem);

            return Convert.ToDecimal(userData.dados.saldo);
        }


        /// <summary>
        /// Reservar Pontos
        /// </summary>
        /// <param name="token"></param>
        /// <param name="environment"></param>
        /// <param name="campaign"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<ChocolateDefaultResponse<string>> BookPoints(string token, string environment, string? campaign, decimal? points)
        {
            var connector = await _campaignConnectorRepository.Find(environment, campaign);
            if (connector == null)
                throw new ArgumentException("Invalid environment/campaign");

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                pontos = Convert.ToInt32(Math.Ceiling((decimal)points)).ToString(new System.Globalization.CultureInfo("en-US"))
            });

            var response = await client.PostAsync($"{connector.ServiceBusUrl}{connector.UserBookPointsEndpoint}", new StringContent(requestBody, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            var userData = JsonConvert.DeserializeObject<ChocolateDefaultResponse<ChocolateUserData>>(content);

            if (!userData.sucesso)
            {
                var res = JsonConvert.DeserializeObject<ChocolateErrorResponse>(content);

                return new ChocolateDefaultResponse<string>()
                {
                    dados = null,
                    mensagem = res.error ?? userData.mensagem,
                    sucesso = false
                };
            }

            return new ChocolateDefaultResponse<string>()
            {
                dados = userData.dados.extratoId,
                mensagem = "",
                sucesso = true
            };
        }

        /// <summary>
        /// Efetivar Débito Pontos
        /// </summary>
        /// <param name="token"></param>
        /// <param name="environment"></param>
        /// <param name="campaign"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async void EffectDebitPoints(string token, string environment, string campaign, string? releaseCode, string? orderNumber)
        {
            var connector = await _campaignConnectorRepository.Find(environment, campaign);
            if (connector == null)
                throw new ArgumentException("Invalid environment/campaign");

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                extratoId = releaseCode,
                numeropedido = orderNumber
            });

            var response = await client.PostAsync($"{connector.ServiceBusUrl}{connector.UserDebitPointsEndpoint}", new StringContent(requestBody, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            var userData = JsonConvert.DeserializeObject<ChocolateDefaultResponse<ChocolateUserData>>(content);

            if (!userData.sucesso)
                throw new ArgumentException(userData.mensagem);
        }

        /// <summary>
        /// Liberar Pontos
        /// </summary>
        /// <param name="token"></param>
        /// <param name="environment"></param>
        /// <param name="campaign"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async void ReleasePoints(string token, string environment, string campaign, string? releaseCode)
        {
            var connector = await _campaignConnectorRepository.Find(environment, campaign);
            if (connector == null)
                throw new ArgumentException("Invalid environment/campaign");

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                extratoId = releaseCode
            });

            var response = await client.PostAsync($"{connector.ServiceBusUrl}{connector.UserReleasePointsEndpoint}", new StringContent(requestBody, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            var userData = JsonConvert.DeserializeObject<ChocolateDefaultResponse<ChocolateUserData>>(content);

            if (!userData.sucesso)
                throw new ArgumentException(userData.mensagem);
        }

        /// <summary>
        /// Estornar Pontos
        /// </summary>
        /// <param name="token"></param>
        /// <param name="environment"></param>
        /// <param name="campaign"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async void ReversePoints(string token, string environment, string campaign, string? points, string? requestNumber)
        {
            var connector = await _campaignConnectorRepository.Find(environment, campaign);
            if (connector == null)
                throw new ArgumentException("Invalid environment/campaign");

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var requestBody = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                points = points,
                requestNumber = requestNumber
            });

            var response = await client.PostAsync($"{connector.ServiceBusUrl}{connector.UserReversePointsEndpoint}", new StringContent(requestBody, Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            var userData = JsonConvert.DeserializeObject<ChocolateDefaultResponse<ChocolateUserData>>(content);

            if (!userData.sucesso)
                throw new ArgumentException(userData.mensagem);
        }
    }
}
