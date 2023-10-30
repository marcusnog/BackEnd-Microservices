using Ms.Api.Utilities.Models;
using Ms.Campaign.Connector.Contracts.DTO.Campaign;
using Ms.Campaign.Connector.Contracts.DTO.Response;

namespace Ms.Campaign.Connector.Contracts.UseCases.Login
{
    public interface LoginUseCase
    {
        string[] campaigns { get; }
        Task<CampaignInfo> GetUser(string environment, string campaign, string token);
        Task<decimal> GetPoints(string token, string environment, string campaign);
        Task<ChocolateDefaultResponse<string>> BookPoints(string token, string environment, string? campaign, decimal? points);
        void EffectDebitPoints(string token, string environment, string campaign, string? releaseCode, string? orderNumber);
        void ReleasePoints(string token, string environment, string campaign, string? releaseCode);
        void ReversePoints(string token, string environment, string campaign, string? points, string? requestNumber);
    }
}
