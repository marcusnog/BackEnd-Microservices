using Microsoft.Extensions.Configuration;
using MsProductIntegrationNetShoes.Contracts.DTOs;
using MsProductIntegrationNetShoes.Contracts.Service;
using NetshoesService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationNetShoes.Services
{
    public class IntegrationNetshoesService : IIntegrationNetshoesService
    {
        readonly IConfiguration _configuration;
        readonly string _productsFileName;
        readonly NetshoesService.ListarProdutosRequest _request;
        readonly NetshoesService.ListarPrecosRequest _requestPrice;
        readonly NetshoesEstoqueService.ProdutoEstoqueZeradoRequest _requestProdZerado;

        public IntegrationNetshoesService(IConfiguration configuration)
        {
            _configuration = configuration;

            _productsFileName = _configuration.GetValue<string>("Products:FileName");

            _request = new NetshoesService.ListarProdutosRequest() 
            { 
                identificacaoLoja = _configuration.GetValue<string>("IdentificacaoLoja"), 
                chaveIdentificacao = _configuration.GetValue<string>("ChaveIdentificacao") 
            };

            _requestPrice = new NetshoesService.ListarPrecosRequest()
            {
                identificacaoLoja = _configuration.GetValue<string>("IdentificacaoLoja"),
                chaveIdentificacao = _configuration.GetValue<string>("ChaveIdentificacao")
            };

            _requestProdZerado = new NetshoesEstoqueService.ProdutoEstoqueZeradoRequest()
            {
                identificacaoLoja = _configuration.GetValue<string>("IdentificacaoLoja"),
                chaveIdentificacao = _configuration.GetValue<string>("ChaveIdentificacao")
            };
        }

        public async Task<IEnumerable<CategoryPartner>> GetCategories()
        {
            CategoryPartner category;
            List<CategoryPartner> lstCategorias = new();

            NetshoesService.ListarProdutosResponse products;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            NetshoesService.ProdutoWCFClient client = new NetshoesService.ProdutoWCFClient();
            client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication();
            client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication.CertificateValidationMode = X509CertificateValidationMode.None;

            products = await client.ListarProdutosAsync(_request);

            var produtos = products.ListarProdutosResult;

            foreach (var item in produtos)
            {
                category = new();
                category.CodigoProduto = item.CodigoProdutoPai;
                category.DescricaoFamilia = Util.Decode(item.DescricaoFamilia.TrimEnd());
                category.DescricaoDepartamento = Util.Decode(item.DescricaoDepartamento.TrimEnd());
                category.DescricaoTipoProduto = Util.Decode(item.DescricaoTipoProduto.TrimEnd());

                lstCategorias.Add(category);
            }

            return lstCategorias;
        }

        #region products
        public async Task<IEnumerable<NetshoesService.ProdutoRplEN>> GetProducts()
        {
            NetshoesService.ListarProdutosResponse products;
            NetshoesService.ListarPrecosResponse price;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            NetshoesService.ProdutoWCFClient client = new NetshoesService.ProdutoWCFClient();
            client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication();
            client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication.CertificateValidationMode = X509CertificateValidationMode.None;

            products = await client.ListarProdutosAsync(_request);
            //price = await client.ListarPrecosAsync(_requestPrice);

            var produtos = products.ListarProdutosResult;
            //var precos = price.ListarPrecosResult;

            return produtos;
        }

        #endregion

        #region availability
        public async Task<IEnumerable<string>> GetAvailabilities()
        {
            List<string> ListaSKUsProdutosZerados = new List<string>();
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            NetshoesEstoqueService.EstoqueWCFClient client = new NetshoesEstoqueService.EstoqueWCFClient();
            client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication();
            client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication.CertificateValidationMode = X509CertificateValidationMode.None;

            var lstEstoqueZerado = await client.ProdutoEstoqueZeradoAsync(_requestProdZerado);

            ListaSKUsProdutosZerados = lstEstoqueZerado.ProdutoEstoqueZeradoResult?.ToList();

            return ListaSKUsProdutosZerados;
        }

        public async Task<IEnumerable<NetshoesService.PrecoRplEN>> GetProductsPrice()
        {
            NetshoesService.ListarPrecosResponse price;
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            NetshoesService.ProdutoWCFClient client = new NetshoesService.ProdutoWCFClient();
            client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication();
            client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication.CertificateValidationMode = X509CertificateValidationMode.None;

            price = await client.ListarPrecosAsync(_requestPrice);

            var precos = price.ListarPrecosResult;

            return precos;
        }

        #endregion


        public void ConfirmReceiptPrices(NetshoesService.ConfirmarRecebimentoPrecoRequest request)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            NetshoesService.ProdutoWCFClient client = new NetshoesService.ProdutoWCFClient();
            client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication();
            client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication.CertificateValidationMode = X509CertificateValidationMode.None;

            client.ConfirmarRecebimentoPrecoAsync(request);
        }
        public void ConfirmReceiptProducts(NetshoesService.ConfirmarRecebimentoProdutoRequest request)
        {
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            NetshoesService.ProdutoWCFClient client = new NetshoesService.ProdutoWCFClient();
            client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication = new X509ServiceCertificateAuthentication();
            client.ClientCredentials.ServiceCertificate.SslCertificateAuthentication.CertificateValidationMode = X509CertificateValidationMode.None;

            client.ConfirmarRecebimentoProdutoAsync(request);
        }

    }
}
