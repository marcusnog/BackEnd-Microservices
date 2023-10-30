using MsPaymentIntegrationTransfeera.Api.Contracts.DTO;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Request;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Response.Billets;
using MsPaymentIntegrationTransfeera.Api.Contracts.Services;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace MsPaymentIntegrationTransfeera.Api.Services
{
    public class IntegrationTransfeeraService : IIntegrationTransfeeraService
    {
        readonly IConfiguration _configuration;
        readonly string _urlLogin;
        readonly string _urlApi;
        readonly string _userAgent;
        readonly HttpClient _httpClient = null;
        public decimal VlrMiniminimumBalanceTransfeera { get => _configuration.GetValue<decimal>("VlrMiniminimumBalanceTransfeera"); }

        public IntegrationTransfeeraService(IConfiguration configuration = null)
        {
            _configuration = configuration;

            _urlLogin = _configuration.GetValue<string>("Transfeera:UrlLogin");
            _urlApi = _configuration.GetValue<string>("Transfeera:UrlApi");
            _userAgent = _configuration.GetValue<string>("Transfeera:UserAgent");
        }

        public IntegrationTransfeeraService(HttpClient httpClient, IConfiguration configuration = null) : this(configuration)
        {
            _httpClient = httpClient;
        }

        private bool RemoteServerCertificateValidationCallback(object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate, System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        /// <summary>
        /// https://docs.transfeera.dev/autenticacao
        /// </summary>
        /// <returns></returns>
        public async Task<TransfeeraAuthResponse> GetAuthentication()
        {
            var client = GetHttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(RemoteServerCertificateValidationCallback);

            TransfeeraAuthResponse retorno;

            var model = new TransfeeraAuthRequest
            {
                client_id = _configuration.GetValue<string>("Transfeera:ClientId"),
                client_secret = _configuration.GetValue<string>("Transfeera:ClientSecret"),
                grant_type = _configuration.GetValue<string>("Transfeera:GrantType")
            };

            var content = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
            var response = await client.PostAsync($"{_urlLogin}/authorization", content);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                retorno = JsonConvert.DeserializeObject<TransfeeraAuthResponse>(jsonContent);
            }
            else if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                var retornoErro = JsonConvert.DeserializeObject<TransfeeraAuthErrorResponse>(jsonContent);
                throw new Exception($"Unexpected return. Details: {retornoErro?.message}");
            }
            else
            {
                throw new Exception($"Unexpected return. Details: {jsonContent}");
            }

            return retorno;
        }

        /// <summary>
        /// https://docs.transfeera.dev/pagamentos/Billets#get-consulta-de-Billets-na-cip
        /// </summary>
        /// <param name="code">Código de barras ou linha digitável</param>
        /// <returns></returns>
        public async Task<ValidateCIPResponse> ValidateBilletOnCIP(string code)
        {
            ValidateCIPResponse returnValidate = null;
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);

            var url = $"{_urlApi}/billet/consult?code={code}";
            var response = await client.GetAsync(url);

            var jsonContent = await response?.Content?.ReadAsStringAsync();

            returnValidate = JsonConvert.DeserializeObject<ValidateCIPResponse>(jsonContent);

            returnValidate.message = TranslateErrorTransfeera(returnValidate.status);

            return returnValidate;
        }

        /// <summary>
        /// https://docs.transfeera.dev/pagamentos/saldo#get-consultar-saldo
        /// </summary>
        /// <returns></returns>
        public async Task<TransfeeraCheckBalanceResponse> CheckBalance()
        {
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);

            var url = $"{_urlApi}/statement/balance";
            var response = await client.GetAsync(url);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TransfeeraCheckBalanceResponse>(jsonContent);
        }

        public async Task<TransfeeraCreateBilletBatchResponse> CreateBilletBatch(TransfeeraCreateBilletBatchRequest request)
        {
            TransfeeraCreateBilletBatchErrorResponse returnCreateError = null;
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);

            var id = 0;
            var url = $"{_urlApi}/batch";

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                returnCreateError = JsonConvert.DeserializeObject<TransfeeraCreateBilletBatchErrorResponse>(jsonContent);

                returnCreateError.message = TranslateErrorTransfeera(returnCreateError?.errorCode);

                throw new Exception($"Api Transfeera - {returnCreateError.message}");
            }
            else
                return JsonConvert.DeserializeObject<TransfeeraCreateBilletBatchResponse>(jsonContent);
        }

        /// <summary>
        /// https://docs.transfeera.dev/pagamentos/Billets#get-consultar-Billet
        /// </summary>
        /// <param name="billetId">Id do Billet</param>
        /// <returns></returns>
        public async Task<TransfeeraGetBilletResponse> GetBillet(long billetId)
        {
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);

            var url = $"{_urlApi}/billet/{billetId}";

            var response = await client.GetAsync(url);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TransfeeraGetBilletResponse>(jsonContent);
        }

        /// <summary>
        /// https://docs.transfeera.dev/pagamentos/Billets#get-consultar-Billets
        /// </summary>
        /// <param name="billetId">Id do Billet</param>
        /// <returns></returns>
        public async Task<List<TransfeeraGetBilletResponse>> GetBillets(TransfeeraGetBilletsRequest request)
        {
            var lstTransfeeraBillets = new List<TransfeeraGetBilletResponse>();
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);

            var url = $"{_urlApi}/billet?initialDate={request.initialDate}&endDate={request.endDate}&page=0";

            var response = await client.GetAsync(url);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            var retornoApi = JsonConvert.DeserializeObject<TransfeeraGetBilletsResponse>(jsonContent);

            lstTransfeeraBillets.AddRange(retornoApi.data);

            var itensPerPage = retornoApi.metadata.pagination.itemsPerPage;
            var page = 0;

            var totalPages = (retornoApi.metadata.pagination.totalItems / itensPerPage);

            while (page < totalPages)
            {
                page++;

                var result2 = client.GetAsync($"{_urlApi}/billet?initialDate={request.initialDate}&endDate={request.endDate}&page={page}").Result;
                var content2 = result2.Content;

                var stringRetorno2 = content2.ReadAsStringAsync().Result;

                if (!result2.IsSuccessStatusCode)
                    throw new Exception("unexpected return", new Exception(stringRetorno2));

                var retornoApi2 = JsonConvert.DeserializeObject<TransfeeraGetBilletsResponse>(stringRetorno2);

                lstTransfeeraBillets.AddRange(retornoApi2.data);
            }

            return lstTransfeeraBillets;
        }

        /// <summary>
        /// https://docs.transfeera.dev/pagamentos/Billets#get-consultar-Billet
        /// </summary>
        /// <param name="billetId">Id do Billet</param>
        /// <returns></returns>
        public async Task<TransfeeraCloseBatchResponse> CloseBatch(int Id)
        {
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);

            var url = $"{_urlApi}/batch/{Id}/close";

            var content = new StringContent(JsonConvert.SerializeObject(Id), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(url, content);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<TransfeeraCloseBatchResponse>(jsonContent);
        }

        HttpClient GetHttpClient(string token = null)
        {
            var client = _httpClient ?? new HttpClient();
            if (!client.DefaultRequestHeaders.Contains("User-Agent") && !string.IsNullOrEmpty(_userAgent))
                client.DefaultRequestHeaders.Add("User-Agent", _userAgent);
            if (!client.DefaultRequestHeaders.Contains("Authorization") && !string.IsNullOrEmpty(token))
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
            if (!client.DefaultRequestHeaders.Contains("Accept"))
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return client;
        }

        /// <summary>
        /// https://docs.transfeera.dev/pagamentos/Billets#get-consultar-Billet
        /// </summary>
        /// <param name="batchId">Id do lote</param>
        /// <returns></returns>
        public async Task<TransfeeraGetBilletResponse> GetBatchBillet(int batchId)
        {
            var auth = await GetAuthentication();
            var client = GetHttpClient(auth.access_token);

            var url = $"{_urlApi}/batch/{batchId}/billet";

            var response = await client.GetAsync(url);
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<TransfeeraGetBilletResponse>>(jsonContent).FirstOrDefault();
        }

        private string TranslateErrorTransfeera(string statusCode)
        {
            switch (statusCode)
            {
                case "INVALIDO": return "O código de barras do Boleto ou a linha digitável são inválidos";
                case "NAO_REGISTRADO": return "Boleto não cadastrado";
                case "NAO_ENCONTRADO": return "Não foi possível encontrar Boleto";
                case "IMPAGAVEL": return "O Boleto ultrapassou o prazo de pagamento";
                case "PAGO": return "O Boleto já foi pago, não é possível consultar";
                case "NAO_PAGO": return "Boleto disponível para pagamento";
                case "BIL_1": return "Hora de processamento fora do intervalo permitido, processamos Boletos até as 19:00";
                case "BIL_2": return "A data de pagamento não pode ser posterior à data de vencimento";
                case "BIL_3": return "Código de barras inválido";
                case "BIL_4": return "Valor maior que o permitido";
                case "BIL_5": return "Valor não permitido";
                case "BIL_6": return "Boleto expirado";
                case "BIL_7": return "Processamos Boletos apenas em dias úteis, você precisa alterar a data para um dia útil";
                case "BIL_8": return "Código de barras duplicado";
                case "BIL_9": return "A data de pagamento é obrigatória";
                case "BIL_10": return "Formato incorreto para a data de pagamento";
                case "BIL_11": return "Data de pagamento antes da data atual";
                case "BIL_12": return "Seu lote não possui Boletos, cadastre os Boletos para fechá-lo";
                case "BIL_13": return "Formato incorreto de um conteúdo de campo de Boleto";
                case "BIL_14": return "ID de integração duplicado";
                case "BIL_15": return "Você não pode agendar um Boleto após 30 dias";
                case "BIL_16": return "O valor do Boleto deve ser maior que R$0,00";
                case "BIL_17": return "O código de barras do Boleto ou a linha digitável são inválidos";
                case "BIL_18": return "Boleto não cadastrado";
                case "BIL_19": return "Não foi possível encontrar Boleto";
                case "BIL_20": return "O Boleto ultrapassou o prazo de pagamento";
                case "BIL_21": return "O Boleto já foi pago, não é possível consultar";
                case "BIL_22": return "Boleto disponível para pagamento";
                case "BIL_23": return "Não consigo agendar o pagamento de Boletos vencidos";
                case "BIL_24": return "Para pagar Boletos vencidos ou Billets com desconto é obrigatório que você primeiro consulte os dados do Boleto em nossa API. Assim garantimos o valor correto, atualizado com multas, juros e descontos.";
                case "BIL_25": return "O valor do Boleto não pode ultrapassar 2 casas decimais";

                default: return statusCode;
            }
        }
    }
}
