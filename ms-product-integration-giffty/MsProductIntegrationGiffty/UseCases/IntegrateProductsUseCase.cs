using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ms.Api.Utilities.Contracts.DTOs;
using Ms.Api.Utilities.Extensions;
using Ms.Api.Utilities.Models;
using MsProductIntegrationGiffty.Api.Contracts.DTOs;
using MsProductIntegrationGiffty.Contracts.DTOs;
using MsProductIntegrationGiffty.Contracts.Services;
using MsProductIntegrationGiffty.Contracts.UseCases;
using Newtonsoft.Json;
using Platform.Events.Core.Contracts.Enums;
using Platform.Events.Core.Contracts.Product;
using System.Diagnostics;
using System.Text;

namespace MsProductIntegrationGiffty.UseCases
{
    public class IntegrateProductsUseCase : IIntegrateProductsUseCase
    {
        readonly IConfiguration _configuration;
        readonly IIntegrationService _integrationService;
        readonly ISendMessageToQueueUseCase _sendMessageToQueueUseCase;
        public readonly string _urlProductsDb;
        public readonly string _urlCategoryDb;
        public readonly string _store;
        public readonly string _urlListStoreDb;
        public readonly string _typeProductSku;
        readonly ILogger<IntegrateProductsUseCase> _logger;

        public IntegrateProductsUseCase(IConfiguration configuration,
            ISendMessageToQueueUseCase sendMessageToQueueUseCase,
            IIntegrationService integrationService,
            ILogger<IntegrateProductsUseCase> logger)
        {
            _configuration = configuration;
            _sendMessageToQueueUseCase = sendMessageToQueueUseCase;
            _integrationService = integrationService;
            _logger = logger;

            _store = _configuration.GetValue<string>("StoreId");
            _typeProductSku = _configuration.GetValue<string>("TypeProductSku");
            _urlProductsDb = String.Format(_configuration.GetValue<string>("DatabaseProducts"), _store, _typeProductSku);
            _urlCategoryDb = _configuration.GetValue<string>("DatabaseCategory");
            _urlListStoreDb = _configuration.GetValue<string>("DatabaseListStores");
        }

