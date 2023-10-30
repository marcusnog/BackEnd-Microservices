using Catalog.Api.Contracts.Data;
using Catalog.Api.Contracts.DTOs;
using Catalog.Api.Contracts.DTOs.Response;
using Catalog.Api.Contracts.Repositories;
using Catalog.Api.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDB.Entities;
using System.Linq.Expressions;

namespace Catalog.Api.Repositories
{
    public class ProductSkuPreDefinedPriceRepository : IProductSkuPreDefinedPriceRepository
    {
        private readonly ICatalogContext _context;

        public ProductSkuPreDefinedPriceRepository(ICatalogContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Product<SkuPreDefinedPrice>>> List()
        {
            return await _context
                            .ProductsPredefinedPrice
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<Product<SkuPreDefinedPrice>> Get(string id)
        {
            return await _context
                           .ProductsPredefinedPrice
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Product<SkuPreDefinedPrice> Product)
        {
            await _context.ProductsPredefinedPrice.InsertOneAsync(Product);
        }

        public async Task<bool> Update(Product<SkuPreDefinedPrice> Product)
        {
            var updateResult = await _context
                                        .ProductsPredefinedPrice
                                        .ReplaceOneAsync(filter: g => g.Id == Product.Id, replacement: Product);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string id)
        {
            FilterDefinition<Product<SkuPreDefinedPrice>> filter = Builders<Product<SkuPreDefinedPrice>>.Filter.Eq(p => p.Id, id);

            DeleteResult deleteResult = await _context
                                                .ProductsPredefinedPrice
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }
        public async Task<IEnumerable<Product<SkuPreDefinedPrice>>> Find(object filter)
        {
            var filterDefinition = MongoExtension.GenerateFilter<Product<SkuPreDefinedPrice>>(filter);
            return await _context.ProductsPredefinedPrice.Find(filterDefinition).ToListAsync();
        }

        public async Task<IEnumerable<GetProductShowCase>> ListBestSellers(int page = 1, int pageSize = 10)
        {

            return (await _context.ProductsPredefinedPrice.AsQueryable().Skip((page - 1) * pageSize).Sample(pageSize).ToListAsync()).Select((x) =>
            new GetProductShowCase
            {
                ProductId = x.Ids,
                Available = x.Skus.FirstOrDefault().Availability,
                Id = x.Skus.FirstOrDefault().Id,
                DisplayName = x.DisplayName,
                ListPrice = Math.Ceiling(x.Skus.FirstOrDefault().ListPrice),
                SalePrice = Math.Ceiling(x.Skus.FirstOrDefault().SalePrice),
                StoreId = x.StoreId,
                Image = x.Images.FirstOrDefault(x => x.Size == Contracts.Enums.ProductImageSize.DESKTOP_M).Url
            });
        }
        public async Task<IEnumerable<GetProductShowCase>> ListOnSale(int page = 1, int pageSize = 10)
        {
            var filter = Builders<Product<SkuPreDefinedPrice>>.Filter
                .ElemMatch(z => z.Skus, a => a.SalePrice > 0);

            return (await _context.ProductsPredefinedPrice.Aggregate()
                .Match(filter)
                .ToListAsync()).Select((x) =>
                    new GetProductShowCase
                    {
                        ProductId = x.Id,
                        Available = x.Skus.FirstOrDefault().Availability,
                        Id = x.Skus.FirstOrDefault().Id,
                        DisplayName = x.DisplayName,
                        ListPrice = Math.Ceiling(x.Skus.FirstOrDefault().ListPrice),
                        SalePrice = Math.Ceiling(x.Skus.FirstOrDefault().SalePrice),
                        StoreId = x.StoreId,
                        Image = x.Images.FirstOrDefault(x => x.Size == Contracts.Enums.ProductImageSize.DESKTOP_M).Url
                    });

        }
        public async Task<IEnumerable<GetProductShowCase>> ListProductsWithinPrice(SkuFilterPrice model, int page = 1, int pageSize = 10)
        {

            var filter = Builders<Product<SkuPreDefinedPrice>>.Filter
                .ElemMatch(z => z.Skus, Builders<SkuPreDefinedPrice>.Filter.Lte(y => y.SalePrice, model.Price));


            return (await _context.ProductsPredefinedPrice.Aggregate(new AggregateOptions { BatchSize = pageSize })
                   .Match(filter)
                   .Skip((page - 1) * pageSize)
                   .ToListAsync()).Select((x) =>
                   new GetProductShowCase
                   {
                       ProductId = x.Id,
                       Available = x.Skus.FirstOrDefault().Availability,
                       Id = x.Skus.FirstOrDefault().Id,
                       DisplayName = x.DisplayName,
                       ListPrice = Math.Ceiling(x.Skus.FirstOrDefault().ListPrice),
                       SalePrice = Math.Ceiling(x.Skus.FirstOrDefault().SalePrice),
                       StoreId = x.StoreId,
                       Image = x.Images.FirstOrDefault(x => x.Size == Contracts.Enums.ProductImageSize.DESKTOP_M).Url
                   });
        }

        public async Task<IEnumerable<Product<SkuPreDefinedPrice>>> FindGifttyProducts(List<Store> stores)
        {
            List<Product<SkuPreDefinedPrice>> lstProducts = await _context.ProductsPredefinedPrice.AsQueryable().Where(x => x.Active).ToListAsync();

            //var query = lstProducts.AsQueryable()
            //            .Join(stores.AsQueryable(), p => p.StoreId, s => s.Id, 
            //            (x, y) => new { products = x, stores = y })
            //            .Select(
            //            (x, y) => new GetProductShowCase
            //            {
            //                ProductId = x.products.Id,
            //                Available = x.products.Skus.FirstOrDefault().Availability,
            //                Id = x.products.Skus.FirstOrDefault().Id,
            //                DisplayName = x.products.DisplayName,
            //                ListPrice = Math.Ceiling(x.products.Skus.FirstOrDefault().ListPrice),
            //                SalePrice = Math.Ceiling(x.products.Skus.FirstOrDefault().SalePrice),
            //                StoreId = x.products.StoreId,
            //                Image = x.products.Images.FirstOrDefault(x => x.Size == Contracts.Enums.ProductImageSize.DESKTOP_M).Url
            //            });

            var query = lstProducts.AsQueryable()
                        .Join(stores.AsQueryable(), p => p.StoreId, s => s.Id,
                        (x, y) => new { products = x, stores = y })
                        .Select(
                        (x, y) => new Product<SkuPreDefinedPrice>
                        {
                            Active = x.products.Active,
                            CategoryId = x.products.CategoryId,
                            CreationDate = x.products.CreationDate, 
                            DeletionDate = x.products.DeletionDate,
                            Description = x.products.Description,
                            DisplayName = x.products.DisplayName,
                            Hash = x.products.Hash,
                            Id = x.products.Id,
                            Images = x.products.Images,
                            ModificationDate = x.products.ModificationDate,
                            Skus = x.products.Skus,
                            StoreItemCode = x.products.StoreItemCode,
                            StoreId = x.products.StoreId,
                        });

            var products = query.ToList();
            return products;

            //var lstProducts = _context.ProductsPredefinedPrice.AsQueryable().Where(x => x.Active);

            //var query = from p in lstProducts
            //            join s in stores.AsQueryable()
            //            on p.StoreId equals s.Id into gifttyProducts
            //            select new GetProductShowCase
            //            {
            //                ProductId = p.Id,
            //                Available = p.Skus.FirstOrDefault().Availability,
            //                Id = p.Skus.FirstOrDefault().Id,
            //                DisplayName = p.DisplayName,
            //                ListPrice = Math.Ceiling(p.Skus.FirstOrDefault().ListPrice),
            //                SalePrice = Math.Ceiling(p.Skus.FirstOrDefault().SalePrice),
            //                StoreId = p.StoreId,
            //                Image = p.Images.FirstOrDefault(x => x.Size == Contracts.Enums.ProductImageSize.DESKTOP_M).Url
            //            };

            //var products = query.ToList();
            //return products;

        }
    }
}
