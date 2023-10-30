using Catalog.Api.Contracts.DTOs;
using Catalog.Api.Contracts.DTOs.Response;
using Catalog.Api.Contracts.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Ms.Api.Utilities.Models;

namespace Catalog.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {

        private readonly ILogger<ProductController> _logger;
        readonly IProductSkuRepository _productRepository;
        readonly IProductSkuBillRepository _productSkuBillRepository;
        readonly IProductSkuPreDefinedPriceRepository _productSkuPreDefinedPriceRepository;
        readonly IProductSkuRangePriceRepository _productSkuRangePriceRepository;

        public ProductController(IProductSkuRepository productRepository,
            IProductSkuBillRepository productSkuBillRepository,
            IProductSkuPreDefinedPriceRepository productSkuPreDefinedPriceRepository,
            IProductSkuRangePriceRepository productSkuRangePriceRepository,
            ILogger<ProductController> logger)
        {
            _productRepository = productRepository;
            _productSkuPreDefinedPriceRepository = productSkuPreDefinedPriceRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("hash/{storeId}/{typeProductSku}")]
        public async Task<IActionResult> GetProductsHash([FromRoute] string storeId, [FromRoute] string typeProductSku)
        {
            IEnumerable<Product<SkuPreDefinedPrice>> prodSkuPreDefinedPrice = null;
            IEnumerable<Product<SkuRangePrice>> prodSkuRangePrice = null;
            IEnumerable<Product<SkuBill>> prodSkuBill = null;
            Dictionary<string, GetProductHashResponse>? products = null;

            switch (typeProductSku)
            {
                case "ProductSkuPreDefinedPrice":
                    prodSkuPreDefinedPrice = await _productSkuPreDefinedPriceRepository.Find( new { StoreId = storeId });

                    products = prodSkuPreDefinedPrice?.Select(p => new GetProductHashResponse()
                    {
                        Id = p.Id,
                        Code = p.StoreItemCode,
                        Hash = p.Hash,
                        Skus = p.Skus?.Select(s => new GetProductHashSku()
                        {
                            Id = s.Id,
                            Code = s.StoreItemCode,
                            Hash = s.Hash
                        })?.ToDictionary(x => x.Code)
                    })?.ToDictionary(p => p.Code);

                    break;
                case "ProductSkuRangePrice":
                    prodSkuRangePrice = await _productSkuRangePriceRepository.Find(new { StoreId = storeId });

                    products = prodSkuRangePrice?.Select(p => new GetProductHashResponse()
                    {
                        Id = p.Id,
                        Code = p.StoreItemCode,
                        Hash = p.Hash,
                        Skus = p.Skus?.Select(s => new GetProductHashSku()
                        {
                            Id = s.Id,
                            Code = s.StoreItemCode,
                            Hash = s.Hash
                        })?.ToDictionary(x => x.Code)
                    })?.ToDictionary(p => p.Code);

                    break;
                case "ProductSkuBill":
                    prodSkuBill = await _productSkuBillRepository.Find(new { StoreId = storeId });

                    products = prodSkuBill?.Select(p => new GetProductHashResponse()
                    {
                        Id = p.Id,
                        Code = p.StoreItemCode,
                        Hash = p.Hash,
                        Skus = p.Skus?.Select(s => new GetProductHashSku()
                        {
                            Id = s.Id,
                            Code = s.StoreItemCode,
                            Hash = s.Hash
                        })?.ToDictionary(x => x.Code)
                    })?.ToDictionary(p => p.Code);

                    break;
                default:
                    break;
            }

            return Ok(products);
        }

        [HttpPost]
        [Route("GetProductsGifttyHash")]
        public async Task<IActionResult> GetProductsGifttyHash([FromBody] List<Store> stores)
        {
            Dictionary<string, GetProductHashResponse>? products = null;

            try
            {
                var prodSkuPreDefinedPrice = await _productSkuPreDefinedPriceRepository.FindGifttyProducts(stores);

                products = prodSkuPreDefinedPrice?.Select(p => new GetProductHashResponse()
                {
                    Id = p.Id,
                    Code = p.StoreItemCode,
                    Hash = p.Hash,
                    Skus = p.Skus?.Select(s => new GetProductHashSku()
                    {
                        Id = s.Id,
                        Code = s.StoreItemCode,
                        Hash = s.Hash
                    })?.ToDictionary(x => x.Code)
                })?.ToDictionary(p => p.Code);


                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("bestsellers")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> SelectBestSellers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            DefaultResponse<IEnumerable<GetProductShowCase>> response;
            try
            {
                response = new(await _productSkuPreDefinedPriceRepository.ListBestSellers(page, pageSize));
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new(ex);
                return BadRequest(response);
            }
        }
       
        [HttpGet]
        [Route("sales")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> SelectOnSales([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            DefaultResponse<IEnumerable<GetProductShowCase>> response;
            try
            {
                response = new(await _productSkuPreDefinedPriceRepository.ListOnSale(page, pageSize));
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new(ex);
                return BadRequest(response);
            }
        }
        
        [HttpPost]
        [Route("within-price")]
        [Authorize("ProfilePolicy")]
        public async Task<IActionResult> SelectWithinPrice([FromBody] SkuFilterPrice model, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            DefaultResponse<IEnumerable<GetProductShowCase>> response;
            try
            {
                response = new(await _productSkuPreDefinedPriceRepository.ListProductsWithinPrice(model, page, pageSize));
                return Ok(response);
            }
            catch (Exception ex)
            {
                response = new(ex);
                return BadRequest(response);
            }
        }

    }
}