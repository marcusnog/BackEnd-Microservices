using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.Enum;
using Ms.Api.Utilities.Models;
using MsPointsPurchaseApi.Services.PagarME;
using MsPointsPurchaseApi.Contracts.DTOs;
using MsPointsPurchaseApi.Contracts.DTOs.Request;
using MsPointsPurchaseApi.Contracts.Repositories;
using System.Diagnostics.Contracts;
using System.Text;
using Ms.Api.Utilities.Contracts.DTOs;

namespace MsPurchasePointsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsPurchaseController : Controller
    {
        #region Fields

        readonly IConfiguration _configuration;
        readonly IPointsPurchaseRepository _repository;
        readonly ILogger<PointsPurchaseController> _logger;
        readonly string _MS_POINTS_URL;

        #endregion

        #region Constructor
        public PointsPurchaseController(IPointsPurchaseRepository repository, IConfiguration configuration, ILogger<PointsPurchaseController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _MS_POINTS_URL = configuration.GetValue<string>("MS_POINTS_URL");
        }

        #endregion

        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            try
            {
                var points = new DefaultResponse<PointsPurchase>(await _repository.Get(id));

                if (points.Data == null)
                    throw new ArgumentException("Points not found");

                return Ok(points);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<PointsPurchase>(ex));
            }
        }

        [HttpGet]
        [Route("GetList")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var points = new DefaultResponse<IEnumerable<PointsPurchase>>(await _repository.List());

                if (points.Data == null)
                    throw new ArgumentException("Points not found");

                return Ok(points);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<Points>(ex));
            }
        }

        [HttpPost]
        [Route("Create")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> Create([FromBody] CreatePointsPurchaseRequest request)
        {

            ED_Retorno_API_Pagamento retornoApiPagamento = new ED_Retorno_API_Pagamento();
            PagarMeService oPagarMeService = new PagarMeService(_configuration);

            ED_ControleFluxo_Pedido oED_Fluxo = new ED_ControleFluxo_Pedido();
            oED_Fluxo.EnviadoPagarMe = true;

            Order oOrder = null;


            try
            {
                var userId = (string?)Request.HttpContext.Items["UserId"];
                var accountId = (string?)Request.HttpContext.Items["Account"];
                DefaultResponse<bool> r;

                if (request.UserId == null)
                    throw new ArgumentException("User not found");

                if (request.PointsId == null)
                    throw new ArgumentException("Points not found");

                String codigoPedidoReconhece = String.Empty;

                oOrder = new Order();

                //oOrder.OrderCode = Convert.ToString(response.Data.CodigoPedido);
                //oOrder.Active = true;
                //oOrder.ConversationFactor = request.FatorConversao;
                //oOrder.TotalOrderAmountPoints = request.TotalOrderAmountPoints;
                //oOrder.Status = OrderStatus.Cadastrado;

                //oOrder.Recipient = new OrderRecipient();
                //oOrder.Recipient.Email = request.Recipient.Email;
                //oOrder.Recipient.CPFCNPJ = request.Recipient.CPFCNPJ;
                //oOrder.Recipient.Name = request.Recipient.Name;
                //oOrder.Recipient.StateRegistration = request.Recipient.StateRegistration;

                //oOrder.TotalOrderAmount = request.TotalOrderAmount;
                //oOrder.TotalOrderAmountCurrency = request.TotalOrderAmountCurrency;




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

                var newPurchasePoints = new PointsPurchase()
                {
                    UserId = request.UserId,
                    PointsId = request.PointsId,
                    AccountId = request.AccountId,
                    PointsValue = request.PointsValue,
                    Active = true,
                    CreatedAt = DateTime.Now,
                    CreationUserId = userId,
                    Enabled = false,
                };

                await _repository.Create(newPurchasePoints);

                r = new(true);
                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<PointsPurchase>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<PointsPurchase>>(ex));
            }
        }

        [HttpPost]
        [Route("UpdateAvailability")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> UpdateAvailability([FromBody] UpdateAvailabilityRequest request)
        {
            try
            {
                var userId = (string?)Request.HttpContext.Items["UserId"];
                DefaultResponse<PointsPurchase> r;

                if (userId == null)
                    throw new ArgumentException("User not found");

                if (request.userPurchaserPointsId == null)
                    throw new ArgumentException("User Purchaser not found");

                var pointsPurchaser = await _repository.GetByUser(request.userPurchaserPointsId);

                var update = new PointsPurchase()
                {
                    Id = pointsPurchaser.Id,
                    UserId = pointsPurchaser.UserId,
                    PointsId = pointsPurchaser.PointsId,
                    AccountId = pointsPurchaser.AccountId,
                    PointsValue = pointsPurchaser.PointsValue,
                    Active = pointsPurchaser.Active,
                    CreatedAt = pointsPurchaser.CreatedAt,
                    CreationUserId = pointsPurchaser.UserId,
                    Enabled = true,
                    UpdatedAt = DateTime.Now,
                    UpdatedUserId = userId
                };

                await _repository.Update(update);

                if (!string.IsNullOrEmpty(update.Id))
                {
                    var createMoviment = new CreditPointsRequest()
                    {
                        AccountId = update.AccountId,
                        Value = update.PointsValue,
                        Description = "Compra de Pontos - Admin"
                    };

                    var response = await CreateMoviment(createMoviment);

                    if (!string.IsNullOrEmpty(response))
                    {
                        r = new(true);
                    }
                    else
                    {
                        r = new("Doesn't possible create a new moviment");
                    }
                }
                else
                    r = new("Doensn't possible create the points purchase");


                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<PointsPurchase>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<PointsPurchase>>(ex));
            }
        }

        internal async Task<string?> CreateMoviment(CreditPointsRequest request)
        {
            var token = Request.Headers.Authorization.FirstOrDefault();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);
            var response = await client.PostAsync($"{_MS_POINTS_URL}/CreditPoints", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            var idMoviment = Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<string>>(await response.Content.ReadAsStringAsync());

            if (!response.IsSuccessStatusCode || !string.IsNullOrEmpty(idMoviment.Message))
                throw new Exception($"{idMoviment.Message}");

            return idMoviment.Data;
        }

        internal async Task<string?> EffectDistributePoints(EffectDebitAdminRequest request)
        {
            var token = Request.Headers.Authorization.FirstOrDefault();

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", token);
            var response = await client.PostAsync($"{_MS_POINTS_URL}/DistributePointsAdmin", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));

            var idMoviment = Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<string>>(await response.Content.ReadAsStringAsync());

            if (!response.IsSuccessStatusCode || !string.IsNullOrEmpty(idMoviment.Message))
                throw new Exception($"{idMoviment.Message}");

            return idMoviment.Data;
        }

    }
}
