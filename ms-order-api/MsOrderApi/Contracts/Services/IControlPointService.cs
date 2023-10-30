using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.Models;

namespace MsOrderApi.Contracts.Services
{
    public interface IControlPointInternalService
    {
        Task<DefaultResponse<string>> Booking(CreateReserveMovimentRequest request);
        Task<DefaultResponse<string>> EffetiveDebit(EffectDebitRequest request);
        Task<DefaultResponse<bool>> ReleasePoints(ReleasePointsRequest request);
        Task<DefaultResponse<string>> ReversePoints(ReversePointsRequest request);

    }

    public interface IControlPointExternalService
    {
        Task<DefaultResponse<string>> BookPoints(CampaignUserRequest request);
        Task<DefaultResponse<string>> EffectDebitPoints(CampaignUserRequest request);
        Task<DefaultResponse<bool>> ReleasePoints(CampaignUserRequest request);
        Task<DefaultResponse<string>> ReversePoints(CampaignUserRequest request);

    }

}
