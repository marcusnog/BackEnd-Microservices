using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ms.Api.Utilities.Models;
using MsPointsPurchaseApi.Contracts.DTOs;
using MsPointsPurchaseApi.Contracts.Repositories;
using System.Drawing;

namespace MsPurchasePointsApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointsController : Controller
    {
        #region Fields

        readonly IConfiguration _configuration;
        readonly IPointsRepository _repository;
        readonly ILogger<PointsController> _logger;

        #endregion

        #region Constructor
        public PointsController(IPointsRepository repository, IConfiguration configuration, ILogger<PointsController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        #endregion

        [HttpGet]
        [Route("GetById/{id}")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            try
            {
                var points = new DefaultResponse<Points>(await _repository.Get(id));

                decimal percentage = (points.Data.MinPointsValue * (decimal)points.Data.Fee);
                points.Data.FeeValue = Convert.ToDouble(percentage);
                points.Data.Price = (points.Data.MinPointsValue + percentage);

                points.Data.Price = (points.Data.MinPointsValue + percentage);

                if (points == null)
                    throw new ArgumentException("Points not found");

                return Ok(points);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<Points>(ex));
            }
        }

        [HttpGet]
        [Route("GetByValue/{value}")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> GetByValue([FromRoute] decimal value)
        {
            try
            {
                var points = new DefaultResponse<Points>(await _repository.GetByValue(value));

                decimal percentage = (value * (decimal)points.Data.Fee);
                points.Data.FeeValue = Convert.ToDouble(percentage);
                points.Data.Price = (value + percentage); 

                if (points == null)
                    throw new ArgumentException("Points not found");

                return Ok(points);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<Points>(ex));
            }
        }

        [HttpGet]
        [Route("GetList")]
        [Authorize("AdminPolicy")]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var points = new DefaultResponse<IEnumerable<Points>>(await _repository.List());

                foreach (var point in points.Data)
                {
                    decimal percentage = (point.MinPointsValue * (decimal)point.Fee);
                    point.FeeValue = Convert.ToDouble(percentage);
                    point.Price = (point.MinPointsValue + percentage);
                }

                if (points.Data == null)
                    throw new ArgumentException("Points not found");

                return Ok(points);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<Points>(ex));
            }
        }
    }
}
