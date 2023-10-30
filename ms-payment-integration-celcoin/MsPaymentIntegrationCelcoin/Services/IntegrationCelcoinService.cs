using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.DTO.Response;
using Ms.Api.Utilities.Models;
using MsPaymentIntegrationCelcoin.Contracts.Services;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace MsPaymentIntegrationCelcoin.Services
{
    public class IntegrationCelcoinService : IIntegrationCelcoinService
    {
        readonly IConfiguration _configuration;
        readonly string _urlApi;
        readonly string _userAgent;
        readonly HttpClient _httpClient = null;

        public IntegrationCelcoinService(IConfiguration configuration = null)
        {
            _configuration = configuration;
            _urlApi = _configuration.GetValue<string>("Celcoin:UrlApi");
            _userAgent = _configuration.GetValue<string>("Celcoin:UserAgent");
        }

        public IntegrationCelcoinService(HttpClient httpClient, IConfiguration configuration = null) : this(configuration)
        {
            _httpClient = httpClient;
        }

        private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        HttpClient GetHttpClient(string token = null)
        {
            var client = _httpClient ?? new HttpClient();
            if (!client.DefaultRequestHeaders.Contains("Authorization") && !string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            if (!client.DefaultRequestHeaders.Contains("Accept"))
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        /// <summary>
        /// https://developers.celcoin.com.br/docs/obtendo-acesso-%C3%A0s-apis
        /// </summary>
        /// <returns></returns>
        public async Task<CelcoinAuthResponse> GetAuthentication()
        {
            var client = GetHttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);

            CelcoinAuthResponse retorno;

            var model = new CelcoinAuthRequest
            {
                client_id = _configuration.GetValue<string>("Celcoin:ClientId"),
                client_secret = _configuration.GetValue<string>("Celcoin:ClientSecret"),
                grant_type = _configuration.GetValue<string>("Celcoin:GrantType")
            };

            var request = new HttpRequestMessage
            {
                Content = new MultipartFormDataContent
                {
                    new StringContent(model.grant_type, Encoding.UTF8, "application/json")
                    {
                        Headers =
                        {
                            ContentDisposition = new ContentDispositionHeaderValue("form-data")
                            {
                                Name = "grant_type",
                            }
                        }
                    },
                    new StringContent(model.client_id, Encoding.UTF8, "application/json")
                    {
                        Headers =
                        {
                            ContentDisposition = new ContentDispositionHeaderValue("form-data")
                            {
                                Name = "client_id",
                            }
                        }
                    },
                    new StringContent(model.client_secret, Encoding.UTF8, "application/json")
                    {
                        Headers =
                        {
                            ContentDisposition = new ContentDispositionHeaderValue("form-data")
                            {
                                Name = "client_secret",
                            }
                        }
                    }
                }
            };

            var response = await client.PostAsync($"{_urlApi}/v5/token", request.Content);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                retorno = JsonConvert.DeserializeObject<CelcoinAuthResponse>(jsonContent);
            else
                throw new Exception($"Unexpected return. Details: {response.ReasonPhrase}");

            return retorno;
        }

        /// <summary>
        /// https://developers.celcoin.com.br/reference/retorna-a-lista-de-operadoras
        /// </summary>
        /// <returns>List cellphone operators</returns>
        public async Task<IEnumerable<SelectItem<string>>?> GetCellphoneOperators()
        {
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);

            var url = $"{_urlApi}/v5/transactions/topups/providers?stateCode=11&type=0&category=1";

            var response = await client.GetAsync(url);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            var operators = JsonConvert.DeserializeObject<CelcoinGetOperatorsResponse>(jsonContent);

            return operators.providers.Select(x => new SelectItem<string>()
            {
                Value = x.providerId.ToString(),
                Label = x.name
            });
        }

        /// <summary>
        /// https://developers.celcoin.com.br/reference/consulta-valores-operacionais
        /// </summary>
        /// <returns>Values to recharge</returns>
        public async Task<IEnumerable<SelectItem<string>>?> GetCellphoneOperatingValues(string providerId)
        {
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);

            var url = $"{_urlApi}/v5/transactions/topups/provider-values?stateCode=11&providerId={providerId}";

            var response = await client.GetAsync(url);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            var operatingValues = JsonConvert.DeserializeObject<CelcoinGetOperatingValuesResponse>(jsonContent);

            return operatingValues.value.Select(x => new SelectItem<string>()
            {
                Value = Convert.ToInt32(x.maxValue).ToString(),
                Label = x.productName
            });
        }

        /// <summary>
        /// https://developers.celcoin.com.br/reference/buscar-operadora-a-partir-de-um-n%C3%BAmero-de-telefone
        /// </summary>
        /// <returns></returns>
        public async Task<CelcoinValidateOperatorResponse> GetOperatorByCellphoneNumber(int stateCode, int phoneNumber)
        {
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);

            var url = $"{_urlApi}/v5/transactions/topups/find-providers?stateCode={stateCode}&PhoneNumber={phoneNumber}";

            var response = await client.GetAsync(url);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<CelcoinValidateOperatorResponse>(jsonContent);
        }

        /// <summary>
        /// https://developers.celcoin.com.br/reference/reserva-saldo-para-realizar-recarga
        /// </summary>
        /// <returns></returns>
        public async Task<CelcoinReserveBalanceResponse> ReserveBalanceRecharge(CelcoinReserveBalanceRequest model)
        {
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);

            CelcoinReserveBalanceResponse retorno = null;

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_urlApi}/v5/transactions/topups", content);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                retorno = JsonConvert.DeserializeObject<CelcoinReserveBalanceResponse>(jsonContent);

                var request = new CelcoinConfirmRechargeRequest()
                {
                    transactionId = retorno.transactionId,
                    externalNSU = 0
                };

                retorno = await this.ConfirmRecharge(request);

                if (retorno.status == 0 && retorno.message.ToLower() == "sucesso")
                {
                    retorno.transactionId = request.transactionId;
                    return retorno;
                }
                else
                    throw new Exception($"Api Celcoin - {TranslateConfirmErrorCelcoin(retorno.message)}");
            }
            else
            {
                var res = JsonConvert.DeserializeObject<CelcoinReserveBalanceResponse>(jsonContent);
                throw new Exception($"Api Celcoin - {TranslateReserveErrorCelcoin(res.errorCode)}");
            }

            return retorno;
        }

        /// <summary>
        /// https://developers.celcoin.com.br/reference/confirma-uma-recarga
        /// </summary>
        /// <returns></returns>
        public async Task<CelcoinReserveBalanceResponse> ConfirmRecharge(CelcoinConfirmRechargeRequest model)
        {
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);

            CelcoinReserveBalanceResponse retorno = null;

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{_urlApi}/v5/transactions/topups/{model.transactionId}/capture", content);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                retorno = JsonConvert.DeserializeObject<CelcoinReserveBalanceResponse>(jsonContent);
            else
                throw new Exception($"Unexpected return. Details: {response.Content.ReadAsStringAsync().Result}");

            return retorno;
        }

        /// <summary>
        /// https://developers.celcoin.com.br/reference/confirma-uma-recarga
        /// </summary>
        /// <returns></returns>
        public async Task<CelcoinCancelRechargeResponse> CancelRecharge(string transactionId)
        {
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);

            CelcoinCancelRechargeResponse retorno = null;

            var response = await client.DeleteAsync($"{_urlApi}/v5/transactions/topups/{transactionId}/void");
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
                retorno = JsonConvert.DeserializeObject<CelcoinCancelRechargeResponse>(jsonContent);
            else
                throw new Exception($"Unexpected return. Details: {response.Content.ReadAsStringAsync().Result}");

            return retorno;
        }

        /// <summary>
        /// https://developers.celcoin.com.br/reference/retorna-as-informa%C3%A7%C3%B5es-do-seu-saldo-atual
        /// </summary>
        /// <returns></returns>
        public async Task<MsPaymentIntegrationCelcoin.Contracts.DTO.Response.CelcoinBalanceResponse> GetBalance()
        {
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);
            MsPaymentIntegrationCelcoin.Contracts.DTO.Response.CelcoinBalanceResponse result = null;

            var url = $"{_urlApi}/merchant/balance";

            var response = await client.GetAsync(url);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
                result = JsonConvert.DeserializeObject<MsPaymentIntegrationCelcoin.Contracts.DTO.Response.CelcoinBalanceResponse>(jsonContent);
            else
                throw new Exception($"Api Celcoin - {response.ReasonPhrase}");

            return result;
        }

        /// <summary>
        /// https://developers.celcoin.com.br/reference/consultar-informa%C3%A7%C3%B5es-de-uma-recarga
        /// </summary>
        /// <returns></returns>
        public async Task<MsPaymentIntegrationCelcoin.Contracts.DTO.Response.CelcoinGetInfosRechargeResponse> GetInfosRecharge(Int32 transactionId)
        {
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);
            MsPaymentIntegrationCelcoin.Contracts.DTO.Response.CelcoinGetInfosRechargeResponse result = null;

            var url = $"{_urlApi}/v5/transactions/status-consult?transactionId={transactionId}";

            var response = await client.GetAsync(url);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            if (response.StatusCode == HttpStatusCode.OK)
                result = JsonConvert.DeserializeObject<MsPaymentIntegrationCelcoin.Contracts.DTO.Response.CelcoinGetInfosRechargeResponse>(jsonContent);
            else
                throw new Exception($"Api Celcoin - {response.ReasonPhrase}");

            return result;
        }

        private string TranslateReserveErrorCelcoin(string statusCode)
        {
            switch (statusCode.Trim())
            {
                case "35": return "Valor não permitido";
                case "044": return "Valor não permitido";
                case "050": return "Excede limite";
                case "089": return "Código de Área não habilitado";
                case "251": return "Telefone não permite recarga";
                case "264": return "Telefone Inválido";
                case "472": return "Valor não permitido para o DDD/Telefone informado";
                case "598": return "Hora de processamento fora do intervalo permitido, processamos Boletos até as 19:00";
                case "599": return "Telefone bloqueado para recarga";
                case "620": return "Falha na comunicação com a instituição. Favor tente novamente";
                case "658": return "CPF ou CNPJ inválido";
                case "734": return "Linha a qual receberia o crédito está bloqueada, cancelada ou inativa na operadora";
                case "024": return "Transação não encontrada";
                case "171": return "Falha na comunicação com a sua instituição";
                case "699": return "CPF/CNPJ inválido";
                case "146": return "DDD não habilitado";

                default: return statusCode;
            }
        }

        private string TranslateConfirmErrorCelcoin(string statusCode)
        {
            switch (statusCode.Trim())
            {
                case "620": return "Falha na comunicação com a instituição. Favor tente novamente";
                case "658": return "Falha na comunicação junto ao nosso parceiro. Favor tente novamente";
                case "024": return "Transação não encontrada";
                case "171": return "Falha na comunicação com a sua instituição";

                default: return statusCode;
            }
        }
    }
}
