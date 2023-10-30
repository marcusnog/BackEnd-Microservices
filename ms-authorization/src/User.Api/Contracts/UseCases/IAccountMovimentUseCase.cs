using Ms.Api.Utilities.DTO;
using Ms.Api.Utilities.Models;
using User.Api.Contracts.DTOs.Request;

namespace User.Api.Contracts.UseCases
{
    public interface IAccountMovimentUseCase
    {
        Task<string> CreditPoints(CreditPointsRequest request, string token);
        Task<bool> DistributePoints(EffectDebitAdminRequest request, string token);
    }
}
