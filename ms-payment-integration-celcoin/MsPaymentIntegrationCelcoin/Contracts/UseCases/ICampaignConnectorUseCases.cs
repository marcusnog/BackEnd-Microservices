using Ms.Api.Utilities.DTO.Request;

namespace MsPaymentIntegrationCelcoin.Contracts.UseCases
{
    public interface ICampaignConnectorUseCases
    {
        Task<string?> GetPoints(CampaignUserRequest request);
        Task<string?> BookPoints(CampaignUserRequest request);
        void EffectDebitPoints(CampaignUserRequest request);
        void ReleasePoints(CampaignUserRequest request);
        void ReversePoints(CampaignUserRequest request);
    }
}
