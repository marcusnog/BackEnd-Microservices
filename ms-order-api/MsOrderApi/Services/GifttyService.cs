using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.DTO.Response;
using Ms.Api.Utilities.Exceptions;
using Ms.Api.Utilities.Extensions;
using Ms.Api.Utilities.Models;
using MsOrderApi.Contracts.Services;
using RestSharp;
using System.Net;
using System.Text;

namespace MsOrderApi.Services
{
    public class GifttyService : IGifttyService
    {
        readonly IConfiguration _configuration;
        readonly string _MS_ORDER_INTEGRATION_URL;

        public GifttyService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _MS_ORDER_INTEGRATION_URL = _configuration.GetValue<string>("MS_ORDER_INTEGRATION_GIFTTY_URL");
        }

        //public async Task<DefaultResponse<CalcShippingResponse_Store>> GetShippingValue(CalcShippingRequest_Store request)
        //{
        //    using HttpClient client = new HttpClient();

        //    var response = await client.PostAsync($"{_MS_ORDER_INTEGRATION_URL}/CalcShipping",
        //                                          new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        //    if (!response.IsSuccessStatusCode)
        //        throw new CodeException($"{ErrorCode.ms_order_integration_netshoes}.{1.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

        //    return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<CalcShippingResponse_Store>>(await response.Content.ReadAsStringAsync());
        //}

        public async Task<DefaultResponse<OrderResponse>> SetCreateOrder(OrderRequest oOrderRequest, Request_OrderStore oRequest_OrderStore)
        {
            using HttpClient client = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");            


            #region Prepara Request

            Giftty_RequestPedido request = new Giftty_RequestPedido();
                        
            int iCount = 0;
            
            if (oRequest_OrderStore.Products != null)
            {
                request.Produtos = new Produto[oRequest_OrderStore.Products.Count()];

                
                foreach (Request_OrderProduct oProductOrderRequest in oRequest_OrderStore.Products)
                {
                    Produto oGiftty_ProductCreateOrder = new Produto();
                    oGiftty_ProductCreateOrder.codigo = oProductOrderRequest.CodeProduct;
                    oGiftty_ProductCreateOrder.quantidade = oProductOrderRequest.Quantity;
                    oGiftty_ProductCreateOrder.valor = oProductOrderRequest.ValueUnitary.ToString();
                    oGiftty_ProductCreateOrder.telefone = "";
                    oGiftty_ProductCreateOrder.codigoCartao = "";
                    oGiftty_ProductCreateOrder.ddd = "";
                    
                    request.Produtos[iCount] = oGiftty_ProductCreateOrder;

                    iCount++;
                }
            }

            request.DadosPessoais = new Giftty_DadosPessoais();
            request.DadosPessoais.cpf = oOrderRequest.Recipient.CPFCNPJ;
            request.DadosPessoais.cnpj = oOrderRequest.Recipient.CPFCNPJ;
            request.DadosPessoais.email = oOrderRequest.Recipient.Email;
            request.DadosPessoais.nome = oOrderRequest.Recipient.Name;            
            request.DadosPessoais.dataNascimento = "22/09/1988";
            request.DadosPessoais.razaoSocial = "aa";
            request.DadosPessoais.tipoPessoa = "F";
            request.DadosPessoais.sexo = "F";
            request.DadosPessoais.ie = "125";

            Giftty_EnderecoEntrega endereco = new Giftty_EnderecoEntrega();

            endereco.cep = oOrderRequest.DeliveryAddress.ZipCode;
            endereco.estado = oOrderRequest.DeliveryAddress.State;
            endereco.logradouro = oOrderRequest.DeliveryAddress.PublicPlace;
            endereco.cidade = oOrderRequest.DeliveryAddress.City;
            endereco.numero = oOrderRequest.DeliveryAddress.Number;
            endereco.bairro = oOrderRequest.DeliveryAddress.District;
            endereco.complemento = oOrderRequest.DeliveryAddress.Complement;
            endereco.telefone = oOrderRequest.DeliveryAddress.Telephone;
            endereco.telefoneCel = oOrderRequest.DeliveryAddress.Telephone2;
            endereco.ddd = "11";
            endereco.dddCel = "11";
            endereco.estado = "SP";

            request.Enderecos = new Giftty_Enderecos();
            request.Enderecos.EnderecoEntrega = new Giftty_EnderecoEntrega();
            request.Enderecos.EnderecoEntrega = endereco;

            request.Enderecos.EnderecoPrincipal = new Giftty_Endereco();

            Giftty_Endereco enderecoPrincipal = new Giftty_Endereco();

            enderecoPrincipal.cep = oOrderRequest.DeliveryAddress.ZipCode;
            enderecoPrincipal.estado = oOrderRequest.DeliveryAddress.State;
            enderecoPrincipal.logradouro = oOrderRequest.DeliveryAddress.PublicPlace;
            enderecoPrincipal.cidade = oOrderRequest.DeliveryAddress.City;
            enderecoPrincipal.numero = oOrderRequest.DeliveryAddress.Number;
            enderecoPrincipal.bairro = oOrderRequest.DeliveryAddress.District;
            enderecoPrincipal.complemento = oOrderRequest.DeliveryAddress.Complement;
            enderecoPrincipal.telefone = oOrderRequest.DeliveryAddress.Telephone;
            enderecoPrincipal.telefoneCel = oOrderRequest.DeliveryAddress.Telephone2;
            enderecoPrincipal.ddd = "11";
            enderecoPrincipal.dddCel = "11";
            enderecoPrincipal.estado = "SP";

            request.Enderecos.EnderecoPrincipal = enderecoPrincipal;


            Giftty_DadosPagamento pagamento = new Giftty_DadosPagamento();
            pagamento.cpfTitular = oOrderRequest.CNPJ;
            pagamento.codSeguranca = oOrderRequest.PaymentData.CodigoSegurancaCartao;
            pagamento.validade = oOrderRequest.PaymentData.MesValidade + oOrderRequest.PaymentData.AnoValidade;
            pagamento.bandeira = oOrderRequest.PaymentData.NomeBandeira;
            pagamento.formaPagamento = oOrderRequest.PaymentData.NomeBandeira;
            pagamento.numCartao = oOrderRequest.PaymentData.NumeroCartao;
            pagamento.numParcelas = oOrderRequest.PaymentData.QuantidadeParcelas;
            pagamento.nomeTitular = oOrderRequest.PaymentData.NomeTitular;
            pagamento.porcentagemPagamento = "10";

            request.DadosPagamento = new Giftty_DadosPagamento();
            request.DadosPagamento = pagamento;

            request.PedidoParceiro = "0";                  
            request.Campanha = "SUPER PREMIOS";
            request.Projeto = "1028";
            

            #endregion Prepara Request

            var urlChamada = $"{_MS_ORDER_INTEGRATION_URL}";

            var response = SendDadosPost("CreateOrder", request);

            //var response = await client.PostAsync(urlChamada,
            //                                      new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
            
            //if (!response.IsSuccessStatusCode)
            //    throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            return response;
        }

