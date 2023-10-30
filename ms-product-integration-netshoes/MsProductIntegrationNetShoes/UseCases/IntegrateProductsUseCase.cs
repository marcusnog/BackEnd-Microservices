using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ms.Api.Utilities.Contracts.DTOs;
using MsProductIntegrationNetShoes.Contracts.DTOs;
using MsProductIntegrationNetShoes.Contracts.Service;
using MsProductIntegrationNetShoes.Contracts.Services;
using MsProductIntegrationNetShoes.Contracts.UseCases;
using MsProductIntegrationNetShoes.Extensions;
using Newtonsoft.Json;
using Platform.Events.Core.Contracts.Enums;
using Platform.Events.Core.Contracts.Product;
using System.Diagnostics;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.ServiceModel.Security;

namespace MsProductIntegrationNetShoes.UseCases
{
    public class IntegrateProductsUseCase : IIntegrateProductsUseCase
    {
        readonly IConfiguration _configuration;
        readonly IIntegrationNetshoesService _integrationNetshoesService;
        readonly IIntegrationServiceProductsService _integrationProductsService;
        readonly ISendMessageToQueueUseCase _sendMessageToQueueUseCase;
        readonly ILogger<IntegrateProductsUseCase> _logger;
        public readonly string _store;
        public readonly string _urlProdImg;

