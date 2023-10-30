using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Ms.Api.Utilities.Models;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;
using System.Drawing;
using User.Api.Contracts.DTOs;
using User.Api.Contracts.DTOs.Request;
using User.Api.Contracts.DTOs.Response;
using User.Api.Contracts.Repositories;
using User.Api.Contracts.UseCases;
using User.Api.Extensions;
using User.Contracts.Repositories;

namespace User.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserParticipantController : Controller
    {
        #region Fields

        readonly IConfiguration _configuration;
        readonly IForgotPasswordUseCase _forgotPasswordUseCase;
        readonly IUserParticipantRepository _repository;
        readonly ISystemRepository _systemRepository;
        readonly IProfileRepository _profileRepository;
        readonly IAddressRepository _addressRepository;
        readonly ILogger<UserController> _logger;
        readonly IAccountUseCase _accountUseCase;
        readonly IAccountMovimentUseCase _accountMovimentUseCase;

        #endregion

        #region Constructor
        public UserParticipantController(IUserParticipantRepository repository, IForgotPasswordUseCase forgotPasswordUseCase, ISystemRepository systemRepository, IConfiguration configuration, IProfileRepository profileRepository, IAddressRepository addressRepository, ILogger<UserController> logger, IAccountUseCase accountUseCase, IAccountMovimentUseCase accountMovimentUseCase)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _forgotPasswordUseCase = forgotPasswordUseCase ?? throw new ArgumentNullException(nameof(forgotPasswordUseCase));
            _systemRepository = systemRepository ?? throw new ArgumentNullException(nameof(systemRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _profileRepository = profileRepository ?? throw new ArgumentNullException(nameof(profileRepository));
            _addressRepository = addressRepository ?? throw new ArgumentNullException(nameof(addressRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _accountUseCase = accountUseCase ?? throw new ArgumentNullException(nameof(accountUseCase));
            _accountMovimentUseCase = accountMovimentUseCase ?? throw new ArgumentNullException(nameof(accountMovimentUseCase));
        }

        #endregion

        #region Web

        [HttpPost]
        [Route("GetUserInfo")]
        [Authorize("ClientPolicy")]
        public async Task<ActionResult<IEnumerable<UserParticipant>>> GetUserInfo(GetUserInfoRequest request)
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

        [HttpGet]
        [Route("{campaignId}")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> List([FromRoute] string campaignId, [FromQuery] int? page = null, [FromQuery] int? limit = null, [FromQuery] bool? status = null, [FromQuery] string? q = null)
        {
            try
            {
                page ??= 1;
                limit ??= 10;
                var data = await _repository.List(campaignId, page.Value, limit.Value, status, q, (int)Utils.Enums.UserType.Participant);
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
                    ProfileName = profiles.FirstOrDefault(x => x.Id == user.IdProfile)?.Name,
                    SystemName = systems.FirstOrDefault(x => x.Id == user.IdSystem)?.Name,
                    Active = user.Active,
                    CreationDate = user.CreationDate,
                    UserType = (int)Utils.Enums.UserType.Participant,
                    CampaignId = campaignId
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
        [Route("Participant/{userId}")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> GetUser([FromRoute] string userId)
        {
            try
            {
                var user = await _repository.Get(userId);
                var lstAddress = await _addressRepository.List(userId);
                var r = new DefaultResponse<UserParticipantResponse>(new UserParticipantResponse()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Nickname = user.Nickname,
                    Phone = user.Phone,
                    Login = user.Login,
                    Password = user.Password,
                    Addresses = lstAddress
                });

                return Ok(r);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<UserParticipantResponse>(ex));
            }
        }

        [HttpPost]
        [Route("UpdateUser")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserParticipant model)
        {
            try
            {
                var user = await _repository.Get(model.Id);

                var updateUser = new Contracts.DTOs.UserParticipant()
                {
                    Id = user.Id,
                    Active = user.Active,
                    CampaignId = user.CampaignId,
                    Email = model.Email,
                    IdProfile = user.IdProfile,
                    IdSystem = user.IdSystem,
                    Login = user.Login,
                    Nickname = model.Nickname,
                    Password = user.Password,
                    Phone = model.Phone,
                    Profile = user.Profile,
                    System = user.System,
                    UserType = user.UserType,
                    CreationDate = user.CreationDate,
                    DeletionDate = user.DeletionDate
                };

                var response = new DefaultResponse<UserParticipant>(await _repository.Update(updateUser));

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<UserParticipant>(ex));
            }
        }

        [HttpPost]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> Create([FromBody] CreateParticipantRequest request)
        {
            try
            {
                var token = Request.Headers.Authorization.FirstOrDefault();

                DefaultResponse<CreateUserResponse> r;
                UserParticipant user = null;
                Contracts.DTOs.System system = null;
                Contracts.DTOs.Profile profile = null;

                var userId = (string?)Request.HttpContext.Items["UserId"];
                if (userId == null)
                    throw new ArgumentException("User not found");

                system = await _systemRepository.GetByName("Catalog");
                if (system == null)
                    throw new ArgumentException("System not found");

                profile = await _profileRepository.GetByName("Participant");
                if (profile == null)
                    throw new ArgumentException("Profile not found");

                user = new Contracts.DTOs.UserParticipant()
                {
                    Nickname = request.Nickname,
                    Email = request.Email,
                    Login = request.Email,
                    Phone = request.Phone,
                    Active = true,
                    Cpf= request.Cpf,
                    Password = Cryptography.GetInstance(_configuration).Encrypt(request.Password),
                    IdProfile = profile.Id,
                    IdSystem = system.Id,
                    CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                    CreationUserId = userId,
                    UserType = (int)Utils.Enums.UserType.Participant,
                    CampaignId = request.CampaignId
                };

                await _repository.Create(user);

                var newAccount = new CreateAccountRequest()
                {
                    Balance = 0,
                    CampaignId = request.CampaignId,
                    UserId = user.Id
                };

                var account = await _accountUseCase.CreateAccount(newAccount, token);

                if (account != null)
                {
                    r = new(new CreateUserResponse()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        IdProfile = user.IdProfile,
                        IdSystem = user.IdSystem,
                        Nickname = user.Nickname,
                        Phone = user.Phone,
                        Cpf = user.Cpf,
                        ProfileName = profile.Name,
                        SystemName = system.Name,
                        Active = user.Active,
                        CreationDate = user.CreationDate,
                        UserType = (int)Utils.Enums.UserType.Participant
                    });
                }
                else
                    r = new("Doesn't possible create the account");


                r = new(new CreateUserResponse()
                {
                    Id = user.Id,
                    Email = user.Email,
                    IdProfile = user.IdProfile,
                    IdSystem = user.IdSystem,
                    Nickname = user.Nickname,
                    Phone = user.Phone,
                    Cpf = user.Cpf,
                    ProfileName = profile.Name,
                    SystemName = system.Name,
                    Active = user.Active,
                    CreationDate = user.CreationDate,
                    UserType = (int)Utils.Enums.UserType.Participant
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

        [HttpGet]
        [Route("GetTemplateParticipants")]
        [Authorize("AdminPolicy")]
        public FileResult CreateExcelTemplateParticipants()
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var package = new ExcelPackage();
                var workbook = package.Workbook;
                var worksheet1 = workbook.Worksheets.Add("Novos Participantes");

                worksheet1.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet1.Row(1).Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                worksheet1.Row(1).Style.Font.Bold = true;
                worksheet1.Cells.AutoFitColumns();

                worksheet1.Column(1).Width = 30;
                worksheet1.Column(2).Width = 30;
                worksheet1.Column(3).Width = 20;
                worksheet1.Column(4).Width= 20;

                worksheet1.Cells[1, 1].Value = "Nome";
                worksheet1.Cells[1, 2].Value = "Email";
                worksheet1.Cells[1, 3].Value = "Telefone";
                worksheet1.Cells[1, 4].Value = "Cpf";

                var stream = new MemoryStream();

                package.SaveAs(stream);
                stream.Seek(0, SeekOrigin.Begin);

                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                string fileName = "TemplateParticipantes-" + DateTime.Now;
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(bytes, contentType, fileName + ".xlsx");
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpPost]
        [Route("ImportParticipants")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> ImportParticipants([FromForm] ImportParticipantsRequest request)
        {
            try
            {
                var token = Request.Headers.Authorization.FirstOrDefault();

                DefaultResponse<bool> r;
                Contracts.DTOs.System system = null;
                Contracts.DTOs.Profile profile = null;

                var userId = (string?)Request.HttpContext.Items["UserId"];
                if (userId == null)
                    throw new ArgumentException("User not found");

                system = await _systemRepository.GetByName("Catalog");
                if (system == null)
                    throw new ArgumentException("System not found");

                profile = await _profileRepository.GetByName("Participant");
                if (profile == null)
                    throw new ArgumentException("Profile not found");

                if (request.File.Length > 0 && request.File != null)
                {
                    // MAke sure that only Excel file is used 
                    string dataFileName = Path.GetFileName(request.File.FileName);

                    string extension = Path.GetExtension(dataFileName);

                    string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

                    if (!allowedExtsnions.Contains(extension))
                        throw new Exception("O arquivo não está no formato correto. Formato permitido: XLSX");

                    // USe this to handle Encodeing differences in .NET Core
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (ExcelPackage package = new ExcelPackage(request.File.OpenReadStream()))
                    {
                        //get the first sheet from the excel file
                        ExcelWorksheet sheet = package.Workbook.Worksheets[0];

                        //exclude header from loop
                        var indexRow = sheet.Dimension.Start.Row + 1;

                        //loop all rows in the sheet
                        for (int i = indexRow; i <= sheet.Dimension.End.Row; i++)
                        {
                            UserParticipant participant = new();
                            participant.Nickname = sheet.Cells[i, 1].Value.ToString();
                            participant.Login = sheet.Cells[i, 2].Value.ToString();
                            participant.Email = sheet.Cells[i, 2].Value.ToString();
                            participant.Password = Cryptography.GetInstance(_configuration).Encrypt("Mudar123");
                            participant.Phone = sheet.Cells[i, 3].Value.ToString();
                            participant.Cpf = sheet.Cells[i, 4].Value.ToString();
                            participant.IdProfile = profile.Id;
                            participant.IdSystem = system.Id;
                            participant.CreationDate = DateTime.UtcNow.ToUnixTimestamp();
                            participant.CreationUserId = userId;
                            participant.UserType = (int)Utils.Enums.UserType.Participant;
                            participant.CampaignId = request.CampaignId;

                            await _repository.Create(participant);

                            if (!string.IsNullOrEmpty(participant.Id))
                            {
                                var newAccount = new CreateAccountRequest()
                                {
                                    Balance = 0,
                                    CampaignId = participant.CampaignId,
                                    UserId = participant.Id
                                };

                                var account = await _accountUseCase.CreateAccount(newAccount, token);

                                if (!string.IsNullOrEmpty(account.Id))
                                {
                                    continue;
                                }
                                else
                                    r = new("Doesn't possible create the account");
                            }
                            else
                                r = new($"Doesn't possible create the participant: {participant.Nickname}");
                        }
                    }
                }

                r = new(true);

                return Ok(r);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpGet]
        [Route("GetTemplateDistributePoints")]
        [Authorize("AdminPolicy")]
        public async Task<FileResult> CreateExcelTemplateDistributePoints([FromBody] IEnumerable<UserParticipant> lstParticipants)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                var package = new ExcelPackage();
                var workbook = package.Workbook;
                var worksheet1 = workbook.Worksheets.Add("Distribuir Pontos");

                var dataTable = ObterDataTable(lstParticipants);

                worksheet1.Cells["A" + 1].LoadFromDataTable(dataTable, true);

                worksheet1.Row(1).Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet1.Row(1).Style.Fill.BackgroundColor.SetColor(Color.LightBlue);
                worksheet1.Row(1).Style.Font.Bold = true;
                worksheet1.Cells.AutoFitColumns();

                var stream = new MemoryStream();

                package.SaveAs(stream);
                stream.Seek(0, SeekOrigin.Begin);

                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);

                string fileName = "TemplateDistribuirPontos-" + DateTime.Now;
                var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                return File(bytes, contentType, fileName + ".xlsx");
            }
            catch (Exception)
            {
                return null;
            }
        }

        private System.Data.DataTable ObterDataTable(IEnumerable<UserParticipant> lstParticipants)
        {
            var dt = new System.Data.DataTable();
            ObterGlobal(dt, lstParticipants);
            return dt;
        }

        private void ObterGlobal(System.Data.DataTable dt, IEnumerable<UserParticipant> lstParticipants)
        {
            dt.Columns.Add("Cpf");
            dt.Columns.Add("Nome Participante");
            dt.Columns.Add("Pontos");
            dt.Columns.Add("Observação");

            if (lstParticipants != null && lstParticipants.Count() > 0)
            {
                foreach (var participant in lstParticipants)
                {
                    dt.Rows.Add(participant.Cpf,
                                participant.Nickname,
                                "",
                                "");
                }
            }
        }

        [HttpPost]
        [Route("ImportDistributePoints")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> ImportDistributePoints([FromForm] ImportPointsRequest request)
        {
            try
            {
                var token = Request.Headers.Authorization.FirstOrDefault();

                DefaultResponse<bool> r;
                Contracts.DTOs.System system = null;
                Contracts.DTOs.Profile profile = null;

                var UserId = (string?)Request.HttpContext.Items["UserId"];
                if (UserId == null)
                    throw new ArgumentException("User not found");

                var accountAdmin = await _accountUseCase.GetAccount(UserId, token);

                if (request.File.Length > 0 && request.File != null)
                {
                    // MAke sure that only Excel file is used 
                    string dataFileName = Path.GetFileName(request.File.FileName);

                    string extension = Path.GetExtension(dataFileName);

                    string[] allowedExtsnions = new string[] { ".xls", ".xlsx" };

                    if (!allowedExtsnions.Contains(extension))
                        throw new Exception("O arquivo não está no formato correto. Formato permitido: XLSX");

                    List<DistributePoints> lstDistribution = new();

                    // USe this to handle Encodeing differences in .NET Core
                    System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                    using (ExcelPackage package = new ExcelPackage(request.File.OpenReadStream()))
                    {
                        //get the first sheet from the excel file
                        ExcelWorksheet sheet = package.Workbook.Worksheets[0];

                        //exclude header from loop
                        var indexRow = sheet.Dimension.Start.Row + 1;

                        //loop all rows in the sheet
                        for (int i = indexRow; i <= sheet.Dimension.End.Row; i++)
                        {
                            DistributePoints distribute = new();
                            distribute.Cpf = sheet.Cells[i, 1].Value.ToString();
                            distribute.ParticipantName = sheet.Cells[i, 2].Value.ToString();
                            distribute.Points = Convert.ToDecimal(sheet.Cells[i, 3].Value);
                            distribute.Observation = sheet.Cells[i, 4].Value.ToString();

                            lstDistribution.Add(distribute);
                        }
                    }

                    var totalPointsInExcel = lstDistribution.Sum(x => x.Points);

                    var totalPointsToDistribute = await _accountUseCase.GetBalancePoints(UserId, token);

                    if (totalPointsToDistribute <= 0)
                    {
                        r = new($"You have no points balance to distribute. Please get points to distribute.");
                        return Ok(r);
                    }

                    if (totalPointsInExcel > totalPointsToDistribute)
                    {
                        r = new($"The total value informed in Excel exceeded the available limit of: {totalPointsToDistribute} points");
                        return Ok(r);
                    }
                    else
                    {
                        decimal remainingBalance = 0;

                        if (totalPointsInExcel < totalPointsToDistribute)
                            remainingBalance = (totalPointsToDistribute - totalPointsInExcel);
                        else if (totalPointsToDistribute == totalPointsInExcel)
                            remainingBalance = 0;

                        foreach (var item in lstDistribution)
                        {
                            var accountParticipant = await _accountUseCase.GetAccount(item.Cpf, token);

                            var creditPoints = new CreditPointsRequest()
                            {
                                Cpf = item.Cpf,
                                Value = item.Points,
                                Description = item.Observation
                            };

                            await _accountMovimentUseCase.CreditPoints(creditPoints, token);

                            var updateBalance = new UpdateBalanceRequest()
                            {
                                Cpf = item.Cpf,
                                Balance = remainingBalance
                            };

                            await _accountUseCase.UpdateBalance(updateBalance, token);
                        }

                        //var updateBalance = new UpdateBalanceRequest()
                        //{
                        //    Cpf = item.Cpf,
                        //    Balance = remainingBalance
                        //};

                        

                        var effectDistribute = new EffectDebitAdminRequest()
                        {
                            CampaignId = request.CampaignId,
                            Value = totalPointsInExcel,
                            AccountId = accountAdmin.Id
                        };

                        await _accountMovimentUseCase.DistributePoints(effectDistribute, token);
                    }
                }

                r = new(true);

                return Ok(r);
            }
            catch (Exception)
            {
                return null;
            }
        }

        [HttpDelete]
        [Route("{campaignId}/{id}")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> ChangeStatus([FromRoute] string campaignId, [FromRoute] string id)
        {
            try
            {
                var userId = (string?)Request.HttpContext.Items["UserId"];
                if (userId == null)
                    throw new ArgumentException("User not found");

                var user = await _repository.GetByUserType(id, (int)Utils.Enums.UserType.Participant);
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

            await _forgotPasswordUseCase.UpdatePasswordUserParticipant(request.tokenId, Cryptography.GetInstance(_configuration).Encrypt(request.password));
            return new NoContentResult();
        }
        #endregion

        #endregion
    }
}
