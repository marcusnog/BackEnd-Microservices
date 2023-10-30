using Ms.Api.Utilities.Models;
using MsProductDetailsApi.Contracts.DTOs;

namespace MsProductDetailsApi.Contracts.Services
{
    public interface INetShoesService
    {
        Task<DefaultResponse<CalcShippingResponse>> GetShippingValue(CalcShipping request);
    }
}
