using MsPaymentIntegrationTransfeera.Api.Contracts.DTO;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Request;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Response.Billets;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.Services
{
    public interface IIntegrationTransfeeraService
    {
        decimal VlrMiniminimumBalanceTransfeera { get; }
        Task<TransfeeraAuthResponse> GetAuthentication();
        Task<ValidateCIPResponse> ValidateBilletOnCIP(string code);
        Task<TransfeeraCheckBalanceResponse> CheckBalance();
        Task<TransfeeraCreateBilletBatchResponse> CreateBilletBatch(TransfeeraCreateBilletBatchRequest request);
        Task<List<TransfeeraGetBilletResponse>> GetBillets(TransfeeraGetBilletsRequest request);
        Task<TransfeeraGetBilletResponse> GetBillet(long billetId);
        Task<TransfeeraCloseBatchResponse> CloseBatch(int Id);
        Task<TransfeeraGetBilletResponse> GetBatchBillet(int batchId);
    }
}
