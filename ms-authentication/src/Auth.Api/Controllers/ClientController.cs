using Auth.Api.Contracts.DTOs;
using Auth.Api.Contracts.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Auth.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("ClientPolicy")]
    public class ClientController : ControllerBase
    {
        #region Fields

        private readonly IIdentityRepository _repository;
        private readonly ILogger<ClientController> _logger;

        #endregion

        #region Constructor
        public ClientController(IIdentityRepository repository, ILogger<ClientController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<IdentityClient>>> Get([FromRoute] string id)
        {
            return Ok(await _repository.Get(id));
        }


        [HttpPost]
        public async Task<IActionResult> Create(IdentityClient client)
        {
            await _repository.Create(client);

            return Ok();
        }

        [HttpPut]
        public async Task<IActionResult> Update(IdentityClient client)
        {
            await _repository.Update(client);

            return new OkObjectResult(client);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteSystem([FromRoute] string id)
        {
            await _repository.Delete(id);

            return new OkResult();
        }

        #endregion
    }
}
