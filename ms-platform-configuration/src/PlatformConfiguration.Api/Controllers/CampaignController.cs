using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ms.Api.Utilities.Extensions;
using Ms.Api.Utilities.Models;
using PlatformConfiguration.Api.Contracts.DTOs;
using PlatformConfiguration.Api.Contracts.DTOs.Request;
using PlatformConfiguration.Api.Contracts.DTOs.Response;
using PlatformConfiguration.Api.Contracts.Repositories;

namespace PlatformConfiguration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize("ProfilePolicy")]
public class CampaignController : ControllerBase
{
    private readonly ILogger<CampaignController> _logger;
    readonly ICampaignRepository _campaignRepository;
    readonly IClientRepository _clientRepository;
    readonly IStoreRepository _storeRepository;
    public CampaignController(ILogger<CampaignController> logger,
        ICampaignRepository campaignRepository,
        IClientRepository clientRepository,
        IStoreRepository storeRepository)
    {
        _campaignRepository = campaignRepository ?? throw new ArgumentException(nameof(campaignRepository));
        _clientRepository = clientRepository ?? throw new ArgumentException(nameof(clientRepository));
        _storeRepository = storeRepository ?? throw new ArgumentException(nameof(storeRepository));
        _logger = logger ?? throw new ArgumentException(nameof(logger));
    }


    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int? page = null, [FromQuery] int? limit = null, [FromQuery] bool? status = null, [FromQuery] string? q = null)
    {
        try
        {
            page ??= 1;
            limit ??= 10;
            var data = await _campaignRepository.List(page.Value, limit.Value, status, q);
            var clients = await _clientRepository.List();
            var stores = await _storeRepository.ListAsOptions();

            var r = new PaginatedResponse<IEnumerable<GetCampaignInfoResponse>>(data.Items.Select(x => new GetCampaignInfoResponse()
            {
                Id = x.Id,
                ClientId = x.ClientId,
                ClientName = clients.FirstOrDefault(c => c.Id == x.ClientId)?.Name,
                Name = x.Name,
                CoinConversionFactor = x.CoinConversionFactor,
                AllowCardPayment = x.AllowCardPayment,
                AllowedCardPaymentPercentage = x.AllowedCardPaymentPercentage,
                AllowPointAmountSelection = x.AllowPointAmountSelection,
                CreationDate = x.CreationDate,
                CreationUserId = x.CreationUserId,
                DeletionDate = x.DeletionDate,
                DeletionUserId = x.DeletionUserId,
                Stores = x.Stores?.Select((x) =>
                {
                    return stores.FirstOrDefault(s => s.Value == x);
                })?.Where(x => x != null)?.ToArray() ?? stores.ToArray(),
                Active = x.Active,
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
            return StatusCode(500, new DefaultResponse<IEnumerable<GetCampaignInfoResponse>>(ex));
        }
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        try
        {
            var campaign = await _campaignRepository.Get(id);
            if (campaign == null)
                throw new ArgumentException("Campaign not found");

            var client = campaign.ClientId.IsValidMongoID() ? await _clientRepository.Get(campaign.ClientId) : null;
            var stores = await _storeRepository.ListAsOptions();

            DefaultResponse<GetCampaignInfoResponse> r = new(new GetCampaignInfoResponse()
            {
                Id = campaign.Id,
                ClientId = campaign.ClientId,
                ClientName = client?.Name,
                Name = campaign.Name,
                CoinConversionFactor = campaign.CoinConversionFactor,
                AllowCardPayment = campaign.AllowCardPayment,
                AllowedCardPaymentPercentage = campaign.AllowedCardPaymentPercentage,
                AllowPointAmountSelection = campaign.AllowPointAmountSelection,
                CreationDate = campaign.CreationDate,
                CreationUserId = campaign.CreationUserId,
                DeletionDate = campaign.DeletionDate,
                DeletionUserId = campaign.DeletionUserId,
                Stores = campaign.Stores?.Select((x) =>
                {
                    return stores.FirstOrDefault(s => s.Value == x);
                })?.Where(x => x != null)?.ToArray() ?? stores.ToArray(),
                Active = campaign.Active
            });
            return Ok(r);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new DefaultResponse<GetCampaignInfoResponse>(ex));
        }
    }

    [HttpGet]
    [Route("/api/[controller]/CampaignName/{name}")]
    public async Task<IActionResult> GetByName([FromRoute] string name)
    {
        try
        {
            var campaign = await _campaignRepository.GetByName(name);
            if (campaign == null)
                throw new ArgumentException("Campaign not found");

            var client = campaign.ClientId.IsValidMongoID() ? await _clientRepository.Get(campaign.ClientId) : null;
            var stores = await _storeRepository.ListAsOptions();

            DefaultResponse<GetCampaignInfoResponse> r = new(new GetCampaignInfoResponse()
            {
                Id = campaign.Id,
                ClientId = campaign.ClientId,
                ClientName = client?.Name,
                Name = campaign.Name,
                CoinConversionFactor = campaign.CoinConversionFactor,
                AllowCardPayment = campaign.AllowCardPayment,
                AllowedCardPaymentPercentage = campaign.AllowedCardPaymentPercentage,
                AllowPointAmountSelection = campaign.AllowPointAmountSelection,
                CreationDate = campaign.CreationDate,
                CreationUserId = campaign.CreationUserId,
                DeletionDate = campaign.DeletionDate,
                DeletionUserId = campaign.DeletionUserId,
                Stores = campaign.Stores?.Select((x) =>
                {
                    return stores.FirstOrDefault(s => s.Value == x);
                })?.Where(x => x != null)?.ToArray() ?? stores.ToArray(),
                Active = campaign.Active
            });
            return Ok(r);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new DefaultResponse<GetCampaignInfoResponse>(ex));
        }
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> ChangeStatus([FromRoute] string id)
    {
        try
        {
            var user = (string?)Request.HttpContext.Items["UserId"];
            if (user == null)
                throw new ArgumentException("User not found");

            var campaign = await _campaignRepository.Get(id);
            if (campaign == null)
                throw new ArgumentException("Campaign not found");

            campaign.Active = !campaign.Active;
            campaign.DeletionDate = !campaign.Active ? DateTime.UtcNow.ToUnixTimestamp() : null;
            campaign.DeletionUserId = !campaign.Active ? user : null;
            await _campaignRepository.Update(campaign);

            DefaultResponse<bool> r = new(true);
            return Ok(r);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new DefaultResponse<bool>(ex));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new DefaultResponse<bool>(ex));
        }
    }

    [HttpPost]
    [Route("Create")]
    public async Task<IActionResult> Create([FromBody] CreateCampaignRequest request)
    {
        try
        {
            var user = (string?)Request.HttpContext.Items["UserId"];
            if (user == null)
                throw new ArgumentException("User not found");

            var client = request.ClientId.IsValidMongoID() ? await _clientRepository.Get(request.ClientId) : null;
            if (client == null)
                throw new ArgumentException("Client not found");

            var filterStores = request.Stores?.Where(x => x.IsValidMongoID());
            IEnumerable<Store>? storesDB = null;
            if (filterStores?.Any() == true)
            {
                storesDB = _storeRepository.Find(x => filterStores.Contains(x.Id));
            }
            if (request.Stores?.Any() == true && (request.Stores.Count() != (storesDB?.Count() ?? 0)))
                throw new ArgumentException("Some stores were not found");

            var campaign = new Campaign()
            {
                ClientId = request.ClientId,
                Name = request.Name,
                CoinConversionFactor = request.CoinConversionFactor,
                AllowCardPayment = request.AllowCardPayment,
                AllowedCardPaymentPercentage = request.AllowedCardPaymentPercentage,
                AllowPointAmountSelection = request.AllowPointAmountSelection,
                CampaignBanner = request.CampaignBanner,
                CampaignLogo = request.CampaignLogo,
                PrimaryColor = request.PrimaryColor,
                SecondaryColor = request.SecondaryColor,
                Active = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                CreationUserId = user,
                Stores = request.Stores
            };
            await _campaignRepository.Create(campaign);
            var stores = await _storeRepository.ListAsOptions();

            DefaultResponse<GetCampaignInfoResponse> r = new(new GetCampaignInfoResponse()
            {
                Id = campaign.Id,
                ClientId = campaign.ClientId,
                ClientName = client?.Name,
                Name = campaign.Name,
                CoinConversionFactor = campaign.CoinConversionFactor,
                AllowCardPayment = campaign.AllowCardPayment,
                AllowedCardPaymentPercentage = campaign.AllowedCardPaymentPercentage,
                AllowPointAmountSelection = campaign.AllowPointAmountSelection,
                CreationDate = campaign.CreationDate,
                CreationUserId = campaign.CreationUserId,
                DeletionDate = campaign.DeletionDate,
                DeletionUserId = campaign.DeletionUserId,
                Stores = campaign.Stores?.Select((x) =>
                {
                    return stores.FirstOrDefault(s => s.Value == x);
                })?.Where(x => x != null)?.ToArray(),
                Active = campaign.Active,
            });
            return Ok(r);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new DefaultResponse<GetCampaignInfoResponse>(ex));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new DefaultResponse<GetCampaignInfoResponse>(ex));
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateCampaignRequest request)
    {
        try
        {
            var campaign = await _campaignRepository.Get(request.Id);
            if (campaign == null)
                throw new ArgumentException("Campaign not found");

            var client = request.ClientId.IsValidMongoID() ? await _clientRepository.Get(request.ClientId) : null;
            if (client == null)
                throw new ArgumentException("Client not found");


            var filterStores = request.Stores?.Where(x => x.IsValidMongoID());
            IEnumerable<Store>? storesDB = null;
            if (filterStores?.Any() == true)
            {
                storesDB = _storeRepository.Find(x => filterStores.Contains(x.Id));
            }
            if (request.Stores?.Any() == true && (request.Stores.Count() != (storesDB?.Count() ?? 0)))
                throw new ArgumentException("Some stores were not found");

            campaign.ClientId = request.ClientId;
            campaign.Name = request.Name;
            campaign.CoinConversionFactor = request.CoinConversionFactor;
            campaign.AllowCardPayment = request.AllowCardPayment;
            campaign.AllowedCardPaymentPercentage = request.AllowedCardPaymentPercentage;
            campaign.AllowPointAmountSelection = request.AllowPointAmountSelection;
            campaign.Stores = request.Stores;

            await _campaignRepository.Update(campaign);
            var stores = await _storeRepository.ListAsOptions();

            DefaultResponse<GetCampaignInfoResponse> r = new(new GetCampaignInfoResponse()
            {
                Id = campaign.Id,
                ClientId = campaign.ClientId,
                ClientName = client?.Name,
                Name = campaign.Name,
                CoinConversionFactor = campaign.CoinConversionFactor,
                AllowCardPayment = campaign.AllowCardPayment,
                AllowedCardPaymentPercentage = campaign.AllowedCardPaymentPercentage,
                AllowPointAmountSelection = campaign.AllowPointAmountSelection,
                CreationDate = campaign.CreationDate,
                CreationUserId = campaign.CreationUserId,
                DeletionDate = campaign.DeletionDate,
                DeletionUserId = campaign.DeletionUserId,
                Stores = campaign.Stores?.Select((x) =>
                {
                    return stores.FirstOrDefault(s => s.Value == x);
                })?.Where(x => x != null)?.ToArray(),
                Active = campaign.Active,
            });
            return Ok(r);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new DefaultResponse<GetCampaignInfoResponse>(ex));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new DefaultResponse<GetCampaignInfoResponse>(ex));
        }
    }
    [HttpGet]
    [Route("options/{clientId}")]
    public async Task<IActionResult> GetOptions([FromRoute] string clientId)
    {
        try
        {
            var client = clientId.IsValidMongoID() ? await _clientRepository.Get(clientId) : null;
            if (client == null)
                throw new ArgumentException("Client not found");

            var r = new DefaultResponse<IEnumerable<SelectItem<string>>>(await _campaignRepository.ListAsOptions(clientId));
            return Ok(r);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new DefaultResponse<IEnumerable<SelectItem<string>>>(ex));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new DefaultResponse<IEnumerable<SelectItem<string>>>(ex));
        }
    }

}
