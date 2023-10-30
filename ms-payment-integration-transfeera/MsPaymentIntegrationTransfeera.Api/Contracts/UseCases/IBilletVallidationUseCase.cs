using Ms.Api.Utilities.Models;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.UseCases
{
    public interface IBilletVallidationUseCase
    {
        Task<DefaultResponse<Billet>> Execute(string barcode, int campaignId, string cpfCnpj, string email, string nome, decimal userPoints);
    }
}
