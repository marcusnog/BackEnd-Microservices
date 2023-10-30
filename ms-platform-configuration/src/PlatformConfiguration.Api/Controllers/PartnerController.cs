using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ms.Api.Utilities.Extensions;
using Ms.Api.Utilities.Models;
using PlatformConfiguration.Api.Contracts.DTOs;
using PlatformConfiguration.Api.Contracts.DTOs.Request;
using PlatformConfiguration.Api.Contracts.Repositories;
using PlatformConfiguration.Api.Contracts.UseCases;

namespace PlatformConfiguration.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize("ProfilePolicy")]
public class PartnerController : ControllerBase
{
    private readonly ILogger<PartnerController> _logger;
    readonly IPartnerRepository _partnerRepository;
    readonly IGetPartnerConfigurationUseCase _getPartnerConfigurationUseCase;
    public PartnerController(ILogger<PartnerController> logger, IPartnerRepository partnerRepository, IGetPartnerConfigurationUseCase getPartnerConfigurationUseCase)
    {
        _getPartnerConfigurationUseCase =  getPartnerConfigurationUseCase ?? throw new ArgumentException(nameof(getPartnerConfigurationUseCase));
        _partnerRepository = partnerRepository ?? throw new ArgumentException(nameof(partnerRepository));
        _logger = logger ?? throw new ArgumentException(nameof(logger));
    }


    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int? page = null, [FromQuery] int? limit = null, [FromQuery] bool? status = null, [FromQuery] string? q = null)
    {
        try
        {
            page ??= 1;
            limit ??= 10;
            var data = await _partnerRepository.List(page.Value, limit.Value, status, q);
            var r = new PaginatedResponse<IEnumerable<Partner>>(data.Items)
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
            return StatusCode(500, new DefaultResponse<IEnumerable<Partner>>(ex));
        }
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        try
        {
            var partner = await _partnerRepository.Get(id);
            if (partner == null)
                throw new ArgumentException("Partner not found");

            DefaultResponse<Partner> r = new(partner);
            return Ok(r);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new DefaultResponse<IEnumerable<Partner>>(ex));
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

            var partner = await _partnerRepository.Get(id);
            if (partner == null)
                throw new ArgumentException("Partner not found");
            
            partner.Active = !partner.Active;
            partner.DeletionDate = !partner.Active ? DateTime.UtcNow.ToUnixTimestamp() : null;
            partner.DeletionUserId = !partner.Active ? user : null;
            await _partnerRepository.Update(partner);

            DefaultResponse<bool> r = new();
            return Ok(r);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new DefaultResponse<Partner>(ex));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new DefaultResponse<IEnumerable<Partner>>(ex));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePartnerRequest request)
    {
        try
        {
            var user = (string?)Request.HttpContext.Items["UserId"];
            if (user == null)
                throw new ArgumentException("User not found");

            var partner = new Partner()
            {
                Name = request.Name,
                AcceptCardPayment = request.AcceptCardPayment,
                Active = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                CreationUserId = user
            };
            await _partnerRepository.Create(partner);

            DefaultResponse<Partner> r = new(partner);
            return Ok(r);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new DefaultResponse<Partner>(ex));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new DefaultResponse<IEnumerable<Partner>>(ex));
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdatePartnerRequest request)
    {
        try
        {
            var partner = await _partnerRepository.Get(request.Id);
            if (partner == null)
                throw new ArgumentException("Partner not found");

            partner.Name = request.Name;
            partner.AcceptCardPayment = request.AcceptCardPayment;

            await _partnerRepository.Update(partner);

            DefaultResponse<Partner> r = new(partner);
            return Ok(r);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new DefaultResponse<Partner>(ex));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new DefaultResponse<IEnumerable<Partner>>(ex));
        }
    }

    [HttpGet]
    [Route("integration-config/{partnerId}/{storeId}")]
    public async Task<IActionResult> Get([FromRoute] string partnerId, [FromRoute] string storeId)
    {
        var result = await _getPartnerConfigurationUseCase.Get(partnerId, storeId);

        return Ok(result);

    }
}
