using Microsoft.Extensions.Configuration;
using MsProductIntegrationGiffty.Contracts.DTOs;
using MsProductIntegrationGiffty.Contracts.Services;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace MsProductIntegrationGiffty.Services
{
    public class IntegrationService : IIntegrationService
    {
        readonly IConfiguration _configuration;
        readonly string _productsFileName;
        readonly string _environment;
        readonly string _chave;

        public IntegrationService(IConfiguration configuration)
        {
            _configuration = configuration;

            _productsFileName = _configuration.GetValue<string>("Products:FileName");
            _environment = _configuration.GetValue<string>("Environment");
            _chave = _configuration.GetValue<string>("Chave");
        }

        public async Task<IEnumerable<string>> GetXml(string fileName)
        {
            List<string> result = new();

            if (_environment == "Homologacao")
            {
                GifftyServiceHml.PontoDeAcessoParceirosGifttyPortTypeClient clientHml = new();
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                foreach (var itemLista in new CodigosFornecedoresGiffty().lista)
                {
                    var request = new Request()
                    {
                        Produtos = new ItemRequest
                        {
                            Projeto = itemLista
                        }
                    };

                    var requestProdutos = Util.ToXml(request.Produtos, fileName);
                    var retornoWsdl = await clientHml.ConsultarProdutosAsync(requestProdutos.ToString());

                    result.Add(retornoWsdl);
                }

                GC.Collect();
            }
            else
            {
                GifftyService.PontoDeAcessoParceirosGifttyPortTypeClient client = new();
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                foreach (var itemLista in new CodigosFornecedoresGiffty().lista)
                {
                    var request = new Request()
                    {
                        Produtos = new ItemRequest
                        {
                            Projeto = itemLista
                        }
                    };

                    var requestProdutos = Util.ToXml(request.Produtos, fileName);
                    var retornoWsdl = await client.ConsultarProdutosAsync(requestProdutos.ToString());

                    result.Add(retornoWsdl);
                }

                GC.Collect();
            }

            return result;
        }

        static T GetValue<T>(XElement element)
        {
            if (element == null)
                return default(T);

            return (T)Convert.ChangeType(element.Value, typeof(T));
        }

        #region products
        public async Task<IEnumerable<ProductPartner>> GetProducts()
        {
            List<ProductPartner> result = new();

            var xmlDocs = await GetXml(_productsFileName);

            if (xmlDocs?.Any() != true)
                return Enumerable.Empty<ProductPartner>();

            foreach (var xml in xmlDocs)
                result.AddRange(ReadProductsXmlFile(xml));

            return result;
        }

        static IEnumerable<ProductPartner> ReadProductsXmlFile(string xml)
        {
            ProductPartner product;
            List<ProductPartner> result = new();
            var xmlDoc = XDocument.Parse(xml);

            var produtos = xmlDoc.Descendants("Produtos").Descendants("Produto");

            foreach (var produto in produtos)
            {
                if (produto == null) continue;

                product = new();
                product.Fotos = new List<Fotos>();
                product.Imagens = new List<Imagens>();
                product.DetalhesDescricao = new List<Detalhes>();
                product.InformacoesAdicionais = new List<Informacao>();
                product.Codigo = GetValue<string>(produto.Element("codigo"));
                product.Fabricante = Util.Decode(GetValue<string>(produto.Element("fabricanteNome")));
                product.Departamento = Util.Decode(GetValue<string>(produto.Element("departamentoNome")));
                product.Categoria = Util.Decode(GetValue<string>(produto.Element("categoriaNome")));
                product.Preco = GetValue<decimal>(produto.Element("preco"));
                product.PrecoDe = GetValue<decimal>(produto.Element("precoDe"));
                product.NomeProduto = Util.Decode(GetValue<string>(produto.Element("produtoNome")));
                product.Descricao = Util.Decode(GetValue<string>(produto.Element("descricao")));
                product.Habilitado = GetValue<string>(produto.Element("habilitado"));
                product.Frete = GetValue<decimal>(produto.Element("frete"));
                product.ImpostosFrete = GetValue<decimal>(produto.Element("impostosFrete"));
                product.CategoriaId = GetValue<string>(produto.Element("categoriaId"));
                product.DepartamentoId = GetValue<string>(produto.Element("departamentoId"));
                product.FabricanteId = GetValue<string>(produto.Element("fabricanteId"));
                product.FornecedorId = GetValue<string>(produto.Element("fornecedorId"));
                product.FornecedorNome = Util.Decode(GetValue<string>(produto.Element("fornecedorNome")));
                product.Desconto = GetValue<string>(produto.Element("desconto"));
                product.Taxa = GetValue<decimal>(produto.Element("taxa"));
                product.ImpostosTaxa = GetValue<decimal>(produto.Element("impostosTaxa"));
                product.PrecoBTD = GetValue<string>(produto.Element("precoBTD"));
                product.PrecoBTDI = GetValue<string>(produto.Element("precoBTDI"));
                product.PrecoBTDIsemFrete = GetValue<string>(produto.Element("precoBTDIsemFrete"));
                product.TipoProduto = GetValue<string>(produto.Element("tipoProduto"));

                var fotos = produto.Descendants("Fotos");

                if (fotos.Any())
                {
                    foreach (var item in fotos)
                    {
                        if (item == null) continue;

                        var foto = new Fotos();

                        foto.FotoP = GetValue<string>(item.Element("p"));
                        foto.FotoM = GetValue<string>(item.Element("m"));
                        foto.FotoG = GetValue<string>(item.Element("g"));

                        product.Fotos.Add(foto);
                    }
                }

                var imagens = produto.Descendants("Imagens");

                if (imagens.Any())
                {
                    foreach (var item in imagens)
                    {
                        if (item == null) continue;

                        var imagen = new Imagens();

                        imagen.ImagemPP = GetValue<string>(item.Element("pp"));
                        imagen.ImagemP = GetValue<string>(item.Element("p"));
                        imagen.ImagemM = GetValue<string>(item.Element("m"));
                        imagen.ImagemG = GetValue<string>(item.Element("g"));
                        imagen.ImagemGG = GetValue<string>(item.Element("gg"));

                        product.Imagens.Add(imagen);
                    }
                }

                var dimensoes = produto.Descendants("Dimensoes").FirstOrDefault();

                product.Altura = GetValue<string>(dimensoes.Element("altura"));
                product.Largura = GetValue<string>(dimensoes.Element("largura"));
                product.Profundidade = GetValue<string>(dimensoes.Element("profundidade"));
                product.Peso = GetValue<string>(dimensoes.Element("peso"));

                var informacoesAdicionais = produto.Descendants("InformacoesAdicionais").Descendants("Informacao");
                

                if (informacoesAdicionais.Any())
                {
                    foreach (var item in informacoesAdicionais)
                    {
                        if (item == null) continue;

                        var infosAdicionais = new Informacao();

                        infosAdicionais.chave = GetValue<string>(item.Element("chave"));
                        infosAdicionais.valor = Util.Decode(GetValue<string>(item.Element("valor")));

                        product.InformacoesAdicionais.Add(infosAdicionais);
                    }
                }

                var detalhesDescricao = produto.Descendants("DetalhesDescricao");

                if (detalhesDescricao.Any())
                {
                    foreach (var item in detalhesDescricao)
                    {
                        if (item == null) continue;

                        var detalhes = new Detalhes();

                        detalhes.prazo = Util.Decode(GetValue<string>(item.Element("prazo")));
                        detalhes.validade = Util.Decode(GetValue<string>(item.Element("validade")));
                        detalhes.utilizacao = Util.Decode(GetValue<string>(item.Element("utilizacao")));
                        detalhes.descricao = Util.Decode(GetValue<string>(item.Element("descricao")));

                        product.DetalhesDescricao.Add(detalhes);
                    }
                }

                result.Add(product);
            }

            return result;

            //var ListaProdutos = result.ToList();
            //List<ProductParceiro> ListaParaRemover = new List<ProductParceiro>();
            //foreach (var produto in ListaProdutos)
            //{
            //    if (produto.ModelProduto != "Virtual")
            //    {
            //        if (!produto.Frete.HasValue || produto.Frete.Value == 0 || !produto.ImpostosFrete.HasValue || produto.ImpostosFrete.Value == 0)
            //        {
            //            ListaParaRemover.Add(produto);
            //        }
            //    }
            //}

            //if (ListaParaRemover.Count > 0)
            //{
            //    foreach (var produto in ListaParaRemover)
            //    {
            //        ListaProdutos.Remove(produto);
            //    }

            //}

            //result = ListaProdutos;
        }

        #endregion

        #region availability

        public async Task<IEnumerable<string>> GetXmlAvailability(string produto)
        {
            List<string> result = new();

            if (_environment == "Homologacao")
            {
                GifftyServiceHml.PontoDeAcessoParceirosGifttyPortTypeClient clientHml = new();
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var request = new RequestAvailability()
                {
                    Availability = new ItemAvailabilityRequest
                    {
                        Produto = produto,
                        chave = _chave
                    }
                };

                var requestEstoques = Util.ToXml(request, "Estoque");
                var retornoWsdl = await clientHml.ConsultarEstoqueAsync(requestEstoques.ToString());

                result.Add(retornoWsdl);

                GC.Collect();
            }
            else
            {
                GifftyServiceHml.PontoDeAcessoParceirosGifttyPortTypeClient clientHml = new();
                ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                var request = new RequestAvailability()
                {
                    Availability = new ItemAvailabilityRequest
                    {
                        Produto = produto,
                        chave = _chave
                    }
                };

                var requestEstoques = Util.ToXml(request, "Estoque");
                var retornoWsdl = await clientHml.ConsultarEstoqueAsync(requestEstoques.ToString());

                result.Add(retornoWsdl);

                GC.Collect();
            }

            return result;
        }

        public async Task<IEnumerable<ProductPartner>> GetAvailabilities()
        {
            List<ProductPartner> result = new();

            var xmlDocs = await GetXml(_productsFileName);

            if (xmlDocs?.Any() != true)
                return Enumerable.Empty<ProductPartner>();

            foreach (var xml in xmlDocs)
                result.AddRange(ReadAvailabilitiesXmlFile(xml));

            return result;
        }

        static IEnumerable<ProductPartner> ReadAvailabilitiesXmlFile(string xml)
        {
            ProductPartner product;
            List<ProductPartner> result = new();
            var xmlDoc = XDocument.Parse(xml);

            var produtos = xmlDoc.Descendants("Produtos").Descendants("Produto");

            foreach (var produto in produtos)
            {
                if (produto == null) continue;

                product = new();
                product.Codigo = GetValue<string>(produto.Element("codigo"));
                product.Fabricante = Util.Decode(GetValue<string>(produto.Element("fabricanteNome")));
                product.Departamento = Util.Decode(GetValue<string>(produto.Element("departamentoNome")));
                product.Categoria = Util.Decode(GetValue<string>(produto.Element("categoriaNome")));
                product.Preco = GetValue<decimal>(produto.Element("preco"));
                product.PrecoDe = GetValue<decimal>(produto.Element("precoDe"));
                product.NomeProduto = Util.Decode(GetValue<string>(produto.Element("produtoNome")));
                product.Descricao = Util.Decode(GetValue<string>(produto.Element("descricao")));
                product.Habilitado = GetValue<string>(produto.Element("habilitado"));
                product.Frete = GetValue<decimal>(produto.Element("frete"));
                product.ImpostosFrete = GetValue<decimal>(produto.Element("impostosFrete"));
                product.CategoriaId = GetValue<string>(produto.Element("categoriaId"));
                product.DepartamentoId = GetValue<string>(produto.Element("departamentoId"));
                product.FabricanteId = GetValue<string>(produto.Element("fabricanteId"));
                product.FornecedorId = GetValue<string>(produto.Element("fornecedorId"));
                product.FornecedorNome = Util.Decode(GetValue<string>(produto.Element("fornecedorNome")));
                product.Desconto = GetValue<string>(produto.Element("desconto"));
                product.Taxa = GetValue<decimal>(produto.Element("taxa"));
                product.ImpostosTaxa = GetValue<decimal>(produto.Element("impostosTaxa"));
                product.PrecoBTD = GetValue<string>(produto.Element("precoBTD"));
                product.PrecoBTDI = GetValue<string>(produto.Element("precoBTDI"));
                product.PrecoBTDIsemFrete = GetValue<string>(produto.Element("precoBTDIsemFrete"));
                product.TipoProduto = GetValue<string>(produto.Element("tipoProduto"));

                var fotos = produto.Descendants("Fotos");

                if (fotos.Any())
                {
                    foreach (var item in fotos)
                    {
                        if (item == null) continue;

                        var foto = new Fotos();

                        foto.FotoP = GetValue<string>(item.Element("p"));
                        foto.FotoM = GetValue<string>(item.Element("m"));
                        foto.FotoG = GetValue<string>(item.Element("G"));

                        product.Fotos.Add(foto);
                    }
                }

                var imagens = produtos.Descendants("Imagens");

                if (imagens.Any())
                {
                    foreach (var item in imagens)
                    {
                        if (item == null) continue;

                        var imagen = new Imagens();

                        imagen.ImagemPP = GetValue<string>(item.Element("pp"));
                        imagen.ImagemP = GetValue<string>(item.Element("p"));
                        imagen.ImagemM = GetValue<string>(item.Element("m"));
                        imagen.ImagemG = GetValue<string>(item.Element("g"));
                        imagen.ImagemGG = GetValue<string>(item.Element("gg"));

                        product.Imagens.Add(imagen);
                    }
                }

                var dimensoes = produto.Descendants("Dimensoes").FirstOrDefault();

                product.Altura = GetValue<string>(dimensoes.Element("altura"));
                product.Largura = GetValue<string>(dimensoes.Element("largura"));
                product.Profundidade = GetValue<string>(dimensoes.Element("profundidade"));
                product.Peso = GetValue<string>(dimensoes.Element("peso"));

                var informacoesAdicionais = produto.Descendants("InformacoesAdicionais");

                if (informacoesAdicionais.Any())
                {
                    foreach (var item in informacoesAdicionais)
                    {
                        if (item == null) continue;

                        var infosAdicionais = new Informacao();

                        infosAdicionais.chave = GetValue<string>(item.Element("chave"));
                        infosAdicionais.valor = Util.Decode(GetValue<string>(item.Element("valor")));

                        product.InformacoesAdicionais.Add(infosAdicionais);
                    }
                }

                var detalhesDescricao = produto.Descendants("DetalhesDescricao");

                if (detalhesDescricao.Any())
                {
                    foreach (var item in detalhesDescricao)
                    {
                        if (item == null) continue;

                        var detalhes = new Detalhes();

                        detalhes.prazo = Util.Decode(GetValue<string>(item.Element("prazo")));
                        detalhes.validade = Util.Decode(GetValue<string>(item.Element("validade")));
                        detalhes.utilizacao = Util.Decode(GetValue<string>(item.Element("utilizacao")));
                        detalhes.descricao = Util.Decode(GetValue<string>(item.Element("descricao")));

                        product.DetalhesDescricao.Add(detalhes);
                    }
                }

                result.Add(product);
            }

            return result;
        }

        //public async Task<IEnumerable<AvailabilityPartner>> GetAvailabilities(string produto)
        //{
        //    List<AvailabilityPartner> result = new();

        //    var xmlDocs = await GetXmlAvailability(produto);

        //    if (xmlDocs.Any() != true)
        //        return Enumerable.Empty<AvailabilityPartner>();

        //    foreach (var xml in xmlDocs)
        //        result.Add(ReadAvailabilitiesXmlFile(xml));

        //    return result;
        //}

        //static AvailabilityPartner ReadAvailabilitiesXmlFile(string xml)
        //{
        //    AvailabilityPartner product = new();
        //    var xmlDoc = XDocument.Parse(xml);

        //    var disponibilidade = xmlDoc.Descendants("Estoque").Descendants("Item");

        //    product.produto = Convert.ToInt32(disponibilidade.Descendants("produto"));
        //    product.quantidade = Convert.ToInt32(disponibilidade.Descendants("quantidade"));
        //    product.preco = Convert.ToDecimal(disponibilidade.Descendants("preco"));

        //    return product;
        //}

        #endregion

        #region stores

        public async Task<IEnumerable<StoreParceiro>> GetStores()
        {
            List<StoreParceiro> result = new();
            var xmlDocs = await GetXml(_productsFileName);

            if (xmlDocs?.Any() != true)
                return Enumerable.Empty<StoreParceiro>();

            foreach (var xml in xmlDocs)
                result.AddRange(ReadStoresXmlFile(xml));

            return result;
        }

        static IEnumerable<StoreParceiro> ReadStoresXmlFile(string xml)
        {
            StoreParceiro store;
            List<StoreParceiro> result = new();
            var xmlDoc = XDocument.Parse(xml);

            var lojas = xmlDoc.Descendants("Produtos").Descendants("Produto");

            foreach (var loja in lojas)
            {
                if (loja == null) continue;

                store = new();
                store.IdFabricante = GetValue<int>(loja.Element("fabricanteId"));
                store.NomeFabricante = Util.Decode(GetValue<string>(loja.Element("fabricanteNome")));
                store.IdFornecedor = GetValue<int>(loja.Element("fornecedorId"));
                store.NomeFornecedor = Util.Decode(GetValue<string>(loja.Element("fornecedorNome")));

                result.Add(store);
            }

            return result;
        }

        #endregion


        public IEnumerable<CategoriesPartner> GetCategories()
        {
            return new List<CategoriesPartner>()
            {
                new CategoriesPartner()
                {
                    DescCategory = "VIRTUAL"
                },
                new CategoriesPartner()
                {
                    DescCategory = "VOUCHER"
                },
                new CategoriesPartner()
                {
                    DescCategory = "FÍSICO"
                }
            };
        }
    }
}
