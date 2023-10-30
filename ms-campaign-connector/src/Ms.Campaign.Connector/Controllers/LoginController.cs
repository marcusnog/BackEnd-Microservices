using Microsoft.AspNetCore.Mvc;
using Ms.Api.Utilities.DTO.Request;
using Ms.Api.Utilities.Models;
using Ms.Campaign.Connector.Contracts.DTO.Request;
using Ms.Campaign.Connector.Contracts.DTO.Response;
using Ms.Campaign.Connector.Contracts.Repositories;
using Ms.Campaign.Connector.Contracts.UseCases.Login;

namespace Ms.Campaign.Connector.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {

        private readonly ILogger<LoginController> _logger;
        private readonly ICampaignConnectorRepository _campaignConnectorRepository;
        private readonly IRedisRepository _redisRepository;

        public LoginController(ILogger<LoginController> logger, IRedisRepository redisRepository, ICampaignConnectorRepository campaignConnectorRepository)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _campaignConnectorRepository = campaignConnectorRepository ?? throw new ArgumentNullException(nameof(campaignConnectorRepository));
            _redisRepository = redisRepository ?? throw new ArgumentNullException(nameof(redisRepository));
        }

        [HttpPost]
        public async Task<ActionResult<CampaignUserInformation>> Login([FromBody] CampaignUserRequest request)
        {
            try
            {
                var loginUseCases = typeof(Program).Assembly.GetTypes()
                            .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(LoginUseCase)) == typeof(LoginUseCase));

                var loginUseCase = loginUseCases.Select(i => ((LoginUseCase)Activator.CreateInstance(i, _redisRepository, _campaignConnectorRepository))).FirstOrDefault(x => x.campaigns?.Contains(request.campaign) == true);
                var userInfo = await loginUseCase.GetUser(request.environment, request.campaign, request.token);

                if (userInfo == null)
                    throw new ArgumentException("Invalid User");

                var r = new DefaultResponse<CampaignInfo>(userInfo);
                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<CampaignUserInformation>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<CampaignUserInformation>(ex));
            }
        }

        //Obter-Pontos
        [HttpPost("~/GetPoints")]
        public async Task<ActionResult<decimal>> GetPoints([FromBody] CampaignUserRequest request)
        {
            try
            {
                var loginUseCases = typeof(Program).Assembly.GetTypes()
                            .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(LoginUseCase)) == typeof(LoginUseCase));

                var loginUseCase = loginUseCases.Select(i => ((LoginUseCase)Activator.CreateInstance(i, _redisRepository, _campaignConnectorRepository))).FirstOrDefault(x => x.campaigns?.Contains(request.campaign) == true);
                var userPoints = await loginUseCase.GetPoints(request.token, request.environment, request.campaign);

                if (userPoints == null)
                    throw new ArgumentException("Invalid Points");

                var r = new DefaultResponse<decimal>(userPoints);
                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<decimal>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<decimal>(ex));
            }
        }

        //Reservar-Pontos
        [HttpPost("~/BookPoints")]
        public async Task<ActionResult<DefaultResponse<string>>> BookPoints([FromBody] CampaignUserRequest request)
        {
            try
            {
                var loginUseCases = typeof(Program).Assembly.GetTypes()
                            .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(LoginUseCase)) == typeof(LoginUseCase));

                var loginUseCase = loginUseCases.Select(i => ((LoginUseCase)Activator.CreateInstance(i, _redisRepository, _campaignConnectorRepository))).FirstOrDefault(x => x.campaigns?.Contains(request.campaign) == true);
                var userExtract = await loginUseCase.BookPoints(request.token, request.environment, request.campaign, request.points);

                if (userExtract == null)
                    throw new ArgumentException("Invalid userExtract");

                if (!userExtract.sucesso)
                    return new DefaultResponse<string>(userExtract?.mensagem);

                var r = new DefaultResponse<string>() { Success = true, Data = userExtract.dados };
                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<string>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<string>(ex));
            }
        }

        //Debitar-Pontos
        [HttpPost("~/EffectDebitPoints")]
        public void EffectDebitPoints([FromBody] CampaignUserRequest request)
        {
            try
            {
                var loginUseCases = typeof(Program).Assembly.GetTypes()
                            .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(LoginUseCase)) == typeof(LoginUseCase));

                var loginUseCase = loginUseCases.Select(i => ((LoginUseCase)Activator.CreateInstance(i, _redisRepository, _campaignConnectorRepository))).FirstOrDefault(x => x.campaigns?.Contains(request.campaign) == true);

                loginUseCase?.EffectDebitPoints(request.token, request.environment, request.campaign, request.releaseCode, request.orderNumber);
            }
            catch (ArgumentException ex)
            {
                BadRequest(new DefaultResponse<string>(ex));
            }
            catch (Exception ex)
            {
                StatusCode(500, new DefaultResponse<string>(ex));
            }
        }

        //Liberar-Pontos
        [HttpPost("~/ReleasePoints")]
        public void ReleasePoints([FromBody] CampaignUserRequest request)
        {
            try
            {
                var loginUseCases = typeof(Program).Assembly.GetTypes()
                            .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(LoginUseCase)) == typeof(LoginUseCase));

                var loginUseCase = loginUseCases.Select(i => ((LoginUseCase)Activator.CreateInstance(i, _redisRepository, _campaignConnectorRepository))).FirstOrDefault(x => x.campaigns?.Contains(request.campaign) == true);
                loginUseCase?.ReleasePoints(request.token, request.environment, request.campaign, request.releaseCode);
            }
            catch (ArgumentException ex)
            {
                BadRequest(new DefaultResponse<string>(ex));
            }
            catch (Exception ex)
            {
                StatusCode(500, new DefaultResponse<string>(ex));
            }
        }

        //Estornar-Pontos
        [HttpPost("~/ReversePoints")]
        public void ReversePoints([FromBody] CampaignUserRequest request)
        {
            try
            {
                var loginUseCases = typeof(Program).Assembly.GetTypes()
                            .Where(x => !x.IsAbstract && x.IsClass && x.GetInterface(nameof(LoginUseCase)) == typeof(LoginUseCase));

                var loginUseCase = loginUseCases.Select(i => ((LoginUseCase)Activator.CreateInstance(i, _redisRepository, _campaignConnectorRepository))).FirstOrDefault(x => x.campaigns?.Contains(request.campaign) == true);
                loginUseCase?.ReversePoints(request.token, request.environment, request.campaign, request.points.ToString(), request.requestNumber);
            }
            catch (ArgumentException ex)
            {
                BadRequest(new DefaultResponse<string>(ex));
            }
            catch (Exception ex)
            {
                StatusCode(500, new DefaultResponse<string>(ex));
            }
        }
    }
}