using User.Api.Contracts.DTOs;
using User.Api.Contracts.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Ms.Api.Utilities.Models;

namespace User.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfileController : Controller
    {
        #region Fields

        private readonly IProfileRepository _repository;
        private readonly ILogger<ProfileController> _logger;

        #endregion

        #region Constructor
        public ProfileController(IProfileRepository repository, ILogger<ProfileController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        #region Methods


        [Authorize("AdminPolicy")]
        [HttpGet]
        [Route("options/{systemId}")]
        public async Task<IActionResult> List(string systemId)
        {
            try
            {
                var r = new DefaultResponse<IEnumerable<SelectItem<string>>>(await _repository.ListAsOptions(systemId));
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
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<Profile>>> GetProfileById([FromRoute] string id)
        {
            var profile = await _repository.Get(id);

            if (profile == null)
            {
                _logger.LogError("Nenhum perfil encontrado!");
                return new NotFoundResult();
            }

            return Ok(profile);
        }

        [HttpPost]
        public async Task<ActionResult<Contracts.DTOs.User>> CreateProfile(Profile profile)
        {
            await _repository.Create(profile);

            return CreatedAtRoute("GetProfileById", new { id = profile.Id }, profile);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProfile(Profile profile)
        {
            await _repository.UpdateProfile(profile);

            return new OkObjectResult(profile);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProfile([FromRoute] string id)
        {
            await _repository.Delete(id);

            return new OkResult();
        }
*/
        #endregion
    }
}
