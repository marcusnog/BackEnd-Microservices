using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.Support;
using MsOrderApi.Contracts.DTOs.Request;
using MsOrderApi.Contracts.DTOs.Response;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace MsOrderApi.Services.PagarME
{
    public class PagarMeService
    {
        private readonly string _urlApiPagarMePedido;
        private readonly string _urlApiPagarMeEstorno;
        private readonly string _pagarMeSecretKey_Usuario;
        private readonly string _pagarMeSecretKey_Senha;

        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient = null;

        public PagarMeService(IConfiguration configuration)
        {
            //_configuration = configuration ?? new BusinessLayer.Contracts.Configuration();
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            
            _urlApiPagarMePedido = _configuration.GetValue<string>("PagarMeUrlPedido");
            _urlApiPagarMeEstorno = _configuration.GetValue<string>("PagarMeUrlEstorno");
            _pagarMeSecretKey_Usuario = _configuration.GetValue<string>("PagarMeSecretKey_Usuario");
            _pagarMeSecretKey_Senha = _configuration.GetValue<string>("PagarMeSecretKey_Senha");
        }
        public PagarMeService(HttpClient httpClient, IConfiguration configuration = null) : this(configuration)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Operadora Financeira tem taxa do crédito (no plano: á vista por 30 dias) de 3,99%
        /// </summary>
        /// <param name="ValorComplemto"></param>
        /// <returns></returns>
        public static decimal CalcularATaxaDePagamento_OperadoraFinanceira(decimal? ValorComplemto)
        {
            try
            {
                if (!ValorComplemto.HasValue)
                    return 0;

                //PageSeguro tem taxa do crédito (no plano: á vista por 30 dias) de 3,99%
                return ValorComplemto.Value * 0.0399M;
            }
            catch
            {
                return 0;
            }
        }

        HttpClient GetHttpClient()
        {
            var client = _httpClient ?? new HttpClient();

            string token = $"{_pagarMeSecretKey_Usuario}:{_pagarMeSecretKey_Senha}";

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(token)));

            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            return client;
        }

        /// <summary>
        /// https://docs.pagar.me/reference/criar-pedido-2
        /// </summary>
        /// <returns></returns>
        public async Task<PagarMeCreateOrderResponse> CreateOrder(PagarMeCreateOrderRequest request)
        {
            String textoLog = String.Empty;
            Int16 textoLog_PontoErro = 0;
            String textoLog_Request = String.Empty;
            String textoLog_Response = String.Empty;

            var client = GetHttpClient();

            PagarMeCreateOrderResponse retorno = null;

            try
            {
                textoLog_Request = JsonConvert.SerializeObject(request);

                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
                textoLog_PontoErro = 1;
                var response = await client.PostAsync($"{_urlApiPagarMePedido}", content);
                textoLog_PontoErro = 2;
                var jsonContent = await response?.Content?.ReadAsStringAsync();
                textoLog_PontoErro = 3;

                textoLog_Response = jsonContent;

                if (!response.IsSuccessStatusCode)
                {
                    if (!response.ReasonPhrase.Equals("Unprocessable Entity"))
                        throw new Exception($"Unexpected return. Details: {response.ReasonPhrase}");
                }

                textoLog_PontoErro = 4;

                retorno = JsonConvert.DeserializeObject<PagarMeCreateOrderResponse>(jsonContent);

            }
            catch (Exception ex)
            {
                textoLog = String.Format("Erro (CreateOrder): Ponto: {0} | Request: {1} | Response: {2} | Descrição: {3}", textoLog_PontoErro, textoLog_Request, textoLog_Response, ex.Message);
                LogTrace.Gravar(this.GetType().Name, textoLog, LogTrace.TipoTrace.Normal);
                throw new Exception(ex.Message);
            }
            finally
            {
            }

            return retorno;
        }

        /// <summary>
        /// https://docs.pagar.me/reference/criar-pedido-2
        /// </summary>
        /// <returns></returns>
        public async Task<PagarMeReverseOrderResponse> ReverseOrder(PagarMeReverseOrderRequest request)
        {
            var client = GetHttpClient();

            PagarMeReverseOrderResponse retorno;

            var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");
            var response = await client.DeleteAsync($"{_urlApiPagarMeEstorno}/{request.charge_id}");
            var jsonContent = await response?.Content?.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Unexpected return. Details: {response.ReasonPhrase}");

            retorno = JsonConvert.DeserializeObject<PagarMeReverseOrderResponse>(jsonContent);

            return retorno;
        }

        private String Tratar_Holder_Name(String nome)
        {
            String retorno = String.Empty;

            if (!String.IsNullOrEmpty(nome))
            {
                retorno = nome.ToUpper();

                /** Troca os caracteres acentuados por não acentuados **/
                string[] acentos = new string[] { "Ç", "Á", "É", "Í", "Ó", "Ú", "Ý", "À", "È", "Ì", "Ò", "Ù", "Ä", "Ë", "Ï", "Ö", "Ü", "Ã", "Õ", "Ñ", "Â", "Ê", "Î", "Ô", "Û" };
                string[] semAcento = new string[] { "C", "A", "E", "I", "O", "U", "Y", "A", "E", "I", "O", "U", "A", "E", "I", "O", "U", "A", "O", "N", "A", "E", "I", "O", "U" };

                for (int i = 0; i < acentos.Length; i++)
                {
                    retorno = retorno.Replace(acentos[i], semAcento[i]);
                }
                /** Troca os caracteres especiais da string por "" **/
                string[] caracteresEspeciais = { "¹", "²", "³", "£", "¢", "¬", "º", "¨", "\"", "'", ".", ",", "-", ":", "(", ")", "ª", "|", "\\\\", "°", "_", "@", "#", "!", "$", "%", "&", "*", ";", "/", "<", ">", "?", "[", "]", "{", "}", "=", "+", "§", "´", "`", "^", "~" };

                for (int i = 0; i < caracteresEspeciais.Length; i++)
                {
                    retorno = retorno.Replace(caracteresEspeciais[i], "");
                }

                ///** Troca os caracteres especiais da string por " " **/
                if (!nome.ToUpper().Equals(retorno.ToUpper()))
                {
                    LogTrace.Gravar(this.GetType().Name, String.Format("Nome do Cartão de Crédito Alterado -> De: {0} | Para: {1}", nome.ToUpper(), retorno.ToUpper()), LogTrace.TipoTrace.Normal);
                }
                else
                {
                    LogTrace.Gravar(this.GetType().Name, String.Format("Nome do Cartão de Crédito -> {0}", retorno.ToUpper()), LogTrace.TipoTrace.Normal);
                }
            }

            return retorno;
        }

        private PagarMeCreateOrderRequest Trata_PagarMeCreateOrderRequest(OrderRequest oRequest, String PedidoReconhece)

        {
            PagarMeCreateOrderRequest oPagarMeCreateOrderRequest = new PagarMeCreateOrderRequest();

            oPagarMeCreateOrderRequest.closed = true;

            #region Cliente

            PagarMeCustomerOrder oPagarMeCustomerOrder = new PagarMeCustomerOrder();
            oPagarMeCustomerOrder.name = oRequest.Recipient.Name;
            oPagarMeCustomerOrder.type = "individual";
            oPagarMeCustomerOrder.email = oRequest.Recipient.Email;
            oPagarMeCustomerOrder.document = oRequest.Recipient.CPFCNPJ.Replace(".", "").Replace("-", "");
            oPagarMeCustomerOrder.document_type = "CPF";
            oPagarMeCustomerOrder.birthdate = DateTime.Now.Date;

            oPagarMeCustomerOrder.phones = new PagarMePhonesOrder();

            String countryCode = "55";
            String areaCode = "11";
            String Number = "23885555";

            if (!String.IsNullOrEmpty(oRequest.DeliveryAddress.Telephone))
            {
                String telefoneTemp = oRequest.DeliveryAddress.Telephone.Replace(" ", "").Replace("-", "");
                if (!String.IsNullOrEmpty(telefoneTemp))
                {
                    if (telefoneTemp.Length > 9)
                    {
                        areaCode = telefoneTemp.Substring(0, 2);
                        Number = telefoneTemp.Substring(2, telefoneTemp.Length - 2);
                    }
                }
            }

            PagarMeHomePhoneOrder oPagarMeHomePhoneOrder = new PagarMeHomePhoneOrder();
            oPagarMeHomePhoneOrder.country_code = countryCode;
            oPagarMeHomePhoneOrder.area_code = areaCode;
            oPagarMeHomePhoneOrder.number = Number;

            PagarMeMobilePhoneOrder oPagarMeMobilePhoneOrder = new PagarMeMobilePhoneOrder();
            oPagarMeMobilePhoneOrder.country_code = countryCode;
            oPagarMeMobilePhoneOrder.area_code = areaCode;
            oPagarMeMobilePhoneOrder.number = Number;

            oPagarMeCustomerOrder.phones.home_phone = oPagarMeHomePhoneOrder;
            oPagarMeCustomerOrder.phones.mobile_phone = oPagarMeMobilePhoneOrder;

            #region Endereço do Cliente

            oPagarMeCustomerOrder.address = new PagarMeAddressOrder();

            String enderecoLogradouro = String.Empty;
            String enderecoNumero = String.Empty;
            String enderecoBairro = String.Empty;
            String enderecoCEP = String.Empty;
            String enderecoCidade = String.Empty;
            String enderecoEstado = String.Empty;

            //Verificar com o Andre se vai existir as 2 coletas de endreço (Verificar)

            //if (!String.IsNullOrEmpty(oRequest.oED_Cartao.DadosTransacaoPagSeguro.EnderecoFaturamentoCEP))
            //{
            //    enderecoLogradouro = oED_Cartao.DadosTransacaoPagSeguro.EnderecoFaturamentoRua;
            //    enderecoNumero = oED_Cartao.DadosTransacaoPagSeguro.EnderecoFaturamentoNumero;
            //    enderecoBairro = oED_Cartao.DadosTransacaoPagSeguro.EnderecoFaturamentoBairro;

            //    if (!String.IsNullOrEmpty(oED_Cartao.DadosTransacaoPagSeguro.EnderecoFaturamentoCEP))
            //        enderecoCEP = oED_Cartao.DadosTransacaoPagSeguro.EnderecoFaturamentoCEP.Replace("-", "").Replace(".", "");

            //    enderecoCidade = oED_Cartao.DadosTransacaoPagSeguro.EnderecoFaturamentoMunicipio;
            //    enderecoEstado = oED_Cartao.DadosTransacaoPagSeguro.EnderecoFaturamentoUF;

            //    LogTrace.Gravar(this.GetType().Name, "Endereço = Cobrança", LogTrace.TipoTrace.Normal);
            //}
            //else
            //{
            //    enderecoLogradouro = oED_Cartao.DadosTransacaoPagSeguro.EnderecoCompraRua;
            //    enderecoNumero = oED_Cartao.DadosTransacaoPagSeguro.EnderecoCompraNumero;
            //    enderecoBairro = oED_Cartao.DadosTransacaoPagSeguro.EnderecoCompraBairro;

            //    if (!String.IsNullOrEmpty(oED_Cartao.DadosTransacaoPagSeguro.EnderecoCompraCEP))
            //        enderecoCEP = oED_Cartao.DadosTransacaoPagSeguro.EnderecoCompraCEP.Replace("-", "").Replace(".", "");

            //    enderecoCidade = oED_Cartao.DadosTransacaoPagSeguro.EnderecoCompraMunicipio;
            //    enderecoEstado = oED_Cartao.DadosTransacaoPagSeguro.EnderecoCompraUF;

            //    LogTrace.Gravar(this.GetType().Name, "Endereço = Entrega", LogTrace.TipoTrace.Normal);
            //}

            String enderecoFormatado = String.Concat(enderecoLogradouro, ", ", enderecoNumero, ", ", enderecoBairro);

            oPagarMeCustomerOrder.address.line_1 = enderecoFormatado;
            oPagarMeCustomerOrder.address.zip_code = enderecoCEP;
            oPagarMeCustomerOrder.address.city = enderecoCidade;
            oPagarMeCustomerOrder.address.state = enderecoEstado;
            oPagarMeCustomerOrder.address.country = "BR";

            #endregion Endereço do Cliente

            #region Itens do Pedido

            oPagarMeCreateOrderRequest.items = new List<PagarMeProductsOrder>();

            var _valorPagamento = oRequest.PaymentData.ValorComplemento;

            _valorPagamento = Convert.ToDecimal(Math.Round(Convert.ToDouble(_valorPagamento), 2).ToString("N2"));

            String valorSemPontuacao = Convert.ToString(_valorPagamento).Replace(".", "").Replace(",", "");

            PagarMeProductsOrder oPagarMeProductsOrder = new PagarMeProductsOrder();
            oPagarMeProductsOrder.code = PedidoReconhece;
            oPagarMeProductsOrder.amount = Convert.ToInt32(valorSemPontuacao);
            oPagarMeProductsOrder.quantity = 1;
            oPagarMeProductsOrder.description = "Pedido: " + PedidoReconhece;
            oPagarMeCreateOrderRequest.items.Add(oPagarMeProductsOrder);

            #endregion Itens do Pedido

            #region Dados de pagamento

            var ValorParcela = Convert.ToDecimal(oRequest.PaymentData.ValorComplemento);
            ValorParcela += CalcularATaxaDePagamento_OperadoraFinanceira(ValorParcela);

            oPagarMeCreateOrderRequest.payments = new List<PagarMePaymentsOrder>();

            PagarMePaymentsOrder oPagarMePaymentsOrder = new PagarMePaymentsOrder();
            oPagarMePaymentsOrder.payment_method = "credit_card";

            PagarMeCreditCardOrder oPagarMeCreditCardOrder = new PagarMeCreditCardOrder();
            oPagarMeCreditCardOrder.installments = 1;
            oPagarMeCreditCardOrder.statement_descriptor = "Reconhece";

            PagarMeCardOrder oPagarMeCardOrder = new PagarMeCardOrder();

            oPagarMeCardOrder.number = oRequest.PaymentData.NumeroCartao;
            oPagarMeCardOrder.holder_name = Tratar_Holder_Name(oRequest.PaymentData.NomeTitular);
            oPagarMeCardOrder.exp_month = Convert.ToInt32(oRequest.PaymentData.MesValidade);
            oPagarMeCardOrder.exp_year = Convert.ToInt32(oRequest.PaymentData.AnoValidade);
            oPagarMeCardOrder.cvv = oRequest.PaymentData.CodigoSegurancaCartao;

            oPagarMeCardOrder.billing_address = new PagarMeAddressOrder();

            oPagarMeCardOrder.billing_address.line_1 = enderecoFormatado;
            oPagarMeCardOrder.billing_address.zip_code = enderecoCEP;
            oPagarMeCardOrder.billing_address.city = enderecoCidade;
            oPagarMeCardOrder.billing_address.state = enderecoEstado;
            oPagarMeCardOrder.billing_address.country = "BR";

            oPagarMeCreditCardOrder.card = oPagarMeCardOrder;

            oPagarMePaymentsOrder.credit_card = oPagarMeCreditCardOrder;
            oPagarMeCreateOrderRequest.payments.Add(oPagarMePaymentsOrder);

            #endregion Dados de pagamento

            oPagarMeCreateOrderRequest.customer = oPagarMeCustomerOrder;

            #endregion Cliente

            return oPagarMeCreateOrderRequest;
        }

        private static PagarMeReverseOrderRequest Trata_PagarMeReverseOrderRequest(String chargeId, Int32 valorTransacao)
        {
            PagarMeReverseOrderRequest oPagarMeReverseOrderRequest = new PagarMeReverseOrderRequest();
            oPagarMeReverseOrderRequest.charge_id = chargeId;
            oPagarMeReverseOrderRequest.amount = valorTransacao;
            return oPagarMeReverseOrderRequest;
        }

        public enum TipoOperacao
        {
            Pedido,
            Estorno
        }

        public ED_Retorno_API_Pagamento Processa(TipoOperacao tipoOperacao, OrderRequest oRequest = null, String idEstorno = "",
                                                 String pedidoReconhece = "", Int32 valorTransacao = 0)
        {
            ED_Retorno_API_Pagamento oED = new ED_Retorno_API_Pagamento();
            oED.SUCESSO = false;
            oED.VALOR_TRANSACAO = 0;

            String textoLog = String.Empty;
            String logRequest = String.Empty;
            String logResponse = String.Empty;
            String logID = String.Empty;
            String logNome = String.Empty;

            Int16 textoLog_PontoErro = 0;

            LogTrace.Gravar(this.GetType().Name, "INICIO TRANSAÇÃO (3)", LogTrace.TipoTrace.Normal);

            try
            {
                switch (tipoOperacao)
                {
                    case TipoOperacao.Pedido:
                        #region Pedido

                        textoLog_PontoErro = 1;

                        oED.oED_Request = Trata_PagarMeCreateOrderRequest(oRequest, pedidoReconhece);

                        if (oED.oED_Request.items != null && oED.oED_Request.items.Count > 0)
                        {
                            oED.VALOR_TRANSACAO = oED.oED_Request.items[0].amount;
                        }

                        textoLog_PontoErro = 2;

                        logRequest = JsonConvert.SerializeObject(oED.oED_Request);
                        logNome = "TRANSACAO";

                        Task<PagarMeCreateOrderResponse> oPagarMeCreateOrderResponse = CreateOrder(oED.oED_Request);

                        textoLog_PontoErro = 3;

                        oED.oED_Response = oPagarMeCreateOrderResponse.Result;

                        textoLog_PontoErro = 4;

                        logResponse = JsonConvert.SerializeObject(oED.oED_Response);
                        logID = oED.ID_TRANSACAO;

                        //Verificar com o Andre se no pedido ja vem a informação do ID da campanha (Verificar)

                        //if (!oED.Tem_Erro_API_Pedido)
                        //{
                        //    oED.SUCESSO = true;
                        //    textoLog = String.Format("Sucesso Campanha: {0} | Pedido: {1} | ID PagarMe: {2} | Valor: {3}",
                        //                             oRequest.Campaign .CampanhaId, pedidoReconhece, oED.ID_TRANSACAO, oED.VALOR_TRANSACAO);
                        //}
                        //else
                        //{
                        //    String erroTexto = oED.Erro_API_Pedido;
                        //    textoLog = String.Format("Erro Campanha: {0} | Pedido: {1} | Detalhe: {2}", requestCartao.CampanhaId, pedidoReconhece, erroTexto);
                        //    throw new Exception(erroTexto);
                        //}

                        #endregion Pedido
                        break;
                    case TipoOperacao.Estorno:
                        #region Estorno

                        oED.oED_Reverse_Request = Trata_PagarMeReverseOrderRequest(idEstorno, valorTransacao);
                        logRequest = JsonConvert.SerializeObject(oED.oED_Reverse_Request);
                        logNome = "ESTORNO";

                        Task<PagarMeReverseOrderResponse> oPagarMeReverseOrderResponse = ReverseOrder(oED.oED_Reverse_Request);
                        oED.oED_Reverse_Response = oPagarMeReverseOrderResponse.Result;
                        logResponse = JsonConvert.SerializeObject(oED.oED_Reverse_Response);
                        logID = oED.ID_TRANSACAO_ESTORNO;

                        //Verificar com o Andre se no pedido ja vem a informação do ID da campanha (Verificar)

                        //if (!oED.Tem_Erro_API_Pedido)
                        //{
                        //    oED.SUCESSO = true;
                        //    textoLog = String.Format("Sucesso Campanha: {0} | Estorno: {1} | ID PagarMe: {2} | Valor: {3}",
                        //                             requestCartao.CampanhaId, pedidoReconhece, oED.ID_TRANSACAO_ESTORNO, valorTransacao);
                        //}
                        //else
                        //{
                        //    textoLog = String.Format("Erro Campanha: {0} | Estorno: {1} | Detalhe: {2}", requestCartao.CampanhaId, pedidoReconhece, oED.Erro_API_Estorno);
                        //    throw new Exception(oED.Erro_API_Estorno);
                        //}

                        #endregion Estorno
                        break;
                }
            }
            catch (Exception ex)
            {
                //Verificar com o Andre se no pedido ja vem a informação do ID da campanha (Verificar)
                //textoLog = String.Format("Erro: Campanha: {0} | Ponto: {1} | Descrição: {2}", requestCartao.CampanhaId, textoLog_PontoErro, ex.Message);
                oED.MENSAGEM_ERRO = String.Format("Erro API (Transação Financeira): {0}", ex.Message);
            }
            finally
            {
                //try
                //{
                //    CadastraLog(new DatabaseLayer.POCO.PagSeguro.LogPagSeguro()
                //    {
                //        Tipo = logNome,
                //        Origem = "PM",
                //        Ativo = true,
                //        DataCadastro = DateTime.Now,
                //        NumeroPedidoReconhece = pedidoReconhece,
                //        TransactionCode = logID,
                //        CampanhaId = requestCartao.CampanhaId,
                //        Request = logRequest,
                //        Response = logResponse,
                //        RequestCatalogoService = JsonConvert.SerializeObject(requestCartao)
                //    });
                //}
                //catch
                //{
                //}
            }

            LogTrace.Gravar(this.GetType().Name, textoLog, LogTrace.TipoTrace.Normal);
            LogTrace.Gravar(this.GetType().Name, "FIM TRANSAÇÃO (3)", LogTrace.TipoTrace.Normal);

            return oED;
        }

        //private void CadastraLog(DatabaseLayer.POCO.PagSeguro.LogPagSeguro log)
        //{
        //    using (CatalogoContext context = new CatalogoContext())
        //    {
        //        context.LogPagSeguro.Add(log);
        //        context.SaveChanges();
        //    }
        //}
    }
}
