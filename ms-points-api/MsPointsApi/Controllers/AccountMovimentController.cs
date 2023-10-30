using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ms.Api.Utilities.Models;
using MsPointsApi.Contracts.DTOs;
using MsPointsApi.Contracts.DTOs.Request;
using MsPointsApi.Contracts.Repositories;

namespace MsPointsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountMovimentController : Controller
    {
        #region Fields

        readonly IConfiguration _configuration;
        readonly IAccount_MovimentRepository _repository;
        readonly IAccountRepository _accountRepository;
        readonly ILogger<AccountMovimentController> _logger;

        #endregion

        #region Constructor
        public AccountMovimentController(IAccount_MovimentRepository repository, IAccountRepository accountRepository, IConfiguration configuration, ILogger<AccountMovimentController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        //Debitar pontos
        [HttpPost]
        [Route("EffetiveDebit")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> EffetiveDebit([FromBody] EffectDebitRequest request)
        {
            try
            {
                DefaultResponse<string> r;
                var response = false;
                var userId = (string?)Request.HttpContext.Items["UserId"];

                if (request.ReleaseCode == null)
                    throw new ArgumentException("ReleaseCode not informed");

                var release = await _repository.Get(request.ReleaseCode);

                var newMoviment = new Contracts.DTOs.Account_Moviment()
                {
                    AccountId = release.AccountId,
                    Value = release.Value,
                    Type = "D",
                    Active = true,
                    CreatedAt = DateTime.Now,
                    CreationUserId = userId,
                    OrderNumber = request.OrderNumber,
                    Description = $"Débito de Pontos: { request.OrderNumber }"
                };

                await _repository.Create(newMoviment);

                if (!string.IsNullOrEmpty(newMoviment.Id))
                {
                    //update previous booking
                    release.DeletedAt = DateTime.Now;
                    release.DeletionUserId = userId;
                    release.UpdatedAt = DateTime.Now;
                    release.UpdatedUserId = userId;

                    response = await _repository.Update(release);
                }
                else
                    r = new("Doesn't possible effect your debit");


                if (response)
                    r = new() { Success = true, Data = newMoviment.Id };
                else
                    r = new("Doesn't possible delete your booking");


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
        [Route("DistributePointsAdmin")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> DistributePointsAdmin([FromBody] EffectDebitAdminRequest request)
        {
            try
            {                
                var userId = (string?)Request.HttpContext.Items["UserId"];

                var newMoviment = new Contracts.DTOs.Account_Moviment()
                {
                    AccountId = request.AccountId,
                    Value = request.Value,
                    Type = "D",
                    Active = true,
                    CreatedAt = DateTime.Now,
                    CreationUserId = userId,
                    OrderNumber = "00",
                    Description = $"Distribuição de Pontos Admin - Campanha: {request.CampaignId}"
                };

                await _repository.Create(newMoviment);

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

        //Reservar pontos
        [HttpPost]
        [Route("Booking")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> Booking([FromBody] CreateReserveMovimentRequest request)
        {
            try
            {
                DefaultResponse<string> r;
                var response = false;
                var userId = (string?)Request.HttpContext.Items["UserId"];

                if (request.AccountId == null)
                    throw new ArgumentException("Account not found");

                var account = await _accountRepository.Get(request.AccountId);

                if (!(request.Value > account.Balance))
                {
                    var accountValue = (account.Balance - request.Value);
                    account.Balance = accountValue;
                    response = await _accountRepository.Update(account);

                    if (response)
                    {
                        var newMoviment = new Account_Moviment()
                        {
                            AccountId = request.AccountId,
                            Value = request.Value,
                            Type = "D",
                            Active = true,
                            CreatedAt = DateTime.Now,
                            CreationUserId = userId,
                            Description = "Reserva de Pontos"
                        };

                        await _repository.Create(newMoviment);

                        if (!string.IsNullOrEmpty(newMoviment.Id))
                        {
                            r = new() { Success = true, Data = newMoviment.Id };
                        }
                        else
                            r = new("Doesn't possible create your booking");
                    }
                    else
                        r = new("Doesn't possible update your balance");
                }
                else
                    r = new("You don't have sufficient points");

                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<Account>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<Account_Moviment>>(ex));
            }
        }

        //Creditar pontos
        [HttpPost]
        [Route("CreditPoints")]
        [Authorize("ProfilePolicy")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> CreditPoints([FromBody] CreditPointsRequest request)
        {
            try
            {
                DefaultResponse<string> r;
                var response = false;
                var userId = (string?)Request.HttpContext.Items["UserId"];

                if (request.AccountId == null)
                    throw new ArgumentException("Account not found");

                var account = await _accountRepository.Get(request.AccountId);

                // call accouunt
                var accountValue = (account.Balance + request.Value);
                account.Balance = accountValue;
                response = await _accountRepository.Update(account);

                if (response)
                {
                    var newMoviment = new Contracts.DTOs.Account_Moviment()
                    {
                        AccountId = request.AccountId,
                        Value = request.Value,
                        Type = "C",
                        Active = true,
                        CreatedAt = DateTime.Now,
                        CreationUserId = userId,
                        Description = request.Description
                    };

                    await _repository.Create(newMoviment);

                    if (!string.IsNullOrEmpty(newMoviment.Id))
                    {
                        r = new (){ Success = true, Data = newMoviment.Id };
                    }
                    else
                        r = new("Doesn't possible create a new moviment");
                }
                else
                {
                    r = new("Doesn't possible update balance");
                }

                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<Account>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<Account_Moviment>>(ex));
            }
        }

        //Liberar pontos em reserva
        [HttpPost]
        [Route("ReleasePoints")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> ReleasePoints([FromBody] ReleasePointsRequest request)
        {
            try
            {
                DefaultResponse<bool> r;
                var response = false;
                var userId = (string?)Request.HttpContext.Items["UserId"];

                if (string.IsNullOrEmpty(request.UserId))
                    throw new ArgumentException("Please inform the UserId");

                if (string.IsNullOrEmpty(request.ReleaseCode))
                    throw new ArgumentException("Please inform the ReleaseCode");

                var account = await _accountRepository.GetByUser(request.UserId);

                var release = await _repository.Get(request.ReleaseCode);

                // call accouunt
                var accountValue = (account.Balance + release.Value);
                account.Balance = accountValue;
                response = await _accountRepository.Update(account);

                if (response)
                {
                    release.UpdatedAt = DateTime.Now;
                    release.UpdatedUserId = userId;
                    release.DeletedAt = DateTime.Now;
                    release.DeletionUserId = userId;
                    release.Status = "Pontos Liberados";

                    response = false;

                    response = await _repository.Update(release);

                    if (response)
                    {
                        r = new(true);
                    }
                    else
                        r = new("Doesn't possible release the points");
                }
                else
                {
                    r = new("Doesn't possible update balance");
                }

                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<Account>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<Account_Moviment>>(ex));
            }
        }

        //Estornar pontos
        [HttpPost]
        [Route("ReversePoints")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> ReversePoints([FromBody] ReversePointsRequest request)
        {
            try
            {
                DefaultResponse<string> r;
                var response = false;
                var userId = (string?)Request.HttpContext.Items["UserId"];

                if (string.IsNullOrEmpty(request.UserId))
                    throw new ArgumentException("Please inform the UserId");

                if (string.IsNullOrEmpty(request.DebitMovimentCode))
                    throw new ArgumentException("Please inform the Debit moviment code");

                var account = await _accountRepository.GetByUser(request.UserId);

                var debit = await _repository.Get(request.DebitMovimentCode);

                // call accouunt
                var accountValue = (account.Balance + debit.Value);
                account.Balance = accountValue;
                response = await _accountRepository.Update(account);

                if (response)
                {
                    debit.UpdatedAt = DateTime.Now;
                    debit.UpdatedUserId = userId;
                    debit.DeletedAt = DateTime.Now;
                    debit.DeletionUserId = userId;
                    debit.Status = "Estorno de Pontos";

                    response = false;

                    response = await _repository.Update(debit);

                    if (response)
                    {
                        var newMoviment = new Contracts.DTOs.Account_Moviment()
                        {
                            AccountId = debit.AccountId,
                            Value = debit.Value,
                            Type = "C",
                            Active = true,
                            CreatedAt = DateTime.Now,
                            CreationUserId = userId,
                            OrderNumber = debit.OrderNumber,
                            Description = "Crédito de Pontos estornados"
                        };

                        await _repository.Create(newMoviment);

                        if (!string.IsNullOrEmpty(newMoviment.Id))
                        {
                            r = new() { Success = true, Data = newMoviment.Id};
                        }
                        else
                            r = new("Doesn't possible create a new moviment");
                    }
                    else
                        r = new("Doesn't possible release the points");
                }
                else
                {
                    r = new("Doesn't possible update balance");
                }

                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<Account>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<Account_Moviment>>(ex));
            }
        }
    }
}
