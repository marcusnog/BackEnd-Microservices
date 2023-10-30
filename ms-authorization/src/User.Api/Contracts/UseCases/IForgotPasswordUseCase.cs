namespace User.Api.Contracts.UseCases
{
    public interface IForgotPasswordUseCase
    {
        Task StartRecoveryPassword(string lang, string userId, string name, string ip, string email, DTOs.System system);
        Task<bool> ValidateToken(string tokenId);
        Task<bool> ValidateToken(string tokenId, string code);

        Task UpdatePassword(string tokenId, string password);
        Task UpdatePasswordUserParticipant(string tokenId, string password);
    }
}