        public IntegrateProductsUseCase(IConfiguration configuration,
            ISendMessageToQueueUseCase sendMessageToQueueUseCase,
            IIntegrationNetshoesService integrationNetshoesService,
            IIntegrationServiceProductsService integrationProductsService,
            ILogger<IntegrateProductsUseCase> logger)
        {
            _configuration = configuration;
            _integrationNetshoesService = integrationNetshoesService;
            _sendMessageToQueueUseCase = sendMessageToQueueUseCase;
            _integrationProductsService = integrationProductsService;
            _logger = logger;
            _store = _configuration.GetValue<string>("StoreId");
            _urlProdImg = _configuration.GetValue<string>("Products:UrlProuductImg");
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

                _logger.LogInformation("Getting products data from Database... ");
                var products = await _integrationProductsService.GetProductsDB();
                _logger.LogInformation($"Totals products database in returning: {products.Count()}");

                _logger.LogInformation("Getting categories data from Database...");
                var categories = await _integrationProductsService.GetCategoriesDB();
                _logger.LogInformation($"Totals categories database in returning: {categories.Count()}");

                // load partner categories
                _logger.LogInformation("Getting categories from partner...");
                var partnerCategories = await _integrationNetshoesService.GetCategories();
                _logger.LogInformation($"Totals categories in returning: {partnerCategories.Count()}");

                // load partner products
                var partnerProducts = await _integrationNetshoesService.GetProducts();

                _logger.LogInformation($"Totals partner products in returning: {partnerProducts.Count()}");

                _logger.LogInformation("Reading products from partner");

                if (partnerProducts?.Any() != true)
                {
                    _logger.LogInformation("No products were found in netshoes");
                    return;
                }

                var lstConfirmReceiptProducts = new List<NetshoesService.ConfirmarRecebimentoProdutoRequest>();

                foreach (var productPartner in partnerProducts)
                {
                    if (string.IsNullOrEmpty(productPartner.CodigoProdutoPai))
                    {
                        #region product

                        var categoryDefault = "";
                        var productCode = productPartner?.CodigoProduto?.ToString();
                        var partnerCategory = partnerCategories?.FirstOrDefault(c => c.DescricaoFamilia == productPartner?.DescricaoFamilia?.TrimEnd());

                        // map partner category to a valid category
                        categoryDefault = MapCategory(categories, partnerCategory?.DescricaoFamilia);

                        if (partnerCategory == null)
                        {
                            partnerCategory = partnerCategories?.FirstOrDefault(c => c.DescricaoTipoProduto == productPartner?.DescricaoTipoProduto?.TrimEnd());
                            // map partner category to a valid category
                            categoryDefault = MapCategory(categories, partnerCategory?.DescricaoTipoProduto);
                        }

                        if (partnerCategory == null)
                        {
                            partnerCategory = partnerCategories?.FirstOrDefault(c => c.DescricaoDepartamento == productPartner?.DescricaoDepartamento?.TrimEnd());
                            // map partner category to a valid category
                            categoryDefault = MapCategory(categories, partnerCategory?.DescricaoDepartamento);
                        }

                        if (partnerCategory == null) continue;

                        if (categoryDefault == null) continue;

                        var imgProductPartner = String.Format(_urlProdImg, productPartner?.CodigoProduto?.Substring(productPartner.CodigoProduto.Length - 2, 2), productPartner?.CodigoProduto, $"{productPartner?.CodigoProduto}_zoom1.jpg");

                        images = new CreateProductSkuPreDefinedPriceEventV1Image[]
                        {
                            new()
                            {
                                Size = Platform.Events.Core.Contracts.Enums.ProductImageSize.MOBILE,
                                Url = imgProductPartner
                            },
                            new()
                            {
                                Size = Platform.Events.Core.Contracts.Enums.ProductImageSize.DESKTOP_M,
                                Url = imgProductPartner
                            },
                            new()
                            {
                                Size = Platform.Events.Core.Contracts.Enums.ProductImageSize.DESKTOP_L,
                                Url = imgProductPartner
                            }
                        };

                        List<Atribute_Generic> LstAttributeProduct = new();

                        if (productPartner.ListaAtributos != null)
                        {
                            foreach (var obj in productPartner.ListaAtributos)
                            {
                                Atribute_Generic AttributeProduct = new();
                                AttributeProduct.Type = Convert.ToString(Atribute_Generic_Type.SKU_Caracteristica);
                                AttributeProduct.Description = obj.Nome;
                                AttributeProduct.Value = obj.Valor;
                                LstAttributeProduct.Add(AttributeProduct);
                            }
                        }

                        // if product does not exists
                        if (!products?.ContainsKey(productCode) == true)
                        {
                            _logger.LogInformation($"Sending creating product {productPartner?.NomeProduto}");

                            // create product
                            await _sendMessageToQueueUseCase.Queue(new CreateProductSkuPreDefinedPriceEventV1()
                            {
                                Code = productCode,
                                DisplayName = productPartner?.NomeProduto,
                                Description = productPartner?.DescricaoComplementar1,
                                CategoryId = categoryDefault,
                                StoreId = _store,
                                Images = images,
                                Attributes = LstAttributeProduct
                            });

                            importedProducts++;

                            continue;
                        }

                        // check if the product was alter since last sync
                        product = products?[productCode];

                        hash = JsonConvert.SerializeObject(new
                        {
                            DisplayName = productPartner?.NomeProduto,
                            Description = productPartner?.DescricaoComplementar1,
                            CategoryId = categoryDefault,
                            Active = true,
                            Images = images,
                            Attributes = LstAttributeProduct
                        }).Sha256();

                        // different hash's will indicate that a product has modification(s)
                        if (product?.Hash == hash)
                        {
                            //In this moment the products wasn't changed then confirm receipt - codigoproduto
                            var request = new NetshoesService.ConfirmarRecebimentoProdutoRequest()
                            {
                                identificacaoLoja = _configuration.GetValue<string>("IdentificacaoLoja"),
                                chaveIdentificacao = _configuration.GetValue<string>("ChaveIdentificacao"),
                                keyProduto = productPartner.Key,
                                codigoProduto = productPartner.CodigoProduto
                            };

                            lstConfirmReceiptProducts.Add(request);

                            continue;
                        }

                        _logger.LogInformation($"Sending updating product {productPartner?.NomeProduto}");

                        // update product
                        await _sendMessageToQueueUseCase.Queue(new UpdateProductSkuPreDefinedPriceEventV1()
                        {
                            Id = product?.Id,
                            StoreId = _store,
                            Code = productCode,
                            DisplayName = productPartner?.NomeProduto,
                            Description = productPartner?.DescricaoComplementar1,
                            CategoryId = categoryDefault,
                            Active = true,
                            Images = images,
                            Attributes = LstAttributeProduct
                        });

                        updatedProducts++;

                        #endregion
                    }
                    else
                    {
                        #region sku

                        if (products?.ContainsKey(productPartner?.CodigoProdutoPai) != true)
                            products = await _integrationProductsService.GetProductsDB();

                        product = products?[productPartner.CodigoProdutoPai];

                        if (String.IsNullOrEmpty(productPartner?.CodigoProduto?.ToString())) continue;

                        List<Atribute_Generic> LstAttributesSku = new();
                        List<SkuImageProduct[]> lstImagesSku = new();

                        if (!string.IsNullOrEmpty(productPartner.Cor))
                        {
                            Atribute_Generic Attribute_Cor = new();
                            Attribute_Cor.Type = Convert.ToString(Atribute_Generic_Type.SKU_Caracteristica);
                            Attribute_Cor.Description = "Cor";
                            Attribute_Cor.Value = productPartner.Cor;
                            LstAttributesSku.Add(Attribute_Cor);
                        }

                        if (!string.IsNullOrEmpty(productPartner.Tamanho))
                        {
                            Atribute_Generic Attribute_Tamanho = new();
                            Attribute_Tamanho.Type = Convert.ToString(Atribute_Generic_Type.SKU_Caracteristica);
                            Attribute_Tamanho.Description = "Tamanho";
                            Attribute_Tamanho.Value = productPartner.Tamanho;
                            LstAttributesSku.Add(Attribute_Tamanho);
                        }

                        if (!string.IsNullOrEmpty(productPartner.Sabor))
                        {
                            Atribute_Generic Attribute_Sabor = new();
                            Attribute_Sabor.Type = Convert.ToString(Atribute_Generic_Type.SKU_Caracteristica);
                            Attribute_Sabor.Description = "Sabor";
                            Attribute_Sabor.Value = productPartner.Sabor;
                            LstAttributesSku.Add(Attribute_Sabor);
                        }

                        if (!string.IsNullOrEmpty(productPartner.Voltagem))
                        {
                            Atribute_Generic Attribute_Voltagem = new();
                            Attribute_Voltagem.Type = Convert.ToString(Atribute_Generic_Type.SKU_Caracteristica);
                            Attribute_Voltagem.Description = "Voltagem";
                            Attribute_Voltagem.Value = productPartner.Voltagem;
                            LstAttributesSku.Add(Attribute_Voltagem);
                        }

                        var _Modelo = "";
                        if (!string.IsNullOrWhiteSpace(productPartner.DescritorPreDefinido1))
                            _Modelo += productPartner.DescritorPreDefinido1.Replace("+", " e ");
                        if (!string.IsNullOrWhiteSpace(productPartner.DescritorPreDefinido2))
                            _Modelo += _Modelo.Length > 0 ? " - " + productPartner.DescritorPreDefinido2 : productPartner.DescritorPreDefinido2;

                        var imgProductPartner = String.Format(_urlProdImg, productPartner?.CodigoProdutoPai?.Substring(productPartner.CodigoProdutoPai.Length - 2, 2), productPartner?.CodigoProdutoPai, $"{productPartner?.CodigoProdutoPai}_zoom1.jpg");

                        if (imgProductPartner != null)
                        {
                            var imagesSku = new SkuImageProduct[]
                            {
                                new()
                                {
                                    Size = Ms.Api.Utilities.Contracts.DTOs.ProductImageSize.MOBILE,
                                    Url = imgProductPartner
                                },
                                new()
                                {
                                    Size = Ms.Api.Utilities.Contracts.DTOs.ProductImageSize.DESKTOP_M,
                                    Url = imgProductPartner
                                },
                                new()
                                {
                                    Size = Ms.Api.Utilities.Contracts.DTOs.ProductImageSize.DESKTOP_L,
                                    Url = imgProductPartner
                                }
                            };

                            lstImagesSku.Add(imagesSku);
                        }

                        for (var i = 2; i <= 6; i++)
                        {
                            if (Util.DoesImageExistRemotely(imgProductPartner.Replace("_zoom1", "_zoom" + i)))
                            {
                                var imagesSku = new SkuImageProduct[]
                                {
                                    new()
                                    {
                                        Size = Ms.Api.Utilities.Contracts.DTOs.ProductImageSize.MOBILE,
                                        Url = imgProductPartner.Replace("_zoom1", "_zoom" + i)
                                    },
                                    new()
                                    {
                                        Size = Ms.Api.Utilities.Contracts.DTOs.ProductImageSize.DESKTOP_M,
                                        Url = imgProductPartner.Replace("_zoom1", "_zoom" + i)
                                    },
                                    new()
                                    {
                                        Size = Ms.Api.Utilities.Contracts.DTOs.ProductImageSize.DESKTOP_L,
                                        Url = imgProductPartner.Replace("_zoom1", "_zoom" + i)
                                    }
                                };

                                lstImagesSku.Add(imagesSku);
                            }
                            else
                            {
                                //sair qaundo achar a primeira foto vazia
                                break;
                            }
                        }

                        // if product does not exists
                        if (!product.Skus.ContainsKey(productPartner?.CodigoProduto?.ToString()))
                        {
                            _logger.LogInformation($"Sending creating product sku {productPartner?.NomeProduto}");

                            // create sku
                            await _sendMessageToQueueUseCase.Queue(new CreateSkuPreDefinedPriceEventV1()
                            {
                                Code = productPartner?.CodigoProduto?.ToString(),
                                ProductId = product.Id,
                                ListPrice = 0,
                                SalePrice = 0,
                                Availability = false,
                                Attributes = LstAttributesSku,
                                Model = _Modelo,
                                Images = lstImagesSku
                            });

                            importedSkuProducts++;

                            continue;
                        }

                        sku = product.Skus?[productPartner?.CodigoProduto?.ToString()];

                        // check if the product was alter since last sync
                        hash = JsonConvert.SerializeObject(new
                        {
                            Code = productPartner?.CodigoProduto?.ToString(),
                            ListPrice = 0,
                            SalePrice = 0,
                            Attributes = LstAttributesSku,
                            Tags = (string[]?)null,
                            Active = true,
                            Availability = false,
                            Model = _Modelo,
                            Images = lstImagesSku
                        }).Sha256();

                        // different hash's will indicate that a product has modification(s)
                        if (sku?.Hash == hash)
                        {
                            //In this moment the products wasn't changed then confirm receipt - codigoproduto
                            var request = new NetshoesService.ConfirmarRecebimentoProdutoRequest()
                            {
                                identificacaoLoja = _configuration.GetValue<string>("IdentificacaoLoja"),
                                chaveIdentificacao = _configuration.GetValue<string>("ChaveIdentificacao"),
                                keyProduto = productPartner.Key,
                                codigoProduto = productPartner.CodigoProduto
                            };

                            lstConfirmReceiptProducts.Add(request);

                            continue;
                        }

                        _logger.LogInformation($"Sending updating product sku {productPartner?.NomeProduto}");
                        // update sku
                        await _sendMessageToQueueUseCase.Queue(new UpdateSkuPreDefinedPriceEventV1()
                        {
                            Id = sku?.Id,
                            Code = productPartner?.CodigoProduto.ToString(),
                            ProductId = product?.Id,
                            ListPrice = 0,
                            SalePrice = 0,
                            Active = true,
                            Availability = false,
                            Attributes = LstAttributesSku,
                            Model = _Modelo,
                            Images = lstImagesSku
                        });

                        updatedSkuProducts++;

                        #endregion
                    }
                }

                if (lstConfirmReceiptProducts?.Any() != true) return;

                foreach (var request in lstConfirmReceiptProducts)
                    _integrationNetshoesService.ConfirmReceiptProducts(request);

                _logger.LogInformation($"Products imported successfully...{DateTime.Now}");

                TimeSpan elapse = s.Elapsed;
                s.Stop();

                _logger.LogInformation($"Time taken to import the store NetShoes: {elapse.ToString(@"hh\:mm\:ss")} ");

                _logger.LogInformation($"Totals Imported Products {importedProducts}");
                _logger.LogInformation($"Totals Updated Products {updatedProducts}");
                _logger.LogInformation($"Totals Imported Sku Products {importedSkuProducts}");
                _logger.LogInformation($"Totals Updated Sku Products {updatedSkuProducts}");
            }
            catch (Exception)
            {
                _logger.LogError($"Error when trying import products from store Netshoes");
                throw;
            }
        }

