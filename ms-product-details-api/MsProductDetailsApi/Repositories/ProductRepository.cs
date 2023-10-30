using MongoDB.Driver;
using MsProductDetailsApi.Contracts.Data;
using MsProductDetailsApi.Contracts.DTOs;
using MsProductDetailsApi.Contracts.Repositories;

namespace MsProductDetailsApi.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<List<Product<Sku>>> List(ProductFilter oProductFilter)
        {
            List<Product<Sku>> oResult = null;

            if (oProductFilter.SkuId == null)
            {
                oResult = await _context.Products.Find(p => p.Id == oProductFilter.IdProduct).ToListAsync();
            }
            else
            {
                var filter = Builders<Product<Sku>>.Filter.Eq("Id", oProductFilter.IdProduct);
                filter = filter & Builders<Product<Sku>>.Filter.ElemMatch(x => x.Skus, oSku => oSku.Id == oProductFilter.SkuId);
                oResult = await _context.Products.Find(filter).ToListAsync();

                if (oResult != null)
                {
                    foreach (Product<Sku> oProduct in oResult)
                    {
                        List<Sku> listSku = new List<Sku>();

                        if (oProduct.Skus != null)
                        {
                            foreach (Sku oSku in oProduct.Skus)
                            {
                                if (oSku.Id == oProductFilter.SkuId)
                                {
                                    listSku.Add(oSku);
                                }
                            }
                        }

                        oProduct.Skus = listSku;
                    }
                }
            }

            foreach (Product<Sku> oProduct in oResult)
            {
                if (oProduct.Skus != null)
                {
                    foreach (Sku oSku in oProduct.Skus)
                    {
                        #region Calculation Point

                        //if (oProductFilter.FatorConversao != null && oSku.SalePrice > 0)
                        //{
                        //    //oSku.ValuePoints = oProductFilter?.FatorConversao * oSku.SalePrice;
                        //}

                        #endregion Calculation Point
                    }
                }
            }

            return oResult;

        }
    }
}
