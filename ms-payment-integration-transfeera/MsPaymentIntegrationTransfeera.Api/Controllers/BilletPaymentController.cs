using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ms.Api.Utilities.Exceptions;
using Ms.Api.Utilities.Models;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Request;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Response.Billets;
using MsPaymentIntegrationTransfeera.Api.Contracts.Services;
using MsPaymentIntegrationTransfeera.Api.Contracts.UseCases;
using MsPaymentIntegrationTransfeera.Contracts.DTO.Request;
using MsPaymentIntegrationTransfeera.Contracts.UseCases;

namespace MsPaymentIntegrationTransfeera.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BilletPaymentController : Controller
    {
        #region Fields

        readonly IConfiguration _configuration;
        readonly IIntegrationTransfeeraService _integrationTransfeeraService;
        readonly ILogger<BilletPaymentController> _logger;
        readonly IBilletCreationUseCase _billetCreationUseCases;
        readonly ICampaignConnectorUseCases _campaignConnectorUseCases;
        public string? _reserveCode;

        #endregion

        #region Constructor
        public BilletPaymentController(IIntegrationTransfeeraService integrationTransfeeraService, IConfiguration configuration, ILogger<BilletPaymentController> logger,
            IBilletCreationUseCase billetCreationUseCases, ICampaignConnectorUseCases campaignConnectorUseCases)
        {
            _integrationTransfeeraService = integrationTransfeeraService ?? throw new ArgumentNullException(nameof(integrationTransfeeraService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _billetCreationUseCases = billetCreationUseCases ?? throw new ArgumentNullException(nameof(billetCreationUseCases));
            _campaignConnectorUseCases = campaignConnectorUseCases ?? throw new ArgumentNullException(nameof(campaignConnectorUseCases));
        }
        #endregion

        [HttpPost]
        [Route("GetBilletById")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> GetBilletById([FromBody] BilletFilterRequest model)
        {
            try
            {
                var data = await _integrationTransfeeraService.GetBillet(Convert.ToInt64(model.BilletId));
                var response = new DefaultResponse<TransfeeraGetBilletResponse?>(data);

                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new DefaultResponse<TransfeeraGetBilletResponse?>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<TransfeeraGetBilletResponse?>(ex) { MessageCode = "FFFF" });
            }
        }

        [HttpPost]
        [Route("GetBillets")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> GetBillets([FromBody] TransfeeraGetBilletsRequest model)
        {
            try
            {
                var data = await _integrationTransfeeraService.GetBillets(model);
                var response = new DefaultResponse<List<TransfeeraGetBilletResponse>?>(data);

                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new DefaultResponse<List<TransfeeraGetBilletResponse>?>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<List<TransfeeraGetBilletResponse>?>(ex) { MessageCode = "FFFF" });
            }
        }

        [HttpGet]
        [Route("CheckBalance")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> CheckBalance()
        {
            try
            {
                var data = await _integrationTransfeeraService.CheckBalance();
                var response = new DefaultResponse<TransfeeraCheckBalanceResponse?>(data);

                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new DefaultResponse<TransfeeraCheckBalanceResponse?>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<TransfeeraCheckBalanceResponse?>(ex) { MessageCode = "FFFF" });
            }
        }

        [HttpPost]
        [Route("GetValidationBilletOnCIP")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> GetValidationBilletOnCIP([FromBody] BarcodeValidationRequest model)
        {
            try
            {
                var data = await _integrationTransfeeraService.ValidateBilletOnCIP(model.Barcode);
                var response = new DefaultResponse<ValidateCIPResponse?>(data);

                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new DefaultResponse<ValidateCIPResponse?>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<ValidateCIPResponse?>(ex) { MessageCode = "FFFF" });
            }
        }

        [HttpPost]
        [Route("ConfirmPaymentBillet")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> ConfirmPaymentBillet([FromBody] BilletRequest model)
        {
            try
            {
                _reserveCode = await _campaignConnectorUseCases.BookPoints(new CampaignUserRequest()
                {
                    campaign = model.campaign,
                    environment = model.environment,
                    token = model.token,
                    points = model.BilletPointsValue
                });

                var response = await _billetCreationUseCases.Execute(model);

                if (!response.Success)
                {
                    _campaignConnectorUseCases.ReleasePoints(new CampaignUserRequest()
                    {
                        campaign = model.campaign,
                        environment = model.environment,
                        token = model.token,
                        releaseCode = _reserveCode
                    });

                    return Ok(response);
                }

                _campaignConnectorUseCases.EffectDebitPoints(new CampaignUserRequest()
                {
                    campaign = model.campaign,
                    environment = model.environment,
                    token = model.token,
                    points = model.BilletPointsValue,
                    releaseCode = _reserveCode,
                    orderNumber = $"P_{response.Data.Id}"
                });

                return Ok(response);
            }
            catch (CodeException ex)
            {
                if (!string.IsNullOrEmpty(_reserveCode))
                {
                    _campaignConnectorUseCases.ReleasePoints(new CampaignUserRequest()
                    {
                        campaign = model.campaign,
                        environment = model.environment,
                        token = model.token,
                        releaseCode = _reserveCode
                    });
                }

                return BadRequest(new DefaultResponse<Billet>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(_reserveCode))
                {
                    _campaignConnectorUseCases.ReleasePoints(new CampaignUserRequest()
                    {
                        campaign = model.campaign,
                        environment = model.environment,
                        token = model.token,
                        releaseCode = _reserveCode
                    });
                }

                return StatusCode(500, new DefaultResponse<Billet>(ex) { MessageCode = "FFFF" });
            }
        }
    }
}
