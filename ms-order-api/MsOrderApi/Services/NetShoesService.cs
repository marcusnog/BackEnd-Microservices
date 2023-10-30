using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.DTO.Response;
using Ms.Api.Utilities.Exceptions;
using Ms.Api.Utilities.Extensions;
using Ms.Api.Utilities.Models;
using MsOrderApi.Contracts.Services;
using System.Text;

namespace MsOrderApi.Services
{
    public class NetShoesService : INetShoesService
    {
        readonly IConfiguration _configuration;
        readonly string _MS_ORDER_INTEGRATION_NETSHOES_URL;

        public NetShoesService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _MS_ORDER_INTEGRATION_NETSHOES_URL = _configuration.GetValue<string>("MS_ORDER_INTEGRATION_NETSHOES_URL");
        }

        //public async Task<DefaultResponse<CalcShippingResponse_Store>> GetShippingValue(CalcShippingRequest_Store request)
        //{
        //    using HttpClient client = new HttpClient();

        //    //var response = await client.PostAsync($"{_MS_ORDER_INTEGRATION_NETSHOES_URL}", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request),
        //    //    Encoding.UTF8, "application/json"));
        //    var response = await client.PostAsync($"{_MS_ORDER_INTEGRATION_NETSHOES_URL}/CalcShipping",
        //                                          new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        //    if (!response.IsSuccessStatusCode)
        //        throw new CodeException($"{ErrorCode.ms_order_integration_netshoes}.{1.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

        //    return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<CalcShippingResponse_Store>>(await response.Content.ReadAsStringAsync());
        //}

        //public async Task<DefaultResponse<NetShoes_ResultPedido>> SetCreateOrder(NetShoes_CreateOrderRequest request)
        //public async Task<DefaultResponse<OrderResponse>> SetCreateOrder(OrderRequest oOrderRequest)
        //{
        //    using HttpClient client = new HttpClient();

        //    #region Prepara Request

        //    NetShoes_CreateOrderRequest request = new NetShoes_CreateOrderRequest();

        //    //int iCount = 0;
        //    //double valorTotal = 0;
        //    if (oOrderRequest.Produtos != null && oOrderRequest.Produtos.Count > 0)
        //    {
        //        request.Produtos = new List<NetShoes_ProductCreateOrder>();
        //        foreach (ProductOrderRequest oProductOrderRequest in oOrderRequest.Produtos)
        //        {
        //            NetShoes_ProductCreateOrder oNetShoes_ProductCreateOrder = new NetShoes_ProductCreateOrder();
        //            oNetShoes_ProductCreateOrder.Codigo = oProductOrderRequest.Codigo;
        //            oNetShoes_ProductCreateOrder.Quantidade = oProductOrderRequest.Quantidade;
        //            oNetShoes_ProductCreateOrder.PrecoVenda = oProductOrderRequest.PrecoVenda;
        //            request.Produtos.Add(oNetShoes_ProductCreateOrder);

        //            //valorTotal += (double)oProductOrderRequest.PrecoVenda * oProductOrderRequest.Quantidade;
        //            //iCount++;
        //        }
        //    }

        //    request.Destinatario = new NetShoes_RecipientOrder();
        //    request.Destinatario.CPFCNPJ = oOrderRequest.EnderecoEntrega.CPFCNPJ;

        //    request.EnderecoEntrega = new NetShoes_DeliveryAddress();
        //    request.EnderecoEntrega.CEP = oOrderRequest.Destinatario.CEP;
        //    request.EnderecoEntrega.Estado = oOrderRequest.Destinatario.Estado;
        //    request.EnderecoEntrega.Logradouro = oOrderRequest.Destinatario.Logradouro;
        //    request.EnderecoEntrega.Cidade = oOrderRequest.Destinatario.Cidade;
        //    request.EnderecoEntrega.Numero = oOrderRequest.Destinatario.Numero;
        //    request.EnderecoEntrega.Bairro = oOrderRequest.Destinatario.Bairro;
        //    request.EnderecoEntrega.Complemento = oOrderRequest.Destinatario.Complemento;
        //    request.EnderecoEntrega.Telefone = oOrderRequest.Destinatario.Telefone;
        //    request.EnderecoEntrega.Telefone2 = oOrderRequest.Destinatario.Telefone2;
        //    request.EnderecoEntrega.Telefone3 = oOrderRequest.Destinatario.Telefone3;
        //    request.EnderecoEntrega.Referencia = oOrderRequest.Destinatario.Referencia;

