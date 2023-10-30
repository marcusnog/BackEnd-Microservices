using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.DTO.Response;
using Ms.Api.Utilities.Models;

namespace MsOrderApi.Contracts.Services
{
    public interface IViaVarejoService
    {
        Task<DefaultResponse<CalcShippingResponse_Store>> GetShippingValue(String CEP, CalcShippingRequest_Store oCalcShippingRequest_Store);
        //Task<DefaultResponse<OrderResponse>> SetCreateOrder(OrderRequest request);
        Task<DefaultResponse<OrderResponse>> SetCreateOrder(OrderRequest request, Request_OrderStore oRequest_OrderStore);

    }
}
