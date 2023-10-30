using Ms.Api.Utilities.Models;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Request;

namespace MsPaymentIntegrationTransfeera.Api.Contracts.UseCases
{
    public interface IBilletCreationUseCase
    {
        Task<DefaultResponse<Billet>> Execute(BilletRequest billet);
    }
}