        //    request.PedidoParceiro = 0;

        //    #endregion Prepara Request

        //    var response = await client.PostAsync($"{_MS_ORDER_INTEGRATION_NETSHOES_URL}/CreateOrder",
        //                                          new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

        //    if (!response.IsSuccessStatusCode)
        //        throw new CodeException($"{ErrorCode.ms_order_integration_novapontocom}.{12.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

        //    return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<OrderResponse>>(await response.Content.ReadAsStringAsync());
        //}


        public async Task<DefaultResponse<OrderResponse>> SetCreateOrder(OrderRequest oOrderRequest, Request_OrderStore oRequest_OrderStore)
        {
            using HttpClient client = new HttpClient();

            #region Prepara Request

            NetShoes_CreateOrderRequest request = new NetShoes_CreateOrderRequest();

            //int iCount = 0;
            //double valorTotal = 0;
            if (oRequest_OrderStore.Products != null)
            {
                request.Produtos = new List<NetShoes_ProductCreateOrder>();
                foreach (Request_OrderProduct oProductOrderRequest in oRequest_OrderStore.Products)
                {
                    NetShoes_ProductCreateOrder oNetShoes_ProductCreateOrder = new NetShoes_ProductCreateOrder();
                    oNetShoes_ProductCreateOrder.Codigo = oProductOrderRequest.CodeProduct;
                    oNetShoes_ProductCreateOrder.Quantidade = oProductOrderRequest.Quantity;
                    oNetShoes_ProductCreateOrder.PrecoVenda = oProductOrderRequest.ValueUnitary;
                    request.Produtos.Add(oNetShoes_ProductCreateOrder);

                    //valorTotal += (double)oProductOrderRequest.PrecoVenda * oProductOrderRequest.Quantidade;
                    //iCount++;
                }
            }

            request.Destinatario = new NetShoes_RecipientOrder();
            request.Destinatario.CPFCNPJ = oOrderRequest.Recipient.CPFCNPJ;

            request.EnderecoEntrega = new NetShoes_DeliveryAddress();
            request.EnderecoEntrega.CEP = oOrderRequest.DeliveryAddress.ZipCode;
            request.EnderecoEntrega.Estado = oOrderRequest.DeliveryAddress.State;
            request.EnderecoEntrega.Logradouro = oOrderRequest.DeliveryAddress.PublicPlace;
            request.EnderecoEntrega.Cidade = oOrderRequest.DeliveryAddress.City;
            request.EnderecoEntrega.Numero = oOrderRequest.DeliveryAddress.Number;
            request.EnderecoEntrega.Bairro = oOrderRequest.DeliveryAddress.District;
            request.EnderecoEntrega.Complemento = oOrderRequest.DeliveryAddress.Complement;
            request.EnderecoEntrega.Telefone = oOrderRequest.DeliveryAddress.Telephone;
            request.EnderecoEntrega.Telefone2 = oOrderRequest.DeliveryAddress.Telephone2;
            request.EnderecoEntrega.Telefone3 = oOrderRequest.DeliveryAddress.Telephone3;
            request.EnderecoEntrega.Referencia = oOrderRequest.DeliveryAddress.Reference;

            request.PedidoParceiro = 0;

            #endregion Prepara Request

            var response = await client.PostAsync($"{_MS_ORDER_INTEGRATION_NETSHOES_URL}/CreateOrder",
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

            //var response = await client.PostAsync($"{_MS_ORDER_INTEGRATION_NETSHOES_URL}", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request),
            //    Encoding.UTF8, "application/json"));
            var response = await client.PostAsync($"{_MS_ORDER_INTEGRATION_NETSHOES_URL}/CalcShipping",
                                                  new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(oRequest), Encoding.UTF8, "application/json"));

            if (!response.IsSuccessStatusCode)
                throw new CodeException($"{ErrorCode.ms_order_integration_netshoes}.{1.ToHex(4)}", "unexpected return", new Exception(response.Content.ReadAsStringAsync().Result));

            //return Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<CalcShippingResponse_Store>>(await response.Content.ReadAsStringAsync());
            DefaultResponse<CalcShippingResponse_Store> oResult = Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<CalcShippingResponse_Store>>(await response.Content.ReadAsStringAsync());
            oResult.Data.Store = Ms.Api.Utilities.Enum.Enums.Store.NetShoes;
            return oResult;
        }

    }
}
