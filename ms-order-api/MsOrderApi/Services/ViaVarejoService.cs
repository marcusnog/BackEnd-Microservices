using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.DTO.Response;
using Ms.Api.Utilities.Exceptions;
using Ms.Api.Utilities.Extensions;
using Ms.Api.Utilities.Models;
using MsOrderApi.Contracts.Services;
using System.Text;

namespace MsOrderApi.Services
{
    public class ViaVarejoService : IViaVarejoService
    {
        readonly IConfiguration _configuration;
        readonly string _MS_ORDER_INTEGRATION_VIA_VAREJO_URL;

        public ViaVarejoService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _MS_ORDER_INTEGRATION_VIA_VAREJO_URL = _configuration.GetValue<string>("MS_ORDER_INTEGRATION_VIA_VAREJO_URL");
        }

        //public async Task<DefaultResponse<CalcShippingResponse_Store>> GetShippingValue(CalcShippingRequest_Store request)
        //{
        //    using HttpClient client = new HttpClient();

        //    var response = await client.PostAsync($"{_MS_ORDER_INTEGRATION_VIA_VAREJO_URL}/CalcShipping", 
        //                                          new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        //    if (!response.IsSuccessStatusCode)
        //        throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

        //    return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<CalcShippingResponse_Store>>(await response.Content.ReadAsStringAsync());
        //}
        //public async Task<DefaultResponse<OrderResponse>> SetCreateOrder(OrderRequest oOrderRequest)
        //{
        //    using HttpClient client = new HttpClient();

        //    #region Prepara Request

        //    ViaVarejo_PedidoB2BRequest request = new ViaVarejo_PedidoB2BRequest();
        //    request.ValorFrete = oOrderRequest.ValorFrete;
        //    request.CNPJ = oOrderRequest.CNPJ;

        //    if (oOrderRequest.Produtos != null && oOrderRequest.Produtos.Count > 0)
        //    {
        //        request.Produtos = new List<ViaVarejo_ProdutoDadosPedidoB2B>();
        //        foreach (ProductOrderRequest oProductOrderRequest in oOrderRequest.Produtos)
        //        {
        //            ViaVarejo_ProdutoDadosPedidoB2B oViaVarejo_ProdutoDadosPedidoB2B = new ViaVarejo_ProdutoDadosPedidoB2B();
        //            oViaVarejo_ProdutoDadosPedidoB2B.Codigo = Convert.ToInt32(oProductOrderRequest.Codigo);
        //            oViaVarejo_ProdutoDadosPedidoB2B.Quantidade = oProductOrderRequest.Quantidade;
        //            oViaVarejo_ProdutoDadosPedidoB2B.PrecoVenda = oProductOrderRequest.PrecoVenda;
        //            request.Produtos.Add(oViaVarejo_ProdutoDadosPedidoB2B);
        //        }
        //    }

        //    request.Destinatario = new ViaVarejo_DestinatarioDadosPedidoB2B();
        //    request.Destinatario.CpfCnpj = oOrderRequest.EnderecoEntrega.CPFCNPJ;

        //    request.EnderecoEntrega = new ViaVarejo_EnderecoEntregaDadosPedidoB2B();
        //    request.EnderecoEntrega.CEP = oOrderRequest.Destinatario.CEP;
        //    request.EnderecoEntrega.Estado = oOrderRequest.Destinatario.Estado;
        //    request.EnderecoEntrega.Logradouro = oOrderRequest.Destinatario.Logradouro;
        //    request.EnderecoEntrega.Cidade = oOrderRequest.Destinatario.Cidade;
        //    request.EnderecoEntrega.Numero = Convert.ToInt32(oOrderRequest.Destinatario.Numero);
        //    request.EnderecoEntrega.Bairro = oOrderRequest.Destinatario.Bairro;
        //    request.EnderecoEntrega.Complemento = oOrderRequest.Destinatario.Complemento;
        //    request.EnderecoEntrega.Telefone = oOrderRequest.Destinatario.Telefone;
        //    request.EnderecoEntrega.Telefone2 = oOrderRequest.Destinatario.Telefone2;
        //    request.EnderecoEntrega.Telefone3 = oOrderRequest.Destinatario.Telefone3;
        //    request.EnderecoEntrega.Referencia = oOrderRequest.Destinatario.Referencia;

        //    request.Campanha = Convert.ToInt32(oOrderRequest.CodigoCampanhaFornecedor);
        //    request.CNPJ = oOrderRequest.CNPJ;
        //    request.PedidoParceiro = 0;