        public async Task IntegrateProducts()
        {
            try
            {
                _logger.LogInformation($"Initializing Importation...{DateTime.Now}");

                Stopwatch s = new Stopwatch();
                s.Start();

                CreateProductSkuPreDefinedPriceEventV1Image[] images;
                string hash;
                ProductsDB product;
                ProductSkuDB sku;

                int importedProducts = 0;
                int updatedProducts = 0;
                int importedSkuProducts = 0;
                int updatedSkuProducts = 0;

                _logger.LogInformation("Getting store data from Database...");
                var stores = await GetStoresListDB();
                _logger.LogInformation($"Totals Stores in returning: {stores.Count()}");

                _logger.LogInformation("Getting products data from Database... ");
                var products = await GetProductsDB(stores);
                _logger.LogInformation($"Totals products database in returning: {products.Count()}");

                _logger.LogInformation("Getting categories data from Database...");
                var categories = await GetCategoriesDB();
                _logger.LogInformation($"Totals categories database in returning: {categories.Count()}");

                // load partner categories
                _logger.LogInformation("Getting categories from partner...");
                var partnerCategories = _integrationService.GetCategories();
                _logger.LogInformation($"Totals categories in returning: {partnerCategories.Count()}");

                IEnumerable<ProductPartner> partnerProducts = null;

                // load partner products
                partnerProducts = await _integrationService.GetProducts();

                _logger.LogInformation($"Totals partner products in returning: {partnerProducts.Count()}");

                _logger.LogInformation($"Reading products from partner");

                foreach (var productPartner in partnerProducts)
                {
                    var productCode = productPartner.Codigo.ToString();
                    var partnerCategory = partnerCategories.FirstOrDefault(c => c.DescCategory == productPartner.TipoProduto);
                    if (partnerCategory == null) continue;

                    // map partner category to a valid category
                    var categoryDefault = MapCategory(categories, partnerCategory.DescCategory);
                    if (categoryDefault == null) continue;

                    string FotoGrande = String.Empty;
                    string FotoPequena = String.Empty;

                    try
                    {
                        if (productPartner.Imagens.Count() > 0)
                        {
                            FotoGrande = string.IsNullOrEmpty(productPartner.Imagens[0].ImagemG) ? string.IsNullOrEmpty(productPartner.Imagens[0].ImagemM) ? productPartner.Imagens[0].ImagemP : productPartner.Imagens[0].ImagemM : productPartner.Imagens[0].ImagemG;
                            if (string.IsNullOrEmpty(FotoGrande))
                                FotoGrande = string.IsNullOrEmpty(productPartner.Fotos[0].FotoG) ? string.IsNullOrEmpty(productPartner.Fotos[0].FotoM) ? productPartner.Fotos[0].FotoP : productPartner.Fotos[0].FotoM : productPartner.Fotos[0].FotoG;

                            FotoPequena = string.IsNullOrEmpty(productPartner.Imagens[0].ImagemP) ? string.IsNullOrEmpty(productPartner.Imagens[0].ImagemM) ? productPartner.Imagens[0].ImagemG : productPartner.Imagens[0].ImagemM : productPartner.Imagens[0].ImagemP;
                            if (string.IsNullOrEmpty(FotoPequena))
                                FotoPequena = string.IsNullOrEmpty(productPartner.Fotos[0].FotoP) ? string.IsNullOrEmpty(productPartner.Fotos[0].FotoM) ? productPartner.Fotos[0].FotoG : productPartner.Fotos[0].FotoM : productPartner.Fotos[0].FotoP;
                        }                       
                    }
                    catch (Exception ex)
                    {

                    }
                    

                    images = new CreateProductSkuPreDefinedPriceEventV1Image[]
                    {
                        new()
                        {
                            Size = Platform.Events.Core.Contracts.Enums.ProductImageSize.MOBILE,
                            Url = FotoPequena
                        },
                        new()
                        {
                            Size = Platform.Events.Core.Contracts.Enums.ProductImageSize.DESKTOP_M,
                            Url = FotoGrande
                        },
                        new()
                        {
                            Size = Platform.Events.Core.Contracts.Enums.ProductImageSize.DESKTOP_L,
                            Url = FotoGrande
                        }
                    };

                    List<Atribute_Generic> AttributeProducts = new();

                    if (productPartner.DetalhesDescricao != null)
                    {
                        foreach (var item in productPartner.DetalhesDescricao)
                        {
                            Atribute_Generic oED_AttributeNew = new Atribute_Generic();

                            oED_AttributeNew.Type = Convert.ToString(Atribute_Generic_Type.SKU_Caracteristica);
                            oED_AttributeNew.Description = "Prazo";
                            oED_AttributeNew.Value = item.prazo;
                            AttributeProducts.Add(oED_AttributeNew);

                            oED_AttributeNew = new Atribute_Generic();
                            oED_AttributeNew.Type = Convert.ToString(Atribute_Generic_Type.SKU_Caracteristica);
                            oED_AttributeNew.Description = "Validade";
                            oED_AttributeNew.Value = item.validade;
                            AttributeProducts.Add(oED_AttributeNew);

                            oED_AttributeNew = new Atribute_Generic();
                            oED_AttributeNew.Type = Convert.ToString(Atribute_Generic_Type.SKU_Caracteristica);
                            oED_AttributeNew.Description = "Utilização";
                            oED_AttributeNew.Value = item.utilizacao;
                            AttributeProducts.Add(oED_AttributeNew);

                            oED_AttributeNew = new Atribute_Generic();
                            oED_AttributeNew.Type = Convert.ToString(Atribute_Generic_Type.SKU_Caracteristica);
                            oED_AttributeNew.Description = "Descrição";
                            oED_AttributeNew.Value = item.descricao;
                            AttributeProducts.Add(oED_AttributeNew);
                        }
                    }

                    if (productPartner.InformacoesAdicionais != null)
                    {
                        foreach (var item in productPartner.InformacoesAdicionais)
                        {
                            Atribute_Generic oED_AttributeNew = new Atribute_Generic();
                            oED_AttributeNew.Type = Convert.ToString(Atribute_Generic_Type.Produto_EspecificacaoTecnica);
                            oED_AttributeNew.Description = item.chave;
                            oED_AttributeNew.Value = item.valor;

                            AttributeProducts.Add(oED_AttributeNew);
                        }
                    }

                    // if product does not exists
                    if (!products.ContainsKey(productCode))
                    {
                        _logger.LogInformation($"Sending creating product {productPartner.NomeProduto}");

                        // create product
                        await _sendMessageToQueueUseCase.Queue(new CreateProductSkuPreDefinedPriceEventV1()
                        {
                            Code = productCode,
                            DisplayName = productPartner.NomeProduto,
                            Description = productPartner.Descricao,
                            CategoryId = categoryDefault,
                            StoreId = _store,
                            Images = images,
                            Attributes = AttributeProducts
                        });

                        importedProducts++;

                        continue;
                    }

                    // check if the product was alter since last sync
                    product = products[productCode];
                    hash = JsonConvert.SerializeObject(new
                    {
                        DisplayName = productPartner.NomeProduto,
                        Description = productPartner.Descricao,
                        CategoryId = categoryDefault,
                        Active = true,
                        Images = images,
                        Attributes = AttributeProducts
                    }).Sha256();

                    // different hash's will indicate that a product has modification(s)
                    if (product.Hash != hash)
                    {
                        _logger.LogInformation($"Sending updating product {productPartner.NomeProduto}");

                        // update product
                        await _sendMessageToQueueUseCase.Queue(new UpdateProductSkuPreDefinedPriceEventV1()
                        {
                            Id = product.Id,
                            StoreId = _store,
                            Code = productCode,
                            DisplayName = productPartner.NomeProduto,
                            Description = productPartner.Descricao,
                            CategoryId = categoryDefault,
                            Active = true,
                            Images = images,
                            Attributes = AttributeProducts
                        });

                        updatedProducts++;
                    }

                    var partnerSkuCode = productCode;

                    decimal _precoPor = productPartner.Preco + productPartner.Taxa.Value + productPartner.ImpostosTaxa.Value;

                    List<Atribute_Generic> AttributeSkus = new();
                    List<SkuImageProduct[]> lstImagesSku = new();

                    if (productPartner.Imagens != null)
                    {
                        foreach (var image in productPartner.Imagens)
                        {
                            var imagesSku = new SkuImageProduct[]
                            {
                                    new()
                                    {
                                        Size = Ms.Api.Utilities.Contracts.DTOs.ProductImageSize.MOBILE,
                                        Url = image.ImagemP
                                    },
                                    new()
                                    {
                                        Size = Ms.Api.Utilities.Contracts.DTOs.ProductImageSize.DESKTOP_M,
                                        Url = image.ImagemM
                                    },
                                    new()
                                    {
                                        Size = Ms.Api.Utilities.Contracts.DTOs.ProductImageSize.DESKTOP_L,
                                        Url = image.ImagemG
                                    }
                            };

                            lstImagesSku.Add(imagesSku);
                        }
                    }

                    if (product.Skus != null)
                    {
                        // if product does not exists
                        if (!product.Skus.ContainsKey(partnerSkuCode))
                        {
                            _logger.LogInformation($"Sending creating product sku {productPartner.NomeProduto}");

                            // create sku
                            await _sendMessageToQueueUseCase.Queue(new CreateSkuPreDefinedPriceEventV1()
                            {
                                Code = partnerSkuCode.ToString(),
                                ProductId = product.Id,
                                ListPrice = 0,
                                SalePrice = _precoPor,
                                Availability = false,
                                Model = productPartner.TipoProduto,
                                Attributes = null,
                                Images = lstImagesSku
                            });

                            importedSkuProducts++;

                            continue;
                        }

                        sku = product.Skus[partnerSkuCode.ToString()];

                        // check if the product was alter since last sync
                        hash = JsonConvert.SerializeObject(new
                        {
                            Code = partnerSkuCode,
                            ListPrice = 0,
                            SalePrice = _precoPor,
                            Attributes = (List<Atribute_Generic>)null,
                            Tags = (string[]?)null,
                            Active = true,
                            Availability = false,
                            Model = productPartner.TipoProduto,
                            Images = lstImagesSku
                        }).Sha256();

                        // different hash's will indicate that a product has modification(s)
                        if (sku.Hash == hash) continue;

                        _logger.LogInformation($"Sending updating product sku {productPartner.NomeProduto}");
                        // update sku
                        await _sendMessageToQueueUseCase.Queue(new UpdateSkuPreDefinedPriceEventV1()
                        {
                            Id = sku.Id,
                            Code = partnerSkuCode.ToString(),
                            ProductId = product.Id,
                            ListPrice = 0,
                            SalePrice = _precoPor,
                            Active = true,
                            Availability = false
                        });

                        updatedSkuProducts++;
                    }                    
                }

                _logger.LogInformation($"Products imported successfully...{DateTime.Now}");

                TimeSpan elapse = s.Elapsed;
                s.Stop();

                _logger.LogInformation($"Time taken to import the store Giffty: {elapse.ToString(@"hh\:mm\:ss")} ");

                _logger.LogInformation($"Totals Imported Products {importedProducts}");
                _logger.LogInformation($"Totals Updated Products {updatedProducts}");
                _logger.LogInformation($"Totals Imported Sku Products {importedSkuProducts}");
                _logger.LogInformation($"Totals Updated Sku Products {updatedSkuProducts}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying import products from store Giffty");
                throw ex;
            }
        }

