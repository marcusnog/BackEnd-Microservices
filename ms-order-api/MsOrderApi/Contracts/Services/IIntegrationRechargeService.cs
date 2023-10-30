using Ms.Api.Utilities.DTO;
using Ms.Api.Utilities.DTO.Response;
using Ms.Api.Utilities.Models;
using MsOrderApi.Services;

namespace MsOrderApi.Contracts.Services
{
    public interface IIntegrationRechargeService
    {
    //        decimal VlrMiniminimumBalanceTransfeera { get; }
    //        Task<TransfeeraAuthResponse> GetAuthentication();
    //        Task<ValidateCIPResponse> ValidateBilletOnCIP(string code);
    //        Task<TransfeeraCheckBalanceResponse> CheckBalance();
            Task<DefaultResponse<CelcoinReserveBalanceResponse>> ConfirmPaymentRecharge(CellphoneRechargeRequest request);
    //        Task<List<TransfeeraGetBilletResponse>> GetBillets(TransfeeraGetBilletsRequest request);
    //        Task<TransfeeraGetBilletResponse> GetBillet(long billetId);
    //        Task<TransfeeraCloseBatchResponse> CloseBatch(int Id);
    //        Task<TransfeeraGetBilletResponse> GetBatchBillet(int batchId);
        }

}