        //    request.EnderecoCobranca = new ViaVarejo_EnderecoCobrancaDadosPedidoB2B();
        //    request.EnderecoCobranca.CEP = oOrderRequest.Destinatario.CEP;
        //    request.EnderecoCobranca.Estado = oOrderRequest.Destinatario.Estado;
        //    request.EnderecoCobranca.Logradouro = oOrderRequest.Destinatario.Logradouro;
        //    request.EnderecoCobranca.Cidade = oOrderRequest.Destinatario.Cidade;
        //    request.EnderecoCobranca.Numero = Convert.ToInt32(oOrderRequest.Destinatario.Numero);
        //    request.EnderecoCobranca.Bairro = oOrderRequest.Destinatario.Bairro;
        //    request.EnderecoCobranca.Complemento = oOrderRequest.Destinatario.Complemento;
        //    request.EnderecoCobranca.Telefone = oOrderRequest.Destinatario.Telefone;
        //    request.EnderecoCobranca.Telefone2 = oOrderRequest.Destinatario.Telefone2;
        //    request.EnderecoCobranca.Telefone3 = oOrderRequest.Destinatario.Telefone3;
        //    request.EnderecoCobranca.Referencia = oOrderRequest.Destinatario.Referencia;

        //    request.AguardarConfirmacao = true;

        //    var intermediadores = new List<ViaVarejo_IntermediadoresFinanceiros>();

        //    if ((oOrderRequest.DadosPagamento?.ValorComplemento ?? 0) > 0)
        //        intermediadores.Add(new ViaVarejo_IntermediadoresFinanceiros()
        //        {
        //            bandeiraOperadoraCartao = ObterIntermediadoresFinanceirosBandeira(oOrderRequest.DadosPagamento?.NomeBandeira),
        //            cnpjIntermediador = "08.561.701/0001-01",
        //            razaoSocialIntermediador = "PAGSEGURO INTERNET S/A",
        //            formaPagamento = oOrderRequest.DadosPagamento.QuantidadeParcelas > 1 ? IntermediadoresFinanceirosFormaPagamento.APrazo : IntermediadoresFinanceirosFormaPagamento.AVista,
        //            meioPagamento = IntermediadoresFinanceirosMeioPagamento.CartaoCredito,
        //            tipoIntegracaoPagamento = IntermediadoresFinanceirosTipoIntegracaoPagamento.PagamentoNaoIntegradoPOS,
        //            valorPagamento = oOrderRequest.DadosPagamento.ValorComplemento,
        //            numAutorizacaoCartao = oOrderRequest.DadosPagamento.NSU

        //        });

        //    if ((oOrderRequest.DadosPagamento?.ValorPontos ?? 0) > 0)
        //        intermediadores.Add(new ViaVarejo_IntermediadoresFinanceiros()
        //        {
        //            bandeiraOperadoraCartao = null,
        //            cnpjIntermediador = "03.297.361/0001-30",
        //            razaoSocialIntermediador = "DIGI AGENCIA S/A",
        //            formaPagamento = IntermediadoresFinanceirosFormaPagamento.AVista,
        //            meioPagamento = IntermediadoresFinanceirosMeioPagamento.ProgramaFidelidadeCashbackCreditoVirtual,
        //            tipoIntegracaoPagamento = IntermediadoresFinanceirosTipoIntegracaoPagamento.PagamentoNaoIntegradoPOS,
        //            valorPagamento = oOrderRequest.DadosPagamento.ValorPontos,
        //            numAutorizacaoCartao = null

        //        });

        //    request.IntermediadoresFinanceiros = intermediadores;

        //    #endregion Prepara Request

        //    var response = await client.PostAsync($"{_MS_ORDER_INTEGRATION_VIA_VAREJO_URL}/CreateOrder",
        //                                          new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        //    if (!response.IsSuccessStatusCode)
        //        throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

        //    return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<OrderResponse>>(await response.Content.ReadAsStringAsync());
        //}

