using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Ms.Api.Utilities.Models;
using MsProductsSearch.Contracts.Data;
using MsProductsSearch.Contracts.DTOs;
using MsProductsSearch.Contracts.Repositories;
using MsProductsSearch.Extensions;
using System.Text.RegularExpressions;

namespace MsProductsSearch.Repositories
{
    public class ProductSkuPreDefinedPriceRepository : BaseRepository, IProductSkuPreDefinedPriceRepository
    {
        private readonly ICatalogContext _context;
        private readonly IPlataformConfigurationContext _contextPlataformConfiguration;

        public ProductSkuPreDefinedPriceRepository(ICatalogContext context, IPlataformConfigurationContext contextPlataformConfiguration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _contextPlataformConfiguration = contextPlataformConfiguration ?? throw new ArgumentNullException(nameof(contextPlataformConfiguration));
        }

        public async Task<QueryPage<IEnumerable<GetProductShowCase>>> List(ProductFilter oProductFilter, int page = 0, int pageSize = 10)
        {
            var filter = Builders<Product<SkuPreDefinedPrice>>.Filter.Eq("Active", true);

            if (!string.IsNullOrEmpty(oProductFilter.ProductName))
            {
                var queryExpr = new BsonRegularExpression(new Regex(oProductFilter.ProductName, RegexOptions.IgnoreCase));
                filter = filter & Builders<Product<SkuPreDefinedPrice>>.Filter.Regex("DisplayName", queryExpr);
            }

            if (!String.IsNullOrEmpty(oProductFilter.SkuCode))
                filter = filter & Builders<Product<SkuPreDefinedPrice>>.Filter.Eq("Skus._id", ObjectId.Parse(oProductFilter.SkuCode));

            if (oProductFilter.StartValue > 0)
                filter = filter & Builders<Product<SkuPreDefinedPrice>>.Filter.Gt("Skus.SalePrice", oProductFilter.StartValue);

            if (oProductFilter.EndValue > 0)
                filter = filter & Builders<Product<SkuPreDefinedPrice>>.Filter.Lte("Skus.SalePrice", oProductFilter.EndValue);

            if (!string.IsNullOrEmpty(oProductFilter.Category))
            {
                var categories = await _context.Categories.Find(p => p.Id == oProductFilter.Category).FirstOrDefaultAsync();

                //if (categories is null)
                //{
                //    var filterChildrenCategories = Builders<Category>.Filter.Eq("Children._id", ObjectId.Parse(oProductFilter.Category));
                //    categories = await _context.Categories.Find(filterChildrenCategories).FirstOrDefaultAsync();

                //    if (categories is null)
                //    {
                //        var filterChildrenOdChildrenCategories = Builders<Category>.Filter.Eq("Children.Children._id", ObjectId.Parse(oProductFilter.Category));
                //        categories = await _context.Categories.Find(filterChildrenOdChildrenCategories).FirstOrDefaultAsync();
                //    }
                //}

                List<ObjectId> categoriesIdsFilter = new();
                categoriesIdsFilter.Add(ObjectId.Parse(oProductFilter.Category));

                if (categories is not null)
                {
                    if (categories.Children != null && categories.Children.Any())
                    {
                        foreach (var children in categories.Children)
                        {
                            categoriesIdsFilter.Add(ObjectId.Parse(children.Id));

                            if (children.Children != null && children.Children.Any())
                            {
                                foreach (var child in children.Children)
                                    categoriesIdsFilter.Add(ObjectId.Parse(child.Id));
                            }
                        }
                    }

                    filter = filter & Builders<Product<SkuPreDefinedPrice>>.Filter.In("CategoryId", categoriesIdsFilter);
                }
            }

            if (!string.IsNullOrEmpty(oProductFilter.StoreId))
                filter = filter & Builders<Product<SkuPreDefinedPrice>>.Filter.Eq("StoreId", ObjectId.Parse(oProductFilter.StoreId));

            var returnDB = await GetPagerResultAsync(page, pageSize, _context.ProductsPredefinedPrice, filter);

            var query = from product in returnDB.Items.AsQueryable()
                        join store in _contextPlataformConfiguration.Stores.AsQueryable()
                        on product.StoreId equals store.Id
                        select new Product<SkuPreDefinedPrice>()
                        {
                            StoreName = store.Name,
                            Id = product.Id,
                            DisplayName = product.DisplayName,
                            Description = product.Description,
                            CategoryId = product.CategoryId,
                            CreationDate = product.CreationDate,
                            ModificationDate = product.ModificationDate,
                            DeletionDate = product.DeletionDate,
                            Active = product.Active,
                            Images = product.Images,
                            StoreId = product.StoreId,
                            StoreItemCode = product.StoreItemCode,
                            Skus = product.Skus,
                            Hash = product.Hash
                        };

            return (new QueryPage<IEnumerable<GetProductShowCase>>()
            {
                Page = returnDB.Page,
                PageSize = returnDB.PageSize,
                TotalItems = returnDB.TotalItems,
                Items = query.ToList().Select((x) =>
                            new GetProductShowCase
                            {
                                ProductId = x.Id,
                                Available = x.Skus.FirstOrDefault().Availability,
                                Id = x.Skus.FirstOrDefault().Id,
                                DisplayName = x.DisplayName,
                                ListPrice = Math.Ceiling(x.Skus.FirstOrDefault().ListPrice),
                                SalePrice = Math.Ceiling(x.Skus.FirstOrDefault().SalePrice),
                                StoreId = x.StoreId,
                                Image = x.Images.FirstOrDefault(x => x.Size == Contracts.Enum.ProductImageSize.DESKTOP_M).Url,
                                StoreName = x.StoreName
                            })
            });


            //return (new QueryPage<IEnumerable<GetProductShowCase>>(){
            //    Page = returnDB.Page,
            //    PageSize = returnDB.PageSize,
            //    TotalItems = returnDB.TotalItems,
            //    Items = returnDB.Items.Select((x) =>
            //                new GetProductShowCase
            //                {
            //                    ProductId = x.Id,
            //                    Available = x.Skus.FirstOrDefault().Availability,
            //                    Id = x.Skus.FirstOrDefault().Id,
            //                    DisplayName = x.DisplayName,
            //                    ListPrice = x.Skus.FirstOrDefault().ListPrice,
            //                    SalePrice = x.Skus.FirstOrDefault().SalePrice,
            //                    StoreId = x.StoreId,
            //                    Image = x.Images.FirstOrDefault(x => x.Size == Contracts.Enum.ProductImageSize.DESKTOP_M).Url,
            //                    StoreName = oProductFilter.StoreName
            //                })
            //});

            //return (await _context.ProductsPredefinedPrice
            //                .Find(filter)
            //                .Skip((page - 1) * pageSize)
            //                .Limit(pageSize)
            //                .ToListAsync()).Select((x) =>
            //                new GetProductShowCase
            //                {
            //                    ProductId = x.Id,
            //                    Available = x.Skus.FirstOrDefault().Availability,
            //                    Id = x.Skus.FirstOrDefault().Id,
            //                    DisplayName = x.DisplayName,
            //                    ListPrice = x.Skus.FirstOrDefault().ListPrice,
            //                    SalePrice = x.Skus.FirstOrDefault().SalePrice,
            //                    StoreId = x.StoreId,
            //                    Image = x.Images.FirstOrDefault(x => x.Size == Contracts.Enum.ProductImageSize.DESKTOP_M).Url,
            //                    StoreName = oProductFilter.StoreName
            //                });
        }

        public async Task<Product<SkuPreDefinedPrice>> Get(string id)
        {
            return await _context
                           .ProductsPredefinedPrice
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Product<SkuPreDefinedPrice>>> Find(object filter)
        {
            var filterDefinition = MongoExtension.GenerateFilter<Product<SkuPreDefinedPrice>>(filter);
            return await _context.ProductsPredefinedPrice.Find(filterDefinition).ToListAsync();
        }
    }
}
