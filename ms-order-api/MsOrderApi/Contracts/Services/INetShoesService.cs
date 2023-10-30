using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.DTO.Response;
using Ms.Api.Utilities.Models;

namespace MsOrderApi.Contracts.Services
{
    public interface INetShoesService
    {
        //Task<DefaultResponse<CalcShippingResponse_Store>> GetShippingValue(CalcShippingRequest_Store request);

        //Task<DefaultResponse<NetShoes_ResultPedido>> SetCreateOrder(NetShoes_CreateOrderRequest request);
        //Task<DefaultResponse<OrderResponse>> SetCreateOrder(OrderRequest request);
        Task<DefaultResponse<OrderResponse>> SetCreateOrder(OrderRequest request, Request_OrderStore oRequest_OrderStore);

        Task<DefaultResponse<CalcShippingResponse_Store>> GetShippingValue(String CEP, CalcShippingRequest_Store oCalcShippingRequest_Store);


    }
}