        public DefaultResponse<OrderResponse> SendDadosPost(string apiPath, object? body = null, Dictionary<string, string>? headers = null)
        {
            try
            {                
                var rest = new RestClient($"{_MS_ORDER_INTEGRATION_URL}");

                var request = new RestRequest(apiPath, Method.Post)
                {
                    RequestFormat = DataFormat.Json,
                    OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; }
                };

                //if (!string.IsNullOrEmpty(token))
                  //  request.AddHeader("authorization", "bearer " + token);

                request.AddHeader("Content-Type", "application/json");

                if (headers != null)
                {
                    foreach (var header in headers)
                    {
                        request.AddHeader(header.Key, header.Value);
                    }
                }

                if (body != null)
                {
                    request.AddJsonBody(body);
                }

                var response = rest.Execute(request);

                if (!response.IsSuccessStatusCode)
                    throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content));

                return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<OrderResponse>>(response.Content);

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<DefaultResponse<CalcShippingResponse_Store>> GetShippingValue(String CEP, CalcShippingRequest_Store oCalcShippingRequest_Store)
        {
            using HttpClient client = new HttpClient();

            #region Prepara Request

            Generic_CalcShipping_Request oRequest = new Generic_CalcShipping_Request();
            oRequest.CEP = CEP;
            //oRequest.FatorConversao = Convert.ToDecimal(oCalcShippingRequest.FatorConversao);
            oRequest.ListaProduto = oCalcShippingRequest_Store.Produtos;

            #endregion Prepara Request

            var response = await client.PostAsync($"{_MS_ORDER_INTEGRATION_URL}/CalcShipping",
                                                  new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(oRequest), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_netshoes}.{1.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            //return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<CalcShippingResponse_Store>>(await response.Content.ReadAsStringAsync());
            DefaultResponse<CalcShippingResponse_Store> oResult = Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<CalcShippingResponse_Store>>(await response.Content.ReadAsStringAsync());
            oResult.Data.Store = Ms.Api.Utilities.Enum.Enums.Store.Giftty;
            return oResult;
        }

    }
}
