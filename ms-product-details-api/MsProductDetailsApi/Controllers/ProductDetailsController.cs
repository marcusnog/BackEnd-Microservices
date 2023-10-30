using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ms.Api.Utilities.Exceptions;
using Ms.Api.Utilities.Models;
using MsProductDetailsApi.Contracts.DTOs;
using MsProductDetailsApi.Contracts.Repositories;
using MsProductDetailsApi.Contracts.Services;
using MsProductDetailsApi.Extensions;

namespace MsProductDetailsApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductDetailsController : ControllerBase
    {
        private readonly ILogger<ProductDetailsController> _logger;
        readonly IProductRepository _productRepository;
        readonly INetShoesService _netShoesService;
        readonly IViaVarejoService _viaVarejoService;
        readonly IProductSkuPreDefinedPriceRepository _productSkuPreDefinedPriceRepository;

        public ProductDetailsController(IProductRepository productRepository, IProductSkuPreDefinedPriceRepository productSkuPreDefinedPriceRepository,
            ILogger<ProductDetailsController> logger, INetShoesService netShoesService, IViaVarejoService viaVarejoService)
        {
            _productRepository = productRepository;
            _logger = logger;

            _netShoesService = netShoesService ?? throw new ArgumentNullException(nameof(netShoesService));
            _viaVarejoService = viaVarejoService ?? throw new ArgumentNullException(nameof(viaVarejoService));
            _productSkuPreDefinedPriceRepository = productSkuPreDefinedPriceRepository ?? throw new ArgumentNullException(nameof(productSkuPreDefinedPriceRepository));
        }

        [HttpPost]
        [Route("GetProduct")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> GetProduct([FromBody] ProductFilter request)
        {
            try
            {
                var response = new DefaultResponse<Product<SkuPreDefinedPrice>>(await _productSkuPreDefinedPriceRepository.Find(request));

                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new DefaultResponse<List<SkuPreDefinedPrice>>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<List<SkuPreDefinedPrice>>(ex) { MessageCode = "FFFF" });
            }
        }

        [HttpGet]
        [Route("GetShippingValue")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> GetShippingValue([FromBody] CalcShippingFilter request)
        {
            try
            {
                DefaultResponse<CalcShippingResponse> response = new();

                CalcShipping oCalcShipping = new CalcShipping();
                oCalcShipping.FatorConversao = request.FatorConversao;
                oCalcShipping.IdUser = request.IdUser;
                oCalcShipping.CEP = request.CEP;

                oCalcShipping.Produtos = new();
                CalcShippingProduct productShipping = new CalcShippingProduct();
                productShipping.Codigo = request.IdProduct;
                productShipping.Quantidade = request.Quantity;
                productShipping.ValorUnitario = request.ValueInReals;
                oCalcShipping.Produtos.Add(productShipping);

                switch (request.StoreName)
                {
                    case "NetShoes":
                        response = await _netShoesService.GetShippingValue(oCalcShipping);
                        break;
                    case "Extra":
                    case "Ponto Frio":
                    case "PontoFrio":
                    case "Ponto":
                    case "CasasBahia":
                    case "Casas Bahia":
                        response = await _viaVarejoService.GetShippingValue(oCalcShipping);
                        break;
                    default:
                        break;
                }

                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new DefaultResponse<List<CalcShipping>>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new DefaultResponse<List<CalcShipping>>(ex) { MessageCode = "FFFF" });
            }
        }
    }
}
