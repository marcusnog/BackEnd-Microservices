using Microsoft.Extensions.Configuration;
using MsProductIntegrationMagalu.Api.Contracts.DTOs;
using MsProductIntegrationMagalu.Contracts.DTOs;
using MsProductIntegrationMagalu.Contracts.Services;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MsProductIntegrationMagalu.Services
{
    public class IntegrationService : IIntegrationService
    {
        readonly IConfiguration _configuration;
        readonly string _productsFileName;
        readonly string _productsUrl;
        readonly string _keyParceiro;
        int retry = 1;

        public object JsonConvert { get; private set; }

        public IntegrationService(IConfiguration configuration)
        {
            _configuration = configuration;

            _productsFileName = _configuration.GetValue<string>("Products:FileName");
            _productsUrl = _configuration.GetValue<string>("Products:UrlParceiro");
            _keyParceiro = _configuration.GetValue<string>("Products:KeyParceiro");
        }

        public async Task<IEnumerable<string>> GetXml(string url, IEnumerable<string> ListStoreCampaignCodes)
        {
            try
            {
                var client = new HttpClient();
                List<string> result = new();
                client.DefaultRequestHeaders.Add("Authorization", _keyParceiro);
                client.BaseAddress = new($"{url}");
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/xml"));
                client.Timeout = new TimeSpan(1, 0, 0);

                // Fazer um loop pegando os CodigoCampanhaFornecedor da tabela de CampanhaLoja passando o id da loja magalu = 150
                // Passando um fixo só pra teste
                //int StoreCampaignCode = 1486;

                if (retry <= 5)
                {
                    foreach (var StoreCampaignCode in ListStoreCampaignCodes)
                    {
                        var retorno = await client.GetAsync($"{url}?IdResgateCampanha={StoreCampaignCode}&cursor=0&page_size=1000&groupImages=1");
                        var content = retorno.Content;
                        var stringRetorno = await content.ReadAsByteArrayAsync();

                        if (!retorno.IsSuccessStatusCode)
                            throw new Exception("retorno inesperado", new Exception());

                        result.Add(Encoding.UTF8.GetString(stringRetorno.ToArray()));

                        var xmlDoc = XDocument.Parse(Encoding.UTF8.GetString(stringRetorno.ToArray()));
                        XNamespace rs = "urn:schemas-microsoft-com:rowset";
                        XNamespace z = "#RowsetSchema";

                        var produtos = xmlDoc.Descendants(rs + "data").Descendants(z + "row");

                        var hasNext = GetValue<string>(produtos.LastOrDefault().Attribute("hasNext"));
                        var nextCursorId = GetValue<string>(produtos.LastOrDefault().Attribute("nextCursorId"));

                        while (hasNext != "False" && Convert.ToInt32(nextCursorId) > 0)
                        {
                            var retorno2 = await client.GetAsync($"{url}?IdResgateCampanha={StoreCampaignCode}&cursor={nextCursorId}&page_size=1000&groupImages=1");
                            var content2 = retorno2.Content;
                            var stringRetorno2 = content2.ReadAsByteArrayAsync().Result;

                            if (!retorno2.IsSuccessStatusCode)
                                throw new Exception("retorno inesperado", new Exception());

                            result.Add(Encoding.UTF8.GetString(stringRetorno2.ToArray()));

                            var xmlDoc2 = XDocument.Parse(Encoding.UTF8.GetString(stringRetorno2.ToArray()));
                            XNamespace rs2 = "urn:schemas-microsoft-com:rowset";
                            XNamespace z2 = "#RowsetSchema";

                            var produtos2 = xmlDoc2.Descendants(rs2 + "data").Descendants(z2 + "row");

                            hasNext = GetValue<string>(produtos2.LastOrDefault().Attribute("hasNext"));
                            nextCursorId = GetValue<string>(produtos2.LastOrDefault().Attribute("nextCursorId"));
                        }

                        GC.Collect();
                    }
                }
                else
                    throw new Exception("Limite de tentativas excedido!");

                return result;
            }
            catch (Exception ex)
            {
                retry++;
                GetXml(_productsUrl, ListStoreCampaignCodes);
                throw ex;
            }
        }

        static T GetValue<T>(XAttribute attribute)
        {
            if (attribute == null)
                return default(T);

            return (T)Convert.ChangeType(attribute.Value, typeof(T));
        }

        #region products
        public async Task<ProductsAndCategoriesPartner> GetProductsAndCategories(IEnumerable<string> StoreCodes)
        {
            ProductsAndCategoriesPartner result = new();
            result.ProductsPartner = new();
            result.CategoriesPartner = new();

            var xmlDocs = await GetXml(_productsUrl, StoreCodes);

            if (xmlDocs?.Any() != true)
                return new ProductsAndCategoriesPartner();

            foreach (var xml in xmlDocs)
            {
                var productsAndCategories = ReadProductsXmlFile(xml);
                result.CategoriesPartner.AddRange(productsAndCategories.CategoriesPartner);
                result.ProductsPartner.AddRange(productsAndCategories.ProductsPartner);
            }

            return result;
        }

        static ProductsAndCategoriesPartner ReadProductsXmlFile(string xml)
        {
            ProductPartner product;
            List<ProductPartner> result = new();
            CategoryPartner cat;
            List<CategoryPartner> resultCat = new();

            ProductsAndCategoriesPartner resultProductsAndCategories = new();

            var xmlDoc = XDocument.Parse(xml);
            XNamespace rs = "urn:schemas-microsoft-com:rowset";
            XNamespace z = "#RowsetSchema";

            var produtos = xmlDoc.Descendants(rs + "data").Descendants(z + "row");

            foreach (var produto in produtos)
            {
                if (produto == null || produto.Attribute("hasNext")?.Value == "True" || produto.Attribute("hasNext")?.Value == "False") continue;

                product = new();
                product.Codigo = GetValue<string>(produto.Attribute("CODIGO"));
                product.Descricao = Util.Decode(GetValue<string>(produto.Attribute("DESCRICAO")));
                product.Voltagem = Utils.Utils.GetVoltagm(GetValue<int>(produto.Attribute("VOLTAGEM")));
                product.Cor = GetValue<string>(produto.Attribute("COR"));
                product.Tamanho = GetValue<string>(produto.Attribute("TAMANHO"));
                product.Modelo = GetValue<string>(produto.Attribute("MODELO"));
                product.Ativo = GetValue<bool>(produto.Attribute("ATIVO"));
                product.ClassificacaoFiscaL = GetValue<string>(produto.Attribute("CLASSIFICACAO_FISCAL"));
                product.DataAlteracao = GetValue<string>(produto.Attribute("DATA_ALTERACAO"));
                product.Categoria = GetValue<string>(produto.Attribute("CATEGORIA"));
                product.DescCategoria = Util.Decode(GetValue<string>(produto.Attribute("DESC_CATEGORIA")));
                product.SubCategoria = GetValue<string>(produto.Attribute("SUBCATEGORIA"));
                product.DescSubcategoria = Util.Decode(GetValue<string>(produto.Attribute("DESC_SUBCATEGORIA")));
                product.Images = Util.Decode(GetValue<string>(produto.Attribute("IMAGES")));
                product.ImagemCategoria = GetValue<string>(produto.Attribute("IMAGEM_CATEGORIA"));
                product.ImagemProdutoDetalhe = GetValue<string>(produto.Attribute("IMAGEM_PRODUTO_DETALHE"));
                product.ImagemProdutoGrande = GetValue<string>(produto.Attribute("IMAGEM_PRODUTO_GRANDE"));
                product.ImagemProdutoPpi = GetValue<string>(produto.Attribute("IMAGEM_PRODUTO_PPI"));
                product.ImagemVitrine = GetValue<string>(produto.Attribute("IMAGEM_VITRINE"));
                product.ImagemVitrineGrande = GetValue<string>(produto.Attribute("IMAGEM_VITRINE_GRANDE"));
                product.Marca = Util.Decode(GetValue<string>(produto.Attribute("MARCA")));
                product.Mestre = Util.Decode(GetValue<string>(produto.Attribute("MESTRE")));
                product.Referencia = Util.Decode(GetValue<string>(produto.Attribute("REFERENCIA")));
                product.Acao = GetValue<string>(produto.Attribute("ACAO"));
                product.TemMontagem = GetValue<string>(produto.Attribute("TEM_MONTAGEM"));
                product.Tipo = GetValue<string>(produto.Attribute("TIPO"));
                product.Valor = GetValue<decimal>(produto.Attribute("VALOR"));
                product.ValorVenda = GetValue<decimal>(produto.Attribute("VALOR_VENDA"));
                product.QtdeDetalhes = GetValue<int>(produto.Attribute("QTDE_DETALHES"));
                product.Video = Util.Decode(GetValue<string>(produto.Attribute("VIDEO")));

                result.Add(product);
            }

            resultProductsAndCategories.ProductsPartner = result;

            foreach (var item in result)
            {
                cat = new();
                cat.DescCategoria = item.DescCategoria;
                cat.DescSubCategoria = item.DescSubcategoria;

                resultCat.Add(cat);
            }

            resultProductsAndCategories.CategoriesPartner = resultCat;

            return resultProductsAndCategories;
        }


        #endregion

        #region availability
        public async Task<IEnumerable<AvailabilityPartner>> GetAvailabilities(IEnumerable<string> StoreCodes)
        {
            var xmlDocs = await GetXml(_productsUrl, StoreCodes);

            if (xmlDocs?.Any() != true)
                return Enumerable.Empty<AvailabilityPartner>();

            return ReadAvailabilityXmlFile(xmlDocs.First());
        }

        static IEnumerable<AvailabilityPartner> ReadAvailabilityXmlFile(string xml)
        {
            AvailabilityPartner sku;
            Imagens img;
            List<AvailabilityPartner> result = new();
            List<DetalheSKU> resultDetalhe = new();
            List<Imagens> resultImg = new();
            var xmlDoc = XDocument.Parse(xml);

            XNamespace rs = "urn:schemas-microsoft-com:rowset";
            XNamespace z = "#RowsetSchema";

            var produtos = xmlDoc.Descendants(rs + "data").Descendants(z + "row");

            foreach (var produto in produtos)
            {
                if (produto == null) continue;

                sku = new();
                sku.CodigoSKU = GetValue<string>(produto.Attribute("CODIGO")) + GetValue<string>(produto.Attribute("MODELO"));
                sku.Preco = GetValue<decimal>(produto.Attribute("VALOR_VENDA"));
                sku.UltimaModificacao = GetValue<DateTime>(produto.Attribute("DATA_ALTERACAO"));
                sku.Modelo = GetValue<string>(produto.Attribute("MODELO"));
                sku.Habilitado = false;
                sku.Ativo = GetValue<bool>(produto.Attribute("ATIVO"));
                sku.EAN = "";
                sku.DataCadastro = DateTime.Now;
                sku.DataDesativado = null;
                //ProdutoId tem que ser o Id do produto e não o códigoProduto
                //sku.ProdutoId
                //SkuId tem que ser o ID e não o codigoSku
                //detalheSku.SkuId = Convert.ToInt32(sku.CodigoSKU);
                sku.PrecoDe = GetValue<decimal>(produto.Attribute("VALOR"));
                sku.PrecoPor = GetValue<decimal>(produto.Attribute("VALOR_VENDA"));
                sku.Disponivel = GetValue<bool>(produto.Attribute("ATIVO"));
                sku.DataCadastro = DateTime.Now;
                sku.Frete = 0;
                sku.DataDesativado = null;

                result.Add(sku);

                //img = new();
                //img.ImagemMenor = GetValue<string>(produto.Attribute("IMAGES"));
                //img.ImagemMaior = GetValue<string>(produto.Attribute("IMAGES"));
                //img.ImagemZoom = GetValue<string>(produto.Attribute("IMAGES"));
                //img.Ordem = 1;
                //img.Ativo = true;
                ////Id da tabela SKU
                ////img.SKUId = 
                //img.DataCadastro = DateTime.Now;
                //img.DataDesativado = null;
            }

            return result;
        }

        #endregion
    }
}
