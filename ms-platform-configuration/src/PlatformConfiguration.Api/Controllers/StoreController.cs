using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ms.Api.Utilities.Extensions;
using Ms.Api.Utilities.Models;
using PlatformConfiguration.Api.Contracts.DTOs;
using PlatformConfiguration.Api.Contracts.Repositories;

namespace PlatformConfiguration.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize("ProfilePolicy")]
    public class StoreController : Controller
    {
        private readonly ILogger<StoreController> _logger;
        readonly IStoreRepository _storeRepository;
        public StoreController(ILogger<StoreController> logger,
            IStoreRepository storeRepository)
        {
            _storeRepository = storeRepository ?? throw new ArgumentException(nameof(storeRepository));
            _logger = logger ?? throw new ArgumentException(nameof(logger));
        }

        [HttpGet]
        [Route("options")]
        public async Task<IActionResult> GetOptions()
        {
            try
            {
                var r = new DefaultResponse<IEnumerable<SelectItem<string>>>(await _storeRepository.ListAsOptions());
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
        [Route("GetStore")]
        public async Task<IActionResult> GetStore([FromQuery] string storeId)
        {
            try
            {
                var r = new DefaultResponse<Store>(await _storeRepository.Get(storeId));
                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<Store>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<Store>(ex));
            }
        }

        [HttpGet]
        [Route("ListStores")]
        public async Task<IActionResult> ListStores()
        {
            try
            {
                var r = new DefaultResponse<IEnumerable<Store>>(await _storeRepository.ListStoresGiftty());
                return Ok(r);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new DefaultResponse<IEnumerable<Store>>(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<IEnumerable<Store>>(ex));
            }
        }

        [HttpPost]
        [Route("InsertNewStore")]
        [AllowAnonymous]
        public async void InsertNewStore([FromBody] Store model)
        {
            try
            {
                 await _storeRepository.Create(model);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
