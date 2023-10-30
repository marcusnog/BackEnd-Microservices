using Microsoft.Extensions.Caching.Distributed;
using MongoDB.Driver;
using Ms.Api.Utilities.Contracts.DTOs;
using Ms.Api.Utilities.Extensions;
using MsOrderApi.Contracts.Data;
using MsOrderApi.Contracts.DTOs.Request;
using MsOrderApi.Contracts.Repository;
using Newtonsoft.Json;
using System.Text;


namespace MsOrderApi.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IDistributedCache _redisCache;
        private readonly ICatalogContext _context;

        //public OrderRepository(IDistributedCache redisCache)
        //{
        //    _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        //}

        //public OrderRepository(ICatalogContext context)
        //{
        //    _context = context ?? throw new ArgumentNullException(nameof(context));
        //}

        public OrderRepository(IDistributedCache redisCache, ICatalogContext context)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        //public async Task<List<StoreOrder>> InsertItemOrder(OrderRequest request)
        //{
        //    //List<ProductOrder> lstProductsOrder = new();
        //    List<StoreOrder> lstStoreProductsOrder = new();
        //    string serializedlstOrdersRedis;
        //    var cacheKey = request.IdUser;
        //    string cacheKeyProductOrder = JsonConvert.SerializeObject(new ProductOrder
        //    {
        //        IdProduct = request?.Product?.IdProduct,
        //        NameProduct = request.Product.NameProduct,
        //        StoreName = request.Product.StoreName,
        //    }).Sha256();


        //    //Primeiro verificar se existe o registro no redis de acordo com a chave 
        //    //Loop/Verificar se o IdOrderOrder já existe, senão existir adicionar a lista e depois envia pro redis

        //    var redisList = await _redisCache.GetAsync(cacheKey);

        //    if (redisList != null)
        //    {
        //        serializedlstOrdersRedis = Encoding.UTF8.GetString(redisList);
        //        //lstProductsOrder = JsonConvert.DeserializeObject<List<ProductOrder>>(serializedlstOrdersRedis);
        //        lstStoreProductsOrder = JsonConvert.DeserializeObject<List<StoreOrder>>(serializedlstOrdersRedis);

        //        //var updateItemOrder = lstProductsOrder.Where(x => x.IdProductOrder == cacheKeyProductOrder).ToList();
        //        var store = lstStoreProductsOrder.Where(x => x.StoreName == request.Product.StoreName).FirstOrDefault();

        //        if (store != null)
        //        {
        //            var updateItemStoreOrder = store.Products.Where(x => x.IdProductOrder == cacheKeyProductOrder).ToList();

        //            if (updateItemStoreOrder.Any())
        //            {
        //                foreach (var item in store.Products)
        //                {
        //                    if (item.IdProductOrder == cacheKeyProductOrder)
        //                    {
        //                        item.ImgProduct = request.Product.ImgProduct;
        //                        item.Quantity = request.Product.Quantity;
        //                        item.ValueInPoints = request.Product.ValueInPoints;
        //                        item.ValueInReals = request.Product.ValueInReals;
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                request.Product.IdProductOrder = cacheKeyProductOrder;
        //                store.Products.Add(request.Product);
        //            }
        //        }
        //        else
        //        {
        //            StoreOrder storeOrder = new();

        //            request.Product.IdProductOrder = cacheKeyProductOrder;
        //            storeOrder.StoreName = request.Product.StoreName;
        //            storeOrder.Products = new();
        //            storeOrder.Products.Add(request.Product);
        //            lstStoreProductsOrder.Add(storeOrder);

        //        }

        //        serializedlstOrdersRedis = JsonConvert.SerializeObject(lstStoreProductsOrder);
        //        redisList = Encoding.UTF8.GetBytes(serializedlstOrdersRedis);

        //        //await _redisCache.SetAsync(cacheKey, redisList,
        //        //    options: new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(2)));
        //        await _redisCache.SetAsync(cacheKey, redisList);
        //    }
        //    else
        //    {
        //        StoreOrder storeOrder = new();
        //        request.Product.IdProductOrder = cacheKeyProductOrder;
        //        storeOrder.StoreName = request.Product.StoreName;
        //        storeOrder.Products = new();
        //        storeOrder.Products.Add(request.Product);
        //        lstStoreProductsOrder.Add(storeOrder);

        //        serializedlstOrdersRedis = JsonConvert.SerializeObject(lstStoreProductsOrder);
        //        redisList = Encoding.UTF8.GetBytes(serializedlstOrdersRedis);

        //        //await _redisCache.SetAsync(cacheKey, redisList,
        //        //    options: new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(2)));
        //        await _redisCache.SetAsync(cacheKey, redisList);
        //    }

        //    return lstStoreProductsOrder;
        //}

        public async Task<List<StoreBasket>> GetBasket(BasketRequest request)
        {
            List<StoreBasket> lstStoreProductsOrder = new();
            string serializedlstOrdersRedis;
            var cacheKey = request.IdUser;

            var redisList = await _redisCache.GetAsync(cacheKey);

            if (redisList == null)
                throw new Exception("Não há produtos no carrinho");

            serializedlstOrdersRedis = Encoding.UTF8.GetString(redisList);
            lstStoreProductsOrder = JsonConvert.DeserializeObject<List<StoreBasket>>(serializedlstOrdersRedis);

            return lstStoreProductsOrder;
        }

        ////public async Task<ProductOrder> UpdateOrder(string cacheKey, ProductOrder request)
        ////{
        ////    await _redisCache.SetStringAsync(cacheKey, JsonConvert.SerializeObject(request), 
        ////        options: new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddMinutes(10)).SetSlidingExpiration(TimeSpan.FromMinutes(2)));
        ////}

        //public async Task<List<StoreOrder>> DeleteItemOrder(OrderRequest request)
        //{
        //    List<StoreOrder> lstStoreProductsOrder = new();
        //    string serializedlstOrdersRedis;
        //    var cacheKey = request.IdUser;

        //    var redisList = await _redisCache.GetAsync(cacheKey);

        //    if (redisList == null)
        //        throw new Exception("Não há produtos no carrinho");

        //    serializedlstOrdersRedis = Encoding.UTF8.GetString(redisList);
        //    lstStoreProductsOrder = JsonConvert.DeserializeObject<List<StoreOrder>>(serializedlstOrdersRedis);

        //    var store = lstStoreProductsOrder.Where(x => x.StoreName == request.Product.StoreName).FirstOrDefault();

        //    var removeItem = store.Products.Where(p => p.IdProductOrder == request.Product.IdProductOrder).FirstOrDefault();
        //    lstStoreProductsOrder.Where(x => x.StoreName == store.StoreName).Select(y => y.Products.Remove(removeItem)).ToList();

        //    serializedlstOrdersRedis = JsonConvert.SerializeObject(lstStoreProductsOrder);

        //    await _redisCache.SetStringAsync(cacheKey, serializedlstOrdersRedis);

        //    return lstStoreProductsOrder;
        //}

        //public async Task DeleteOrder(OrderRequest request)
        //{
        //    var cacheKey = request.IdUser;

        //    var redisList = await _redisCache.GetAsync(cacheKey);

        //    if (redisList == null)
        //        throw new Exception("Não há produtos no carrinho");

        //    await _redisCache.RemoveAsync(cacheKey);
        //}

        public async Task Create(Order oRecord)
        {
            oRecord.DateCreation = DateTime.UtcNow.ToUnixTimestamp();
            await _context.Orders.InsertOneAsync(oRecord);
        }

        public async Task<bool> Update(Order oRecord)
        {
            //oRecord. = DateTime.UtcNow.ToUnixTimestamp();

            var updateResult = await _context
                                        .Orders
                                        .ReplaceOneAsync(filter: g => g.Id == oRecord.Id, replacement: oRecord);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        //public async Task<List<Order>> List(OrderFilter oRecord)
        //{
        //    return await _context
        //        .Orders
        //        .Find(p => p.Active == true)
        //        .ToListAsync();
        //}

        //public async Task<List<Order>> Find(object filter)
        //{
        //    var filterDefinition = MongoExtension.GenerateFilter<Order>(filter);
        //    return await _context.Orders.Find(filterDefinition).ToListAsync();
        //}

        public async Task<List<Order>> List(OrderFilter oFilter)
        {
            List<Order> oResult = null;

            //if (oProductFilter.PageSize < 1)
            //{
            //    oProductFilter.PageSize = 1;
            //}

            //if (oProductFilter.Page < 1)
            //{
            //    oProductFilter.Page = 1;
            //}

            //var filter = Builders<Order>.Filter.Eq("DisplayName", oFilter.ProductName);
            //filter = filter & Builders<Order>.Filter.Eq("Active", true);

            var filter = Builders<Order>.Filter.Eq("Active", true);
            //var filter;
            //if (!String.IsNullOrEmpty(oProductFilter.SkuCode))
            //{
            //    filter = filter & Builders<Product>.Filter.ElemMatch(x => x.Skus, oSku => oSku.StoreItemCode == oProductFilter.SkuCode);
            //}

            if (!String.IsNullOrEmpty(oFilter.CPFCNPJ))
            {
                //filter = filter & Builders<OrderRecipient>.Filter.ElemMatch(x => x., oSku => oSku.cn == oFilter.CPFCNPJ);
                //filter = filter & Builders<OrderRecipient>.Filter.Equals("CPFCNPJ", oFilter.CPFCNPJ);
                //filter = filter & Builders<OrderRecipient>.Filter.AnyEq(x => x.CPFCNPJ, oFilter.CPFCNPJ);

                //filter = filter & Builders<OrderRecipient>.Filter.Eq(x => x.CPFCNPJ, "1");
                //filter &= (Builders<User>.Filter.Eq(x => x.B, "4") | Builders<User>.Filter.Eq(x => x.B, "5"));

                //filter = filter & Builders<OrderRecipient>.Filter.Eq("CPFCNPJ", oFilter.CPFCNPJ);
            }

            //if (oProductFilter.StartValue > 0 && oProductFilter.EndValue > 0)
            //{
            //    filter = filter & Builders<Product>.Filter.ElemMatch(x => x.Skus, oSku => oSku.SalePrice >= oProductFilter.StartValue && oSku.SalePrice <= oProductFilter.EndValue);
            //}

            //if (oProductFilter.CategoryList != null && oProductFilter.CategoryList.Count > 0)
            //{
            //    filter = filter & Builders<Product>.Filter.In("CategoryId", oProductFilter.CategoryList);
            //}


            //if (oProductFilter.StoreList != null && oProductFilter.StoreList.Count > 0)
            //{
            //    filter = filter & Builders<Product>.Filter.In("StoryId", oProductFilter.StoreList);
            //}

            oResult = await _context.Orders.Find(filter)
                //                .Sort(x => x.ProductName)
                //.Skip((oProductFilter.Page - 1) * oProductFilter.PageSize)
                //.Limit(oProductFilter.PageSize)
                .ToListAsync();

            //if (oResult != null)
            //{
            //    foreach (Order oOrder in oResult)
            //    {
            //        List<OrderRecipient> listOrderRecipient = new List<OrderRecipient>();

            //        if (oOrder.Recipient != null)
            //        {
            //            foreach (Sku oSku in oProduct.Skus)
            //            {
            //                Boolean ConsiderSKU = true;

            //                if (!String.IsNullOrEmpty(oProductFilter.SkuCode))
            //                {
            //                    if (oSku.StoreItemCode == oProductFilter.SkuCode)
            //                    {
            //                        ConsiderSKU = true;
            //                    }
            //                    else
            //                    {
            //                        ConsiderSKU = false;
            //                    }

            //                }

            //                if (ConsiderSKU)
            //                {
            //                    listSku.Add(oSku);
            //                }

            //            }
            //        }

            //        oProduct.Skus = listSku;
            //    }
            //}
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

            return oResult;

        }
    }
}
