using Ms.Api.Utilities.DTO;
using Ms.Api.Utilities.DTO.Request;

namespace MsOrderApi.Contracts.Services
{
    public interface IIntegrationTransfeeraService
        {
    //        decimal VlrMiniminimumBalanceTransfeera { get; }
    //        Task<TransfeeraAuthResponse> GetAuthentication();
    //        Task<ValidateCIPResponse> ValidateBilletOnCIP(string code);
    //        Task<TransfeeraCheckBalanceResponse> CheckBalance();
            Task<Billet> ConfirmPaymentBillet(BilletRequest request);
    //        Task<List<TransfeeraGetBilletResponse>> GetBillets(TransfeeraGetBilletsRequest request);
    //        Task<TransfeeraGetBilletResponse> GetBillet(long billetId);
    //        Task<TransfeeraCloseBatchResponse> CloseBatch(int Id);
    //        Task<TransfeeraGetBilletResponse> GetBatchBillet(int batchId);
        }

}
