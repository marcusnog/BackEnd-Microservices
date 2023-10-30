
using Ms.Api.Utilities.Extensions;
using MsPaymentIntegrationCelcoin.Contracts.Data;
using MsPaymentIntegrationCelcoin.Contracts.DTO;
using MsPaymentIntegrationCelcoin.Contracts.DTO.Request;
using MsPaymentIntegrationCelcoin.Contracts.Repository;
using MsPaymentIntegrationCelcoin.Contracts.UseCases;

namespace MsPaymentIntegrationCelcoin.UseCases
{
    public class CellphoneRechargeUseCases : ICellphoneRechargeUseCases
    {
        readonly ICellphoneRechargeContext _context;
        readonly ICellphoneRechargeRepository _cellphoneRechargeRepository;

        public CellphoneRechargeUseCases(ICellphoneRechargeContext context, ICellphoneRechargeRepository cellphoneRechargeRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _cellphoneRechargeRepository = cellphoneRechargeRepository ?? throw new ArgumentNullException(nameof(cellphoneRechargeRepository));
        }

        public CelcoinReserveBalanceRequest FillCelcoinObject(CellphoneRechargeRequest model)
        {
            var newId = Guid.NewGuid().GetHashCode();

            var celcoinModel = new CelcoinReserveBalanceRequest()
            {
                externalNsu = newId,
                topupData = new CelcoinTopupData()
                {
                    value = model.RechargeValue
                },
                cpfCnpj = model.ParticipantDocument,
                providerId = model.ProviderId,
                phone = new CelcoinPhone()
                {
                    countryCode = 55,
                    stateCode = model.StateCode,
                    number = model.PhoneNumber
                }
            };

            return celcoinModel;
        }

        public async Task<string> Execute(CellphoneRechargeRequest model)
        {
            var modelInsert = new CellphoneRecharge()
            {
                CodeRequestReconhece = model.CodeRequestReconhece,
                CampaignId = model.CampaignId,
                CelcoinTransactionId = model.CelcoinTransactionId,
                CellphoneOperator = model.CellphoneOperator,
                ParticipantId = model.ParticipantId,
                ParticipantName = model.ParticipantName,
                PhoneNumber = model.PhoneNumber,
                StateCode = model.StateCode,
                RechargeValue = model.RechargeValue,
                RechargeFeeValue = model.RechargeFeeValue,
                RechargePointsValue = model.RechargePointsValue,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp()
            };

            await _cellphoneRechargeRepository.Create(modelInsert);

            return modelInsert.Id;
        }
    }
}
