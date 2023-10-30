using Microsoft.AspNetCore.Mvc;
using Ms.Api.Utilities.Exceptions;
using Ms.Api.Utilities.Models;
using MsProductsSearch.Contracts.Repositories;
using MsProductsSearch.Contracts.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace MsProductsSearch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsSearchController : ControllerBase
    {
        private readonly ILogger<ProductsSearchController> _logger;
        readonly IProductRepository _productRepository;
        readonly IProductSkuPreDefinedPriceRepository _productSkuPreDefinedPriceRepository;

        public ProductsSearchController(IProductRepository productRepository, IProductSkuPreDefinedPriceRepository productSkuPreDefinedPriceRepository, ILogger<ProductsSearchController> logger)
        {
            _productRepository = productRepository;
            _productSkuPreDefinedPriceRepository = productSkuPreDefinedPriceRepository;
            _logger = logger;
        }

        [HttpPost]
        [Route("GetListProduct")]
        //[Authorize("ProfilePolicy")]
        public async Task<IActionResult> GetListProduct([FromBody] ProductFilter request, [FromQuery] int? page = 1, [FromQuery] int? pageSize = 10)
        {
            try
            {
                 
                var data = await _productSkuPreDefinedPriceRepository.List(request, page.Value, pageSize.Value); 
                
                List<StoresProducts> lstStores = new();
                foreach (var item in data.Items)
                {
                    var store = new StoresProducts();
                    if (lstStores.Count > 0)
                    {
                        if (!lstStores.Select(x => x.Name == item.StoreName).FirstOrDefault())
                        {
                            store.Id = item.StoreId;
                            store.Name = item.StoreName;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else
                    {
                        store.Id = item.StoreId;
                        store.Name = item.StoreName;
                    }
                    lstStores.Add(store);
                }

                var response = new PaginatedResponse<GetStoresProduct>()
                {
                    Data = new GetStoresProduct { lstProducts = data.Items, lstStores = lstStores },                
                    Metadata = new Metadata()
                    {
                        Page = page.Value,
                        PageSize = pageSize.Value,
                        TotalItems = data.TotalItems,
                        //lstStores = lstStores
                    },

            };
              
                return Ok(response);
            }
            catch (CodeException ex)
            {
                return BadRequest(new PaginatedResponse<IEnumerable<GetProductShowCase>>(ex) { MessageCode = ex.ErrorCode });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new PaginatedResponse<IEnumerable<GetProductShowCase>>(ex) { MessageCode = "FFFF" });
            }
        }

    }
}
