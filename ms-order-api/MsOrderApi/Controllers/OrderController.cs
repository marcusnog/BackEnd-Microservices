using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.WebEncoders.Testing;
using Ms.Api.Utilities.Contracts.DTOs;
using Ms.Api.Utilities.DTO;
using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.DTO.Response;
using Ms.Api.Utilities.Enum;
using Ms.Api.Utilities.Exceptions;
using Ms.Api.Utilities.Extensions;
using Ms.Api.Utilities.Models;
using MsOrderApi.Contracts.DTOs;
using MsOrderApi.Contracts.DTOs.Request;
using MsOrderApi.Contracts.Repository;
using MsOrderApi.Contracts.Services;
//using MsOrderApi.Contracts.UseCases;
using MsOrderApi.Services.PagarME;
using System.Collections.Generic;

namespace MsOrderApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : Controller
    {
        readonly ILogger<OrderController> _logger;
        readonly IOrderRepository _OrderRepository;
        readonly INetShoesService _netShoesService;
        readonly IViaVarejoService _viaVarejoService;
        readonly IMagaluService _magaluService;
        readonly IGifttyService _gifttyService;
        readonly IControlPointInternalService _controlPointInternalService;
        readonly IControlPointExternalService _controlPointExternalService;
        //readonly IIntegrationTransfeeraService _integrationTransfeeraService;
        //readonly IBilletCreationUseCase _billetCreationUseCases;
        readonly IIntegrationTransfeeraService _integrationTransfeeraService;
        readonly IIntegrationRechargeService _integrationRechargeService;
        readonly IConfiguration _configuration;

        public OrderController(ILogger<OrderController> logger, IOrderRepository OrderRepository, INetShoesService netShoesService,
                                                                                                  IViaVarejoService viaVarejoService,
                                                                                                  //IMagaluService magaluService,
                                                                                                  IGifttyService gifttyService,
                                                                                                  IControlPointInternalService controlPointInternalService,
                                                                                                  IControlPointExternalService controlPointExternalService,
                                                                                                  IIntegrationTransfeeraService integrationTransfeeraService,
                                                                                                  IIntegrationRechargeService integrationRechargeService,
                                                                                                  IConfiguration configuration)
        {
            _logger = logger;
            _OrderRepository = OrderRepository ?? throw new ArgumentNullException(nameof(OrderRepository));
            _netShoesService = netShoesService ?? throw new ArgumentNullException(nameof(netShoesService));
            _viaVarejoService = viaVarejoService ?? throw new ArgumentNullException(nameof(_viaVarejoService));
            //_magaluService = magaluService ?? throw new ArgumentNullException(nameof(_magaluService));
            _gifttyService = gifttyService ?? throw new ArgumentNullException(nameof(_gifttyService));
            _controlPointInternalService = controlPointInternalService ?? throw new ArgumentNullException(nameof(_controlPointInternalService));
            _controlPointExternalService = controlPointExternalService ?? throw new ArgumentNullException(nameof(_controlPointExternalService));
            _integrationTransfeeraService = integrationTransfeeraService ?? throw new ArgumentNullException(nameof(integrationTransfeeraService));
            _integrationRechargeService = integrationRechargeService ?? throw new ArgumentNullException(nameof(integrationRechargeService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            //_billetCreationUseCases = billetCreationUseCases ?? throw new ArgumentNullException(nameof(billetCreationUseCases));
        }

        //[HttpPost]
        //[Route("CreateItemOrder")]
        ////[Authorize("AdminPolicy")]
        //public async Task<IActionResult> CreateItemOrder([FromBody] OrderRequest request)
        //{
        //    try
        //    {
        //        var response = new DefaultResponse<List<StoreOrder>> (await _OrderRepository.InsertItemOrder(request));

        //        return Ok(response);
        //    }
        //    catch (CodeException ex)
        //    {
        //        return BadRequest(new DefaultResponse<List<StoreOrder>>(ex) { MessageCode = ex.ErrorCode });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new DefaultResponse<List<StoreOrder>>(ex) { MessageCode = "FFFF" });
        //    }
        //}

        //[HttpGet]
        //[Route("GetListOrder")]
        ////[Authorize("AdminPolicy")]
        //public async Task<IActionResult> GetListOrder([FromBody] OrderRequest request)
        //{
        //    try
        //    {
        //        var response = new DefaultResponse<List<StoreOrder>>(await _OrderRepository.GetOrder(request));

        //        return Ok(response);
        //    }
        //    catch (CodeException ex)
        //    {
        //        return BadRequest(new DefaultResponse<List<StoreOrder>>(ex) { MessageCode = ex.ErrorCode });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new DefaultResponse<List<StoreOrder>>(ex) { MessageCode = "FFFF" });
        //    }
        //}

        //[HttpDelete]
        //[Route("DeleteItemOrder")]
        ////[Authorize("AdminPolicy")]
        //public async Task<IActionResult> DeleteItemOrder([FromBody] OrderRequest request)
        //{
        //    try
        //    {
        //        var response = new DefaultResponse<List<StoreOrder>>(await _OrderRepository.DeleteItemOrder(request));

        //        return Ok(response);
        //    }
        //    catch (CodeException ex)
        //    {
        //        return BadRequest(new DefaultResponse<List<StoreOrder>>(ex) { MessageCode = ex.ErrorCode });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new DefaultResponse<List<StoreOrder>>(ex) { MessageCode = "FFFF" });
        //    }
        //}

        //[HttpDelete]
        //[Route("DeleteOrder")]
        ////[Authorize("AdminPolicy")]
        //public async Task<IActionResult> DeleteOrder([FromBody] OrderRequest request)
        //{
        //    try
        //    {
        //        await _OrderRepository.DeleteOrder(request);

        //        return Ok();
        //    }
        //    catch (CodeException ex)
        //    {
        //        return BadRequest(new DefaultResponse<List<StoreOrder>>(ex) { MessageCode = ex.ErrorCode });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new DefaultResponse<List<StoreOrder>>(ex) { MessageCode = "FFFF" });
        //    }
        //}

        //[HttpGet]
        //[Route("GetShippingValue")]
        ////[Authorize("AdminPolicy")]
        //public async Task<IActionResult> GetShippingValue([FromBody] CalcShippingRequest request)
        //{
        //    try
        //    {
        //        List<StoreBasket> storeBasket = new();
        //        DefaultResponse<CalcShippingResponse> response = new();

        //        storeBasket = await _OrderRepository.GetBasket(new BasketRequest { IdUser = request.IdUser });
        //        var products = storeBasket.Where(x => x.StoreName == request.Store).Select(y => y.Products).FirstOrDefault();

        //        foreach (var product in products)
        //        {
        //            ProductBasketRequest productShipping = new();
        //            productShipping.Codigo = product.IdProduct;
        //            productShipping.Quantidade = product.Quantity;
        //            productShipping.ValorUnitario = product.ValueInReals;
        //            request.Produtos = new();

        //            request.Produtos.Add(productShipping);
        //        }

        //        switch (request.Store)
        //        {
        //            case "NetShoes":
        //                response = await _netShoesService.GetShippingValue(request);
        //                break;
        //            case "Extra":
        //            case "Ponto Frio":
        //            case "PontoFrio":
        //            case "Ponto":
        //            case "CasasBahia":
        //            case "Casas Bahia":
        //                response = await _viaVarejoService.GetShippingValue(request);
        //                break;
        //            default:
        //                break;
        //        }

        //        //var response = new DefaultResponse<List<ProductBasket>>(await _basketRepository.GetBasket(request));

        //        return Ok(response);
        //    }
        //    catch (CodeException ex)
        //    {
        //        return BadRequest(new DefaultResponse<List<ProductBasket>>(ex) { MessageCode = ex.ErrorCode });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new DefaultResponse<List<ProductBasket>>(ex) { MessageCode = "FFFF" });
        //    }
        //}


        //[HttpPost]
        //[Route("CreateOrder")]
        ////[Authorize("AdminPolicy")]
        //public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        //{
        //    try
        //    {
        //        DefaultResponse<OrderResponse> response = new();

        //        //switch (request.Store)
        //        //{
        //        //    case Enums.Store.NetShoes:
        //        //        response = await _netShoesService.SetCreateOrder(request);
        //        //        break;

        //        //    case Enums.Store.Extra:
        //        //    case Enums.Store.Ponto:
        //        //    case Enums.Store.CasasBahia:
        //        //        response = await _viaVarejoService.SetCreateOrder(request);
        //        //        break;
        //        //    default:
        //        //        break;
        //        //}

        //        if (request != null && request.Shops != null)
        //        {
        //            foreach (Request_OrderStore oRequest_OrderStore in request.Shops)
        //            {
        //                switch (oRequest_OrderStore.Store)
        //                {
        //                    case Enums.Store.NetShoes:
        //                        response = await _netShoesService.SetCreateOrder(request, oRequest_OrderStore);
        //                        break;

        //                    case Enums.Store.Extra:
        //                    case Enums.Store.Ponto:
        //                    case Enums.Store.CasasBahia:
        //                        response = await _viaVarejoService.SetCreateOrder(request, oRequest_OrderStore);
        //                        break;
        //                    default:
        //                        break;
        //                }
        //            }
        //        }

        //        #region Salva Pedido na Base Reconhece
        //        // 1 Pedido x 1 Parceiro;


        //        Order oOrder = new Order();
        //        //oOrder.CampaignId = request.Campaign;
        //        oOrder.OrderCode = Convert.ToString(response.Data.CodigoPedido);
        //        oOrder.Active = true;
        //        oOrder.ConversationFactor = request.FatorConversao;
        //        oOrder.TotalOrderAmountPoints = request.TotalOrderAmountPoints;

        //        oOrder.DeliveryAddress = new OrderDeliveryAddress();
        //        oOrder.DeliveryAddress.Active = true;
        //        oOrder.DeliveryAddress.City = request.DeliveryAddress.City;
        //        oOrder.DeliveryAddress.Complement = request.DeliveryAddress.Complement;
        //        oOrder.DeliveryAddress.District = request.DeliveryAddress.District;
        //        oOrder.DeliveryAddress.Number = request.DeliveryAddress.Number;
        //        oOrder.DeliveryAddress.PublicPlace = request.DeliveryAddress.PublicPlace;
        //        oOrder.DeliveryAddress.Reference = request.DeliveryAddress.Reference;
        //        oOrder.DeliveryAddress.State = request.DeliveryAddress.State;
        //        oOrder.DeliveryAddress.Telephone = request.DeliveryAddress.Telephone;
        //        //oOrder.DeliveryAddress.Telephone1 = request.DeliveryAddress.Telephone1;
        //        oOrder.DeliveryAddress.Telephone2 = request.DeliveryAddress.Telephone2;
        //        oOrder.DeliveryAddress.Telephone3 = request.DeliveryAddress.Telephone3;
        //        oOrder.DeliveryAddress.ZipCode = request.DeliveryAddress.ZipCode;

        //        oOrder.Recipient = new OrderRecipient();
        //        oOrder.Recipient.Email = request.Recipient.Email;
        //        oOrder.Recipient.CPFCNPJ = request.Recipient.CPFCNPJ;
        //        oOrder.Recipient.Name = request.Recipient.Name;
        //        oOrder.Recipient.StateRegistration = request.Recipient.StateRegistration;

        //        oOrder.TotalOrderAmount = request.TotalOrderAmount;
        //        oOrder.TotalOrderAmountCurrency = request.TotalOrderAmountCurrency;

        //        oOrder.Shops = (IEnumerable<OrderStore>)request.Shops;

        //        await _OrderRepository.Create(oOrder);

        //        #endregion Salva Pedido na Base Reconhece


        //        return Ok(response);
        //    }
        //    catch (CodeException ex)
        //    {
        //        return BadRequest(new DefaultResponse<List<StoreOrder>>(ex) { MessageCode = ex.ErrorCode });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new DefaultResponse<List<StoreOrder>>(ex) { MessageCode = "FFFF" });
        //    }
        //}

        [HttpGet]
        [Route("GetListOrder/{cpf}")]
        //[Authorize("AdminPolicy")]
        public async Task<IActionResult> GetListOrder([FromRoute] string cpf)
        {
            try
            {
                OrderFilter order = new OrderFilter() { CPFCNPJ = cpf };
                var response = await _OrderRepository.List(order);
                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new DefaultResponse<List<Order>>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<List<Order>>(ex) { MessageCode = "FFFF" });
            }
        }

        [HttpPost]
        [Route("CreateOrder")]
        //[Authorize("AdminPolicy")]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequest request)
        {
            ED_Retorno_API_Pagamento retornoApiPagamento = new ED_Retorno_API_Pagamento();
            PagarMeService oPagarMeService = new PagarMeService(_configuration);

            ED_ControleFluxo_Pedido oED_Fluxo = new ED_ControleFluxo_Pedido();
            oED_Fluxo.EnviadoPagarMe = true;

            Order oOrder = null;

            try
            {
                DefaultResponse<OrderResponse> response = new();

                #region Reserva Ponto do Usuario

                if (request.TotalOrderAmountPoints > 0)
                {
                    DefaultResponse<String> responseReserva = new DefaultResponse<String>();

                    //ATENCAO = Definir ID do usuario

                    if (request.ControlPointInternal)
                    {
                        #region Controle de Pontuação Interna

                        CreateReserveMovimentRequest oCreateReserveMovimentRequest = new CreateReserveMovimentRequest();
                        oCreateReserveMovimentRequest.Value = request.TotalOrderAmountPoints;
                        responseReserva = await _controlPointInternalService.Booking(oCreateReserveMovimentRequest);

                        if (responseReserva != null && !String.IsNullOrEmpty(responseReserva.Data))
                            oED_Fluxo.idReservaPonto = responseReserva.Data;

                        #endregion Controle de Pontuação Interna
                    }
                    else
                    {
                        #region Controle de Pontuação Externa

                        CampaignUserRequest oCampaignUserRequest = new CampaignUserRequest();
                        oCampaignUserRequest.points = request.TotalOrderAmountPoints;
                        responseReserva = await _controlPointExternalService.BookPoints(oCampaignUserRequest);

                        if (responseReserva != null && !String.IsNullOrEmpty(responseReserva.Data))
                            oED_Fluxo.idReservaPonto = responseReserva.Data;

                        #endregion Controle de Pontuação Externa
                    }

                    oED_Fluxo.PontoReservado = true;
                }

                #endregion Reserva Ponto do Usuario

                #region Envia ao Parceiro

                if (request.Type == Enums.TypeRequest.Produto)
                {
                    if (request != null && request.Shops != null)
                    {
                        foreach (Request_OrderStore oRequest_OrderStore in request.Shops)
                        {
                            switch (oRequest_OrderStore.Store)
                            {
                                case Enums.Store.NetShoes:
                                    response = await _netShoesService.SetCreateOrder(request, oRequest_OrderStore);
                                    break;

                                case Enums.Store.Extra:
                                case Enums.Store.Ponto:
                                case Enums.Store.CasasBahia:
                                    response = await _viaVarejoService.SetCreateOrder(request, oRequest_OrderStore);
                                    break;

                                case Enums.Store.Magalu:
                                    //response = await _magaluService.SetCreateOrder(request, oRequest_OrderStore);
                                    break;
                                case Enums.Store.Giftty:
                                    response = await _gifttyService.SetCreateOrder(request, oRequest_OrderStore);
                                    break;

                                default:
                                    break;
                            }
                        }
                    }

                    oED_Fluxo.EnviadoLoja = true;
                }

                #endregion Envia ao Parceiro

                #region Salva Pedido na Base Reconhece
                // 1 Pedido x 1 Parceiro;

                String codigoPedidoReconhece = String.Empty;

                oOrder = new Order();
                //oOrder.CampaignId = request.Campaign;
                oOrder.OrderCode = Convert.ToString(response.Data.CodigoPedido);
                oOrder.Active = true;
                oOrder.ConversationFactor = request.FatorConversao;
                oOrder.TotalOrderAmountPoints = request.TotalOrderAmountPoints;
                oOrder.Status = OrderStatus.Cadastrado;

                oOrder.DeliveryAddress = new OrderDeliveryAddress();
                oOrder.DeliveryAddress.Active = true;
                oOrder.DeliveryAddress.City = request.DeliveryAddress.City;
                oOrder.DeliveryAddress.Complement = request.DeliveryAddress.Complement;
                oOrder.DeliveryAddress.District = request.DeliveryAddress.District;
                oOrder.DeliveryAddress.Number = request.DeliveryAddress.Number;
                oOrder.DeliveryAddress.PublicPlace = request.DeliveryAddress.PublicPlace;
                oOrder.DeliveryAddress.Reference = request.DeliveryAddress.Reference;
                oOrder.DeliveryAddress.State = request.DeliveryAddress.State;
                oOrder.DeliveryAddress.Telephone = request.DeliveryAddress.Telephone;
                //oOrder.DeliveryAddress.Telephone1 = request.DeliveryAddress.Telephone1;
                oOrder.DeliveryAddress.Telephone2 = request.DeliveryAddress.Telephone2;
                oOrder.DeliveryAddress.Telephone3 = request.DeliveryAddress.Telephone3;
                oOrder.DeliveryAddress.ZipCode = request.DeliveryAddress.ZipCode;

                oOrder.Recipient = new OrderRecipient();
                oOrder.Recipient.Email = request.Recipient.Email;
                oOrder.Recipient.CPFCNPJ = request.Recipient.CPFCNPJ;
                oOrder.Recipient.Name = request.Recipient.Name;
                oOrder.Recipient.StateRegistration = request.Recipient.StateRegistration;

                oOrder.TotalOrderAmount = request.TotalOrderAmount;
                oOrder.TotalOrderAmountCurrency = request.TotalOrderAmountCurrency;

                
                //oOrder.Shops = (IEnumerable<OrderStore>)request.Shops;

                //if (request.Type == Enums.TypeRequest.Boleto)
                //{
                //    if (request.Billet != null)
                //    {
                //        oOrder.Billet = new Billet();
                //        oOrder.Billet.ParticipantId = request.Billet.ParticipantId;
                //        oOrder.Billet.ParticipantName = request.Billet.ParticipantName;
                //        oOrder.Billet.Email = request.Billet.Email;
                //        oOrder.Billet.CampaignId = request.Billet.CampaignId;
                //        oOrder.Billet.BilletValue = request.Billet.BilletValue;
                //        oOrder.Billet.BilletFeeValue = request.Billet.BilletFeeValue;
                //        oOrder.Billet.BilletPointsValue = request.Billet.BilletPointsValue;
                //        oOrder.Billet.CreationDate = DateTime.UtcNow.ToUnixTimestamp();
                //        //oOrder.Billet.StatusTransfeera = "CRIADO";
                //        oOrder.Billet.TransfeeraTransactionId = null;
                //        oOrder.Billet.ErrorMessage = null;
                //        oOrder.Billet.BilletDetails = new BilletDetails()
                //        {
                //            BankCode = request.Billet.BilletDetails.BankCode,
                //            BankName = request.Billet.BilletDetails.BankName,
                //            Barcode = request.Billet.BilletDetails.Barcode,
                //            DigitableLine = request.Billet.BilletDetails.DigitableLine,
                //            DueDate = request.Billet.BilletDetails.DueDate,
                //            Value = request.Billet.BilletDetails.Value,
                //            Type = request.Billet.BilletDetails.Type
                //        };
                //        oOrder.Billet.BilletPaymentInfos = new BilletPayment()
                //        {
                //            RecipientDocument = request.Billet.BilletPaymentInfos.RecipientDocument,
                //            RecipientName = request.Billet.BilletPaymentInfos.RecipientName,
                //            PayerDocument = request.Billet.BilletPaymentInfos.PayerDocument,
                //            PayerName = request.Billet.BilletPaymentInfos.PayerName,
                //            DueDate = request.Billet.BilletPaymentInfos.DueDate,
                //            LimitDate = request.Billet.BilletPaymentInfos.LimitDate,
                //            MinValue = request.Billet.BilletPaymentInfos.MinValue,
                //            MaxValue = request.Billet.BilletPaymentInfos.MaxValue,
                //            FineValue = request.Billet.BilletPaymentInfos.FineValue,
                //            InterestValue = request.Billet.BilletPaymentInfos.InterestValue,
                //            OriginalValue = request.Billet.BilletPaymentInfos.OriginalValue,
                //            TotalUpdatedValue = request.Billet.BilletPaymentInfos.TotalUpdatedValue,
                //            TotalDiscountValue = request.Billet.BilletPaymentInfos.TotalDiscountValue,
                //            TotalAdditionalValue = request.Billet.BilletPaymentInfos.TotalAdditionalValue
                //        };
                //    }

                //}

                await _OrderRepository.Create(oOrder);

                oED_Fluxo.SalvoBanco = true;

                #endregion Salva Pedido na Base Reconhece

                #region PagarME

                if (request.Type == Enums.TypeRequest.Produto)
                {
                    if (request.PaymentData != null && request.PaymentData.ValorComplemento > 0)
                    {
                        oED_Fluxo.TransacaoFinanceira_Utilizada = true;
                        retornoApiPagamento = oPagarMeService.Processa(PagarMeService.TipoOperacao.Pedido, request, "", oED_Fluxo.PedidoReconheceID);
                        if (retornoApiPagamento.SUCESSO)
                        {
                            //transacaoFinanceira_Utilizada = true;
                        }
                        else
                        {
                            oED_Fluxo.EnviadoPagarMe = false;
                            throw new Exception(retornoApiPagamento.MENSAGEM_ERRO);
                        }
                    }
                }

                #endregion PagarME

                #region Tranfeera

                if (request.Type == Enums.TypeRequest.Boleto)
                {
                    if (request.Billet != null)
                    {
                        request.Billet.CodeRequestReconhece = codigoPedidoReconhece;
                        var responseBillet = await _integrationTransfeeraService.ConfirmPaymentBillet(request.Billet);

                        if (responseBillet != null)
                            oED_Fluxo.TransacaoBoleto_Utilizada = true;
                    }
                }

                #endregion Tranfeera

                #region Recarga

                if (request.Type == Enums.TypeRequest.Recarga)
                {
                    if (request.Recharge != null)
                    {
                        request.Recharge.CodeRequestReconhece = codigoPedidoReconhece;
                        var responseRecharge = await _integrationRechargeService.ConfirmPaymentRecharge(request.Recharge);

                        if (responseRecharge != null)
                            oED_Fluxo.TransacaoRecarga_Utilizada = true;
                    }
                }

                #endregion Recarga

                #region Efetivar Ponto do Usuario

                if (request.TotalOrderAmountPoints > 0)
                {
                    if (!String.IsNullOrEmpty(oED_Fluxo.idReservaPonto))
                    {
                        DefaultResponse<String> responseDebito = new DefaultResponse<String>();

                        //ATENCAO = Definir ID do usuario

                        if (request.ControlPointInternal)
                        {
                            #region Controle de Pontuação Interna

                            EffectDebitRequest oEffectDebitRequest = new EffectDebitRequest();
                            oEffectDebitRequest.ReleaseCode = oED_Fluxo.idReservaPonto;
                            responseDebito = await _controlPointInternalService.EffetiveDebit(oEffectDebitRequest);

                            if (responseDebito != null && !String.IsNullOrEmpty(responseDebito.Data))
                                oED_Fluxo.idDebitoPonto = responseDebito.Data;

                            #endregion Controle de Pontuação Interna
                        }
                        else
                        {
                            #region Controle de Pontuação Externa

                            CampaignUserRequest oCampaignUserRequest = new CampaignUserRequest();
                            oCampaignUserRequest.releaseCode = oED_Fluxo.idReservaPonto;
                            responseDebito = await _controlPointExternalService.BookPoints(oCampaignUserRequest);

                            if (responseDebito != null && !String.IsNullOrEmpty(responseDebito.Data))
                                oED_Fluxo.idDebitoPonto = responseDebito.Data;

                            #endregion Controle de Pontuação Externa
                        }

                        oED_Fluxo.PontoEfetivado = true;
                    }
                }

                #endregion Efetivar Ponto do Usuario

                #region Envia Confirmação Pedido ao Parceiro (Loja)

                //REVER

                if (oED_Fluxo.EnviadoLoja && oED_Fluxo.SalvoBanco && oED_Fluxo.EnviadoPagarMe)
                {
                    if (request != null && request.Shops != null)
                    {
                        foreach (Request_OrderStore oRequest_OrderStore in request.Shops)
                        {
                            switch (oRequest_OrderStore.Store)
                            {
                                //case Enums.Store.NetShoes:
                                //    response = await _netShoesService.SetCreateOrder(request, oRequest_OrderStore);
                                //    break;

                                case Enums.Store.Extra:
                                case Enums.Store.Ponto:
                                case Enums.Store.CasasBahia:
                                    //response = await _viaVarejoService.SetCreateOrder(request, oRequest_OrderStore);
                                    break;

                                //case Enums.Store.Magalu:
                                //    response = await _magaluService.SetCreateOrder(request, oRequest_OrderStore);
                                //    break;
                                //case Enums.Store.Giftty:
                                    //response = await _gifttyService.SetCreateOrder(request, oRequest_OrderStore);
                                    //break;

                                default:
                                    break;
                            }
                        }
                    }
                }

                #endregion Envia Confirmação Pedido ao Parceiro (Loja)

                #region Atualiza Situação do Pedido Salvo

                oOrder.Status = OrderStatus.Finalizado;
                await _OrderRepository.Update(oOrder);

                #endregion Atualiza Situação do Pedido Salvo

                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new DefaultResponse<List<OrderResponse>>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                #region ERRO

                var _error = ex.Message.ToLower().Contains("inner exception") || ex.Message.ToLower().Contains("one or more errors occurred") ?
                              (ex.InnerException.Message.ToLower().Contains("inner exception") || ex.InnerException.Message.ToLower().Contains("an error occurred while sending the request") ?
                              ex.InnerException.InnerException.Message : ex.InnerException.Message)
                              : ex.Message;

                #region Transação Financeira

                if (request.Type == Enums.TypeRequest.Produto)
                {
                    if (oED_Fluxo.TransacaoFinanceira_Utilizada && oED_Fluxo.EnviadoPagarMe == false)
                    {
                        bool TransacaoCancelada = false;
                        string _returnroReembolo = "Falha ao cancelar transação.";
                        try
                        {
                            #region PagarMe

                            if (retornoApiPagamento.Tem_ID_TRANSACAO)
                            {
                                ED_Retorno_API_Pagamento retornoApiPagamento_Estorno = oPagarMeService.Processa(PagarMeService.TipoOperacao.Estorno,
                                                                                    request,
                                                                                    retornoApiPagamento.ID_TRANSACAO, "",
                                                                                    retornoApiPagamento.VALOR_TRANSACAO);
                                if (retornoApiPagamento_Estorno.SUCESSO)
                                {
                                    TransacaoCancelada = true;
                                    _returnroReembolo = retornoApiPagamento_Estorno.MENSAGEM_ERRO;
                                }
                                else
                                {
                                    _returnroReembolo = retornoApiPagamento_Estorno.MENSAGEM_ERRO;
                                }

                            }

                            #endregion PagarMe
                        }
                        catch (Exception ex2)
                        {
                            _returnroReembolo = ex2.Message.ToLower().Contains("inner exception") || ex2.Message.ToLower().Contains("one or more errors occurred") ?
                                                    (ex2.InnerException.Message.ToLower().Contains("inner exception") || ex2.InnerException.Message.ToLower().Contains("an error occurred while sending the request") ?
                                                    ex2.InnerException.InnerException.Message : ex2.InnerException.Message)
                                                    : ex2.Message;
                        }

                        //_Assunto = "Estorno de credito - PGTO Realizado com erro no pedido. No: " + request.PedidoParceiro.ToString();
                        //try
                        //{
                        //    _Body = new Generico().MensagemEstorno_Erro("ViaVarejo", ex, TransacaoCancelada, _returnroReembolo, request, request, retornoApiPagamento);
                        //}
                        //catch
                        //{
                        //    _Body = new Generico().MensagemEstorno_Erro_Erro("ViaVarejo", ex, request, request, retornoApiPagamento);
                        //}

                        //var _CC = new List<string>();

                        //new Email("contato@reconhece.vc").Enviar(InfoDigi.Email.ListaResponsavel_Financeiro, _CC, _Assunto, _Body);
                    }
                }

                #endregion Transação Financeira

                #region Estornar Ponto do Usuario

                if (request.TotalOrderAmountPoints > 0)
                {
                    if (oED_Fluxo.PontoEfetivado)
                    {
                        DefaultResponse<String> responseEstorno = new DefaultResponse<String>();

                        //ATENCAO = Definir ID do usuario

                        if (request.ControlPointInternal)
                        {
                            #region Controle de Pontuação Interna

                            ReversePointsRequest oReversePointsRequest = new ReversePointsRequest();
                            oReversePointsRequest.DebitMovimentCode = oED_Fluxo.idDebitoPonto;
                            responseEstorno = await _controlPointInternalService.ReversePoints(oReversePointsRequest);

                            #endregion Controle de Pontuação Interna
                        }
                        else
                        {
                            #region Controle de Pontuação Externa

                            CampaignUserRequest oCampaignUserRequest = new CampaignUserRequest();
                            oCampaignUserRequest.requestNumber = oED_Fluxo.idDebitoPonto;
                            responseEstorno = await _controlPointExternalService.ReversePoints(oCampaignUserRequest);

                            #endregion Controle de Pontuação Externa
                        }
                    }
                    else if (oED_Fluxo.PontoReservado)
                    {
                        DefaultResponse<bool> responseLiberacao = new DefaultResponse<bool>();

                        //ATENCAO = Definir ID do usuario

                        if (request.ControlPointInternal)
                        {
                            #region Controle de Pontuação Interna

                            ReleasePointsRequest oReleasePointsRequest = new ReleasePointsRequest();
                            oReleasePointsRequest.ReleaseCode = oED_Fluxo.idReservaPonto;
                            responseLiberacao = await _controlPointInternalService.ReleasePoints(oReleasePointsRequest);

                            #endregion Controle de Pontuação Interna
                        }
                        else
                        {
                            #region Controle de Pontuação Externa

                            CampaignUserRequest oCampaignUserRequest = new CampaignUserRequest();
                            oCampaignUserRequest.releaseCode = oED_Fluxo.idReservaPonto;
                            responseLiberacao = await _controlPointExternalService.ReleasePoints(oCampaignUserRequest);

                            #endregion Controle de Pontuação Externa
                        }
                    }

                }

                #endregion Estornar Ponto do Usuario

                #region Atualiza Situação do Pedido Salvo

                if (oOrder != null)
                {
                    oOrder.Status = OrderStatus.Erro;
                    await _OrderRepository.Update(oOrder);
                }

                #endregion Atualiza Situação do Pedido Salvo

                #endregion ERRO

                return StatusCode(500, new DefaultResponse<List<OrderResponse>>(ex) { MessageCode = "FFFF" });
            }
        }

        [HttpGet]
        [Route("GetShippingValue")]
        //[Authorize("AdminPolicy")]
        public async Task<IActionResult> GetShippingValue([FromBody] CalcShippingRequest request)
        {
            try
            {
                //List<StoreBasket> storeBasket = new();
                DefaultResponse<CalcShippingResponse> response = new();

                if (request != null && request.Shops != null)
                {
                    response = new DefaultResponse<CalcShippingResponse>();
                    //response.Data = new CalcShippingResponse();

                    CalcShippingResponse oCalcShippingResponse = new CalcShippingResponse();

                    foreach (CalcShippingRequest_Store oCalcShippingRequest_Store in request.Shops)
                    {
                        //storeBasket = await _OrderRepository.GetBasket(new BasketRequest { IdUser = request.IdUser });
                        //var products = storeBasket.Where(x => x.StoreName == request.Store).Select(y => y.Products).FirstOrDefault();

                        //foreach (var product in products)
                        //{
                        //    ProductBasketRequest productShipping = new();
                        //    productShipping.Codigo = product.IdProduct;
                        //    productShipping.Quantidade = product.Quantity;
                        //    productShipping.ValorUnitario = product.ValueInReals;
                        //    request.Produtos = new();

                        //    request.Produtos.Add(productShipping);
                        //}

                        //switch (request.Store)
                        //{
                        //    case "NetShoes":
                        //        response = await _netShoesService.GetShippingValue(request);
                        //        break;
                        //    case "Extra":
                        //    case "Ponto Frio":
                        //    case "PontoFrio":
                        //    case "Ponto":
                        //    case "CasasBahia":
                        //    case "Casas Bahia":
                        //        response = await _viaVarejoService.GetShippingValue(request);
                        //        break;
                        //    default:
                        //        break;
                        //}

                        //CalcShippingResponse_Store oCalcShippingResponse_Store = null;
                        DefaultResponse<CalcShippingResponse_Store> oCalcShippingResponse_Store = new();


                        switch (oCalcShippingRequest_Store.Store)
                        {
                            case Enums.Store.NetShoes:
                                oCalcShippingResponse_Store = await _netShoesService.GetShippingValue(request.CEP, oCalcShippingRequest_Store);
                                break;

                            case Enums.Store.Extra:
                            case Enums.Store.Ponto:
                            case Enums.Store.CasasBahia:
                                oCalcShippingResponse_Store = await _viaVarejoService.GetShippingValue(request.CEP, oCalcShippingRequest_Store);
                                break;

                            case Enums.Store.Magalu:
                                //oCalcShippingResponse_Store = await _magaluService.GetShippingValue(request.CEP, oCalcShippingRequest_Store); //OK
                                break;
                            case Enums.Store.Giftty:
                                oCalcShippingResponse_Store = await _gifttyService.GetShippingValue(request.CEP, oCalcShippingRequest_Store); //OK
                                break;

                            default:
                                break;
                        }

                        if (oCalcShippingResponse_Store.Data != null)
                        {
                            //Deve ser implementado aqui o calculo de conversao de pontos no retorno dos valores.
                            //FAZER DEPOIS

                            oCalcShippingResponse.Shops.Add(oCalcShippingResponse_Store.Data);
                        }
                    }

                    response.Data = oCalcShippingResponse;
                }

                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new DefaultResponse<CalcShippingResponse>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<CalcShippingResponse>(ex) { MessageCode = "FFFF" });
            }
        }



        #region Transfeera

        //[HttpPost]
        //[Route("GetBilletById")]
        ////[Authorize("ProfilePolicy")]
        //public async Task<IActionResult> GetBilletById([FromBody] BilletFilterRequest model)
        //{
        //    try
        //    {
        //        var data = await _integrationTransfeeraService.GetBillet(Convert.ToInt64(model.BilletId));
        //        var response = new DefaultResponse<TransfeeraGetBilletResponse?>(data);

        //        return Ok(response);
        //    }
        //    catch (CodeException ex)
        //    {
        //        return BadRequest(new DefaultResponse<TransfeeraGetBilletResponse?>(ex) { MessageCode = ex.ErrorCode });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new DefaultResponse<TransfeeraGetBilletResponse?>(ex) { MessageCode = "FFFF" });
        //    }
        //}

        //[HttpPost]
        //[Route("GetBillets")]
        ////[Authorize("ProfilePolicy")]
        //public async Task<IActionResult> GetBillets([FromBody] TransfeeraGetBilletsRequest model)
        //{
        //    try
        //    {
        //        var data = await _integrationTransfeeraService.GetBillets(model);
        //        var response = new DefaultResponse<List<TransfeeraGetBilletResponse>?>(data);

        //        return Ok(response);
        //    }
        //    catch (CodeException ex)
        //    {
        //        return BadRequest(new DefaultResponse<List<TransfeeraGetBilletResponse>?>(ex) { MessageCode = ex.ErrorCode });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new DefaultResponse<List<TransfeeraGetBilletResponse>?>(ex) { MessageCode = "FFFF" });
        //    }
        //}

        //[HttpGet]
        //[Route("CheckBalance")]
        ////[Authorize("ProfilePolicy")]
        //public async Task<IActionResult> CheckBalance()
        //{
        //    try
        //    {
        //        var data = await _integrationTransfeeraService.CheckBalance();
        //        var response = new DefaultResponse<TransfeeraCheckBalanceResponse?>(data);

        //        return Ok(response);
        //    }
        //    catch (CodeException ex)
        //    {
        //        return BadRequest(new DefaultResponse<TransfeeraCheckBalanceResponse?>(ex) { MessageCode = ex.ErrorCode });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new DefaultResponse<TransfeeraCheckBalanceResponse?>(ex) { MessageCode = "FFFF" });
        //    }
        //}

        //[HttpPost]
        //[Route("GetValidationBilletOnCIP")]
        ////[Authorize("ProfilePolicy")]
        //public async Task<IActionResult> GetValidationBilletOnCIP([FromBody] BarcodeValidationRequest model)
        //{
        //    try
        //    {
        //        var data = await _integrationTransfeeraService.ValidateBilletOnCIP(model.Barcode);
        //        var response = new DefaultResponse<ValidateCIPResponse?>(data);

        //        return Ok(response);
        //    }
        //    catch (CodeException ex)
        //    {
        //        return BadRequest(new DefaultResponse<ValidateCIPResponse?>(ex) { MessageCode = ex.ErrorCode });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new DefaultResponse<ValidateCIPResponse?>(ex) { MessageCode = "FFFF" });
        //    }
        //}

        //[HttpPost]
        //[Route("ConfirmPaymentBillet")]
        //[Authorize("ProfilePolicy")]
        //public async Task<IActionResult> ConfirmPaymentBillet([FromBody] BilletRequest model)
        //{
        //    try
        //    {
        //        _reserveCode = await _campaignConnectorUseCases.BookPoints(new CampaignUserRequest()
        //        {
        //            campaign = model.campaign,
        //            environment = model.environment,
        //            token = model.token,
        //            points = model.BilletPointsValue
        //        });

        //        var response = await _billetCreationUseCases.Execute(model);

        //        if (!response.Success)
        //        {
        //            _campaignConnectorUseCases.ReleasePoints(new CampaignUserRequest()
        //            {
        //                campaign = model.campaign,
        //                environment = model.environment,
        //                token = model.token,
        //                releaseCode = _reserveCode
        //            });

        //            return Ok(response);
        //        }

        //        _campaignConnectorUseCases.EffectDebitPoints(new CampaignUserRequest()
        //        {
        //            campaign = model.campaign,
        //            environment = model.environment,
        //            token = model.token,
        //            points = model.BilletPointsValue,
        //            releaseCode = _reserveCode,
        //            orderNumber = $"P_{response.Data.Id}"
        //        });

        //        return Ok(response);
        //    }
        //    catch (CodeException ex)
        //    {
        //        if (!string.IsNullOrEmpty(_reserveCode))
        //        {
        //            _campaignConnectorUseCases.ReleasePoints(new CampaignUserRequest()
        //            {
        //                campaign = model.campaign,
        //                environment = model.environment,
        //                token = model.token,
        //                releaseCode = _reserveCode
        //            });
        //        }

        //        return BadRequest(new DefaultResponse<Billet>(ex) { MessageCode = ex.ErrorCode });
        //    }
        //    catch (Exception ex)
        //    {
        //        if (!string.IsNullOrEmpty(_reserveCode))
        //        {
        //            _campaignConnectorUseCases.ReleasePoints(new CampaignUserRequest()
        //            {
        //                campaign = model.campaign,
        //                environment = model.environment,
        //                token = model.token,
        //                releaseCode = _reserveCode
        //            });
        //        }

        //        return StatusCode(500, new DefaultResponse<Billet>(ex) { MessageCode = "FFFF" });
        //    }
        //}

        #endregion Transfeera

    }
}