        public static Dictionary<string, string> IntermediadoresFinanceirosBandeiras = new Dictionary<string, string>() {
            { "VISA", "01" },
            { "MASTERCARD", "02" },
            { "AMERICANEXPRESS", "03" },
            { "SOROCRED", "04" },
            { "DINERSCLUB", "05" },
            { "ELO", "06" },
            { "HIPERCARD", "07" },
            { "AURA", "08" },
            { "CABAL", "09" },
            { "ALELO", "10" },
            { "BANESCARD", "11" },
            { "CALCARD", "12" },
            { "CREDZ", "13" },
            { "DISCOVER", "14" },
            { "GOODCARD", "15" },
            { "GREENCARD", "16" },
            { "HIPER", "17" },
            { "JCB", "18" },
            { "MAIS", "19" },
            { "MAXVAN", "20" },
            { "POLICARD", "21" },
            { "REDECOMPRAS", "22" },
            { "SODEXO", "23" },
            { "VALECARD", "24" },
            { "VEROCHEQUE", "25" },
            { "VR", "26" },
            { "TICKET", "27" },
            { "OUTROS", "99" }
        };
        public static string ObterIntermediadoresFinanceirosBandeira(string nomeBandeira)
        {
            if (IntermediadoresFinanceirosBandeiras.ContainsKey(nomeBandeira?.ToUpper()?.Trim()))
                return IntermediadoresFinanceirosBandeiras[nomeBandeira];

            return IntermediadoresFinanceirosBandeiras["OUTROS"];
        }
        public static class IntermediadoresFinanceirosFormaPagamento
        {
            public static int AVista = 0;
            public static int APrazo = 1;
        }

        public static class IntermediadoresFinanceirosMeioPagamento
        {
            public static string Dinheiro = "01";
            public static string Cheque = "02";
            public static string CartaoCredito = "03";
            public static string CartaoDebito = "04";
            public static string CreditoLoja = "05";
            public static string ValeAlimentacao = "10";
            public static string ValeRefeicao = "11";
            public static string ValePresente = "12";
            public static string ValeCombustivel = "13";
            public static string BoletoBancario = "15";
            public static string DepositoBancario = "16";
            public static string PagamentoInstantaneoPIX = "17";
            public static string TransferenciaBancariaCarteiraDigital = "18";
            public static string ProgramaFidelidadeCashbackCreditoVirtual = "19";
            public static string SemPagamento = "90";
        }
        public static class IntermediadoresFinanceirosTipoIntegracaoPagamento
        {
            public static int PagamentoIntegradoTEF = 1;
            public static int PagamentoNaoIntegradoPOS = 2;
        }

