using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MsProductDetailsApi.Contracts.Data;
using MsProductDetailsApi.Contracts.DTOs;
using MsProductDetailsApi.Contracts.Repositories;
using MsProductDetailsApi.Extensions;

namespace MsProductDetailsApi.Repositories
{
    public class ProductSkuPreDefinedPriceRepository : IProductSkuPreDefinedPriceRepository
    {
        private readonly ICatalogContext _context;
        private readonly IPlataformConfigurationContext _contextPlataformConfiguration;

        public ProductSkuPreDefinedPriceRepository(ICatalogContext context, IPlataformConfigurationContext contextPlataformConfiguration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _contextPlataformConfiguration = contextPlataformConfiguration ?? throw new ArgumentNullException(nameof(contextPlataformConfiguration));
        }

        public async Task<Product<SkuPreDefinedPrice>> Find(ProductFilter oProductFilter)
        {
            var filter = Builders<Product<SkuPreDefinedPrice>>.Filter.Eq("Active", true);

            if (!String.IsNullOrEmpty(oProductFilter.IdProduct))
                filter = filter & Builders<Product<SkuPreDefinedPrice>>.Filter.Eq("_id", ObjectId.Parse(oProductFilter.IdProduct));

            var retorno = await _context.ProductsPredefinedPrice
                            .Find(filter)
                            .FirstAsync();

            retorno.StoreName = _contextPlataformConfiguration.Stores.Find(x => x.Id == retorno.StoreId).FirstOrDefault().Name;

            foreach (var item in retorno.Skus)
            {
                item.ListPrice = Math.Ceiling(ConverInPoints.ConvertInPonts(item.ListPrice, oProductFilter.FatorConversao.Value));
                item.SalePrice = Math.Ceiling(ConverInPoints.ConvertInPonts(item.SalePrice, oProductFilter.FatorConversao.Value));
            } 

            return retorno;
        }

        public async Task<Product<SkuPreDefinedPrice>> Get(string id)
        {
            return await _context
                           .ProductsPredefinedPrice
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        //public async Task<IEnumerable<Product<SkuPreDefinedPrice>>> Find(object filter)
        //{
        //    var filterDefinition = MongoExtension.GenerateFilter<Product<SkuPreDefinedPrice>>(filter);
        //    return await _context.ProductsPredefinedPrice.Find(filterDefinition).ToListAsync();
        //}
    }
}
