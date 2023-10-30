using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationMagalu.Contracts.DTOs
{
    public class ProductPartner
    {
        //Variação do produto (CODIGO + MODELO = SKU do Produto)
        public string Codigo { get; set; }
        public string Modelo { get; set; }
        public string Categoria { get; set; }
        public string DescCategoria { get; set; }
        public string SubCategoria { get; set; }
        public string DescSubcategoria { get; set; }
        public string Referencia { get; set; }

        //Título do produto.
        //Importante: O nome comercial do produto no site do Magazine Luiza é composto de <DESCRICAO> - <REFERENCIA>. Sugerimos a mesma implementação em seu site
        public string Descricao { get; set; }

        //0 = Bivolt
        //1 = 110 Volts
        //2 = 220 Volts
        //3 = Não utiliza energia elétrica
        //4 = 380 volts
        //5 = Bateria Recarregável
        //6 = Pilha
        //7 = Energia Solar
        public string Voltagem { get; set; }
        public string Marca { get; set; }

        //Lista de links de todas as imagens separados em vírgula
        /*
         * Caso groupImages=1. 
         * O tamanho da imagem deve ser definido substituindo o valor {w}x{h}, largura e altura da imagem respectivamente.
         * Link das imagens do produto separados por vírgula. Ao utilizar o parâmetro groupImages=1.
         */
        public string Images { get; set; }
        public List<ImageMagaLu> Images_list
        {
            get
            {
                return GetImages(this.Images);
            }
        }

        //Link de imagem do produto. Tamanho preferencial: 210x210 px.
        public string Imagem { get; set; }
        //Link de imagem de vitrine do produto. Tamanho preferencial: 210x210 px.	
        public string ImagemVitrine { get; set; }
        //Link de imagem grande de vitrine do produto. Tamanho preferencial: 1500x1500 px.	
        public string ImagemVitrineGrande { get; set; }
        //Link de imagem de categoria do produto. Tamanho preferencial: 210x210 px.	
        public string ImagemCategoria { get; set; }
        //Link de imagem PPI* do produto. Tamanho prefencial: 210x210 px.	
        public string ImagemProdutoPpi { get; set; }
        //Link de imagem de detalhe do produto. Tamanho prefencial: 410x308 px.	
        public string ImagemProdutoDetalhe { get; set; }
        //Link de imagem ampliada do produto. Tamanho prefencial: 1500x1500 px.	
        public string ImagemProdutoGrande { get; set; }
        //Quantidade de detalhes do produto para ser utilizada para visualização das imagens. Se a imagem tiver 5 detalhes, além da imagem principal enviada nos campos acima, haverá imagens de A à E (010502100a.jpg, ... 010502100e.jpg).	
        public int? QtdeDetalhes { get; set; }
        //Valor de tabela do produto (Preço de) em R$	
        public decimal? Valor { get; set; }
        //Valor de venda do produto (Preço por) em R$	
        public decimal? ValorVenda { get; set; }
        public bool? Ativo { get; set; }
        public string ClassificacaoFiscaL { get; set; }
        public string Acao { get; set; }
        public string DataAlteracao { get; set; }
        //Indica se o produto necessita de montagem. 0=Não / 1=Sim	
        public string TemMontagem { get; set; }
        //Indica qual é o produto mestre deste. Usado para produtos com mais de uma cor, ou voltagem, etc. Quando um produto não tem um mestre isto quer dizer que ele é o mestre, então o valor preenchido é o código do próprio produto (campo CODIGO).
        public string Mestre { get; set; }
        //Atualmente exitem dois tipos: produto ou bundle. Bundle é a composição de 2 ou 3 produtos selecionados pelo Magazine Luiza.	
        public string Tipo { get; set; }
        public string Video { get; set; }
        public string Especificacao { get; set; }
        public string Cor { get; set; }
        public string Tamanho { get; set; }

        public static List<ImageMagaLu> GetImages(string CompactLinks)
        {
            try
            {
                if (string.IsNullOrEmpty(CompactLinks))
                    return null;

                if (CompactLinks.IndexOf(',') == -1)
                    return new List<ImageMagaLu>() {
                        new ImageMagaLu()
                        {
                            Imagem = CompactLinks.Replace("{w}","210").Replace("{h}","210"),
                            ImagemVitrineGrande = CompactLinks.Replace("{w}","1500").Replace("{h}","1500"),
                            ImagemProdutoDetalhe = CompactLinks.Replace("{w}","410").Replace("{h}","308"),
                        }
                    };

                List<ImageMagaLu> _LinksList = new List<ImageMagaLu>();
                var _LinksArray = CompactLinks.Split(',');

                for (int i = 0; i < _LinksArray.Length; i++)
                {
                    try
                    {
                        _LinksList.Add(new ImageMagaLu()
                        {
                            Imagem = _LinksArray[i].Replace("{w}", "210").Replace("{h}", "210"),
                            ImagemVitrineGrande = _LinksArray[i].Replace("{w}", "1500").Replace("{h}", "1500"),
                            ImagemProdutoDetalhe = _LinksArray[i].Replace("{w}", "410").Replace("{h}", "308"),
                        });
                    }
                    catch { }
                }

                return _LinksList;
            }
            catch
            {
                return null;
            }
        }
    }

    public class ImageMagaLu
    {
        //210x210 px.
        public string Imagem { get; set; }
        //1500x1500 px.
        public string ImagemVitrineGrande { get; set; }
        //410x308 px.
        public string ImagemProdutoDetalhe { get; set; }
    }
}
