using Ms.Api.Utilities.DTO.Response;
using Ms.Api.Utilities.Models;
using MsPaymentIntegrationCelcoin.Contracts.DTO.Request;

namespace MsPaymentIntegrationCelcoin.Contracts.Services
{
    public interface IIntegrationCelcoinService
    {
        Task<IEnumerable<SelectItem<string>>?> GetCellphoneOperators();
        Task<IEnumerable<SelectItem<string>>?> GetCellphoneOperatingValues(string providerId);
        Task<CelcoinValidateOperatorResponse> GetOperatorByCellphoneNumber(int stateCode, int phoneNumber);
        Task<CelcoinReserveBalanceResponse> ReserveBalanceRecharge(CelcoinReserveBalanceRequest model);
        Task<CelcoinReserveBalanceResponse> ConfirmRecharge(CelcoinConfirmRechargeRequest model);
        Task<CelcoinCancelRechargeResponse> CancelRecharge(string transactionId);
        Task<MsPaymentIntegrationCelcoin.Contracts.DTO.Response.CelcoinBalanceResponse> GetBalance();
        Task<MsPaymentIntegrationCelcoin.Contracts.DTO.Response.CelcoinGetInfosRechargeResponse> GetInfosRecharge(Int32 transactionId);
    }
}
