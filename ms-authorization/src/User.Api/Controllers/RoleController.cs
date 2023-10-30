using User.Api.Contracts.DTOs;
using User.Api.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace User.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize("ClientPolicy")]
    public class RoleController : Controller
    {
        #region Fields

        private readonly IRoleRepository _repository;
        private readonly ILogger<RoleController> _logger;

        #endregion

        #region Constructor
        public RoleController(IRoleRepository repository, ILogger<RoleController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Methods

        [HttpGet]
        [Route("List")]
        public async Task<ActionResult<IEnumerable<Role>>> List()
        {
            var listRole = await _repository.ListRoles();

            if (listRole == null)
            {
                _logger.LogError("Nenhum retorno encontrado!");
                return new NotFoundResult();
            }

            return Ok(listRole);
        }

        [HttpGet]
        [Route("GetRoleById")]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoleById([FromBody] RoleFilter model)
        {
            var role = await _repository.GetRole(model);

            if (role == null)
            {
                _logger.LogError("Nenhuma funcionalidade encontrada!");
                return new NotFoundResult();
            }

            return Ok(role);
        }

        [HttpPost]
        [Route("CreateRole")]
        public async Task<ActionResult<Role>> CreateRole([FromBody] Role role)
        {
            await _repository.CreateRole(role);

            return CreatedAtRoute("GetRoleById", new { id = role.Id }, role);
        }

        [HttpPut("UpdateRole")]
        public async Task<IActionResult> UpdateRole(Role role)
        {
            await _repository.UpdateRole(role);

            return new OkObjectResult(role);
        }

        [HttpDelete("DeleteRole")]
        public async Task<IActionResult> DeleteSystem(RoleFilter role)
        {
            await _repository.DeleteRole(role);

            return new OkResult();
        }

        #endregion
    }
}
