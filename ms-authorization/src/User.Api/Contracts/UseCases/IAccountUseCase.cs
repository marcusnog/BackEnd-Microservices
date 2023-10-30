using Ms.Api.Utilities.Models;
using User.Api.Contracts.DTOs.Request;

namespace User.Api.Contracts.UseCases
{
    public interface IAccountUseCase
    {
        Task<Account> GetAccount(string cpf, string token);
        Task<Account> CreateAccount(CreateAccountRequest request, string token);
        Task<decimal> GetBalancePoints(string cpf, string token);
        Task<DefaultResponse<bool>> UpdateBalance(UpdateBalanceRequest request, string token);
    }
}