        //public async Task IntegratePrices()
        //{
        //    try
        //    {
        //        string hash;
        //        ProductSkuDB sku;

        //        var stores = await GetStoresListDB();
        //        var products = await GetProductsDB(stores);

        //        var availabilityPartner = await _integrationService.GetAvailabilities();

        //        foreach (var availability in availabilityPartner)
        //        {
        //            decimal _precoPor = availability.Preco + availability.Taxa.Value + availability.ImpostosTaxa.Value;
        //            decimal _precoDe = availability.PrecoDe > 0 ? (availability.PrecoDe + availability.Taxa.Value + availability.ImpostosTaxa.Value) : _precoPor;

        //            var produto = products.Values.Where(x => x.Skus.ContainsKey(availability.Codigo.ToString())).FirstOrDefault();

        //            if (produto == null) continue;

        //            sku = produto.Skus[availability.Codigo.ToString()];

        //            await _sendMessageToQueueUseCase.Queue(new UpdateSkuPreDefinedPriceEventV1()
        //            {
        //                SkuId = sku.Id,
        //                ProductId = produto.Id,
        //                ListPrice = _precoDe,
        //                SalePrice = _precoPor,
        //            });
        //        }

        //        _logger.LogInformation($"Prices imported successfully...");
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError($"Error when trying import products availability from store Giffty");
        //        throw ex;
        //    }
        //}

