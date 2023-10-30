using IdentityServer4.Extensions;
using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ms.Api.Utilities.Models;
using System.Text;
using User.Api.Contracts.DTOs;
using User.Api.Contracts.DTOs.Request;
using User.Api.Contracts.DTOs.Response;
using User.Api.Contracts.Repositories;
using User.Api.Contracts.UseCases;
using User.Api.Extensions;

namespace User.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserAdministratorController : Controller
    {
        #region Fields

        readonly IConfiguration _configuration;
        readonly IForgotPasswordUseCase _forgotPasswordUseCase;
        readonly IUserAdministratorRepository _repository;
        readonly ISystemRepository _systemRepository;
        readonly IProfileRepository _profileRepository;
        readonly ILogger<UserController> _logger;
        readonly string _MS_PLATFORM_CONFIGURATION_URL;
        readonly IAccountUseCase _accountUseCase;
        

        #endregion

        #region Constructor
        public UserAdministratorController(IUserAdministratorRepository repository, IForgotPasswordUseCase forgotPasswordUseCase, ISystemRepository systemRepository, IConfiguration configuration, IProfileRepository profileRepository, ILogger<UserController> logger, IAccountUseCase accountUseCase)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _forgotPasswordUseCase = forgotPasswordUseCase ?? throw new ArgumentNullException(nameof(forgotPasswordUseCase));
            _systemRepository = systemRepository ?? throw new ArgumentNullException(nameof(systemRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _MS_PLATFORM_CONFIGURATION_URL = configuration.GetValue<string>("MS_PLATFORM_CONFIGURATION_URL");
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accountUseCase = accountUseCase ?? throw new ArgumentNullException(nameof(accountUseCase));
        }

        #endregion

        #region Web

        [HttpGet]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> List([FromQuery] int? page = null, [FromQuery] int? limit = null, [FromQuery] bool? status = null, [FromQuery] string? q = null)
        {
            try
            {
                var userId = (string?)Request.HttpContext.Items["UserId"];
                if (userId == null)
                    throw new ArgumentException("User not found");
                var userX = await _repository.Get(userId);


                page ??= 1;
                limit ??= 10;
                var data = await _repository.List(page.Value, limit.Value, status, q, (int)Utils.Enums.UserType.Administrator, clientId: userX.ClientId);
                var systems = await _systemRepository.List();
                var profiles = await _profileRepository.List();
                var r = new PaginatedResponse<IEnumerable<GetUserResponse>>(data.Items.Select(user => new GetUserResponse()
                {
                    Id = user.Id,
                    Email = user.Email,
                    IdProfile = user.IdProfile,
                    IdSystem = user.IdSystem,
                    Nickname = user.Nickname,
                    Phone = user.Phone,
                    ProfileName = profiles?.FirstOrDefault(x => x.Id == user.IdProfile)?.Name,
                    SystemName = systems?.FirstOrDefault(x => x.Id == user.IdSystem)?.Name,
                    Active = user.Active,
                    CreationDate = user.CreationDate,
                    UserType = (int)Utils.Enums.UserType.Administrator,
                    ClientId = user.ClientId
                }))
                {
                    Metadata = new Metadata()
                    {
                        Page = page.Value,
                        PageSize = limit.Value,
                        TotalItems = data.TotalItems
                    }
                };
                return Ok(r);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<IEnumerable<GetUserResponse>>>(ex));
            }
        }
        [HttpGet]
        [Route("{id}")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            try
            {
                var user = await _repository.GetByUserType(id, (int)Utils.Enums.UserType.Administrator);
                if (user == null)
                    throw new ArgumentException("User not found");

                var profile = user.IdProfile.IsValidMongoID() ? await _profileRepository.Get(user.IdProfile) : null;
                var system = user.IdSystem.IsValidMongoID() ? await _systemRepository.Get(user.IdSystem) : null;

                DefaultResponse<GetUserResponse> r = new(new GetUserResponse()
                {
                    Id = user.Id,
                    Email = user.Email,
                    IdProfile = user.IdProfile,
                    IdSystem = user.IdSystem,
                    Nickname = user.Nickname,
                    Phone = user.Phone,
                    ProfileName = profile?.Name,
                    SystemName = system?.Name,
                    Active = user.Active,
                    CreationDate = user.CreationDate,
                    UserType = (int)Utils.Enums.UserType.Administrator,
                    ClientId = user.ClientId
                });
                return Ok(r);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<GetUserResponse>>(ex));
            }
        }
        [HttpDelete]
        [Route("{id}")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> ChangeStatus([FromRoute] string id)
        {
            try
            {
                var userId = (string?)Request.HttpContext.Items["UserId"];
                if (userId == null)
                    throw new ArgumentException("User not found");

                var user = await _repository.GetByUserType(id, (int)Utils.Enums.UserType.Administrator);
                if (user == null)
                    throw new ArgumentException("User not found");

                user.Active = !user.Active;
                user.DeletionDate = !user.Active ? DateTime.UtcNow.ToUnixTimestamp() : null;
                user.DeletionUserId = !user.Active ? userId : null;

                await _repository.Update(user);

                DefaultResponse<bool> r = new(true);
                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<bool>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<bool>>(ex));
            }
        }

        [HttpPost]
        [Authorize("AdminPolicy")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
        {
            try
            {
                var token = Request.Headers.Authorization.FirstOrDefault();

                DefaultResponse<CreateUserResponse> r;
                UserAdministrator user = null;
                Contracts.DTOs.System system = null;
                Contracts.DTOs.Profile profile = null;

                if ((bool?)request.NewClientUser != true)
                {
                    var userId = (string?)Request.HttpContext.Items["UserId"];
                    if (userId == null)
                        throw new ArgumentException("User not found");

                    system = await _systemRepository.GetByName("Admin");
                    if (system == null)
                        throw new ArgumentException("System not found");

                    profile = await _profileRepository.GetByName("Administrator");
                    if (profile == null)
                        throw new ArgumentException("Profile not found");

                    user = new Contracts.DTOs.UserAdministrator()
                    {
                        Nickname = request.Nickname,
                        Email = request.Email,
                        Login = request.Email,
                        Phone = request.Phone,
                        Active = true,
                        Password = Cryptography.GetInstance(_configuration).Encrypt(request.Password),
                        IdProfile = profile.Id,
                        IdSystem = system.Id,
                        CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                        CreationUserId = userId,
                        UserType = (int)Utils.Enums.UserType.Administrator
                    };

                }
                else
                {
                    system = await _systemRepository.GetByName("Admin");
                    if (system == null)
                        throw new ArgumentException("System not found");

                    profile = await _profileRepository.GetByName("Administrator");
                    if (profile == null)
                        throw new ArgumentException("Profile not found");

                    user = new Contracts.DTOs.UserAdministrator()
                    {
                        Nickname = request.Nickname,
                        Email = request.Email,
                        Login = request.Email,
                        Phone = request.Phone,
                        Active = true,
                        Password = Cryptography.GetInstance(_configuration).Encrypt(request.Password),
                        IdProfile = profile.Id,
                        IdSystem = system.Id,
                        Profile = profile,
                        System = system,
                        CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                        UserType = (int)Utils.Enums.UserType.Administrator
                    };
                }
                await _repository.Create(user);

                if (!string.IsNullOrEmpty(request.ClientId))
                {
                    user.ClientId = request.ClientId;
                    await _repository.Update(user);
                }
                else if (!string.IsNullOrEmpty(request.Cliente) && !string.IsNullOrEmpty(request.CNPJ))
                {
                    var newClient = new CreateClientRequest()
                    {
                        Name = request.Cliente,
                        Documents = new List<Document>
                        {
                            new Document()
                            {
                                Type = "CNPJ",
                                Value = request.CNPJ
                            }
                        }.ToArray(),
                    };

                    var result = await CreateClient(newClient);


                    if (result == null) throw new Exception("An error has occurred while creating client");

                    user.ClientId = result.Id;
                    await _repository.Update(user);
                }

                var newAccount = new CreateAccountRequest()
                {
                    Balance = 0,
                    CampaignId = "000000000000000000000000",
                    UserId = user.Id
                };

                var account = await _accountUseCase.CreateAccount(newAccount, null);

                if (account == null) throw new Exception("An error has occurred while creating user account");



                r = new(new CreateUserResponse()
                {
                    Id = user.Id,
                    Email = user.Email,
                    IdProfile = user.IdProfile,
                    IdSystem = user.IdSystem,
                    Nickname = user.Nickname,
                    Phone = user.Phone,
                    ProfileName = profile.Name,
                    SystemName = system.Name,
                    Active = user.Active,
                    CreationDate = user.CreationDate,
                    UserType = (int)Utils.Enums.UserType.Administrator
                });
                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<CreateUserResponse>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<CreateUserResponse>>(ex));
            }
        }

        [HttpPut]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> Update([FromBody] UpdateUserRequest request)
        {
            try
            {
                var userId = (string?)Request.HttpContext.Items["UserId"];
                if (userId == null)
                    throw new ArgumentException("User not found");

                var user = request.Id.IsValidMongoID() ? await _repository.GetByUserType(request.Id, (int)Utils.Enums.UserType.Administrator) : null;
                if (user == null)
                    throw new ArgumentException("User not found");


                var profile = request.IdProfile.IsValidMongoID() ? await _profileRepository.Get(request.IdProfile) : null;
                if (profile == null)
                    throw new ArgumentException("Profile not found");

                var system = user.IdSystem.IsValidMongoID() ? await _systemRepository.Get(user.IdSystem) : null;


                user.Nickname = request.Nickname;
                user.Phone = request.Phone;
                user.IdProfile = profile.Id;

                await _repository.Update(user);

                DefaultResponse<UpdateUserResponse> r = new(new UpdateUserResponse()
                {
                    Id = user.Id,
                    Email = user.Email,
                    IdProfile = user.IdProfile,
                    IdSystem = user.IdSystem,
                    Nickname = user.Nickname,
                    Phone = user.Phone,
                    ProfileName = profile.Name,
                    SystemName = system?.Name,
                    //AcceptedConsentTerm = user.AcceptedConsentTerm
                });
                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<UpdateUserResponse>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<UpdateUserResponse>>(ex));
            }
        }

        [HttpPut]
        [Route("UpdateConsentTerm")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> UpdateConsentTerm([FromBody] UpdateUserRequest request)
        {
            try
            {
                var user = request.Id.IsValidMongoID() ? await _repository.GetByUserType(request.Id, (int)Utils.Enums.UserType.Administrator) : null;
                if (user == null)
                    throw new ArgumentException("User not found");

                var profile = request.IdProfile.IsValidMongoID() ? await _profileRepository.Get(request.IdProfile) : null;
                if (profile == null)
                    throw new ArgumentException("Profile not found");

                //user.AcceptedConsentTerm = request.AcceptedConsentTerm;

                await _repository.Update(user);

                return Ok();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<UpdateUserResponse>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<UpdateUserResponse>>(ex));
            }
        }

        internal async Task<GetClientInfoResponse> CreateClient(CreateClientRequest request)
        {
            using var client = new HttpClient();
            var response = await client.PostAsync($"{_MS_PLATFORM_CONFIGURATION_URL}", new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json"));
            var content = await response.Content.ReadAsStringAsync();

            var result = Newtonsoft.Json.JsonConvert.DeserializeObject<DefaultResponse<GetClientInfoResponse>>(content);

            if (!response.IsSuccessStatusCode || !string.IsNullOrEmpty(result.Message))
                throw new Exception($"{result.Message}");

            return result.Data;
        }

        #endregion
    }
}
