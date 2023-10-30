using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using MsProductIntegrationNetShoes.Contracts.DTOs;
using MsProductIntegrationNetShoes.Contracts.Service;
using MsProductIntegrationNetShoes.Contracts.Services;
using MsProductIntegrationNetShoes.Contracts.UseCases;
using MsProductIntegrationNetShoes.UseCases;
using NetshoesService;
using NUnit.Framework;
using Platform.Events.Core.Contracts.Product;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MsProductIntegrationNetShoes.Tests.UseCases
{
    public class IntegrateProductsUseCaseTests
    {
        Mock<IConfiguration> configuration = null;
        [SetUp]
        public void Setup()
        {
            var mockConfSection = new Mock<IConfigurationSection>();

            Mock<IConfigurationSection> sectionStoreId = new Mock<IConfigurationSection>();
            sectionStoreId.SetupGet(m => m.Value).Returns("000000000000000000000052");
            Mock<IConfigurationSection> sectionIdentificacaoLoja = new Mock<IConfigurationSection>();
            sectionIdentificacaoLoja.SetupGet(m => m.Value).Returns("CAT_DIGI_NETSHOES");
            Mock<IConfigurationSection> sectionChaveIdentificacao = new Mock<IConfigurationSection>();
            sectionChaveIdentificacao.SetupGet(m => m.Value).Returns("42CA9397-10F5-40AE-8995-A8039DF49E00");
            Mock<IConfigurationSection> sectionUrlProductsImg = new Mock<IConfigurationSection>();
            sectionUrlProductsImg.SetupGet(m => m.Value).Returns("http://static.netshoes.com.br/produtos/{0}/{1}/{2}");

            configuration = new Mock<IConfiguration>();
            configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "StoreId"))).Returns(sectionStoreId.Object);
            configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "IdentificacaoLoja"))).Returns(sectionIdentificacaoLoja.Object);
            configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "ChaveIdentificacao"))).Returns(sectionChaveIdentificacao.Object);
            configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "Products:UrlProuductImg"))).Returns(sectionUrlProductsImg.Object);
        }

        [Test]
        public async Task IntegratePrices_WithoutPrice_MustNotCallConfirmReceiptPrices()
        {
            // prepare
            var qtdCallsConfirmReceiptPrices = 0;
            //configuration.Setup(x => x.GetSection(It.IsAny<string>())).Returns(null);
            var sendMessageToQueueUseCase = new Mock<ISendMessageToQueueUseCase>();
            var integrationNetshoesService = new Mock<IIntegrationNetshoesService>();
            integrationNetshoesService.Setup(x => x.GetProductsPrice()).Returns(Task.FromResult((IEnumerable<PrecoRplEN>?)null));
            integrationNetshoesService.Setup(x => x.ConfirmReceiptPrices(It.IsAny<ConfirmarRecebimentoPrecoRequest>())).Callback(() =>
             {
                 qtdCallsConfirmReceiptPrices++;
             });

            var integrationProductsService = new Mock<IIntegrationServiceProductsService>();
            var logger = new Mock<ILogger<IntegrateProductsUseCase>>();

            var integrateProductsUseCase = new IntegrateProductsUseCase(
                configuration.Object,
                sendMessageToQueueUseCase.Object,
                integrationNetshoesService.Object,
                integrationProductsService.Object,
                logger.Object
                );

            // act
            await integrateProductsUseCase.IntegratePrices();

            // assert
            Assert.AreEqual(0, qtdCallsConfirmReceiptPrices);
        }

        [Test]
        public async Task IntegratePrices_WithPrices_MustCallConfirmReceiptPrices()
        {
            // prepare
            var qtdCallsConfirmReceiptPrices = 0;
            var sendMessageToQueueUseCase = new Mock<ISendMessageToQueueUseCase>();
            var integrationNetshoesService = new Mock<IIntegrationNetshoesService>();
            var integrationProductsService = new Mock<IIntegrationServiceProductsService>();

            integrationNetshoesService.Setup(x => x.GetProductsPrice()).Returns(Task.FromResult(new List<PrecoRplEN>() { 
                new PrecoRplEN()
                {
                    CodigoProdutok__BackingField = "D36-1463-274-10",
                    Keyk__BackingField = 0001
                }
            }.AsEnumerable()));

            integrationProductsService.Setup(x => x.GetProductsDB()).Returns(Task.FromResult(new Dictionary<string, ProductsDB>()
            {
                { "D36-1463-274", new ProductsDB() { Id = "", Code = "D36-1463-274", Skus = new Dictionary<string, ProductSkuDB>
                {
                    { "D36-1463-274-10", new ProductSkuDB() { Id = "gggkcdsk", Code = "D36-1463-274-10", Hash = "7Qu8KgQwUf4+0DMiTWCn8XNvFHx/47Uw2bWy8OgQ/sA=" } }
                }
                }
             }
            }));

            integrationNetshoesService.Setup(x => x.ConfirmReceiptPrices(It.IsAny<ConfirmarRecebimentoPrecoRequest>())).Callback(() =>
            {
                qtdCallsConfirmReceiptPrices++;
            });

            var logger = new Mock<ILogger<IntegrateProductsUseCase>>();

            var integrateProductsUseCase = new IntegrateProductsUseCase(
                configuration.Object,
                sendMessageToQueueUseCase.Object,
                integrationNetshoesService.Object,
                integrationProductsService.Object,
                logger.Object
                );

            // act
            await integrateProductsUseCase.IntegratePrices();

            // assert
            Assert.AreEqual(1, qtdCallsConfirmReceiptPrices);
        }

        [Test]
        public async Task IntegrateProducts_WithoutProducts_MustNotCallConfirmReceiptProducts()
        {
            // prepare
            var qtdCallsConfirmReceiptProducts = 0;
            var sendMessageToQueueUseCase = new Mock<ISendMessageToQueueUseCase>();
            var integrationNetshoesService = new Mock<IIntegrationNetshoesService>();
            integrationNetshoesService.Setup(x => x.GetProducts()).Returns(Task.FromResult((IEnumerable<ProdutoRplEN>?)null));
            integrationNetshoesService.Setup(x => x.ConfirmReceiptProducts(It.IsAny<ConfirmarRecebimentoProdutoRequest>())).Callback(() =>
            {
                qtdCallsConfirmReceiptProducts++;
            });

            var integrationProductsService = new Mock<IIntegrationServiceProductsService>();
            var logger = new Mock<ILogger<IntegrateProductsUseCase>>();

            var integrateProductsUseCase = new IntegrateProductsUseCase(
                configuration.Object,
                sendMessageToQueueUseCase.Object,
                integrationNetshoesService.Object,
                integrationProductsService.Object,
                logger.Object
                );

            // act
            await integrateProductsUseCase.IntegrateProducts();

            // assert
            Assert.AreEqual(0, qtdCallsConfirmReceiptProducts);
        }

        [Test]
        public async Task IntegrateProducts_WithProducts_MustNotCallConfirmReceiptProducts()
        {
            // prepare
            var qtdCallsConfirmReceiptProducts = 0;
            var sendMessageToQueueUseCase = new Mock<ISendMessageToQueueUseCase>();
            var integrationNetshoesService = new Mock<IIntegrationNetshoesService>();
            var integrationProductsService = new Mock<IIntegrationServiceProductsService>();
            integrationNetshoesService.Setup(x => x.GetProducts()).Returns(Task.FromResult(new List<ProdutoRplEN>() {
                new ProdutoRplEN()
                {
                    CodigoProduto = "D36-1463-274-10",
                    Key = 00001,
                    DescricaoFamilia = "Maiôs e Sungas"

                }
            }.AsEnumerable()));

            integrationProductsService.Setup(x => x.GetProductsDB()).Returns(Task.FromResult(new Dictionary<string, ProductsDB>()
            {
                { "D36-1463-274", new ProductsDB() { Id = "", Code = "D36-1463-274", Skus = new Dictionary<string, ProductSkuDB>
                {
                    { "D36-1463-274-10", new ProductSkuDB() { Id = "gggkcdsk", Code = "D36-1463-274-10" } }
                }
                }
             }
            }));
            integrationNetshoesService.Setup(x => x.GetCategories()).Returns(Task.FromResult(new List<CategoryPartner>() {
                new CategoryPartner()
                {
                    DescricaoFamilia = "Maiôs e Sungas"
                }
            }.AsEnumerable()));
            integrationProductsService.Setup(x => x.GetCategoriesDB()).Returns(Task.FromResult(new List<CategoryDB>(){
                new CategoryDB()
                {
                    Id = "1jgchdg2w3223454kjhjhkh11",
                    Name = "Maiôs"
                }
            }.AsEnumerable()));

            integrationNetshoesService.Setup(x => x.ConfirmReceiptProducts(It.IsAny<ConfirmarRecebimentoProdutoRequest>())).Callback(() =>
            {
                qtdCallsConfirmReceiptProducts++;
            });

            var logger = new Mock<ILogger<IntegrateProductsUseCase>>();

            var integrateProductsUseCase = new IntegrateProductsUseCase(
                configuration.Object,
                sendMessageToQueueUseCase.Object,
                integrationNetshoesService.Object,
                integrationProductsService.Object,
                logger.Object
                );

            // act
            await integrateProductsUseCase.IntegrateProducts();

            // assert
            Assert.AreEqual(0, qtdCallsConfirmReceiptProducts);
        }

        [Test]
        public async Task IntegrateProducts_WithProductsCategory_MustNotCallConfirmReceiptProducts()
        {
            // prepare
            var qtdCallsConfirmReceiptProducts = 0;
            var sendMessageToQueueUseCase = new Mock<ISendMessageToQueueUseCase>();
            var integrationNetshoesService = new Mock<IIntegrationNetshoesService>();
            var integrationProductsService = new Mock<IIntegrationServiceProductsService>();

            integrationNetshoesService.Setup(x => x.GetProducts()).Returns(Task.FromResult(new List<ProdutoRplEN>() {
                new ProdutoRplEN()
                {
                     CodigoProduto = "D36-1463-274-10",
                    Key = 00001,
                    DescricaoFamilia = "Maiôs e Sungas"
                }
            }.AsEnumerable()));

            integrationNetshoesService.Setup(x => x.GetCategories()).Returns(Task.FromResult(new List<CategoryPartner>() {
                new CategoryPartner()
                {
                    DescricaoFamilia = "Maiôs e Sungas"
                }
            }.AsEnumerable()));

            integrationProductsService.Setup(x => x.GetProductsDB()).Returns(Task.FromResult(new Dictionary<string, ProductsDB>()
            {
                { "D36-1463-274", new ProductsDB() { Id = "", Code = "D36-1463-274", Skus = new Dictionary<string, ProductSkuDB>
                {
                    { "D36-1463-274-10", new ProductSkuDB() { Id = "gggkcdsk", Code = "D36-1463-274-10" } }
                }
                }
             }
            }));

            integrationNetshoesService.Setup(x => x.ConfirmReceiptProducts(It.IsAny<ConfirmarRecebimentoProdutoRequest>())).Callback(() =>
            {
                qtdCallsConfirmReceiptProducts++;
            });

            integrationProductsService.Setup(x => x.GetCategoriesDB()).Returns(Task.FromResult(new List<CategoryDB>(){
                new CategoryDB()
                {
                    Id = "1jgchdg2w3223454kjhjhkh11",
                    Name = "Maiôs"
                }
            }.AsEnumerable()));

            var logger = new Mock<ILogger<IntegrateProductsUseCase>>();

            var integrateProductsUseCase = new IntegrateProductsUseCase(
                configuration.Object,
                sendMessageToQueueUseCase.Object,
                integrationNetshoesService.Object,
                integrationProductsService.Object,
                logger.Object
                );

            // act
            await integrateProductsUseCase.IntegrateProducts();

            // assert
            Assert.AreEqual(0, qtdCallsConfirmReceiptProducts);
        }

        [Test]
        public async Task IntegrateProducts_WithProductsSku_MustNotCallConfirmReceiptProducts()
        {
            // prepare
            var qtdCallsConfirmReceiptProducts = 0;
            var sendMessageToQueueUseCase = new Mock<ISendMessageToQueueUseCase>();
            var integrationNetshoesService = new Mock<IIntegrationNetshoesService>();
            var integrationProductsService = new Mock<IIntegrationServiceProductsService>();
            integrationNetshoesService.Setup(x => x.GetProducts()).Returns(Task.FromResult(new List<ProdutoRplEN>() {
                new ProdutoRplEN()
                {
                    CodigoProdutoPai = "D36-1463-274",
                     Key = 00001,
                    DescricaoFamilia = "Maiôs e Sungas",
                    CodigoProduto = "D36-1463-274"

                }
            }.AsEnumerable()));
            integrationProductsService.Setup(x => x.GetCategoriesDB()).Returns(Task.FromResult(new List<CategoryDB>(){
                new CategoryDB()
                {
                    Id = "1jgchdg2w3223454kjhjhkh11",
                    Name = "Maiôs"
                }
            }.AsEnumerable()));
            integrationNetshoesService.Setup(x => x.GetCategories()).Returns(Task.FromResult(new List<CategoryPartner>() {
                new CategoryPartner()
                {
                    DescricaoFamilia = "Maiôs e Sungas"
                }
            }.AsEnumerable()));
            integrationProductsService.Setup(x => x.GetProductsDB()).Returns(Task.FromResult(new Dictionary<string, ProductsDB>()
            {
                { "D36-1463-274", new ProductsDB() { Id = "", Code = "D36-1463-274", Skus = new Dictionary<string, ProductSkuDB>
                {
                    { "D36-1463-274-10", new ProductSkuDB() { Id = "gggkcdsk", Code = "D36-1463-274-10" } }
                }
                }
             }
            }));
            integrationNetshoesService.Setup(x => x.ConfirmReceiptProducts(It.IsAny<ConfirmarRecebimentoProdutoRequest>())).Callback(() =>
            {
                qtdCallsConfirmReceiptProducts++;
            });


            var logger = new Mock<ILogger<IntegrateProductsUseCase>>();

            var integrateProductsUseCase = new IntegrateProductsUseCase(
                configuration.Object,
                sendMessageToQueueUseCase.Object,
                integrationNetshoesService.Object,
                integrationProductsService.Object,
                logger.Object
                );

            // act
            await integrateProductsUseCase.IntegrateProducts();

            // assert
            Assert.AreEqual(0, qtdCallsConfirmReceiptProducts);
        }

        [Test]
        public async Task IntegrateProducts_WithProducts_MustCallCreateProductEvent()
        {
            // prepare
            var qtdCallsCreateProductEventV1 = 0;
            var sendMessageToQueueUseCase = new Mock<ISendMessageToQueueUseCase>();
            var integrationNetshoesService = new Mock<IIntegrationNetshoesService>();
            var integrationProductsService = new Mock<IIntegrationServiceProductsService>();

            integrationNetshoesService.Setup(x => x.GetProducts()).Returns(Task.FromResult(new List<ProdutoRplEN>() {
                new ProdutoRplEN()
                {
                    CodigoProduto = "D36-1463-274",
                    DescricaoFamilia = "Maiôs e Sungas"
                }
            }.AsEnumerable()));

            integrationNetshoesService.Setup(x => x.GetCategories()).Returns(Task.FromResult(new List<CategoryPartner>() {
                new CategoryPartner()
                {
                    DescricaoFamilia = "Maiôs e Sungas"
                }
            }.AsEnumerable()));

            integrationProductsService.Setup(x => x.GetProductsDB()).Returns(Task.FromResult(new Dictionary<string, ProductsDB>() 
            {
                { "dghgajgasdcs", new ProductsDB() { Code = "D36-1463-274" } }
            }));

            integrationProductsService.Setup(x => x.GetCategoriesDB()).Returns(Task.FromResult(new List<CategoryDB>() 
            {
                 new CategoryDB() 
                 {
                     Id = "skxgshagc",
                     Name = "Maiôs e Sungas" 
                 } 
            }.AsEnumerable()));

            sendMessageToQueueUseCase.Setup(x => x.Queue(It.IsAny<CreateProductEventV1>())).Callback(() => 
            {
                qtdCallsCreateProductEventV1++;
            });

            var logger = new Mock<ILogger<IntegrateProductsUseCase>>();

            var integrateProductsUseCase = new IntegrateProductsUseCase(
                configuration.Object,
                sendMessageToQueueUseCase.Object,
                integrationNetshoesService.Object,
                integrationProductsService.Object,
                logger.Object
                );

            // act
            await integrateProductsUseCase.IntegrateProducts();

            // assert
            Assert.AreEqual(1, qtdCallsCreateProductEventV1);
        }

        [Test]
        public async Task IntegrateProducts_WithProducts_MustCallUpdateProductEvent()
        {
            // prepare
            var qtdCallsUpdateProductEventV1 = 0;
            var sendMessageToQueueUseCase = new Mock<ISendMessageToQueueUseCase>();
            var integrationNetshoesService = new Mock<IIntegrationNetshoesService>();
            var integrationProductsService = new Mock<IIntegrationServiceProductsService>();

            integrationNetshoesService.Setup(x => x.GetProducts()).Returns(Task.FromResult(new List<ProdutoRplEN>() {
                new ProdutoRplEN()
                {
                    DescricaoFamilia = "Maiôs e Sungas",
                    CodigoProduto = "D36-1463-274",
                }
            }.AsEnumerable()));

            integrationNetshoesService.Setup(x => x.GetCategories()).Returns(Task.FromResult(new List<CategoryPartner>() {
                new CategoryPartner()
                {
                    DescricaoFamilia = "Maiôs e Sungas"
                }
            }.AsEnumerable()));

            integrationProductsService.Setup(x => x.GetProductsDB()).Returns(Task.FromResult(new Dictionary<string, ProductsDB>()
            {
                { "D36-1463-274", new ProductsDB() { Id = "", Code = "D36-1463-274", Skus = new Dictionary<string, ProductSkuDB>
                {
                    { "D36-1463-274-10", new ProductSkuDB() { Id = "gggkcdsk", Code = "D36-1463-274-10" } }
                }
                }
             }
            }));

            integrationProductsService.Setup(x => x.GetCategoriesDB()).Returns(Task.FromResult(new List<CategoryDB>()
            {
                 new CategoryDB()
                 {
                     Id = "skxgshagc",
                     Name = "Maiôs e Sungas"
                 }
            }.AsEnumerable()));

            sendMessageToQueueUseCase.Setup(x => x.Queue(It.IsAny<UpdateProductEventV1>())).Callback(() =>
            {
                qtdCallsUpdateProductEventV1++;
            });

            var logger = new Mock<ILogger<IntegrateProductsUseCase>>();

            var integrateProductsUseCase = new IntegrateProductsUseCase(
                configuration.Object,
                sendMessageToQueueUseCase.Object,
                integrationNetshoesService.Object,
                integrationProductsService.Object,
                logger.Object
                );

            // act
            await integrateProductsUseCase.IntegrateProducts();

            // assert
            Assert.AreEqual(1, qtdCallsUpdateProductEventV1);
        }

        [Test]
        public async Task IntegrateProducts_WithoutProductsSku_MustNotCallCreateSkuEvent()
        {
            // prepare
            var qtdCallsCreateSkuEventV1 = 0;
            var sendMessageToQueueUseCase = new Mock<ISendMessageToQueueUseCase>();
            var integrationNetshoesService = new Mock<IIntegrationNetshoesService>();
            var integrationProductsService = new Mock<IIntegrationServiceProductsService>();

            integrationNetshoesService.Setup(x => x.GetProducts()).Returns(Task.FromResult(new List<ProdutoRplEN>() {
                new ProdutoRplEN()
                {
                    CodigoProdutoPai = "D36-1463-274",
                    DescricaoFamilia = "Maiôs e Sungas"
                }
            }.AsEnumerable()));

            integrationNetshoesService.Setup(x => x.GetCategories()).Returns(Task.FromResult(new List<CategoryPartner>() {
                new CategoryPartner()
                {
                    DescricaoFamilia = "Maiôs e Sungas"
                }
            }.AsEnumerable()));

            integrationProductsService.Setup(x => x.GetProductsDB()).Returns(Task.FromResult(new Dictionary<string, ProductsDB>()
            {
                { "D36-1463-274", new ProductsDB() { Id = "", Code = "D36-1463-274" } }
            }));

            integrationProductsService.Setup(x => x.GetCategoriesDB()).Returns(Task.FromResult(new List<CategoryDB>()
            {
                 new CategoryDB()
                 {
                     Id = "skxgshagc",
                     Name = "Maiôs"
                 }
            }.AsEnumerable()));

            sendMessageToQueueUseCase.Setup(x => x.Queue(It.IsAny<CreateSkuPreDefinedPriceEventV1>())).Callback(() =>
            {
                qtdCallsCreateSkuEventV1++;
            });

            var logger = new Mock<ILogger<IntegrateProductsUseCase>>();

            var integrateProductsUseCase = new IntegrateProductsUseCase(
                configuration.Object,
                sendMessageToQueueUseCase.Object,
                integrationNetshoesService.Object,
                integrationProductsService.Object,
                logger.Object
                );

            // act
            await integrateProductsUseCase.IntegrateProducts();

            // assert
            Assert.AreEqual(0, qtdCallsCreateSkuEventV1);
        }

        [Test]
        public async Task IntegrateProducts_WithProductsSku_MustCallCreateSkuEvent()
        {
            // prepare
            var qtdCallsCreateSkuEventV1 = 0;
            var sendMessageToQueueUseCase = new Mock<ISendMessageToQueueUseCase>();
            var integrationNetshoesService = new Mock<IIntegrationNetshoesService>();
            var integrationProductsService = new Mock<IIntegrationServiceProductsService>();

            integrationNetshoesService.Setup(x => x.GetProducts()).Returns(Task.FromResult(new List<ProdutoRplEN>() {
                new ProdutoRplEN()
                {
                    CodigoProdutoPai = "D36-1463-274",
                    CodigoProduto = "D36-1463-274-10",
                    DescricaoFamilia = "Maiôs e Sungas"
                }
            }.AsEnumerable()));

            integrationNetshoesService.Setup(x => x.GetCategories()).Returns(Task.FromResult(new List<CategoryPartner>() {
                new CategoryPartner()
                {
                    DescricaoFamilia = "Maiôs e Sungas"
                }
            }.AsEnumerable()));

            integrationProductsService.Setup(x => x.GetProductsDB()).Returns(Task.FromResult(new Dictionary<string, ProductsDB>()
            {
                { "D36-1463-274", new ProductsDB() { Id = "", Code = "D36-1463-274", Skus = new Dictionary<string, ProductSkuDB> 
                {
                    { "sdfsffsgf", new ProductSkuDB() { Id = "gggkcdsk", Code = "D36-1463-274-10" } }
                } 
                } 
             }
            }));

            integrationProductsService.Setup(x => x.GetCategoriesDB()).Returns(Task.FromResult(new List<CategoryDB>()
            {
                 new CategoryDB()
                 {
                     Id = "skxgshagc",
                     Name = "Maiôs"
                 }
            }.AsEnumerable()));

            sendMessageToQueueUseCase.Setup(x => x.Queue(It.IsAny<CreateSkuPreDefinedPriceEventV1>())).Callback(() =>
            {
                qtdCallsCreateSkuEventV1++;
            });

            var logger = new Mock<ILogger<IntegrateProductsUseCase>>();

            var integrateProductsUseCase = new IntegrateProductsUseCase(
                configuration.Object,
                sendMessageToQueueUseCase.Object,
                integrationNetshoesService.Object,
                integrationProductsService.Object,
                logger.Object
                );

            // act
            await integrateProductsUseCase.IntegrateProducts();

            // assert
            Assert.AreEqual(1, qtdCallsCreateSkuEventV1);
        }

        [Test]
        public async Task IntegrateProducts_WithProductsSku_MustCallUpdateSkuEvent()
        {
            // prepare
            var qtdCallsUpdateSkuEventV1 = 0;
            var sendMessageToQueueUseCase = new Mock<ISendMessageToQueueUseCase>();
            var integrationNetshoesService = new Mock<IIntegrationNetshoesService>();
            var integrationProductsService = new Mock<IIntegrationServiceProductsService>();

            integrationNetshoesService.Setup(x => x.GetProducts()).Returns(Task.FromResult(new List<ProdutoRplEN>() {
                new ProdutoRplEN()
                {
                    CodigoProdutoPai = "D36-1463-274",
                    CodigoProduto = "D36-1463-274-10",
                    DescricaoFamilia = "Maiôs e Sungas"
                }
            }.AsEnumerable()));

            integrationNetshoesService.Setup(x => x.GetCategories()).Returns(Task.FromResult(new List<CategoryPartner>() {
                new CategoryPartner()
                {
                    DescricaoFamilia = "Maiôs e Sungas"
                }
            }.AsEnumerable()));

            integrationProductsService.Setup(x => x.GetProductsDB()).Returns(Task.FromResult(new Dictionary<string, ProductsDB>()
            {
                { "D36-1463-274", new ProductsDB() { Id = "", Code = "D36-1463-274", Skus = new Dictionary<string, ProductSkuDB>
                {
                    { "D36-1463-274-10", new ProductSkuDB() { Id = "sdfsffsgf", Code = "D36-1463-274-10" } }
                }
                }
             }
            }));

            integrationProductsService.Setup(x => x.GetCategoriesDB()).Returns(Task.FromResult(new List<CategoryDB>()
            {
                 new CategoryDB()
                 {
                     Id = "skxgshagc",
                     Name = "Maiôs"
                 }
            }.AsEnumerable()));

            sendMessageToQueueUseCase.Setup(x => x.Queue(It.IsAny<UpdateSkuPreDefinedPriceEventV1>())).Callback(() =>
            {
                qtdCallsUpdateSkuEventV1++;
            });

            var logger = new Mock<ILogger<IntegrateProductsUseCase>>();

            var integrateProductsUseCase = new IntegrateProductsUseCase(
                configuration.Object,
                sendMessageToQueueUseCase.Object,
                integrationNetshoesService.Object,
                integrationProductsService.Object,
                logger.Object
                );

            // act
            await integrateProductsUseCase.IntegrateProducts();

            // assert
            Assert.AreEqual(1, qtdCallsUpdateSkuEventV1);
        }

        [Test]
        public async Task IntegrateAvailability_WithoutPrices_MustNotCallUpdateProductEvent()
        {
            // prepare
            var qtdCallsAvailabilitySkuEventV1 = 0;
            var sendMessageToQueueUseCase = new Mock<ISendMessageToQueueUseCase>();
            var integrationNetshoesService = new Mock<IIntegrationNetshoesService>();
            var integrationProductsService = new Mock<IIntegrationServiceProductsService>();

            integrationProductsService.Setup(x => x.GetProductsDB()).Returns(Task.FromResult(new Dictionary<string, ProductsDB>()
            {
                { "D36-1463-274", new ProductsDB() { Id = "", Code = "D36-1463-274", Skus = new Dictionary<string, ProductSkuDB>
                {
                    { "D36-1463-274-10", new ProductSkuDB() { Id = "gggkcdsk", Code = "D36-1463-274-10" } }
                }
                }
             }
            }));

            sendMessageToQueueUseCase.Setup(x => x.Queue(It.IsAny<AvailabilitySkuEventV1>())).Callback(() =>
            {
                qtdCallsAvailabilitySkuEventV1++;
            });

            var logger = new Mock<ILogger<IntegrateProductsUseCase>>();

            var integrateProductsUseCase = new IntegrateProductsUseCase(
                configuration.Object,
                sendMessageToQueueUseCase.Object,
                integrationNetshoesService.Object,
                integrationProductsService.Object,
                logger.Object
                );

            // act
            await integrateProductsUseCase.IntegrateAvailability();

            // assert
            Assert.AreEqual(0, qtdCallsAvailabilitySkuEventV1);
        }

        [Test]
        public async Task IntegrateAvailability_WithPrices_MustNotCallUpdateProductEvent()
        {
            // prepare
            var qtdCallsAvailabilitySkuEventV1 = 0;
            var sendMessageToQueueUseCase = new Mock<ISendMessageToQueueUseCase>();
            var integrationNetshoesService = new Mock<IIntegrationNetshoesService>();
            var integrationProductsService = new Mock<IIntegrationServiceProductsService>();

            integrationNetshoesService.Setup(x => x.GetProductsPrice()).Returns(Task.FromResult(new List<PrecoRplEN>()
            {
                new PrecoRplEN()
                {
                    CodigoProdutok__BackingField = "D36-1463-274-10",
                    PrecoTabelak__BackingField = 10,
                    PrecoPromocionalk__BackingField = 7
                }
            }.AsEnumerable()));

            integrationProductsService.Setup(x => x.GetProductsDB()).Returns(Task.FromResult(new Dictionary<string, ProductsDB>()
            {
                { "D36-1463-274", new ProductsDB() { Id = "", Code = "D36-1463-274", Skus = new Dictionary<string, ProductSkuDB>
                {
                    { "D36-1463-274-10", new ProductSkuDB() { Id = "gggkcdsk", Code = "D36-1463-274-10" } }
                }
                }
             }
            }));

            sendMessageToQueueUseCase.Setup(x => x.Queue(It.IsAny<AvailabilitySkuEventV1>())).Callback(() =>
            {
                qtdCallsAvailabilitySkuEventV1++;
            });

            var logger = new Mock<ILogger<IntegrateProductsUseCase>>();

            var integrateProductsUseCase = new IntegrateProductsUseCase(
                configuration.Object,
                sendMessageToQueueUseCase.Object,
                integrationNetshoesService.Object,
                integrationProductsService.Object,
                logger.Object
                );

            // act
            await integrateProductsUseCase.IntegrateAvailability();

            // assert
            Assert.AreEqual(1, qtdCallsAvailabilitySkuEventV1);
        }
    }
}