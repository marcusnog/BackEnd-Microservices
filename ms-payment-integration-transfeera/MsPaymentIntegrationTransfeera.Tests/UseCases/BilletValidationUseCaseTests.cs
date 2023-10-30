using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO;
using MsPaymentIntegrationTransfeera.Api.Contracts.DTO.Response.ValidateCIP;
using MsPaymentIntegrationTransfeera.Api.Contracts.Services;
using MsPaymentIntegrationTransfeera.Api.Services;
using MsPaymentIntegrationTransfeera.Api.UseCases;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace MsPaymentIntegrationTransfeera.Tests.UseCases
{
    public class BilletValidationUseCaseTests
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

        public async Task TransfeeraBilletValidationUseCase_TypeSimple_ShouldReturnFee_5()
        {
            // prepare
            var campaignId = 1;
            var cpfcnpj = "22222222222";

            var cipResponse = new ValidateCIPResponse()
            {
                status = "NAO_PAGO",
                message = "Boleto disponível para pagamento",
                barcode_details = new BarcodeDetails()
                {
                    bank_code = "001",
                    bank_name = "Banco do Brasil",
                    barcode = "00191832500000102260000002834429024087106917",
                    digitable_line = "00190000090283442902540871069171183250000010226",
                    due_date = "2020-07-23",
                    value = 100m,
                    type = "SIMPLES"
                },
                payment_info = new PaymentInfo()
                {

                    recipient_document = "21.568.259/0001-00",
                    recipient_name = "recipient name",
                    payer_document = "96.906.497/0001-00",
                    payer_name = "payer name",
                    due_date = "2020-07-23",
                    limit_date = "2022-02-22",
                    min_value = 0m,
                    max_value = 0m,
                    fine_value = 0m,
                    interest_value = 0m,
                    original_value = 100m,
                    total_updated_value = 100m,
                    total_discount_value = 0m,
                    total_additional_value = 0m
                }
            };
            
            //var campaignValidator = new Mock<ICampaignValidator>();
            //campaignValidator.Setup(x => x.CampanhaPossuiLoja(It.IsAny<int>(), It.IsAny<int>())).Returns(new BusinessLayer.DTO.Resultado<bool>()
            //{
            //    Sucesso = true,
            //    Data = true
            //});
            //campaignValidator.Setup(x => x.ObterPorId(It.IsAny<int>())).Returns(new BusinessLayer.DTO.Resultado<CampanhaDTO>()
            //{
            //    Sucesso = true,
            //    Data = new CampanhaDTO()
            //    {
            //        FatorConversao = 1
            //    }
            //});

            var transfeeraService = new Mock<IIntegrationTransfeeraService>();
            transfeeraService.Setup(x => x.ValidateBilletOnCIP(It.IsAny<string>())).Returns(Task.FromResult(cipResponse));
            transfeeraService.Setup(x => x.CheckBalance()).Returns(Task.FromResult(new TransfeeraCheckBalanceResponse()
            {
                value = 200
            }));
            var useCase = new BilletValidationUseCase(transfeeraService.Object);

            // act
            var result = await useCase.Execute("", campaignId, cpfcnpj, "", "", 105);

            // assert
            Assert.AreEqual(true, result.Success);
            var billet = result.Data;
            //Assert.AreNotEqual(null, billet);
            //Assert.AreEqual(campaignId, billet.ID_CAMPANHA);
            //Assert.AreEqual(cipResponse.barcode_details.value, billet.VALOR_BILLET);
            //Assert.AreEqual(cipResponse.barcode_details.value + 5, billet.VALOR_BILLET_TAXA);
            //Assert.AreEqual(cipResponse.barcode_details.value + 5, billet.VALOR_BILLET_PONTOS);
            //Assert.AreEqual(cpfcnpj, billet.CPFCNPJ);

            //Assert.AreNotEqual(null, billet.Detalhe);
            //Assert.AreEqual(cipResponse.barcode_details.bank_code, billet.Detalhe.BANK_CODE);
            //Assert.AreEqual(cipResponse.barcode_details.bank_name, billet.Detalhe.BANK_NAME);
            //Assert.AreEqual(cipResponse.barcode_details.barcode, billet.Detalhe.BARCODE);
            //Assert.AreEqual(cipResponse.barcode_details.digitable_line, billet.Detalhe.DIGITABLE_LINE);
            //Assert.AreEqual(cipResponse.barcode_details.due_date, billet.Detalhe.DUE_DATE);
            //Assert.AreEqual(cipResponse.barcode_details.value, billet.Detalhe.VALUE);
            //Assert.AreEqual(cipResponse.barcode_details.type, billet.Detalhe.TYPE);

            //Assert.AreNotEqual(null, billet.InformacoesPagamento);
            //Assert.AreEqual(cipResponse.payment_info.recipient_document, billet.InformacoesPagamento.RECIPIENT_DOCUMENT);
            //Assert.AreEqual(cipResponse.payment_info.recipient_name, billet.InformacoesPagamento.RECIPIENT_NAME);
            //Assert.AreEqual(cipResponse.payment_info.payer_document, billet.InformacoesPagamento.PAYER_DOCUMENT);
            //Assert.AreEqual(cipResponse.payment_info.payer_name, billet.InformacoesPagamento.PAYER_NAME);
            //Assert.AreEqual(cipResponse.payment_info.due_date, billet.InformacoesPagamento.DUE_DATE);
            //Assert.AreEqual(cipResponse.payment_info.limit_date, billet.InformacoesPagamento.LIMIT_DATE);
            //Assert.AreEqual(cipResponse.payment_info.min_value, billet.InformacoesPagamento.MIN_VALUE);
            //Assert.AreEqual(cipResponse.payment_info.max_value, billet.InformacoesPagamento.MAX_VALUE);
            //Assert.AreEqual(cipResponse.payment_info.fine_value, billet.InformacoesPagamento.FINE_VALUE);
            //Assert.AreEqual(cipResponse.payment_info.interest_value, billet.InformacoesPagamento.INTEREST_VALUE);
            //Assert.AreEqual(cipResponse.payment_info.original_value, billet.InformacoesPagamento.ORIGINAL_VALUE);
            //Assert.AreEqual(cipResponse.payment_info.total_updated_value, billet.InformacoesPagamento.TOTAL_UPDATED_VALUE);
            //Assert.AreEqual(cipResponse.payment_info.total_discount_value, billet.InformacoesPagamento.TOTAL_DISCOUNT_VALUE);
            //Assert.AreEqual(cipResponse.payment_info.total_additional_value, billet.InformacoesPagamento.TOTAL_ADDITIONAL_VALUE);

        }

        [Test]
        public async Task TransfeeraBilletValidationUseCase_TypeArrecadacao_ShouldReturnFee_6()
        {
            // prepare
            var campaignId = 1;
            var cpfCnpj = "223234234210";

            var cipResponse = new ValidateCIPResponse()
            {
                status = "NAO_PAGO",
                message = "Boleto disponível para pagamento",
                barcode_details = new BarcodeDetails()
                {
                    bank_code = "001",
                    bank_name = "Banco do Brasil",
                    barcode = "00191832500000102260000002834429024087106917",
                    digitable_line = "00190000090283442902540871069171183250000010226",
                    due_date = "2020-07-23",
                    value = 100m,
                    type = "ARRECADACAO"
                },
                payment_info = new PaymentInfo()
                {

                    recipient_document = "21.568.259/0001-00",
                    recipient_name = "recipient name",
                    payer_document = "96.906.497/0001-00",
                    payer_name = "payer name",
                    due_date = "2020-07-23",
                    limit_date = "2022-02-22",
                    min_value = 0m,
                    max_value = 0m,
                    fine_value = 0m,
                    interest_value = 0m,
                    original_value = 100m,
                    total_updated_value = 100m,
                    total_discount_value = 0m,
                    total_additional_value = 0m
                }
            };

            //var campaignValidator = new Mock<ICampaignValidator>();
            //campaignValidator.Setup(x => x.CampanhaPossuiLoja(It.IsAny<int>(), It.IsAny<int>())).Returns(new BusinessLayer.DTO.Resultado<bool>()
            //{
            //    Sucesso = true,
            //    Data = true
            //});
            //campaignValidator.Setup(x => x.ObterPorId(It.IsAny<int>())).Returns(new BusinessLayer.DTO.Resultado<CampanhaDTO>()
            //{
            //    Sucesso = true,
            //    Data = new CampanhaDTO()
            //    {
            //        FatorConversao = 1
            //    }
            //});

            var transfeeraService = new Mock<IIntegrationTransfeeraService>();
            transfeeraService.Setup(x => x.ValidateBilletOnCIP(It.IsAny<string>())).Returns(Task.FromResult(cipResponse));
            transfeeraService.Setup(x => x.CheckBalance()).Returns(Task.FromResult(new TransfeeraCheckBalanceResponse()
            {
                value = 200
            }));
            var useCase = new BilletValidationUseCase(transfeeraService.Object);

            // act
            var result = await useCase.Execute("", campaignId, cpfCnpj, "", "", 106);

            // assert
            Assert.AreEqual(true, result.Success);
            var billet = result.Data;
            Assert.AreNotEqual(null, billet);
            Assert.AreEqual(campaignId, billet.ID_CAMPANHA);
            Assert.AreEqual(cipResponse.barcode_details.value, billet.VALOR_BILLET);
            Assert.AreEqual(cipResponse.barcode_details.value + 6, billet.VALOR_BILLET_TAXA);
            Assert.AreEqual(cipResponse.barcode_details.value + 6, billet.VALOR_BILLET_PONTOS);
            Assert.AreEqual(cpfCnpj, billet.CPFCNPJ);

            Assert.AreNotEqual(null, billet.Detalhe);
            Assert.AreEqual(cipResponse.barcode_details.bank_code, billet.Detalhe.BANK_CODE);
            Assert.AreEqual(cipResponse.barcode_details.bank_name, billet.Detalhe.BANK_NAME);
            Assert.AreEqual(cipResponse.barcode_details.barcode, billet.Detalhe.BARCODE);
            Assert.AreEqual(cipResponse.barcode_details.digitable_line, billet.Detalhe.DIGITABLE_LINE);
            Assert.AreEqual(cipResponse.barcode_details.due_date, billet.Detalhe.DUE_DATE);
            Assert.AreEqual(cipResponse.barcode_details.value, billet.Detalhe.VALUE);
            Assert.AreEqual(cipResponse.barcode_details.type, billet.Detalhe.TYPE);

            Assert.AreNotEqual(null, billet.InformacoesPagamento);
            Assert.AreEqual(cipResponse.payment_info.recipient_document, billet.InformacoesPagamento.RECIPIENT_DOCUMENT);
            Assert.AreEqual(cipResponse.payment_info.recipient_name, billet.InformacoesPagamento.RECIPIENT_NAME);
            Assert.AreEqual(cipResponse.payment_info.payer_document, billet.InformacoesPagamento.PAYER_DOCUMENT);
            Assert.AreEqual(cipResponse.payment_info.payer_name, billet.InformacoesPagamento.PAYER_NAME);
            Assert.AreEqual(cipResponse.payment_info.due_date, billet.InformacoesPagamento.DUE_DATE);
            Assert.AreEqual(cipResponse.payment_info.limit_date, billet.InformacoesPagamento.LIMIT_DATE);
            Assert.AreEqual(cipResponse.payment_info.min_value, billet.InformacoesPagamento.MIN_VALUE);
            Assert.AreEqual(cipResponse.payment_info.max_value, billet.InformacoesPagamento.MAX_VALUE);
            Assert.AreEqual(cipResponse.payment_info.fine_value, billet.InformacoesPagamento.FINE_VALUE);
            Assert.AreEqual(cipResponse.payment_info.interest_value, billet.InformacoesPagamento.INTEREST_VALUE);
            Assert.AreEqual(cipResponse.payment_info.original_value, billet.InformacoesPagamento.ORIGINAL_VALUE);
            Assert.AreEqual(cipResponse.payment_info.total_updated_value, billet.InformacoesPagamento.TOTAL_UPDATED_VALUE);
            Assert.AreEqual(cipResponse.payment_info.total_discount_value, billet.InformacoesPagamento.TOTAL_DISCOUNT_VALUE);
            Assert.AreEqual(cipResponse.payment_info.total_additional_value, billet.InformacoesPagamento.TOTAL_ADDITIONAL_VALUE);

        }

        //[Test]
        //public async Task TransfeeraBilletValidationUseCase_CampaignWithoutStore_ShouldReturnError()
        //{
        //    // prepare
        //    var campaignId = 1;
        //    var cpfcnpj = "23243443342";

        //    var cipResponse = new ValidateCIPResponse()
        //    {
        //        status = "NAO_PAGO",
        //        message = "Boleto disponível para pagamento",
        //        barcode_details = new BarcodeDetails()
        //        {
        //            bank_code = "001",
        //            bank_name = "Banco do Brasil",
        //            barcode = "00191832500000102260000002834429024087106917",
        //            digitable_line = "00190000090283442902540871069171183250000010226",
        //            due_date = "2020-07-23",
        //            value = 100m,
        //            type = "SIMPLES"
        //        },
        //        payment_info = new PaymentInfo()
        //        {

        //            recipient_document = "21.568.259/0001-00",
        //            recipient_name = "recipient name",
        //            payer_document = "96.906.497/0001-00",
        //            payer_name = "payer name",
        //            due_date = "2020-07-23",
        //            limit_date = "2022-02-22",
        //            min_value = 0m,
        //            max_value = 0m,
        //            fine_value = 0m,
        //            interest_value = 0m,
        //            original_value = 100m,
        //            total_updated_value = 100m,
        //            total_discount_value = 0m,
        //            total_additional_value = 0m
        //        }
        //    };

        //    //var campaignValidator = new Mock<ICampaignValidator>();
        //    //campaignValidator.Setup(x => x.CampanhaPossuiLoja(It.IsAny<int>(), It.IsAny<int>())).Returns(new BusinessLayer.DTO.Resultado<bool>()
        //    //{
        //    //    Sucesso = true,
        //    //    Data = false
        //    //});
        //    //campaignValidator.Setup(x => x.ObterPorId(It.IsAny<int>())).Returns(new BusinessLayer.DTO.Resultado<CampanhaDTO>()
        //    //{
        //    //    Sucesso = true,
        //    //    Data = new CampanhaDTO()
        //    //    {
        //    //        FatorConversao = 1
        //    //    }
        //    //});

        //    var transfeeraService = new Mock<IIntegrationTransfeeraService>();
        //    transfeeraService.Setup(x => x.ValidateBilletOnCIP(It.IsAny<string>())).Returns(Task.FromResult(cipResponse));
        //    transfeeraService.Setup(x => x.CheckBalance()).Returns(Task.FromResult(new TransfeeraCheckBalanceResponse()
        //    {
        //        value = 200
        //    }));
        //    var useCase = new BilletValidationUseCase(transfeeraService.Object);

        //    // act
        //    var result = await useCase.Execute("", campaignId, cpfcnpj, "", "", 105);

        //    // assert
        //    Assert.AreEqual(false, result.Sucess);
        //    Assert.AreEqual(1, result.ErrorsValidation.Count());
        //    Assert.AreEqual("Campanha não permite pagamento de contas", result.ErrorsValidation[0]);

        //}

        //[Test]
        //public async Task TransfeeraBilletValidationUseCase_CampaignNotFound_ShouldReturnError()
        //{
        //    // prepare
        //    var campaignId = 1;
        //    var cpfcnpj = "2";

        //    var cipResponse = new ValidateCIPResponse()
        //    {
        //        status = "NAO_PAGO",
        //        message = "Boleto disponível para pagamento",
        //        barcode_details = new BarcodeDetails()
        //        {
        //            bank_code = "001",
        //            bank_name = "Banco do Brasil",
        //            barcode = "00191832500000102260000002834429024087106917",
        //            digitable_line = "00190000090283442902540871069171183250000010226",
        //            due_date = "2020-07-23",
        //            value = 100m,
        //            type = "SIMPLES"
        //        },
        //        payment_info = new PaymentInfo()
        //        {

        //            recipient_document = "21.568.259/0001-00",
        //            recipient_name = "recipient name",
        //            payer_document = "96.906.497/0001-00",
        //            payer_name = "payer name",
        //            due_date = "2020-07-23",
        //            limit_date = "2022-02-22",
        //            min_value = 0m,
        //            max_value = 0m,
        //            fine_value = 0m,
        //            interest_value = 0m,
        //            original_value = 100m,
        //            total_updated_value = 100m,
        //            total_discount_value = 0m,
        //            total_additional_value = 0m
        //        }
        //    };

        //    //var campaignValidator = new Mock<ICampaignValidator>();
        //    //campaignValidator.Setup(x => x.CampanhaPossuiLoja(It.IsAny<int>(), It.IsAny<int>())).Returns(new BusinessLayer.DTO.Resultado<bool>()
        //    //{
        //    //    Sucesso = true,
        //    //    Data = true
        //    //});
        //    //campaignValidator.Setup(x => x.ObterPorId(It.IsAny<int>())).Returns(new BusinessLayer.DTO.Resultado<CampanhaDTO>()
        //    //{
        //    //    Sucesso = true,
        //    //    Data = null
        //    //});

        //    var transfeeraService = new Mock<IIntegrationTransfeeraService>();
        //    transfeeraService.Setup(x => x.ValidateBilletOnCIP(It.IsAny<string>())).Returns(Task.FromResult(cipResponse));
        //    transfeeraService.Setup(x => x.CheckBalance()).Returns(Task.FromResult(new TransfeeraCheckBalanceResponse()
        //    {
        //        value = 200
        //    }));
        //    var useCase = new BilletValidationUseCase(transfeeraService.Object);

        //    // act
        //    var result = await useCase.Execute("", campaignId, cpfcnpj, "", "", 105);

        //    // assert
        //    Assert.AreEqual(false, result.Sucess);
        //    Assert.AreEqual(1, result.ErrorsValidation.Count());
        //    Assert.AreEqual($"Não foi possivel obter dados da campanha ({campaignId})", result.ErrorsValidation[0]);

        //}

        [Test]
        public async Task TransfeeraBilletValidationUseCase_CIPValidationFails_ShouldReturnError()
        {
            // prepare
            var campaignId = 1;
            var cpfcnpj = "2";

            var cipResponse = new ValidateCIPResponse()
            {
                message = "Boleto não disponível para pagamento",
            };

            //var campaignValidator = new Mock<ICampaignValidator>();
            //campaignValidator.Setup(x => x.CampanhaPossuiLoja(It.IsAny<int>(), It.IsAny<int>())).Returns(new BusinessLayer.DTO.Resultado<bool>()
            //{
            //    Sucesso = true,
            //    Data = true
            //});
            //campaignValidator.Setup(x => x.ObterPorId(It.IsAny<int>())).Returns(new BusinessLayer.DTO.Resultado<CampanhaDTO>()
            //{
            //    Sucesso = true,
            //    Data = new CampanhaDTO()
            //    {
            //        FatorConversao = 1
            //    }
            //});

            var transfeeraService = new Mock<IIntegrationTransfeeraService>();
            transfeeraService.Setup(x => x.ValidateBilletOnCIP(It.IsAny<string>())).Returns(Task.FromResult(cipResponse));
            transfeeraService.Setup(x => x.CheckBalance()).Returns(Task.FromResult(new TransfeeraCheckBalanceResponse()
            {
                value = 200
            }));
            var useCase = new BilletValidationUseCase(transfeeraService.Object);

            // act
            var result = await useCase.Execute("", campaignId, cpfcnpj, "", "", 105);

            // assert
            Assert.AreEqual(false, result.Success);
            //Assert.AreEqual(1, result.Errors.Count());
            //Assert.AreEqual(cipResponse.message, result.ErrorsValidation[0]);

        }

        [Test]
        public async Task TransfeeraBilletValidationUseCase_UserWithoutNecessaryPointAmmount_ShouldReturnError()
        {
            // prepare
            var campaignId = 1;
            var cpfcnpj = "2";

            var cipResponse = new ValidateCIPResponse()
            {
                status = "NAO_PAGO",
                message = "Boleto disponível para pagamento",
                barcode_details = new BarcodeDetails()
                {
                    bank_code = "001",
                    bank_name = "Banco do Brasil",
                    barcode = "00191832500000102260000002834429024087106917",
                    digitable_line = "00190000090283442902540871069171183250000010226",
                    due_date = "2020-07-23",
                    value = 100m,
                    type = "SIMPLES"
                },
                payment_info = new PaymentInfo()
                {

                    recipient_document = "21.568.259/0001-00",
                    recipient_name = "recipient name",
                    payer_document = "96.906.497/0001-00",
                    payer_name = "payer name",
                    due_date = "2020-07-23",
                    limit_date = "2022-02-22",
                    min_value = 0m,
                    max_value = 0m,
                    fine_value = 0m,
                    interest_value = 0m,
                    original_value = 100m,
                    total_updated_value = 100m,
                    total_discount_value = 0m,
                    total_additional_value = 0m
                }
            };

            //var campaignValidator = new Mock<ICampaignValidator>();
            //campaignValidator.Setup(x => x.CampanhaPossuiLoja(It.IsAny<int>(), It.IsAny<int>())).Returns(new BusinessLayer.DTO.Resultado<bool>()
            //{
            //    Sucesso = true,
            //    Data = true
            //});
            //campaignValidator.Setup(x => x.ObterPorId(It.IsAny<int>())).Returns(new BusinessLayer.DTO.Resultado<CampanhaDTO>()
            //{
            //    Sucesso = true,
            //    Data = new CampanhaDTO()
            //    {
            //        FatorConversao = 1
            //    }
            //});

            var transfeeraService = new Mock<IIntegrationTransfeeraService>();
            transfeeraService.Setup(x => x.ValidateBilletOnCIP(It.IsAny<string>())).Returns(Task.FromResult(cipResponse));
            transfeeraService.Setup(x => x.CheckBalance()).Returns(Task.FromResult(new TransfeeraCheckBalanceResponse()
            {
                value = 200
            }));
            var useCase = new BilletValidationUseCase(transfeeraService.Object);

            // act
            var result = await useCase.Execute("", campaignId, cpfcnpj, "", "", 104);

            // assert
            Assert.AreEqual(false, result.Success);
            //Assert.AreEqual(1, result.ErrorsValidation.Count());
            //Assert.AreEqual($"Você não possui saldo suficiente para o pagamento (Pontos necessários: 105)", result.ErrorsValidation[0]);

        }


        [Test]
        public async Task TransfeeraBilletValidationUseCase_DigiAccountWithouBalance_ShouldReturnError()
        {
            // prepare
            var campaignId = 1;
            var cpfcnpj = "2";

            var cipResponse = new ValidateCIPResponse()
            {
                status = "NAO_PAGO",
                message = "Boleto disponível para pagamento",
                barcode_details = new BarcodeDetails()
                {
                    bank_code = "001",
                    bank_name = "Banco do Brasil",
                    barcode = "00191832500000102260000002834429024087106917",
                    digitable_line = "00190000090283442902540871069171183250000010226",
                    due_date = "2020-07-23",
                    value = 100m,
                    type = "SIMPLES"
                },
                payment_info = new PaymentInfo()
                {

                    recipient_document = "21.568.259/0001-00",
                    recipient_name = "recipient name",
                    payer_document = "96.906.497/0001-00",
                    payer_name = "payer name",
                    due_date = "2020-07-23",
                    limit_date = "2022-02-22",
                    min_value = 0m,
                    max_value = 0m,
                    fine_value = 0m,
                    interest_value = 0m,
                    original_value = 100m,
                    total_updated_value = 100m,
                    total_discount_value = 0m,
                    total_additional_value = 0m
                }
            };

            //var campaignValidator = new Mock<ICampaignValidator>();
            //campaignValidator.Setup(x => x.CampanhaPossuiLoja(It.IsAny<int>(), It.IsAny<int>())).Returns(new BusinessLayer.DTO.Resultado<bool>()
            //{
            //    Sucesso = true,
            //    Data = true
            //});
            //campaignValidator.Setup(x => x.ObterPorId(It.IsAny<int>())).Returns(new BusinessLayer.DTO.Resultado<CampanhaDTO>()
            //{
            //    Sucesso = true,
            //    Data = new CampanhaDTO()
            //    {
            //        FatorConversao = 1
            //    }
            //});

            var transfeeraService = new Mock<IIntegrationTransfeeraService>();
            transfeeraService.Setup(x => x.ValidateBilletOnCIP(It.IsAny<string>())).Returns(Task.FromResult(cipResponse));
            transfeeraService.Setup(x => x.CheckBalance()).Returns(Task.FromResult(new TransfeeraCheckBalanceResponse()
            {
                value = 0
            }));
            var useCase = new BilletValidationUseCase(transfeeraService.Object);

            // act
            var result = await useCase.Execute("", campaignId, cpfcnpj, "", "", 105);

            // assert
            Assert.AreEqual(false, result.Success);
            //Assert.AreEqual(1, result.ErrorsValidation.Count());
            //Assert.AreEqual($"Não foi possivel realizar a validação, por favor tente novamente mais tarde", result.ErrorsValidation[0]);

        }
    }
}
