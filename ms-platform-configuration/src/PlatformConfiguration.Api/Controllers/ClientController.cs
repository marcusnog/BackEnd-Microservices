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
public class ClientController : ControllerBase
{
    private readonly ILogger<ClientController> _logger;
    readonly IClientRepository _clientRepository;
    public ClientController(ILogger<ClientController> logger, IClientRepository clientRepository)
    {
        _clientRepository = clientRepository ?? throw new ArgumentException(nameof(clientRepository));
        _logger = logger ?? throw new ArgumentException(nameof(logger));
    }

    [HttpGet]
    public async Task<IActionResult> List([FromQuery] int? page = null, [FromQuery] int? limit = null, [FromQuery] bool? status = null, [FromQuery] string? q = null)
    {
        try
        {
            page ??= 1;
            limit ??= 10;
            var data = await _clientRepository.List(page.Value, limit.Value, status, q);
            var clients = await _clientRepository.List();
            var r = new PaginatedResponse<IEnumerable<GetClientInfoResponse>>(data.Items.Select(client => new GetClientInfoResponse()
            {
                Id = client.Id,
                Name = client.Name,
                Documents = client.Documents,
                CreationDate = client.CreationDate,
                DeletionDate = client.DeletionDate,
                Active = client.Active,
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
            return StatusCode(500, new DefaultResponse<IEnumerable<GetClientInfoResponse>>(ex));
        }
    }

    [HttpGet]
    [Route("options")]
    public async Task<IActionResult> List()
    {
        try
        {
            var r = new DefaultResponse<IEnumerable<SelectItem<string>>>(await _clientRepository.ListAsOptions());
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

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] string id)
    {
        try
        {
            var client = await _clientRepository.Get(id);
            if (client == null)
                throw new ArgumentException("Client not found");


            DefaultResponse<GetClientInfoResponse> r = new(new GetClientInfoResponse()
            {
                Id = client.Id,
                Name = client.Name,
                Documents = client.Documents,
                CreationDate = client.CreationDate,
                DeletionDate = client.DeletionDate,
                Active = client.Active,
            });
            return Ok(r);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new DefaultResponse<GetClientInfoResponse>(ex));
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

            var client = await _clientRepository.Get(id);
            if (client == null)
                throw new ArgumentException("Client not found");

            client.Active = !client.Active;
            client.DeletionDate = !client.Active ? DateTime.UtcNow.ToUnixTimestamp() : null;
            await _clientRepository.Update(client);

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
    [AllowAnonymous]
    public async Task<IActionResult> Create([FromBody] CreateClientRequest request)
    {
        try
        {
            var client = new Client()
            {
                Name = request.Name,
                Documents = request.Documents,
                Active = true,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                //CreationUserId = user
            };
            await _clientRepository.Create(client);

            DefaultResponse<GetClientInfoResponse> r = new(new GetClientInfoResponse()
            {
                Id = client.Id,
                Name = client.Name,
                //Documents = client.Documents,
                CreationDate = client.CreationDate,
                DeletionDate = client.DeletionDate,
                Active = client.Active,
            });
            return Ok(r);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new DefaultResponse<GetClientInfoResponse>(ex));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new DefaultResponse<GetClientInfoResponse>(ex));
        }
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateClientRequest request)
    {
        try
        {
            var client = await _clientRepository.Get(request.Id);
            if (client == null)
                throw new ArgumentException("Client not found");           

            client.Name = request.Name;
            client.Documents = request.Documents;

            await _clientRepository.Update(client);

            DefaultResponse<GetClientInfoResponse> r = new(new GetClientInfoResponse()
            {
                Id = client.Id,
                Name = client.Name,
                Documents = client.Documents,
                CreationDate = client.CreationDate,
                DeletionDate = client.DeletionDate,
                Active = client.Active,
            });
            return Ok(r);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new DefaultResponse<GetClientInfoResponse>(ex));
        }
        catch (Exception ex)
        {
            return StatusCode(500, new DefaultResponse<GetClientInfoResponse>(ex));
        }
    }

}
