using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MsProductIntegrationNovapontocom.Contracts.DTOs;
using MsProductIntegrationNovapontocom.Contracts.Service;
using MsProductIntegrationNovapontocom.Contracts.UseCases;
using System.Globalization;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace MsProductIntegrationNovapontocom.Services
{
    public class IntegrationService : IIntegrationService
    {
        readonly IConfiguration _configuration;
        readonly ISendMessageToQueueUseCase _sendMessageToQueueUseCase;

        readonly string _categoriesUrlDownload;
        readonly string _categoriesFileName;
        readonly string _productsPartialUrlDownload;
        readonly string _productsPartialFileName;
        readonly string _productsUrlDownload;
        readonly string _productsFileName;
        readonly string _availabilityUrlDownload;
        readonly string _availabilityFileName;
        readonly string _tokenHml;
        readonly ILogger<IntegrationService> _logger;

        public IntegrationService(ISendMessageToQueueUseCase sendMessageToQueueUseCase, IConfiguration configuration, ILogger<IntegrationService> logger)
        {
            _sendMessageToQueueUseCase = sendMessageToQueueUseCase;
            _configuration = configuration;
            _logger = logger;

            _tokenHml = _configuration.GetValue<string>("TokenHml");

            #region Categories

            var store = _configuration.GetValue<string>("Store");
            var environment = _configuration.GetValue<string>("Environment");

            _categoriesUrlDownload = String.Format(_configuration.GetValue<string>("Categories:UrlDownload"), store, environment);
            _categoriesFileName = _configuration.GetValue<string>("Categories:FileName");
            #endregion

            #region Products

            #region Partial
            _productsPartialUrlDownload = String.Format(_configuration.GetValue<string>("ProductsPartial:UrlDownload"), store, environment);
            _productsPartialFileName = _configuration.GetValue<string>("ProductsPartial:FileName");
            #endregion

            #region Complete
            _productsUrlDownload = String.Format(_configuration.GetValue<string>("ProductsFull:UrlDownload"), store, environment);
            _productsFileName = _configuration.GetValue<string>("ProductsFull:FileName");
            #endregion

            #region Availability
            _availabilityUrlDownload = String.Format(_configuration.GetValue<string>("Availability:UrlDownload"), store, environment);
            _availabilityFileName = _configuration.GetValue<string>("Availability:FileName");
            #endregion

            #endregion

        }

        public async Task<IEnumerable<string>> GetXml(string url, string fileName)
        {
            _logger.LogInformation($"Initializing get the XML...");
            List<string> result = new();

            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            var client = new HttpClient(clientHandler);
            client.Timeout = TimeSpan.FromMinutes(5);
            using var stream = await client.GetStreamAsync(url);

            _logger.LogInformation($"XML returned succesfully...");

            using ZipArchive zip = new(stream, ZipArchiveMode.Read);

            var filtro = new Regex(fileName, RegexOptions.CultureInvariant | RegexOptions.IgnoreCase);
            var fileStream = zip.Entries.Where(x => filtro.Match(x.Name).Success);
            if (fileStream?.Any() != true) return Enumerable.Empty<string>();

            _logger.LogInformation($"Reading the XMLs...");
            foreach (var entry in fileStream)
            {
                using MemoryStream xmlStream = new();
                entry.Open().CopyTo(xmlStream);
                result.Add(Encoding.UTF8.GetString(xmlStream.ToArray()));

                stream.Close();
                xmlStream.Close();
                GC.Collect();
            }

            _logger.LogInformation($"End of XMLs reading...");

            return result;
        }

        public static T GetValue<T>(XAttribute attribute)
        {
            if (attribute == null)
                return default(T);

            switch (Type.GetTypeCode(typeof(T)))
            {
                case TypeCode.DateTime:
                         if (DateTime.TryParseExact(attribute?.Value, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime result))
                        return (T)Convert.ChangeType(result, typeof(T));
                    break;
                case TypeCode.Decimal:
                    return (T)Convert.ChangeType(attribute?.Value, typeof(T), new CultureInfo("pt-BR"));
                    break;
                default:
                    break;
            }

            return (T)Convert.ChangeType(attribute.Value, typeof(T));
        }

        #region categories
        public async Task<IEnumerable<CategoryPartner>> GetCategories()
        {
            var xmlDocs = await GetXml(_categoriesUrlDownload, _categoriesFileName);

            if (xmlDocs?.Any() != true)
                return Enumerable.Empty<CategoryPartner>();

            return ReadCategoriesXmlFile(xmlDocs.First());
        }

        public static IEnumerable<CategoryPartner> ReadCategoriesXmlFile(string xml)
        {
            CategoryPartner cat;
            List<CategoryPartner> result = new();
            XmlDocument xmlDoc = new();
            xmlDoc.LoadXml(xml);

            var tmp = xmlDoc.SelectNodes("//*");
            if (tmp == null) return result;

            foreach (XmlNode xmlNode in tmp)
            {
                if (xmlNode?.Name != "Categoria") continue;
                if (xmlNode?.Attributes["IdCategoria"]?.Value == null
                    || xmlNode?.Attributes["IdDepartamento"]?.Value == null
                    || xmlNode?.Attributes["Level"]?.Value == null
                    || xmlNode?.Attributes["Nome"]?.Value == null) continue;

                cat = new();
                cat.Id = Convert.ToInt32(xmlNode.Attributes["IdCategoria"]?.Value);
                cat.DepartamentoId = Convert.ToInt32(xmlNode.Attributes["IdDepartamento"]?.Value);
                cat.Level = Convert.ToInt32(xmlNode.Attributes["Level"]?.Value);

                cat.Nome = Util.Decode(xmlNode.Attributes["Nome"]?.Value);
                cat.CategoriaPaiId = xmlNode.Attributes["IdCategoriaPai"] == null ? null : Convert.ToInt32(xmlNode.Attributes["IdCategoriaPai"]?.Value);

                cat.Ativo = true;
                cat.DataCadastro = DateTime.Now;
                cat.LojaId = 1;

                result.Add(cat);
            }

            return result;
        }

        #endregion

        #region products partial
        public async Task<IEnumerable<ProductPartner>> GetProductsPartial()
        {
            List<ProductPartner> result = new();
            var xmlDoc = await GetXml(_productsPartialUrlDownload, _productsPartialFileName);

            if (xmlDoc?.Any() != true)
                return Enumerable.Empty<ProductPartner>();

            foreach (var xml in xmlDoc)
                result.AddRange(ReadProductsXmlFile(xml));

            return result;
        }
        #endregion

        #region products
        public async Task<IEnumerable<ProductPartner>> GetProducts()
        {
            List<ProductPartner> result = new(); 
            var xmlDoc = await GetXml(_productsUrlDownload, _productsFileName);

            _logger.LogInformation($"Initializing read of products...");

            if (xmlDoc?.Any() != true)
                return Enumerable.Empty<ProductPartner>();

            foreach (var xml in xmlDoc)
                result.AddRange(ReadProductsXmlFile(xml));

            return result;
        }

        public static IEnumerable<ProductPartner> ReadProductsXmlFile(string xml)
        {
            try
            {
                ProductPartner product;
                List<ProductPartner> result = new();
                var xmlDoc = XDocument.Parse(xml);

                var produtos = xmlDoc.Descendants("Produtos").Descendants("Produto");

                foreach (var produto in produtos)
                {
                    if (produto == null) continue;

                    product = new();
                    product.Codigo = GetValue<int>(produto.Attribute("Codigo"));
                    product.DisplayName = Util.Decode(GetValue<string>(produto?.Attribute("DisplayName")));
                    product.DescricaoLonga = Util.Decode(GetValue<string>(produto?.Attribute("DescricaoLonga")));
                    product.Categoria = GetValue<int>(produto.Attribute("Categoria"));
                    product.CodigoFabricante = GetValue<int>(produto.Attribute("CodigoFabricante"));
                    product.Fabricante = Util.Decode(GetValue<string>(produto?.Attribute("Fabricante")));
                    product.FotoPequena = GetValue<string>(produto.Attribute("FotoPequena"));
                    product.FotoMedia = GetValue<string>(produto.Attribute("FotoMedia"));
                    product.FotoGrande = GetValue<string>(produto.Attribute("FotoGrande"));
                    product.PalavraChave = Util.Decode(GetValue<string>(produto?.Attribute("PalavraChave")));
                    product.MaisVendidos = GetValue<int>(produto.Attribute("MaisVendidos"));

                    var Skus = produto.Descendants("Skus").Descendants("Sku");

                    if (Skus.Any())
                    {
                        foreach (var sku in Skus)
                        {
                            if (sku == null) continue;

                            var objSku = new SkuPartner();
                            objSku.Codigo = GetValue<int>(sku.Attribute("Codigo"));
                            objSku.Preco = GetValue<decimal>(sku.Attribute("Preco"));
                            objSku.Habilitado = GetValue<bool>(sku.Attribute("Habilitado"));
                            objSku.Modelo = Util.Decode(GetValue<string>(sku?.Attribute("Modelo")));
                            objSku.EAN = GetValue<string>(sku.Attribute("EAN"));
                            objSku.Peso = GetValue<decimal>(sku.Attribute("Peso"));
                            objSku.Altura = GetValue<decimal>(sku.Attribute("Altura"));
                            objSku.Largura = GetValue<decimal>(sku.Attribute("Largura"));
                            objSku.Comprimento = GetValue<decimal>(sku.Attribute("Comprimento"));
                            objSku.IdLojista = GetValue<int>(sku.Attribute("IdLojista"));

                            var Imagens = sku.Descendants("Imagens").Descendants("Imagem");

                            if (Imagens.Any())
                            {
                                foreach (var img in Imagens)
                                {
                                    if (img == null) continue;

                                    var objImagem = new ImagesPartner();
                                    objImagem.UrlImagemMenor = GetValue<string>(img.Attribute("UrlImagemMenor"));
                                    objImagem.UrlImagemMaior = GetValue<string>(img.Attribute("UrlImagemMaior"));
                                    objImagem.UrlImagemZoom = GetValue<string>(img.Attribute("UrlImagemZoom"));
                                    objImagem.Ordem = GetValue<int>(img.Attribute("Ordem"));

                                    objSku.Imagens.Add(objImagem);
                                }
                            }

                            var GrupoSku = sku.Descendants("Grupos").Descendants("Grupo");

                            if (GrupoSku.Any())
                            {
                                foreach (var grupo in GrupoSku)
                                {
                                    if (grupo == null) continue;

                                    var objGrupo = new GroupsPartner();
                                    objGrupo.Codigo = GetValue<int>(grupo.Attribute("Codigo"));
                                    objGrupo.Nome = GetValue<string>(grupo.Attribute("Nome"));

                                    var Itens = grupo.Descendants("Itens").Descendants("Item");

                                    if (Itens.Any())
                                    {
                                        foreach (var item in Itens)
                                        {
                                            if (item == null) continue;

                                            var objItem = new ItemsPartner();
                                            objItem.Descricao = Util.Decode(GetValue<string>(item?.Attribute("Descricao")));
                                            objItem.Valor = GetValue<string>(item.Attribute("Valor"));

                                            objGrupo.Itens.Add(objItem);
                                        }
                                    }

                                    objSku.GruposSku.Add(objGrupo);
                                }
                            }

                            product.Skus.Add(objSku);
                        }
                    }

                    var Grupos = produto.Descendants("FichaTecnica").Descendants("Grupos").Descendants("Grupo");

                    if (Grupos.Any())
                    {
                        foreach (var grupo in Grupos)
                        {
                            if (grupo == null) continue;

                            var objGrupo = new GroupsPartner();
                            objGrupo.Codigo = GetValue<int>(grupo.Attribute("Codigo"));
                            objGrupo.Nome = GetValue<string>(grupo.Attribute("Nome"));

                            var Itens = grupo.Descendants("Itens").Descendants("Item");

                            if (Itens.Any())
                            {
                                foreach (var item in Itens)
                                {
                                    if (item == null) continue;

                                    var objItem = new ItemsPartner();
                                    objItem.Descricao = Util.Decode(GetValue<string>(item?.Attribute("Descricao")));
                                    objItem.Valor = GetValue<string>(item.Attribute("Valor"));

                                    objGrupo.Itens.Add(objItem);
                                }
                            }

                            product.Grupos.Add(objGrupo);
                        }
                    }

                    result.Add(product);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region availability
        public async Task<IEnumerable<AvailabilityPartner>> GetAvailabilities()
        {
            var xmlDocs = await GetXml(_availabilityUrlDownload, _availabilityFileName);

            if (xmlDocs?.Any() != true)
                return Enumerable.Empty<AvailabilityPartner>();

            return ReadAvailabilityXmlFile(xmlDocs.First());
        }

        public static IEnumerable<AvailabilityPartner> ReadAvailabilityXmlFile(string xml)
        {
            AvailabilityPartner availability;
            List<AvailabilityPartner> result = new();

            string msg_xml = Encoding.UTF8.GetString(Encoding.UTF8.GetPreamble());
            if (xml.StartsWith(msg_xml))
                xml = xml.Remove(0, msg_xml.Length);

            var xmlDoc = XDocument.Parse(xml);

            var itens = xmlDoc.Descendants("Produtos").Descendants("Produto");

            foreach (var item in itens)
            {
                availability = new();
                availability.Codigo = GetValue<int>(item.Attribute("codigo"));
                availability.PrecoDe = GetValue<decimal>(item.Attribute("precoDe")).ToString("N0", new CultureInfo("en-US")).Replace(",", ".");
                availability.PrecoPor = GetValue<decimal>(item.Attribute("precoPor")).ToString("N0", new CultureInfo("en-US")).Replace(",", ".");
                availability.Disponibilidade = GetValue<byte>(item.Attribute("disponibilidade"));

                result.Add(availability);
            }

            return result;
        }
        #endregion
    }
}
