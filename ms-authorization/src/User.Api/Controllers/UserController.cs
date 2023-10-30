using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using User.Api.Contracts.DTOs;
using User.Api.Contracts.DTOs.Request;
using User.Api.Contracts.Repositories;
using User.Api.Contracts.UseCases;
using User.Api.Extensions;

namespace User.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        #region Fields

        readonly IConfiguration _configuration;
        readonly IForgotPasswordUseCase _forgotPasswordUseCase;
        readonly IUserAdministratorRepository _repository;
        readonly ISystemRepository _systemRepository;
        readonly IProfileRepository _profileRepository;
        readonly ILogger<UserController> _logger;

        #endregion

        #region Constructor
        public UserController(IUserAdministratorRepository repository, IForgotPasswordUseCase forgotPasswordUseCase, ISystemRepository systemRepository, IConfiguration configuration, IProfileRepository profileRepository, ILogger<UserController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _forgotPasswordUseCase = forgotPasswordUseCase ?? throw new ArgumentNullException(nameof(forgotPasswordUseCase));
            _systemRepository = systemRepository ?? throw new ArgumentNullException(nameof(systemRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Authorizaion

        [HttpPost]
        [Route("GetUserInfo")]
        [Authorize("ClientPolicy")]
        public async Task<ActionResult<IEnumerable<Contracts.DTOs.UserAdministrator>>> GetUserInfo(GetUserInfoRequest request)
        {
            var instance = Cryptography.GetInstance(_configuration);
            var user = await _repository.GetUserInfo(request.login, request.system, instance);

            if (user == null)
            {
                _logger.LogError("Nenhum usuário encontrado!");
                return new NotFoundResult();
            }
            return Ok(user);
        }

        #endregion

        #region Forgot Password
        [HttpPost]
        [Route("forgot-password/recovery")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
        {
            var system = (await _systemRepository.FindByName(request?.Scope?.ToLower()));
            if (system == null) return BadRequest("Invalid scope");

            var user = await _repository.GetUserByEmail(request.Username, system.Id);
            if (user == null) return BadRequest("User not found");

            //var profile = await _profileRepository.GetProfile(user.IdProfile);

            var ip = Request.HttpContext.Connection.RemoteIpAddress?.ToString();
            await _forgotPasswordUseCase.StartRecoveryPassword(request.Lang, user.Id, user.Nickname, ip, user.Email, system);

            return new OkResult();
        }

        [HttpPost]
        [Route("forgot-password/validate/{tokenId}")]
        public async Task<IActionResult> ValidateToken([FromRoute] string tokenId)
        {
            if (!(await _forgotPasswordUseCase.ValidateToken(tokenId)))
                return BadRequest("Invalid token");

            return new NoContentResult();
        }

        [HttpPost]
        [Route("forgot-password/validate/{tokenId}/code/{code}")]
        public async Task<IActionResult> ValidateTokenCode([FromRoute] string tokenId, string code)
        {
            if (!(await _forgotPasswordUseCase.ValidateToken(tokenId, code)))
                return BadRequest("Invalid token");

            return new NoContentResult();
        }

        [HttpPost]
        [Route("forgot-password/update")]
        public async Task<IActionResult> UpdatePassword([FromBody] UpdatePasswordRequest request)
        {
            if (!await _forgotPasswordUseCase.ValidateToken(request.tokenId, request.code))
                return BadRequest("Invalid token");

            await _forgotPasswordUseCase.UpdatePassword(request.tokenId, Cryptography.GetInstance(_configuration).Encrypt(request.password));
            return new NoContentResult();
        }
        #endregion
    }
}
