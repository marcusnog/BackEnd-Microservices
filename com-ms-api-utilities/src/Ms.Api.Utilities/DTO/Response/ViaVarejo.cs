using Ms.Api.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms.Api.Utilities.DTO.Response
{
    public class ViaVarejo_PedidoB2BResponse : DefaultResponse<ViaVarejo_PedidoB2B> { }
    public class ViaVarejo_PedidoB2B
    {
        public decimal ValorProduto { get; set; }
        public decimal ValorTotalPedido { get; set; }
        public int CodigoPedido { get; set; }
        public long PedidoParceiro { get; set; }
        public string IdPedidoMktplc { get; set; }
        public ViaVarejo_ProdutoPedidoB2B[] Produtos { get; set; }
        public string ParametrosExtras { get; set; }
        public ViaVarejo_PedidoDadosEntrega DadosEntrega { get; set; }
        public bool AguardandoConfirmacao { get; set; }
        public ViaVarejo_DadosPagamentoComplementarResposta DadosPagamentoComplementar { get; set; }
    }

    public class ViaVarejo_PedidoDadosEntrega
    {
        public string PrevisaoDeEntrega { get; set; }
        public System.Nullable<decimal> ValorFrete { get; set; }
        public System.Nullable<int> IdEntregaTipo { get; set; }
        public System.Nullable<int> IdEnderecoLojaFisica { get; set; }
        public System.Nullable<int> IdUnidadeNegocio { get; set; }
    }

    public class ViaVarejo_ProdutoPedidoB2B
    {
        public int? IdLojista { get; set; }
        public int Codigo { get; set; }
        public int Quantidade { get; set; }
        public int Premio { get; set; }
        public decimal? PrecoVenda { get; set; }
    }

    public class ViaVarejo_DadosPagamentoComplementarResposta
    {
        public ViaVarejo_PagamentoComplementarItemCalculadoDto[] Pagamentos { get; set; }
        public decimal ValorTotalComplementar { get; set; }
        public decimal ValorTotalComplementarComJuros { get; set; }
    }

    public class ViaVarejo_PagamentoComplementarItemCalculadoDto
    {
        public string CodigoDoErro { get; set; }
        public decimal ValorComplementar { get; set; }
        public int QuantidadeParcelas { get; set; }
        public decimal ValorParcela { get; set; }
        public int IdFormaPagamento { get; set; }
        public bool Erro { get; set; }
        public string MensagemDeErro { get; set; }
        public string Url { get; set; }
    }

    #region CalcShipping

    //public class CalcShippingResponse : DefaultResponse<CalcShipping> { }

    //public class CalcShipping
    //{
    //    public List<ProductsCart> Produtos { get; set; }
    //    public decimal ValorFrete { get; set; }
    //    public decimal ValorImpostos { get; set; }
    //    public decimal ValorTotaldoPedido { get; set; }
    //    public decimal ValorTotaldosProdutos { get; set; }
    //}

    //public class ProductsCart
    //{
    //    public int IdSku { get; set; }
    //    public string PrevisaoEntrega { get; set; }
    //    public decimal ValorUnitario { get; set; }
    //    public decimal ValorTotal { get; set; }
    //    public decimal ValorTotalFrete { get; set; }
    //    public decimal ValorTotalImpostos { get; set; }
    //    public bool Erro { get; set; }
    //    public string MensagemDeErro { get; set; }
    //    public string CodigoDoErro { get; set; }
    //}

    #endregion CalcShipping
}
