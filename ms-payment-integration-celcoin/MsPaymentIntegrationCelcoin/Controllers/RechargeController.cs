using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ms.Api.Utilities.DTO;
using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.DTO.Response;
using Ms.Api.Utilities.Exceptions;
using Ms.Api.Utilities.Models;
using MsPaymentIntegrationCelcoin.Contracts.Services;
using MsPaymentIntegrationCelcoin.Contracts.UseCases;

namespace MsPaymentIntegrationCelcoin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RechargeController : Controller
    {
        #region Fields

        readonly IConfiguration _configuration;
        readonly IIntegrationCelcoinService _integrationCelcoinService;
        readonly ILogger<RechargeController> _logger;
        readonly ICellphoneRechargeUseCases _cellphoneRechargeUseCases;
        readonly ICampaignConnectorUseCases _campaignConnectorUseCases;
        public string? _reserveCode;
        public int? _transactionId { get; set; }

        #endregion

        #region Constructor
        public RechargeController(IIntegrationCelcoinService integrationCelcoinService, IConfiguration configuration, ILogger<RechargeController> logger,
            ICellphoneRechargeUseCases cellphoneRechargeUseCases, ICampaignConnectorUseCases campaignConnectorUseCases)
        {
            _integrationCelcoinService = integrationCelcoinService ?? throw new ArgumentNullException(nameof(integrationCelcoinService));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _cellphoneRechargeUseCases = cellphoneRechargeUseCases ?? throw new ArgumentNullException(nameof(cellphoneRechargeUseCases));
            _campaignConnectorUseCases = campaignConnectorUseCases ?? throw new ArgumentNullException(nameof(campaignConnectorUseCases));
        }
        #endregion

        [HttpGet]
        [Route("GetCellPhoneOperators")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> GetCellPhoneOperators()
        {
            try
            {
                var data = await _integrationCelcoinService.GetCellphoneOperators();
                var response = new DefaultResponse<IEnumerable<SelectItem<string>>?>(data);

                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new DefaultResponse<IEnumerable<SelectItem<string>>?>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<List<IEnumerable<SelectItem<string>>?>>(ex) { MessageCode = "FFFF" });
            }
        }

        [HttpPost]
        [Route("GetCellPhoneOperatingValues")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> GetCellPhoneOperatingValues([FromBody] OperatingValuesRequest model)
        {
            try
            {
                var data = await _integrationCelcoinService.GetCellphoneOperatingValues(model.ProviderId);
                var response = new DefaultResponse<IEnumerable<SelectItem<string>>?>(data);

                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new DefaultResponse<IEnumerable<SelectItem<string>>?>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<SelectItem<string>>?>(ex) { MessageCode = "FFFF" });
            }
        }

        [HttpPost]
        [Route("GetOperatorByCellPhoneNumber")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> GetOperatorByCellPhoneNumber([FromBody] OperatingCellphoneNumber model)
        {
            try
            {
                var data = await _integrationCelcoinService.GetOperatorByCellphoneNumber(model.StateCode, model.PhoneNumber);
                var response = new DefaultResponse<CelcoinValidateOperatorResponse>(data);

                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new DefaultResponse<CelcoinValidateOperatorResponse>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<CelcoinValidateOperatorResponse>(ex) { MessageCode = "FFFF" });
            }
        }

        [HttpPost]
        [Route("ConfirmPaymentRecharge")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> ConfirmPaymentRecharge([FromBody] CellphoneRechargeRequest model)
        {
            try
            {
                //_reserveCode = await _campaignConnectorUseCases.BookPoints(new CampaignUserRequest()
                //{
                //    campaign = model.campaign,
                //    environment = model.environment,
                //    token = model.token,
                //    points = model.RechargePointsValue
                //});

                var modelCelcoin = _cellphoneRechargeUseCases.FillCelcoinObject(model);

                var data = await _integrationCelcoinService.ReserveBalanceRecharge(modelCelcoin);

                if (data.message.ToLower() != "sucesso")
                {
                    //_campaignConnectorUseCases.ReleasePoints(new CampaignUserRequest()
                    //{
                    //    campaign = model.campaign,
                    //    environment = model.environment,
                    //    token = model.token,
                    //    releaseCode = _reserveCode
                    //});
                }

                _transactionId = data.transactionId;

                model.CelcoinTransactionId = data.transactionId;

                string rechargeId = await _cellphoneRechargeUseCases.Execute(model);

                //_campaignConnectorUseCases.EffectDebitPoints(new CampaignUserRequest()
                //{
                //    campaign = model.campaign,
                //    environment = model.environment,
                //    token = model.token,
                //    points = model.RechargePointsValue,
                //    releaseCode = _reserveCode,
                //    orderNumber = $"RC_{rechargeId}"
                //});

                var response = new DefaultResponse<CelcoinReserveBalanceResponse>(data);

                return Ok(response);
            }
            catch (CodeException ex)
            {
                if (!string.IsNullOrEmpty(_reserveCode) && string.IsNullOrEmpty(_transactionId.ToString()))
                {
                    //_campaignConnectorUseCases.ReleasePoints(new CampaignUserRequest()
                    //{
                    //    campaign = model.campaign,
                    //    environment = model.environment,
                    //    token = model.token,
                    //    releaseCode = _reserveCode
                    //});
                }
                else if (!string.IsNullOrEmpty(_reserveCode) && !string.IsNullOrEmpty(_transactionId.ToString()))
                {
                    var cancel = await _integrationCelcoinService.CancelRecharge(_transactionId.ToString());

                    if (cancel.message.ToLower() != "sucesso")
                    {
                        //_campaignConnectorUseCases.ReleasePoints(new CampaignUserRequest()
                        //{
                        //    campaign = model.campaign,
                        //    environment = model.environment,
                        //    token = model.token,
                        //    releaseCode = _reserveCode
                        //});
                    }
                }

                return BadRequest(new DefaultResponse<CelcoinReserveBalanceResponse>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                if (!string.IsNullOrEmpty(_reserveCode) && string.IsNullOrEmpty(_transactionId.ToString()))
                {
                    //_campaignConnectorUseCases.ReleasePoints(new CampaignUserRequest()
                    //{
                    //    campaign = model.campaign,
                    //    environment = model.environment,
                    //    token = model.token,
                    //    releaseCode = _reserveCode
                    //});
                }
                else if (!string.IsNullOrEmpty(_reserveCode) && !string.IsNullOrEmpty(_transactionId.ToString()))
                {
                    var cancel = await _integrationCelcoinService.CancelRecharge(_transactionId.ToString());

                    if (cancel.message.ToLower() != "sucesso")
                    {
                        //_campaignConnectorUseCases.ReleasePoints(new CampaignUserRequest()
                        //{
                        //    campaign = model.campaign,
                        //    environment = model.environment,
                        //    token = model.token,
                        //    releaseCode = _reserveCode
                        //});
                    }
                }

                return StatusCode(500, new DefaultResponse<CelcoinReserveBalanceResponse>(ex) { MessageCode = "FFFF" });
            }
        }

        [HttpGet]
        [Route("GetBalance")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> GetGetBalance()
        {
            try
            {
                var data = await _integrationCelcoinService.GetBalance();
                var response = new DefaultResponse<MsPaymentIntegrationCelcoin.Contracts.DTO.Response.CelcoinBalanceResponse>(data);

                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new DefaultResponse<MsPaymentIntegrationCelcoin.Contracts.DTO.Response.CelcoinBalanceResponse>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<MsPaymentIntegrationCelcoin.Contracts.DTO.Response.CelcoinBalanceResponse>(ex) { MessageCode = "FFFF" });
            }
        }

        [HttpGet]
        [Route("GetInfosRecharge/{transactionId}")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> GetInfosRecharge(Int32 transactionId)
        {
            try
            {
                var data = await _integrationCelcoinService.GetInfosRecharge(transactionId);
                var response = new DefaultResponse<MsPaymentIntegrationCelcoin.Contracts.DTO.Response.CelcoinGetInfosRechargeResponse>(data);

                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new DefaultResponse<MsPaymentIntegrationCelcoin.Contracts.DTO.Response.CelcoinGetInfosRechargeResponse>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<MsPaymentIntegrationCelcoin.Contracts.DTO.Response.CelcoinGetInfosRechargeResponse>(ex) { MessageCode = "FFFF" });
            }
        }
    }
}
