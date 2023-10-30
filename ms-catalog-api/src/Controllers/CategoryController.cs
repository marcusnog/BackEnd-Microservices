using Catalog.Api.Contracts.DTOs.Response;
using Catalog.Api.Contracts.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ms.Api.Utilities.Models;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoryController : ControllerBase
    {
        private readonly ILogger<CategoryController> _logger;
        readonly ICategoryRepository _categoryRepository;
        readonly IMainCategoryRepository _mainCategoryRepository;
        public CategoryController(ICategoryRepository categoryRepository, IMainCategoryRepository mainCategoryRepository, ILogger<CategoryController> logger)
        {
            _categoryRepository = categoryRepository;
            _mainCategoryRepository = mainCategoryRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryRepository.List();
            return Ok(categories?.Select(x => GetCategoriesResponse.Get(x)));
        }

        [HttpGet]
        [Route("options")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> List()
        {
            try
            {
                var r = new DefaultResponse<IEnumerable<SelectItem<string>>>(await _categoryRepository.ListAsOptions());
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
        [Route("MainCategories")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> ListMainCategories()
        {
            try
            {
                var r = new DefaultResponse<IEnumerable<SelectItem<string>>>(await _mainCategoryRepository.ListAsOptions());
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
}