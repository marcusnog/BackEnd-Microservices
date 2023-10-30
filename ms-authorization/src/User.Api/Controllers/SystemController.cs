using User.Api.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;
using User.Api.Contracts.DTOs;
using Microsoft.AspNetCore.Authorization;
using Ms.Api.Utilities.Models;

namespace User.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SystemController : Controller
    {
        #region Fields

        private readonly ISystemRepository _repository;
        private readonly ILogger<SystemController> _logger;

        #endregion

        #region Constructor
        public SystemController(ISystemRepository repository, ILogger<SystemController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Methods

        [Authorize("AdminPolicy")]
        [HttpGet]
        [Route("options")]
        public async Task<IActionResult> List()
        {
            try
            {
                var r = new DefaultResponse<IEnumerable<SelectItem<string>>>(await _repository.ListAsOptions());
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
        /*
        [HttpGet]
        [Route("List")]
        public async Task<ActionResult<IEnumerable<User.Api.Contracts.DTOs.System>>> List()
        {
            var listSystem = await _repository.List();

            if (listSystem == null)
            {
                _logger.LogError("Nenhum retorno encontrado!");
                return new NotFoundResult();
            }

            return Ok(listSystem);
        }

        [HttpGet]
        [Route("GetSystemById")]
        public async Task<ActionResult<IEnumerable<Contracts.DTOs.System>>> GetSystemById(SystemFilter model)
        {
            var system = await _repository.Find((x) => x.Id == model.Id);

            if (system == null)
            {
                _logger.LogError("Nenhum sistema encontrado!");
                return new NotFoundResult();
            }

            return Ok(system);
        }

        [HttpPost]
        [Route("CreateSystem")]
        public async Task<ActionResult<Contracts.DTOs.System>> CreateSystem(Contracts.DTOs.System system)
        {
            await _repository.Create(system);

            return CreatedAtAction("GetSystemById", value: new SystemFilter() { Id = system.Id });
        }

        [HttpPut("UpdateSystem")]
        public async Task<IActionResult> UpdateSystem(Contracts.DTOs.System system)
        {
            await _repository.UpdateSystem(system);

            return new OkObjectResult(system);
        }

        [HttpDelete("DeleteSystem")]
        public async Task<IActionResult> DeleteSystem(SystemFilter system)
        {
            await _repository.Delete(system.Id);

            return new OkResult();
        }
*/
        #endregion
    }
}
