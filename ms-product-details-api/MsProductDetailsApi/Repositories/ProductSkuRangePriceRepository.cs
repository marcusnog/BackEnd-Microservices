using MongoDB.Driver;
using MsProductDetailsApi.Contracts.Data;
using MsProductDetailsApi.Contracts.DTOs;
using MsProductDetailsApi.Contracts.Repositories;
using MsProductDetailsApi.Extensions;

namespace MsProductDetailsApi.Repositories
{
    public class ProductSkuRangePriceRepository : IProductSkuRangePriceRepository
    {
        private readonly ICatalogContext _context;

        public ProductSkuRangePriceRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product<SkuRangePrice>>> List()
        {
            return await _context
                            .ProductsRangePrice
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Product<SkuRangePrice>> Get(string id)
        {
            return await _context
                           .ProductsRangePrice
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product<SkuRangePrice>>> Find(object filter)
        {
            var filterDefinition = MongoExtension.GenerateFilter<Product<SkuRangePrice>>(filter);
            return await _context.ProductsRangePrice.Find(filterDefinition).ToListAsync();
        }
    }
}