        public async Task IntegratePrices()
        {
            try
            {
                ProductSkuDB sku;
                string hash;

                var prices = await _integrationNetshoesService.GetProductsPrice();

                if (prices?.Any() != true)
                {
                    _logger.LogInformation("No prices were found in netshoes");
                    return;
                }

                var products = await _integrationProductsService.GetProductsDB();

                var lstConfirmReceiptPrices = new List<NetshoesService.ConfirmarRecebimentoPrecoRequest>();

                foreach (var price in prices)
                {
                    var product = products?.Values?.Where(x => x.Skus?.ContainsKey(price?.CodigoProdutok__BackingField?.ToString()) == true)?.FirstOrDefault();

                    if (product == null) continue;

                    sku = product.Skus[price.CodigoProdutok__BackingField.ToString()];

                    // check if the product was alter since last sync
                    hash = JsonConvert.SerializeObject(new
                    {
                        Code = price?.CodigoProdutok__BackingField?.ToString(),
                        ListPrice = Convert.ToDecimal(price.PrecoTabelak__BackingField),
                        SalePrice = price.PrecoPromocionalk__BackingField == 0 ? Convert.ToDecimal(price.PrecoTabelak__BackingField) : Convert.ToDecimal(price.PrecoPromocionalk__BackingField),
                        Attributes = (Dictionary<string, string>?)null,
                        Tags = (string[]?)null,
                        Active = true
                    }).Sha256();

                    // different hash's will indicate that a product has modification(s)
                    if (sku?.Hash == hash)
                    {
                        //In this moment the prices wasn't changed then confirm receipt - codigoproduto
                        var request = new NetshoesService.ConfirmarRecebimentoPrecoRequest()
                        {
                            identificacaoLoja = _configuration.GetValue<string>("IdentificacaoLoja"),
                            chaveIdentificacao = _configuration.GetValue<string>("ChaveIdentificacao"),
                            keyPreco = price.Keyk__BackingField,
                            codigoProduto = price?.CodigoProdutok__BackingField?.ToString()
                        };

                        lstConfirmReceiptPrices.Add(request);

                        continue;
                    }

                    await _sendMessageToQueueUseCase.Queue(new UpdateSkuPreDefinedPriceEventV1()
                    {
                        SkuId = sku.Id,
                        ProductId = product.Id,
                        ListPrice = Convert.ToDecimal(price.PrecoTabelak__BackingField),
                        SalePrice = price.PrecoPromocionalk__BackingField == 0 ? Convert.ToDecimal(price.PrecoTabelak__BackingField) : Convert.ToDecimal(price.PrecoPromocionalk__BackingField),
                    });
                }

                if (lstConfirmReceiptPrices?.Any() != true) return;

                foreach (var request in lstConfirmReceiptPrices)
                {
                    _integrationNetshoesService.ConfirmReceiptPrices(request);
                }
            }
            catch (Exception)
            {
                _logger.LogError($"Error when trying import products availability from store Netshoes");
                throw;
            }
        }