        public async Task<DefaultResponse<OrderResponse>> SetCreateOrder(OrderRequest oOrderRequest, Request_OrderStore oRequest_OrderStore)
        {
            using HttpClient client = new HttpClient();

            #region Prepara Request

            ViaVarejo_PedidoB2BRequest request = new ViaVarejo_PedidoB2BRequest();
            request.ValorFrete = oRequest_OrderStore.ShippingValue;
            request.CNPJ = oOrderRequest.CNPJ;

            if (oRequest_OrderStore.Products != null)
            {
                request.Produtos = new List<ViaVarejo_ProdutoDadosPedidoB2B>();
                foreach (Request_OrderProduct oProductOrderRequest in oRequest_OrderStore.Products)
                {
                    ViaVarejo_ProdutoDadosPedidoB2B oViaVarejo_ProdutoDadosPedidoB2B = new ViaVarejo_ProdutoDadosPedidoB2B();
                    oViaVarejo_ProdutoDadosPedidoB2B.Codigo = Convert.ToInt32(oProductOrderRequest.CodeProduct);
                    oViaVarejo_ProdutoDadosPedidoB2B.Quantidade = oProductOrderRequest.Quantity;
                    oViaVarejo_ProdutoDadosPedidoB2B.PrecoVenda = oProductOrderRequest.ValueUnitary;
                    request.Produtos.Add(oViaVarejo_ProdutoDadosPedidoB2B);
                }
            }

            request.Destinatario = new ViaVarejo_DestinatarioDadosPedidoB2B();
            request.Destinatario.CpfCnpj = oOrderRequest.Recipient.CPFCNPJ;

            request.EnderecoEntrega = new ViaVarejo_EnderecoEntregaDadosPedidoB2B();
            request.EnderecoEntrega.CEP = oOrderRequest.DeliveryAddress.ZipCode;
            request.EnderecoEntrega.Estado = oOrderRequest.DeliveryAddress.State;
            request.EnderecoEntrega.Logradouro = oOrderRequest.DeliveryAddress.PublicPlace;
            request.EnderecoEntrega.Cidade = oOrderRequest.DeliveryAddress.City;
            request.EnderecoEntrega.Numero = Convert.ToInt32(oOrderRequest.DeliveryAddress.Number);
            request.EnderecoEntrega.Bairro = oOrderRequest.DeliveryAddress.District;
            request.EnderecoEntrega.Complemento = oOrderRequest.DeliveryAddress.Complement;
            request.EnderecoEntrega.Telefone = oOrderRequest.DeliveryAddress.Telephone;
            request.EnderecoEntrega.Telefone2 = oOrderRequest.DeliveryAddress.Telephone2;
            request.EnderecoEntrega.Telefone3 = oOrderRequest.DeliveryAddress.Telephone3;
            request.EnderecoEntrega.Referencia = oOrderRequest.DeliveryAddress.Reference;

            request.Campanha = Convert.ToInt32(oOrderRequest.CodigoCampanhaFornecedor);
            request.CNPJ = oOrderRequest.CNPJ;
            request.PedidoParceiro = 0;

            request.EnderecoCobranca = new ViaVarejo_EnderecoCobrancaDadosPedidoB2B();
            request.EnderecoCobranca.CEP = oOrderRequest.DeliveryAddress.ZipCode;
            request.EnderecoCobranca.Estado = oOrderRequest.DeliveryAddress.State;
            request.EnderecoCobranca.Logradouro = oOrderRequest.DeliveryAddress.PublicPlace;
            request.EnderecoCobranca.Cidade = oOrderRequest.DeliveryAddress.City;
            request.EnderecoCobranca.Numero = Convert.ToInt32(oOrderRequest.DeliveryAddress.Number);
            request.EnderecoCobranca.Bairro = oOrderRequest.DeliveryAddress.District;
            request.EnderecoCobranca.Complemento = oOrderRequest.DeliveryAddress.Complement;
            request.EnderecoCobranca.Telefone = oOrderRequest.DeliveryAddress.Telephone;
            request.EnderecoCobranca.Telefone2 = oOrderRequest.DeliveryAddress.Telephone2;
            request.EnderecoCobranca.Telefone3 = oOrderRequest.DeliveryAddress.Telephone3;
            request.EnderecoCobranca.Referencia = oOrderRequest.DeliveryAddress.Reference;

            request.AguardarConfirmacao = true;

            var intermediadores = new List<ViaVarejo_IntermediadoresFinanceiros>();

            if ((oOrderRequest.PaymentData?.ValorComplemento ?? 0) > 0)
                intermediadores.Add(new ViaVarejo_IntermediadoresFinanceiros()
                {
                    bandeiraOperadoraCartao = ObterIntermediadoresFinanceirosBandeira(oOrderRequest.PaymentData?.NomeBandeira),
                    cnpjIntermediador = "08.561.701/0001-01",
                    razaoSocialIntermediador = "PAGSEGURO INTERNET S/A",
                    formaPagamento = oOrderRequest.PaymentData.QuantidadeParcelas > 1 ? IntermediadoresFinanceirosFormaPagamento.APrazo : IntermediadoresFinanceirosFormaPagamento.AVista,
                    meioPagamento = IntermediadoresFinanceirosMeioPagamento.CartaoCredito,
                    tipoIntegracaoPagamento = IntermediadoresFinanceirosTipoIntegracaoPagamento.PagamentoNaoIntegradoPOS,
                    valorPagamento = oOrderRequest.PaymentData.ValorComplemento,
                    numAutorizacaoCartao = oOrderRequest.PaymentData.NSU

                });

            if ((oOrderRequest.PaymentData?.ValorPontos ?? 0) > 0)
                intermediadores.Add(new ViaVarejo_IntermediadoresFinanceiros()
                {
                    bandeiraOperadoraCartao = null,
                    cnpjIntermediador = "03.297.361/0001-30",
                    razaoSocialIntermediador = "DIGI AGENCIA S/A",
                    formaPagamento = IntermediadoresFinanceirosFormaPagamento.AVista,
                    meioPagamento = IntermediadoresFinanceirosMeioPagamento.ProgramaFidelidadeCashbackCreditoVirtual,
                    tipoIntegracaoPagamento = IntermediadoresFinanceirosTipoIntegracaoPagamento.PagamentoNaoIntegradoPOS,
                    valorPagamento = oOrderRequest.PaymentData.ValorPontos,
                    numAutorizacaoCartao = null

                });

            request.IntermediadoresFinanceiros = intermediadores;

            #endregion Prepara Request

            var response = await client.PostAsync($"{_MS_ORDER_INTEGRATION_VIA_VAREJO_URL}/CreateOrder",
                                                  new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<OrderResponse>>(await response.Content.ReadAsStringAsync());
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

            var response = await client.PostAsync($"{_MS_ORDER_INTEGRATION_VIA_VAREJO_URL}/CalcShipping",
                                                  new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(oRequest), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            //return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<CalcShippingResponse_Store>>(await response.Content.ReadAsStringAsync());

            DefaultResponse<CalcShippingResponse_Store> oResult = Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<CalcShippingResponse_Store>>(await response.Content.ReadAsStringAsync());
            oResult.Data.Store = Ms.Api.Utilities.Enum.Enums.Store.Ponto;
            return oResult;
        }

    }
}
