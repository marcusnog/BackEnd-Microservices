using MongoDB.Driver;
using MsProductsSearch.Contracts.Repositories;
using MsProductsSearch.Contracts.Data;
using MsProductsSearch.Contracts.DTOs;

namespace MsProductsSearch.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ICatalogContext _context;

        public ProductRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //public async Task<List<Product<Sku>>> List(ProductFilter oProductFilter)
        //{
            //List<Product<Sku>> oResult = null;

            //if (oProductFilter.PageSize < 1)
            //{
            //    oProductFilter.PageSize = 1;
            //}

            //if (oProductFilter.Page < 1)
            //{
            //    oProductFilter.Page = 1;
            //}

            //if (oProductFilter.SkuId == null)
            //{
            //    oResult = await _context.Products.Find(p => p.Id == oProductFilter.IdProduct).ToListAsync();
            //}
            //else
            //{
//            var filter = Builders<Product<Sku>>.Filter.Eq("DisplayName", oProductFilter.ProductName);
//            //var filter = Builders<Product>.Filter.Regex("DisplayName", oProductFilter.ProductName); //Like
//            filter = filter & Builders<Product<Sku>>.Filter.Eq("Active", true);

//            if (!String.IsNullOrEmpty(oProductFilter.SkuCode))
//            {
//                filter = filter & Builders<Product<Sku>>.Filter.ElemMatch(x => x.Skus, oSku => oSku.StoreItemCode == oProductFilter.SkuCode);
//            }

//            if (oProductFilter.StartValue > 0 && oProductFilter.EndValue > 0)
//            {
//                filter = filter & Builders<Product<Sku>>.Filter.ElemMatch(x => x.Skus, oSku => oSku.SalePrice >= oProductFilter.StartValue && oSku.SalePrice <= oProductFilter.EndValue);
//            }

//            if (oProductFilter.CategoryList != null && oProductFilter.CategoryList.Count > 0)
//            {
//                filter = filter & Builders<Product>.Filter.In("CategoryId", oProductFilter.CategoryList);
//            }


//            if (oProductFilter.StoreList != null && oProductFilter.StoreList.Count > 0)
//            {
//                filter = filter & Builders<Product>.Filter.In("StoryId", oProductFilter.StoreList);
//            }

//            oResult = await _context.Products.Find(filter)
////                .Sort(x => x.ProductName)
//                .Skip((oProductFilter.Page - 1) * oProductFilter.PageSize)
//                .Limit(oProductFilter.PageSize)
//                .ToListAsync();

//                if (oResult != null)
//                {
//                    foreach (Product<Sku> oProduct in oResult)
//                    {
//                        List<Sku> listSku = new List<Sku>();

//                        if (oProduct.Skus != null)
//                        {
//                            foreach (Sku oSku in oProduct.Skus)
//                            {
//                                Boolean ConsiderSKU = true;

//                                if (!String.IsNullOrEmpty(oProductFilter.SkuCode))
//                                {
//                                    if (oSku.StoreItemCode == oProductFilter.SkuCode)
//                                    {
//                                        ConsiderSKU = true;
//                                    }
//                                    else
//                                    {
//                                        ConsiderSKU = false;
//                                    }
                                        
//                                }

//                                if (ConsiderSKU)
//                                {
//                                    listSku.Add(oSku);
//                                }

//                            }
//                        }

//                        oProduct.Skus = listSku;
//                    }
//                }
            //}

            //foreach (Product oProduct in oResult)
            //{
            //    if (oProduct.Skus != null)
            //    {
            //        foreach (Sku oSku in oProduct.Skus)
            //        {
            //            #region Calculation Point

            //            if (oProductFilter.FatorConversao != null && oSku.SalePrice > 0)
            //            {
            //                oSku.ValuePoints = oProductFilter.FatorConversao * oSku.SalePrice;
            //            }

            //            #endregion Calculation Point
            //        }
            //    }
            //}

            //return oResult;

       // }
    }
}