        public async Task IntegrateAvailability()
        {
            try
            {
                _logger.LogInformation("Getting stores data from Database...");
                var stores = await GetStoresListDB();
                _logger.LogInformation($"Totals stores database in returning: {stores.Count()}");

                _logger.LogInformation("Getting products data from Database...");
                var products = await GetProductsDB(stores);
                _logger.LogInformation($"Totals products database in returning: {products.Count()}");

                _logger.LogInformation("Getting availability data from partner...");
                var availabilityPartner = await _integrationService.GetAvailabilities();
                _logger.LogInformation($"Totals availability in returning: {availabilityPartner.Count()}");

                string hash;
                ProductSkuDB sku;

                foreach (var availability in availabilityPartner)
                {
                    decimal _precoPor = availability.Preco + availability.Taxa.Value + availability.ImpostosTaxa.Value;
                    decimal _precoDe = availability.PrecoDe > 0 ? (availability.PrecoDe + availability.Taxa.Value + availability.ImpostosTaxa.Value) : _precoPor;

                    var produto = products.Values.Where(x => x.Skus.ContainsKey(availability.Codigo.ToString())).FirstOrDefault();

                    if (produto == null) continue;

                    sku = produto.Skus[availability.Codigo.ToString()];

                    List<SkuImageProduct[]> lstImagesSku = new();

                    if (availability.Imagens != null)
                    {
                        foreach (var image in availability.Imagens)
                        {
                            var imagesSku = new SkuImageProduct[]
                            {
                                    new()
                                    {
                                        Size = Ms.Api.Utilities.Contracts.DTOs.ProductImageSize.MOBILE,
                                        Url = image.ImagemP
                                    },
                                    new()
                                    {
                                        Size = Ms.Api.Utilities.Contracts.DTOs.ProductImageSize.DESKTOP_M,
                                        Url = image.ImagemM
                                    },
                                    new()
                                    {
                                        Size = Ms.Api.Utilities.Contracts.DTOs.ProductImageSize.DESKTOP_L,
                                        Url = image.ImagemG
                                    }
                            };

                            lstImagesSku.Add(imagesSku);
                        }
                    }

                    // check if the product was alter since last sync
                    hash = JsonConvert.SerializeObject(new
                    {
                        Code = availability?.Codigo.ToString(),
                        ListPrice = Convert.ToDecimal(_precoDe),
                        SalePrice = Convert.ToDecimal(_precoPor),
                        Attributes = (List<Atribute_Generic>)null,
                        Tags = (string[]?)null,
                        Active = true,
                        Model = availability.TipoProduto,
                        Availability = availability.Habilitado,
                        Images = lstImagesSku,
                    }).Sha256();


                    // different hash's will indicate that a product has modification(s)
                    if (sku?.Hash == hash) continue;

                    await _sendMessageToQueueUseCase.Queue(new AvailabilitySkuPreDefinedPriceEventV1()
                    {
                        SkuId = sku.Id,
                        ProductId = produto.Id,
                        ListPrice = _precoDe,
                        SalePrice = _precoPor,
                        Availability = Convert.ToBoolean(availability.Habilitado)
                    });
                }

                _logger.LogInformation($"Availabilities importeds successfully...");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying import products availability from store Giffty");
                throw ex;
            }
        }


