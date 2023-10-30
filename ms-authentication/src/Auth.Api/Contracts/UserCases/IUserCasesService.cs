using Auth.Api.Contracts.DTOs;

namespace Auth.Api.Contracts.UserCases
{
    public interface ICampaignConnectorLoginUseCase
    {
        Task<CampaignInfo?> Login(string environment, string campaign, string token);
    }
}
