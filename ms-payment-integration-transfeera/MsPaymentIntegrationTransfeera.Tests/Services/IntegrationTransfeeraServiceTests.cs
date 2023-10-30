using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Request;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Response.Billets;
using MsPaymentIntegrationTransfeera.Api.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MsPaymentIntegrationTransfeera.Tests.Services
{
    public class IntegrationTransfeeraServiceTests
    {
        Mock<IConfiguration> configuration = null;
        [SetUp]
        public void Setup()
        {
            var mockConfSection = new Mock<IConfigurationSection>();

            Mock<IConfigurationSection> sectionUrlLogin = new Mock<IConfigurationSection>();
            Mock<IConfigurationSection> sectionUserAgent = new Mock<IConfigurationSection>();
            Mock<IConfigurationSection> sectionUrlApi = new Mock<IConfigurationSection>();
            Mock<IConfigurationSection> sectionClientId = new Mock<IConfigurationSection>();
            Mock<IConfigurationSection> sectionClientSecret = new Mock<IConfigurationSection>();
            Mock<IConfigurationSection> sectionGrantType = new Mock<IConfigurationSection>();
            sectionUrlLogin.SetupGet(m => m.Value).Returns("http://xpto.com/xpto");
            sectionUserAgent.SetupGet(m => m.Value).Returns("XXXX");
            sectionUrlApi.SetupGet(m => m.Value).Returns("http://xpto.com/xpto");
            sectionClientId.SetupGet(m => m.Value).Returns("XXXX");
            sectionClientSecret.SetupGet(m => m.Value).Returns("XXXX");
            sectionGrantType.SetupGet(m => m.Value).Returns("XXXX");

            configuration = new Mock<IConfiguration>();
            configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "Transfeera:UrlLogin"))).Returns(sectionUrlLogin.Object);
            configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "Transfeera:UserAgent"))).Returns(sectionUserAgent.Object);
            configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "Transfeera:UrlApi"))).Returns(sectionUrlApi.Object);
            configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "Transfeera:ClientId"))).Returns(sectionClientId.Object);
            configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "Transfeera:ClientSecret"))).Returns(sectionClientSecret.Object);
            configuration.Setup(a => a.GetSection(It.Is<string>(s => s == "Transfeera:GrantType"))).Returns(sectionGrantType.Object);
        }

        [Test]
        public async Task GetAuthentication_Success()
        {
            var authRequest = new
            {
                access_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                expires_in = 1800
            };
            var authResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(authRequest)),
            };
            var handlerMock = new Mock<HttpMessageHandler>();

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(authResponse);

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new IntegrationTransfeeraService(httpClient, configuration.Object);

            // act
            //34191790010104351004791020150008389060026000
            var retorno = await service.GetAuthentication();
            //var retorno = service.ValidateTicketOnCIP("00191832500000102260000002834429024087106917");

            // assert
            Assert.AreEqual(authRequest.access_token, retorno.access_token);
            Assert.AreEqual(authRequest.expires_in, retorno.expires_in);
        }

        [Test]
        public void GetAuthentication_Error()
        {
            // prepare
            var responseContent = new
            {
                statusCode = 401,
                error = "Unauthorized",
                message = "Unauthorized"
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(responseContent)),
            };

            handlerMock
               .Protected()
               .Setup<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new IntegrationTransfeeraService(httpClient, configuration.Object);

            // act
            // assert            
            var exception = Assert.Throws<AggregateException>(() =>
            {
                service.GetAuthentication().Wait();
            });

            Assert.AreNotEqual(null, exception.InnerException);
            Assert.AreEqual($"Unexpected return. Details: {responseContent.message}", exception.InnerException.Message);

        }

        [Test]
        public async Task VallidateTicketOnCIP_Success()
        {
            // prepare
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";
            var expiresIn = 1800;
            var handlerMock = new Mock<HttpMessageHandler>();

            var validate = new
            {
                status = "NAO_PAGO",
                message = "Boleto disponível para pagamento",
                barcode_details = new
                {
                    bank_code = "001",
                    bank_name = "Banco do Brasil",
                    barcode = "00191832500000102260000002834429024087106917",
                    digitable_line = "00190000090283442902540871069171183250000010226",
                    due_date = "2020-07-23",
                    value = 102.26,
                    type = "SIMPLES"
                },
                payment_info = new
                {
                    recipient_document = "21.568.259/0001-00",
                    recipient_name = "recipient name",
                    payer_document = "96.906.497/0001-00",
                    payer_name = "payer name",
                    due_date = "2020-07-23",
                    limit_date = "2022-02-22",
                    min_value = 41.72,
                    max_value = 166.9,
                    fine_value = 2.04,
                    interest_value = 0.01,
                    original_value = 102.26,
                    total_updated_value = 104.31,
                    total_discount_value = 0,
                    total_additional_value = 2.05
                }
            };

            var responseAuth = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Format("{{ \"access_token\": \"{0}\",   \"expires_in\": {1} }}", accessToken, expiresIn.ToString())),
            };

            var responseValidate = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(validate)),
            };

            handlerMock
               .Protected()
               .SetupSequence<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(responseAuth)
               .ReturnsAsync(responseValidate);


            var httpClient = new HttpClient(handlerMock.Object);

            var service = new IntegrationTransfeeraService(httpClient, configuration.Object);

            // act
            //34191790010104351004791020150008389060026000
            var retornoValidate = await service.ValidateBilletOnCIP("00191832500000102260000002834429024087106917");

            // assert
            Assert.AreEqual(validate.status, retornoValidate.status);
            Assert.AreEqual(validate.message, retornoValidate.message);
            Assert.AreEqual(validate.barcode_details.bank_code, retornoValidate.barcode_details.bank_code);
            Assert.AreEqual(validate.barcode_details.bank_name, retornoValidate.barcode_details.bank_name);
            Assert.AreEqual(validate.barcode_details.barcode, retornoValidate.barcode_details.barcode);
            Assert.AreEqual(validate.barcode_details.digitable_line, retornoValidate.barcode_details.digitable_line);
            Assert.IsNotNull(validate.barcode_details.due_date, retornoValidate.barcode_details.due_date);
            Assert.AreEqual(validate.barcode_details.value, retornoValidate.barcode_details.value);
            Assert.AreEqual(validate.barcode_details.type, retornoValidate.barcode_details.type);
            Assert.AreEqual(validate.payment_info.recipient_document, retornoValidate.payment_info.recipient_document);
            Assert.AreEqual(validate.payment_info.recipient_name, retornoValidate.payment_info.recipient_name);
            Assert.AreEqual(validate.payment_info.payer_document, retornoValidate.payment_info.payer_document);
            Assert.AreEqual(validate.payment_info.payer_name, retornoValidate.payment_info.payer_name);
            Assert.IsNotNull(validate.payment_info.due_date, retornoValidate.payment_info.due_date);
            Assert.IsNotNull(validate.payment_info.limit_date, retornoValidate.payment_info.limit_date);
            Assert.AreEqual(validate.payment_info.min_value, retornoValidate.payment_info.min_value);
            Assert.AreEqual(validate.payment_info.max_value, retornoValidate.payment_info.max_value);
            Assert.AreEqual(validate.payment_info.fine_value, retornoValidate.payment_info.fine_value);
            Assert.AreEqual(validate.payment_info.interest_value, retornoValidate.payment_info.interest_value);
            Assert.AreEqual(validate.payment_info.original_value, retornoValidate.payment_info.original_value);
            Assert.AreEqual(validate.payment_info.total_updated_value, retornoValidate.payment_info.total_updated_value);
            Assert.AreEqual(validate.payment_info.total_discount_value, retornoValidate.payment_info.total_discount_value);
            Assert.AreEqual(validate.payment_info.total_additional_value, retornoValidate.payment_info.total_additional_value);
        }

        [Test]
        public async Task ValidateTicketOnCIP_Error()
        {
            // prepare
            var responseContent = new
            {
                statusCode = 401,
                error = "Unauthorized",
                message = "Unauthorized"
            };

            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...";
            var expiresIn = 1800;

            var handlerMock = new Mock<HttpMessageHandler>();

            var responseAuth = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(string.Format("{{ \"access_token\": \"{0}\",   \"expires_in\": {1} }}", accessToken, expiresIn.ToString())),
            };

            var responseValidate = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(responseContent)),
            };

            handlerMock
               .Protected()
               .SetupSequence<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(responseAuth)
               .ReturnsAsync(responseValidate);

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new IntegrationTransfeeraService(httpClient, configuration.Object);

            // act
            // assert            
            var retorno = await service.ValidateBilletOnCIP("00191832500000102260000002834429024087106917");

            Assert.AreEqual(false, retorno.isSuccess);
            //Assert.AreEqual(responseContent.message, retorno.message);

        }


        [Test]
        public async Task CheckBalance_Success()
        {
            // prepare
            var authRequest = new
            {
                access_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                expires_in = 1800
            };
            var authResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(authRequest)),
            };
            var checkBalanceRequest = new
            {
                value = 15.2m,
                waiting_value = 1m,
            };
            var checkBalanceResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(checkBalanceRequest)),
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .SetupSequence<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(authResponse)
               .ReturnsAsync(checkBalanceResponse);

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new IntegrationTransfeeraService(httpClient, configuration.Object);

            // act
            var retorno = await service.CheckBalance();

            // assert
            Assert.AreEqual(checkBalanceRequest.value, retorno.value);
            Assert.AreEqual(checkBalanceRequest.waiting_value, retorno.waiting_value);
        }


        [Test]
        public async Task CreateBilletBatch_Success()
        {
            // prepare
            var authRequest = new
            {
                access_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                expires_in = 1800
            };
            var authResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(authRequest)),
            };
            var createBilletRequest = new TransfeeraCreateBilletRequest()
            {
                value = 100,
                barcode = "123",
                description = "desc",
                integration_id = "123",
                payment_date = DateTime.Now.ToString("yyyy-MM-dd")
            };
            var createBilletResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(new TransfeeraCreateBilletBatchResponse()
                {
                    id = 123
                })),
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .SetupSequence<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(authResponse)
               .ReturnsAsync(createBilletResponse);

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new IntegrationTransfeeraService(httpClient, configuration.Object);

            // act
            var retorno = await service.CreateBilletBatch(new TransfeeraCreateBilletBatchRequest()
            {
                name = "BatchWithBillet",
                billets = new List<TransfeeraCreateBilletRequest>
                    {
                        new TransfeeraCreateBilletRequest()
                        {
                            barcode = "00191832500000102260000002834429024087106917",
                            description = $"Pagamento 123",
                            integration_id = "123",
                            payment_date = DateTime.Now.ToString("yyyy-MM-dd"),
                            value = 123,
                        }
                    },
                type = "Billet"
            });

            // assert
            Assert.AreEqual(123, retorno.id);
        }

        [Test]
        public async Task CheckBillet_Success()
        {
            // prepare
            var authRequest = new
            {
                access_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                expires_in = 1800
            };
            var authResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(authRequest)),
            };

            var billet = 123;

            var checkBilletRequest = new
            {
                id = "123"
            };

            var checkBilletResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(checkBilletRequest)),
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .SetupSequence<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(authResponse)
               .ReturnsAsync(checkBilletResponse);

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new IntegrationTransfeeraService(httpClient, configuration.Object);

            // act
            var retorno = await service.GetBillet(billet);

            // assert
            Assert.AreEqual(checkBilletRequest.id, retorno.id);
        }

        [Test]
        public async Task CheckBillets_Success()
        {
            // prepare
            var authRequest = new
            {
                access_token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                expires_in = 1800
            };
            var authResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(authRequest)),
            };

            var lstResponse = new
            {
                data = new List<TransfeeraGetBilletResponse>() 
                {
                    new TransfeeraGetBilletResponse()
                    {
                        id = "123"
                    },
                    new TransfeeraGetBilletResponse()
                    {
                        id = "321"
                    }
                },
                metadata = new
                {
                    pagination = new
                    {
                        itemsPerPage = 45,
                        totalItems = 2
                    }
                }
            };

            var checkBilletsRequest = new TransfeeraGetBilletsRequest()
            {
                initialDate = "2022-03-08",
                endDate = DateTime.Now.ToString("yyyy-MM-dd"),

            };

            var checkBilletsResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(Newtonsoft.Json.JsonConvert.SerializeObject(lstResponse)),
            };

            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
               .Protected()
               .SetupSequence<Task<HttpResponseMessage>>(
                  "SendAsync",
                  ItExpr.IsAny<HttpRequestMessage>(),
                  ItExpr.IsAny<CancellationToken>())
               .ReturnsAsync(authResponse)
               .ReturnsAsync(checkBilletsResponse);

            var httpClient = new HttpClient(handlerMock.Object);

            var service = new IntegrationTransfeeraService(httpClient, configuration.Object);

            // act
            var retorno = await service.GetBillets(checkBilletsRequest);

            // assert
            Assert.AreEqual(2, retorno.Count);
        }
    }
}