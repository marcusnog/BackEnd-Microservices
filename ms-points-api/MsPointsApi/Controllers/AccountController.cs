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
    public class AccountController : Controller
    {
        #region Fields

        readonly IConfiguration _configuration;
        readonly IAccountRepository _repository;
        readonly IAccount_MovimentRepository _movimentRepository;
        readonly ILogger<AccountController> _logger;

        #endregion

        #region Constructor
        public AccountController(IAccountRepository repository, IAccount_MovimentRepository movimentRepository, IConfiguration configuration, ILogger<AccountController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _movimentRepository = movimentRepository ?? throw new ArgumentNullException(nameof(movimentRepository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion
       
        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            try
            {
                var account = new DefaultResponse<Account>(await _repository.Get(id));

                if (account == null)
                    throw new ArgumentException("Account not found");

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<Account>(ex));
            }
        }

        [HttpGet]
        [Route("GetByUserId/{userId}")]
        [Authorize("AdminPolicy")]
        [AllowAnonymous]
        public async Task<IActionResult> GetByUserId([FromRoute] string userId)
        {
            try
            {
                var account = new DefaultResponse<Account>(await _repository.GetByUser(userId));

                if (account == null)
                    throw new ArgumentException("Account not found");

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<Account>(ex));
            }
        }

        [HttpGet]
        [Route("GetPoints/{cpf}")]
        [Authorize("ProfilePolicy")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> GetPoints([FromRoute] string cpf)
        {
            try
            {
                var account = new DefaultResponse<Account>(await _repository.GetByUserCpf(cpf));

                if (account == null)
                    throw new ArgumentException("Account not found");

                var balance = account?.Data?.Balance;

                return Ok(balance);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<Account>(ex));
            }
        }

        [HttpPost]
        [Route("ChangeStatus")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> ChangeStatus([FromBody] string id)
        {
            try
            {
                var userId = (string?)Request.HttpContext.Items["UserId"];

                var account = await _repository.Get(id);
                if (account == null)
                    throw new ArgumentException("Account not found");

                account.Active = !account.Active;
                account.DeletedAt = !account.Active ? DateTime.Now : null;
                account.DeletionUserId = userId ?? null;

                await _repository.Update(account);

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

        [HttpGet]
        [Route("GetAccountMoviments/{userId}")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> GetAccountMoviments([FromRoute] string userId)
        {
            try
            {
                var account = new DefaultResponse<Account>(await _repository.GetAccountMoviments(userId));

                if (account == null)
                    throw new ArgumentException("Account not found");

                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<Account>(ex));
            }
        }

        [HttpPost]
        [Route("Create")]
        [Authorize("ProfilePolicy")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateAccountRequest request)
        {
            try
            {
                var userId = (string?)Request.HttpContext.Items["UserId"];
                DefaultResponse<Account> r;

                if (request.UserId == null)
                    throw new ArgumentException("User not found");

                if (request.CampaignId == null)
                    throw new ArgumentException("Campaign not found");

                var account = await _repository.GetByUser(request.UserId);

                if (account == null)
                {
                    var newAccount = new Contracts.DTOs.Account()
                    {
                        UserId = request.UserId,
                        CampaignId = request.CampaignId,
                        Balance = request.Balance,
                        Active = true,
                        CreatedAt = DateTime.Now,
                        CreationUserId = userId,
                    };

                    await _repository.Create(newAccount);

                    if (!string.IsNullOrEmpty(newAccount.Id))
                    {
                        r = new(new Account()
                        {
                            Id = newAccount.Id,
                            UserId = newAccount.UserId,
                            CampaignId = newAccount.CampaignId,
                            Active = newAccount.Active,
                            Balance = newAccount.Balance,
                            CreatedAt = newAccount.CreatedAt,
                            CreationUserId = userId
                        });
                    }
                    else
                        r = new("Doensn't possible create account");
                }   
                else
                {
                    r = new(new Account()
                    {
                        Id = account.Id,
                        UserId = request.UserId,
                        CampaignId = request.CampaignId,
                        Balance = request.Balance,
                        CreatedAt = DateTime.Now,
                        CreationUserId = userId
                    });
                }

                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<Account>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<Account>>(ex));
            }
        }

        [HttpPost]
        [Route("UpdateBalance")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> UpdateBalance([FromBody] UpdateBalanceRequest request)
        {
            try
            {
                var userId = (string?)Request.HttpContext.Items["UserId"];
                DefaultResponse<bool> r;

                if (request.UserId == null)
                    throw new ArgumentException("User not found");

                var account = await _repository.GetByUser(request.UserId);

                if (account != null)
                {
                    var updateBalance = new Contracts.DTOs.Account()
                    {
                        Id= account.Id,
                        UserId = account.UserId,
                        CampaignId = account.CampaignId,
                        Balance = request.Balance,
                        Active = true,
                        CreatedAt = DateTime.Now,
                        CreationUserId = userId,
                    };

                    await _repository.Update(updateBalance);
                }

                r = new(true);
                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<Account>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<Account>>(ex));
            }
        }
    }
}
