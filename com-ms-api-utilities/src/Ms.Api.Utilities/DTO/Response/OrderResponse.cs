using Ms.Api.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ms.Api.Utilities.DTO.Response
{
    public class OrderResponse : BaseResponse
    {
        public long CodigoPedido { get; set; }
        public long PedidoParceiro { get; set; }
        public bool AguardandoConfirmacao { get; set; }
        public List<ProductOrderResponse> Produtos { get; set; }

        //TODO-RAFAEL Não exibir no site
        //public string PrevisaoDeEntrega { get; set; }

        /// <summary>
        /// Total do frete calculado para o pedido
        /// </summary>
        public decimal ValorFrete { get; set; }

        /// <summary>
        /// Total do frete calculado para o pedido em pontos
        /// </summary>
        public decimal ValorFretePontos { get; set; }

        /// <summary>
        /// Total calculado do valor dos produtos
        /// </summary>
        public decimal ValorTotalProdutos { get; set; }

        /// <summary>
        /// Total calculado do valor dos produtos em pontos
        /// </summary>
        public decimal ValorTotalProdutosPontos { get; set; }

        /// <summary>
        /// Soma do ValorFrete + ValorProduto
        /// </summary>
        public decimal ValorTotalPedido { get; set; }

        /// <summary>
        /// /// <summary>
        /// Soma do ValorFretePontos + ValorProdutoPontos
        /// </summary>
        /// </summary>
        public decimal ValorTotalPedidoPontos { get; set; }

        public DateTime Data { get; set; }

        public decimal ValorComplementoComJuros { get; set; }

        /// <summary>
        /// Vem do parceiro
        /// </summary>
        public string PrevisaoDeEntrega { get; set; }
    }

    public class ProductOrderResponse
    {
        public string Nome { get; set; }
        public string CodigoSku { get; set; }
        public string PrevisaoEntrega { get; set; }

        public int Quantidade { get; set; }

        public decimal ValorUnitarioProdutoDe { get; set; }
        public decimal ValorUnitarioProdutoDePontos { get; set; }


        public decimal ValorUnitarioProduto { get; set; }
        public decimal ValorUnitarioProdutoPontos { get; set; }

        public decimal ValorUnitarioProdutoComSubsidio { get; set; }
        public decimal ValorUnitarioProdutoComSubsidioPontos { get; set; }


        public decimal ValorTotalProdutos { get; set; }
        public decimal ValorTotalProdutosPontos { get; set; }

        public decimal ValorTotalProdutosComSubsidio { get; set; }
        public decimal ValorTotalProdutosComSubsidioPontos { get; set; }

        public decimal ValorFrete { get; set; }
        public decimal ValorFretePontos { get; set; }

        public decimal ValorTotalImpostos { get; set; }
        public decimal ValorTotalImpostosPontos { get; set; }

        /// <summary>
        /// Valor que está sendo subsidiado. Ex: Produto custa R$ 100,00 e o subsídio é de R$ 20,00
        /// </summary>
        public decimal ValorSubsidio { get; set; }
        public decimal ValorSubsidioPontos { get; set; }

        public bool Sucesso { get; set; }

        /// <summary>
        /// Informa se existe alguma mensagem (MensagemDeErro) que deve ser exibidada para o usuário. Não é um erro no produto.
        /// </summary>
        public bool Warning { get; set; }

        //public Enums.TipoDeErro TipoDeErro { get; set; }
        //public Enums.CodigoErro CodigoDoErro { get; set; }
        public string MensagemDeErro { get; set; }
        public string MensagemDeErroEmPontos { get; set; }
        //public List<ProdutosCarrinhoDTO> ProdutoKitFilho { get; set; }

        public decimal ValorUnitarioPontosDe { get; set; }
    }
}