        public async Task IntegrateStores()
        {
            try
            {
                List<Store> newStores = new();

                var storesDb = await GetStoresListDB();

                var availabilityPartner = await _integrationService.GetStores();

                foreach (var storePartner in availabilityPartner)
                {
                    if (!storesDb.Select(x => x.Name == storePartner.NomeFabricante).Any())
                    {
                        newStores.Add(new Store()
                        {
                            Name = storePartner.NomeFabricante,
                            PartnerId = _store,
                            CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                            Active = true,
                            AcceptCardPayment = true,
                            CampaignConfiguration = storesDb[0].CampaignConfiguration
                        });
                    }
                }

                foreach (var store in newStores)
                {
                    using var client = new HttpClient();
                    var response = await client.PostAsync($"{_configuration.GetValue<string>("MS_PLATFORMCONFIGURATION_URL")}/InsertNewStore", new StringContent(JsonConvert.SerializeObject(store), Encoding.UTF8, "application/json"));

                    if (!response.IsSuccessStatusCode)
                        throw new ArgumentException(response.ReasonPhrase);

                    var resContent = await response.Content.ReadAsStringAsync();
                }

                _logger.LogInformation($"New Stores importeds successfully...");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying import products availability from store Giffty");
                throw ex;
            }
        }

        async Task<Dictionary<string, ProductsDB>> GetProductsDB(List<Store> stores)
        {
            try
            {
                HttpClient client = new();

                client.Timeout = TimeSpan.FromMinutes(5);
                var response = await client.PostAsync($"{_urlProductsDb}", new StringContent(JsonConvert.SerializeObject(stores), Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception("unexpected return", new Exception(content));

                return JsonConvert.DeserializeObject<Dictionary<string, ProductsDB>>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying get products from Database");
                throw ex;
            }
        }

        async Task<IEnumerable<CategoryDB>> GetCategoriesDB()
        {
            try
            {
                HttpClient client = new();

                var response = await client.GetAsync($"{_urlCategoryDb}");
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception("unexpected return", new Exception(content));


                List<CategoryDB> lstCategoriesComplete = new();
                var lstLevel1 = JsonConvert.DeserializeObject<IEnumerable<CategoryDB>>(content);

                if (lstLevel1 != null && lstLevel1.Any())
                {
                    lstCategoriesComplete.AddRange(lstLevel1);

                    foreach (var level1 in lstLevel1)
                    {
                        if (level1.Children != null && level1.Children.Any())
                            lstCategoriesComplete.AddRange(level1.Children);

                        foreach (var level2 in level1.Children)
                        {
                            if (level2.Children != null && level2.Children.Any())
                                lstCategoriesComplete.AddRange(level2.Children);
                        }
                    }
                }

                return lstCategoriesComplete.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying get categories from Database");
                throw ex;
            }
        }

        async Task<List<Store>> GetStoresListDB()
        {
            try
            {
                HttpClient client = new();

                List<Store> stores = new();

                var response = await client.GetAsync($"{_urlListStoreDb}");
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception("unexpected return", new Exception(content));

                var result = JsonConvert.DeserializeObject<DefaultResponse<List<Store>>?>(content);

                stores = result?.Data;

                return stores;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying get Store from Database");
                throw ex;
            }
        }

        string? MapCategory(IEnumerable<CategoryDB> categoriesDb, string partnerCategory)
        {
            return partnerCategory?.Trim() switch
            {
                "VOUCHER" => categoriesDb.FirstOrDefault(x => x.Name == "Cartão Presente Virtual")?.Id,
                "VIRTUAL" => categoriesDb.FirstOrDefault(x => x.Name == "Cartão Presente Virtual")?.Id,
                "FÍSICO" => categoriesDb.FirstOrDefault(x => x.Name == "Cartão Presente Físico")?.Id,
                _ => null,
            };

        }
    }
}
