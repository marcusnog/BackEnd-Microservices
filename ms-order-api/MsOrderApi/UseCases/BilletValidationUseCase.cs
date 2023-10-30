//using Ms.Api.Utilities.DTO;
//using Ms.Api.Utilities.Models;
//using MsOrderApi.Contracts.Services;
//using MsOrderApi.Contracts.UseCases;
//using MsOrderApi.Services.Transfeera;

//namespace MsOrderApi.UseCases
//{
//    public class BilletValidationUseCase : IBilletVallidationUseCase
//    {
//        static int IdLojaTransfeera = 204;
//        //readonly ICampaignValidator _campaignValidator;
//        readonly IIntegrationTransfeeraService _transfeeraService;

//        public BilletValidationUseCase(IIntegrationTransfeeraService transfeeraService = null)
//        {
//            //_campaignValidator = campaignValidator ?? new CampanhaBusiness();
//            _transfeeraService = transfeeraService ?? new IntegrationTransfeeraService();
//        }

//        public async Task<DefaultResponse<Billet>> Execute(string barcode, int campaignId, string cpfCnpj, string email, string nome, decimal userPoints)
//        {
//            try
//            {
//                // check if campaign has Transfeera partner enabled
//                //if (_campaignValidator?.CampanhaPossuiLoja(campaignId, IdLojaTransfeera)?.Data != true)
//                //    return new Result<Billet>("Campanha não permite pagamento de contas");

//                //// get campaign conversion factor
//                //var campanha = _campaignValidator.ObterPorId(campaignId)?.Data;
//                //if (campanha == null)
//                //    return new Result<Billet>($"Não foi possivel obter dados da campanha ({campaignId})");

//                var billetValidation = await _transfeeraService.ValidateBilletOnCIP(barcode);
//                if (!billetValidation.isSuccess)
//                    return new DefaultResponse<Billet>(billetValidation.message);

//                // TODO: converter pontos, adicionar taxa DIGI
//                //var digiFee = CalculateDigiFee(billetValidation.payment_info.total_updated_value, billetValidation.barcode_details.type, campanha.FatorConversao);
//                var digiFee = CalculateDigiFee(billetValidation.payment_info.total_updated_value, billetValidation.barcode_details.type, 1);

//                // TODO: check user points
//                if (userPoints < digiFee.Item2)
//                    return new DefaultResponse<Billet>($"You do not have enough for payment (Balance Points: {digiFee.Item2})");

//                var vlrSaldoMinimoTransfeera = _transfeeraService.VlrMiniminimumBalanceTransfeera;

//                var balanceResponse = await _transfeeraService.CheckBalance();

//                if (balanceResponse.value <= vlrSaldoMinimoTransfeera)
//                {
//                    //new Email("contato@reconhece.vc").Enviar(new List<string> { "levi.esteves@digi.ag", "gabriel@digi.ag", "andre.araujo@digi.ag" }, null, "Saldo em conta Transfeera", $"Informamos que o saldo atual da conta na Transfeera está abaixo do valor mínimo de R${vlrSaldoMinimoTransfeera.ToString("N2")}.{"<br/>"}Por favor, faça uma recarga.", null);
//                }

//                if (balanceResponse.value < billetValidation.payment_info.total_updated_value)
//                    return new DefaultResponse<Billet>($"Failed to perform validation, please try again later");

//                // salvar Billet
//                //var billet = new Billet()
//                //{
//                //    ID_CAMPANHA = campaignId,
//                //    VALOR_BILLET = billetValidation?.barcode_details?.value ?? 0,
//                //    VALOR_BILLET_TAXA = digiFee.Item1,
//                //    VALOR_BILLET_PONTOS = digiFee.Item2,
//                //    EMAIL = email,
//                //    NOME = nome,
//                //    CPFCNPJ = cpfCnpj,
//                //    Detalhe = new BilletDetails()
//                //    {
//                //        BANK_CODE = billetValidation?.barcode_details?.bank_code,
//                //        BANK_NAME = billetValidation?.barcode_details?.bank_name,
//                //        BARCODE = billetValidation?.barcode_details?.barcode,
//                //        DIGITABLE_LINE = billetValidation?.barcode_details?.digitable_line,
//                //        DUE_DATE = billetValidation?.barcode_details?.due_date,
//                //        VALUE = billetValidation?.barcode_details?.value,
//                //        TYPE = billetValidation?.barcode_details?.type,
//                //    },
//                //    InformacoesPagamento = new BilletPayment()
//                //    {
//                //        RECIPIENT_DOCUMENT = billetValidation?.payment_info?.recipient_document,
//                //        RECIPIENT_NAME = billetValidation?.payment_info?.recipient_name,
//                //        PAYER_DOCUMENT = billetValidation?.payment_info?.payer_document,
//                //        PAYER_NAME = billetValidation?.payment_info?.payer_name,
//                //        DUE_DATE = billetValidation?.payment_info?.due_date,
//                //        LIMIT_DATE = billetValidation?.payment_info?.limit_date,
//                //        MIN_VALUE = billetValidation?.payment_info?.min_value,
//                //        MAX_VALUE = billetValidation?.payment_info?.max_value,
//                //        FINE_VALUE = billetValidation?.payment_info?.fine_value,
//                //        INTEREST_VALUE = billetValidation?.payment_info?.interest_value,
//                //        ORIGINAL_VALUE = billetValidation?.payment_info?.original_value,
//                //        TOTAL_UPDATED_VALUE = billetValidation?.payment_info?.total_updated_value,
//                //        TOTAL_DISCOUNT_VALUE = billetValidation?.payment_info?.total_discount_value,
//                //        TOTAL_ADDITIONAL_VALUE = billetValidation?.payment_info?.total_additional_value,
//                //    }
//                //};

//                // retornar sucesso
//                //return new DefaultResponse<Billet>(billet);
//                return null;
//            }
//            catch (Exception ex)
//            {
//                return new DefaultResponse<Billet>(ex.Message);
//            }
//        }

//        Tuple<decimal, decimal> CalculateDigiFee(decimal billetTotalValue, string billetType, decimal conversionFactor)
//        {
//            decimal points;
//            switch (billetType)
//            {
//                case "SIMPLES":
//                    billetTotalValue += 5;
//                    break;
//                case "ARRECADACAO":
//                    billetTotalValue += 6;
//                    break;
//                default:
//                    throw new Exception($"Unrecognized Billet Type ({billetType}).");
//            }

//            if (conversionFactor == 0)
//                throw new Exception($"Invalid conversion factor (0).");

//            points = (int)Math.Round(billetTotalValue / conversionFactor);
//            return new Tuple<decimal, decimal>(billetTotalValue, points);
//        }
//    }
//}
