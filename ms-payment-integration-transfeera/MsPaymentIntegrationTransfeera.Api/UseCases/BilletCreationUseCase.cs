using Ms.Api.Utilities.Extensions;
using Ms.Api.Utilities.Models;
using MsPaymentIntegrationCelcoin.Contracts.Repository;
using MsPaymentIntegrationCelcoin.Repository;
using MsPaymentIntegrationTransfeera.Api.Contracts.Data;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Request;
using MsPaymentIntegrationTransfeera.Api.Contracts.Services;
using MsPaymentIntegrationTransfeera.Api.Contracts.UseCases;
using MsPaymentIntegrationTransfeera.Api.Services;

namespace MsPaymentIntegrationTransfeera.Api.UseCases
{
    public class BilletCreationUseCase : IBilletCreationUseCase
    {
        readonly IBilletContext _context;
        readonly IBilletRepository _billetRepository;
        readonly IIntegrationTransfeeraService _transfeeraService;
        Billet _newBillet = null;

        public BilletCreationUseCase(IBilletContext context, IIntegrationTransfeeraService transfeeraService, IBilletRepository billetRepository)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _billetRepository = billetRepository ?? new BilletRepository(_context);
            _transfeeraService = transfeeraService ?? new IntegrationTransfeeraService();
        }

        public async Task<DefaultResponse<Billet>> Execute(BilletRequest billet)
        {
            try
            {
                _newBillet = new Billet()
                {
                    ParticipantId = billet.ParticipantId,
                    ParticipantName = billet.ParticipantName,
                    Email = billet.Email,
                    CampaignId = billet.CampaignId,
                    BilletValue = billet.BilletValue,
                    BilletFeeValue = billet.BilletFeeValue,
                    BilletPointsValue = billet.BilletPointsValue,
                    CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                    StatusTransfeera = "CRIADO",
                    TransfeeraTransactionId = null,
                    ErrorMessage = null,
                    BilletDetails = new BilletDetails()
                    {
                        BankCode = billet.BilletDetails.BankCode,
                        BankName = billet.BilletDetails.BankName,
                        Barcode = billet.BilletDetails.Barcode,
                        DigitableLine = billet.BilletDetails.DigitableLine,
                        DueDate = billet.BilletDetails.DueDate,
                        Value = billet.BilletDetails.Value,
                        Type = billet.BilletDetails.Type
                    },
                    BilletPaymentInfos = new BilletPayment()
                    {
                        RecipientDocument = billet.BilletPaymentInfos.RecipientDocument,
                        RecipientName = billet.BilletPaymentInfos.RecipientName,
                        PayerDocument = billet.BilletPaymentInfos.PayerDocument,
                        PayerName = billet.BilletPaymentInfos.PayerName,
                        DueDate = billet.BilletPaymentInfos.DueDate,
                        LimitDate = billet.BilletPaymentInfos.LimitDate,
                        MinValue = billet.BilletPaymentInfos.MinValue,
                        MaxValue = billet.BilletPaymentInfos.MaxValue,
                        FineValue = billet.BilletPaymentInfos.FineValue,
                        InterestValue = billet.BilletPaymentInfos.InterestValue,
                        OriginalValue = billet.BilletPaymentInfos.OriginalValue,
                        TotalUpdatedValue = billet.BilletPaymentInfos.TotalUpdatedValue,
                        TotalDiscountValue = billet.BilletPaymentInfos.TotalDiscountValue,
                        TotalAdditionalValue = billet.BilletPaymentInfos.TotalAdditionalValue
                    }
                };

                // salvar na base
                await _billetRepository.Create(_newBillet);

                // criar boleto na transfeera
                var response = await _transfeeraService.CreateBilletBatch(new TransfeeraCreateBilletBatchRequest()
                {
                    name = "BatchWithBillet",
                    billets = new List<TransfeeraCreateBilletRequest>
                    {
                        new TransfeeraCreateBilletRequest()
                        {
                            barcode = _newBillet.BilletDetails.Barcode ?? _newBillet.BilletDetails.DigitableLine,
                            description = $"Pagamento {_newBillet.Id}",
                            integration_id = _newBillet.Id.ToString(),
                            payment_date = DateTime.Now.ToString("yyyy-MM-dd"),
                            value = billet.BilletValue,
                        }
                    },
                    type = "BOLETO"
                });

                // atualizar id na base
                _newBillet.TransfeeraTransactionId = response.id.ToString();

                await _billetRepository.Update(_newBillet);

                //fechar lote
                var retornoClose = _transfeeraService.CloseBatch(response.id);

                // retornar sucesso
                return new DefaultResponse<Billet>(_newBillet);

            }
            catch (Exception ex)
            {
                _newBillet.StatusTransfeera = "CANCELADO";
                _newBillet.ErrorMessage = ex.Message.ToString();
                await _billetRepository.Update(_newBillet);
                return new DefaultResponse<Billet>($"Failed to confirm payment: {ex.Message}");
            }
        }
    }
}