        public async Task IntegrateAvailability()
        {
            try
            {
                ProductSkuDB sku;
                string hash;

                var products = await _integrationProductsService.GetProductsDB();

                var prodSkuZeroeds = await _integrationNetshoesService.GetAvailabilities();

                var prices = await _integrationNetshoesService.GetProductsPrice();

                if (prices?.Any() != true)
                {
                    _logger.LogInformation("No prices were found in netshoes");
                    return;
                }

                foreach (var price in prices)
                {
                    var product = products?.Values?.Where(x => x.Skus?.ContainsKey(price?.CodigoProdutok__BackingField?.ToString()) == true)?.FirstOrDefault();

                    if (product == null) continue;

                    sku = product.Skus[price.CodigoProdutok__BackingField.ToString()];

                    // check if the product was alter since last sync
                    hash = JsonConvert.SerializeObject(new
                    {
                        Code = price?.CodigoProdutok__BackingField?.ToString(),
                        ListPrice = Convert.ToDecimal(price.PrecoTabelak__BackingField),
                        SalePrice = Convert.ToDecimal(price.PrecoPromocionalk__BackingField) == 0 ? Convert.ToDecimal(price.PrecoTabelak__BackingField) : Convert.ToDecimal(price.PrecoPromocionalk__BackingField),
                        Active = true,
                        Availability = prodSkuZeroeds.Contains(price.CodigoProdutok__BackingField) ? false : true
                    }).Sha256();

                    // different hash's will indicate that a product has modification(s)
                    if (sku?.Hash == hash) continue;

                    await _sendMessageToQueueUseCase.Queue(new AvailabilitySkuPreDefinedPriceEventV1()
                    {
                        SkuId = sku.Id,
                        ProductId = product.Id,
                        ListPrice = Convert.ToDecimal(price.PrecoTabelak__BackingField),
                        SalePrice = Convert.ToDecimal(price.PrecoPromocionalk__BackingField) == 0 ? Convert.ToDecimal(price.PrecoTabelak__BackingField) : Convert.ToDecimal(price.PrecoPromocionalk__BackingField),
                        Availability = prodSkuZeroeds.Contains(price.CodigoProdutok__BackingField) ? false : true
                    });
                }
            }
            catch (Exception)
            {
                _logger.LogError($"Error when trying import products availability from store Netshoes");
                throw;
            }
        }

