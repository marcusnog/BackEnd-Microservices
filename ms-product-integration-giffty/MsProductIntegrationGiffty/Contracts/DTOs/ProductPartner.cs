using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationGiffty.Contracts.DTOs
{
    public class ProductPartner
    {
        public string Codigo { get; set; }
        public string Fabricante { get; set; }
        public string Departamento { get; set; }
        public decimal Preco { get; set; }
        public decimal PrecoDe { get; set; }
        public string NomeProduto { get; set; }
        public string Descricao { get; set; }
        public string Habilitado { get; set; }
        public List<Fotos> Fotos { get; set; }
        public List<Imagens> Imagens { get; set; }
        public string Altura { get; set; }
        public string Largura { get; set; }
        public string Profundidade { get; set; }
        public string Peso { get; set; }
        public string ModelProduto
        {
            get
            {
                return NomeProduto.ToUpper().Contains("VIRTUAL") || TipoProduto.ToUpper().Contains("VOUCHER") ? "Virtual" : "Físico";
            }
        }
        public int? CategoriaReconheceId { get; set; }



        public decimal? Frete { get; set; }
        public decimal? ImpostosFrete { get; set; }

        //Novo

        public string FabricanteId { get; set; }
        public string FornecedorId { get; set; }
        public string FornecedorNome { get; set; }
        public string DepartamentoId { get; set; }
        public string CategoriaId { get; set; }
        public string Categoria { get; set; }


        public string PrecoBTD { get; set; }
        public string PrecoBTDI { get; set; }
        public string PrecoBTDIsemFrete { get; set; }
        public decimal? Taxa { get; set; }
        public decimal? ImpostosTaxa { get; set; }
        public string Desconto { get; set; }
        public List<Informacao> InformacoesAdicionais { get; set; }
        public string TipoProduto { get; set; }
        public List<Detalhes> DetalhesDescricao { get; set; }

    }

    public class Fotos
    {
        public string FotoP { get; set; }
        public string FotoM { get; set; }
        public string FotoG { get; set; }
    }

    public class Imagens
    {
        public string ImagemPP { get; set; }
        public string ImagemP { get; set; }
        public string ImagemM { get; set; }
        public string ImagemG { get; set; }
        public string ImagemGG { get; set; }
    }

    public class Informacao
    {
        public string chave { get; set; }
        public string valor { get; set; }
    }

    public class Detalhes
    {
        public string prazo { get; set; }
        public string validade { get; set; }
        public string utilizacao { get; set; }
        public string descricao { get; set; }
    }
}
