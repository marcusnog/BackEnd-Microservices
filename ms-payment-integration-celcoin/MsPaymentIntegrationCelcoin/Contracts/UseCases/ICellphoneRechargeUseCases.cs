
using Ms.Api.Utilities.DTO.Request;
using MsPaymentIntegrationCelcoin.Contracts.DTO;

namespace MsPaymentIntegrationCelcoin.Contracts.UseCases
{
    public interface ICellphoneRechargeUseCases
    {
        CelcoinReserveBalanceRequest FillCelcoinObject(CellphoneRechargeRequest model);
        Task<string> Execute(CellphoneRechargeRequest model);
    }
}