        string? MapCategory(IEnumerable<CategoryDB> categoriesDb, string partnerCategory)
        {
            var reconheceCategory = "";

            if (String.IsNullOrEmpty(partnerCategory)) return null;

            reconheceCategory = categoriesDb?.FirstOrDefault(x => x.Name.Trim().Equals(partnerCategory.Trim(), StringComparison.OrdinalIgnoreCase))?.Id;

            if (String.IsNullOrEmpty(reconheceCategory))
                reconheceCategory = categoriesDb.OrderBy(x => x.Name.Length).FirstOrDefault(x => x.Name.ToLower().Trim().StartsWith(partnerCategory.ToLower().Trim(), StringComparison.OrdinalIgnoreCase))?.Id;

            if (String.IsNullOrEmpty(reconheceCategory))
            {
                var newCategory = "";

                newCategory.Split(' ').Where(x => x.Length > 3 && x.ToUpper() != "KIT" && x.ToUpper() != "KITS" && x.ToUpper() != "CONJUNTO" && x.ToUpper() != "CONJUNTOS" && x.ToUpper() != "CJ" && x.ToUpper() != "PAR" && x.ToUpper() != "PARES")
                        .ToList().ForEach(x => newCategory += x + " ");
                newCategory = newCategory.Trim();

                reconheceCategory = categoriesDb.OrderBy(x => x.Name.Length).FirstOrDefault(x => x.Name.ToLower().Trim().Equals(newCategory.ToLower().Trim(), StringComparison.OrdinalIgnoreCase))?.Id;
            }

            if (String.IsNullOrEmpty(reconheceCategory))
            {
                var category = partnerCategory.Trim().ToUpper();

                reconheceCategory = category switch
                {
                    string cat when cat.Contains("MOCHILA") && cat.Contains("ESPORTIVA") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "MOCHILA ESPORTIVA")?.Id,
                    string cat when cat.Contains("MOCHILA") && cat.Contains("CASUAL") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "MOCHILA CASUAL")?.Id,
                    string cat when cat.Contains("MALAS E MOCHILAS") || cat.Contains("BOLSAS E MOCHILAS") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "MALAS E MOCHILAS")?.Id,
                    string cat when cat.Contains("MOCHILAS") || cat.Contains("MOCHILA") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "MOCHILA ESPORTIVA")?.Id,
                    string cat when cat.Contains("ÓCULOS NATAÇÃO") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "ÓCULOS DE NATAÇÃO")?.Id,
                    string cat when cat.Contains("ÓCULOS PARA NATAÇÃO") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "ÓCULOS DE NATAÇÃO")?.Id,
                    string cat when cat.Contains("ÓCULOS") && cat.Contains("SOL") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "ÓCULOS DE SOL")?.Id,
                    string cat when cat.Contains("ÓCULOS") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "ÓCULOS")?.Id,
                    string cat when cat.Contains("EQUIPAMENTOS DE TREINO") || cat.Contains("ACESSÓRIOS DE TREINO") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "ACESSÓRIOS DE TREINO")?.Id,
                    string cat when cat.Contains("EQUIPAMENTOS DE SEGURANÇA") || cat.Contains("ACESSÓRIOS DE SEGURANÇA") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "EQUIPAMENTOS DE SEGURANÇA")?.Id,
                    string cat when cat.Contains("APARELHOS PARA LIMPEZA DE PELE") || cat.Contains("MASSAGEADOR") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "MASSAGEADORES")?.Id,
                    string cat when cat.Equals("CONJUNTOS", StringComparison.OrdinalIgnoreCase) => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "CONJUNTOS")?.Id,
                    string cat when cat.Equals("BIKE", StringComparison.OrdinalIgnoreCase) => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "BICICLETAS")?.Id,
                    string cat when cat.Contains("BIKE") && cat.Contains("ACESSÓRIO") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "ACESSÓRIOS PARA CICLISTAS")?.Id,
                    string cat when cat.Contains("ACESSÓRIOS") || cat.Contains("ACESSÓRIO") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "ACESSÓRIOS")?.Id,
                    string cat when cat.Contains("BOTAS") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "BOTAS")?.Id,
                    string cat when cat.Contains("QUADROS") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "Quadros")?.Id,
                    string cat when cat.Contains("TV") || cat.Contains("VÍDEO") || cat.Contains("VÍDEO") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "TV E VÍDEO")?.Id,
                    string cat when cat.Contains("MÓVEIS") || cat.Contains("MOVEIS") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "MÓVEIS")?.Id,
                    string cat when cat.Contains("PERFUMARIA") || cat.Contains("PERFUM") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "PERFUMARIA")?.Id,
                    string cat when cat.Contains("ROUPA") && !cat.Contains("SECADORA") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "ROUPAS")?.Id,
                    string cat when cat.Contains("ROUPA") => categoriesDb.FirstOrDefault(x => x.Name.Trim().ToUpper() == "ROUPAS")?.Id,
                    _ => null
                };
            }

            if (String.IsNullOrEmpty(reconheceCategory))
            {
                string[] ListaPartesDoNome = partnerCategory.Replace(".", "").Replace(",", "").Split(' ');
                //Excluir todas letras unicas como e, é, & ... etc.
                //Excluir todas palavras com 2 letras como de, da, do & ... etc.
                ListaPartesDoNome = ListaPartesDoNome.Where(x => x.Length > 2 && x.ToUpper() != "KIT" && x.ToUpper() != "KITS" && x.ToUpper() != "CONJUNTO" && x.ToUpper() != "CONJUNTOS" && x.ToUpper() != "CJ" && x.ToUpper() != "PAR" && x.ToUpper() != "PARES").ToArray();

                List<CategoriaEncontrada> ListaEncontrados = new List<CategoriaEncontrada>();
                if (partnerCategory == "Mercado")
                    return null;

                foreach (string Parte in ListaPartesDoNome)
                {
                    categoriesDb.OrderBy(x => x.Name.Length).Where(x => x.Name.Contains(Parte)).ToList().ForEach(x =>
                    {
                        var _Item = ListaEncontrados.FirstOrDefault(i => i.Id == x.Id);
                        if (_Item == null)
                            ListaEncontrados.Add(new CategoriaEncontrada { Qtd = 1, Id = x.Id, Nome = x.Name });
                        else
                            _Item.Qtd++;
                    });
                }

                if (ListaEncontrados.Count > 0)
                {
                    CategoriaEncontrada CategoriaMaisAparecida = new CategoriaEncontrada();

                    if (ListaPartesDoNome.Length == 1)
                    {
                        var MenosWordLength = ListaPartesDoNome[0].Length;

                        //A categoria que está igual a palavria
                        var ListaTheSameLength = ListaEncontrados.Where(x => x.Nome.Length == MenosWordLength).ToList();
                        if (ListaTheSameLength.Count > 0)
                        {
                            if (ListaTheSameLength.Count == 1)
                                CategoriaMaisAparecida = ListaTheSameLength.FirstOrDefault();
                            else
                            {
                                CategoriaMaisAparecida = ListaTheSameLength.FirstOrDefault(x => x.Qtd == ListaTheSameLength.Max(i => i.Qtd));
                            }
                        }

                        if (CategoriaMaisAparecida == null)
                        {
                            ListaTheSameLength = ListaEncontrados.Where(x => x.Nome.Length > MenosWordLength).ToList();
                            if (ListaTheSameLength.Count > 0)
                            {
                                if (ListaTheSameLength.Count == 1)
                                    CategoriaMaisAparecida = ListaTheSameLength.FirstOrDefault();
                                else
                                {
                                    CategoriaMaisAparecida = ListaTheSameLength.FirstOrDefault(x => x.Qtd == ListaTheSameLength.Max(i => i.Qtd));
                                }
                            }
                        }
                    }

                    if (ListaPartesDoNome.Length > 1 || CategoriaMaisAparecida == null)
                        CategoriaMaisAparecida = ListaEncontrados.FirstOrDefault(x => x.Qtd == ListaEncontrados.Max(i => i.Qtd));

                    if (CategoriaMaisAparecida != null)
                        return categoriesDb.FirstOrDefault(x => x.Id == CategoriaMaisAparecida.Id).Id;
                }
                return null;
            }

            return reconheceCategory;
        }
    }
}
