namespace MsProductDetailsApi.Contracts.DTOs
{
    public class CalcShipping
    {
        public string IdUser { get; set; }
        public string CEP { get; set; }
        public string? CNPJ { get; set; }
        public string? CPFParticipante { get; set; }
        public string? NumeroParticipante { get; set; }
        public decimal? FatorConversao { get; set; }
        public int? IdCampanha { get; set; }
        public string Store { get; set; }
        public List<CalcShippingProduct>? Produtos { get; set; }
    }

    public class CalcShippingFilter
    {
        public string IdProduct { get; set; }
        public int Quantity { get; set; }
        public string StoreName { get; set; }
        public decimal ValueInReals { get; set; }
        public string IdUser { get; set; }
        public decimal? FatorConversao { get; set; }
        public string CEP { get; set; }
    }

    public class CalcShippingProduct
    {
        public string Codigo { get; set; }
        public int Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
    }

    #region Response

    public class CalcShippingResponse
    {
        public decimal ValorFrete { get; set; }
        public decimal ValorFretePontos { get; set; }
        public decimal ValorImpostos { get; set; }
        public decimal ValorImpostosPontos { get; set; }
        public decimal ValorTotalProdutos { get; set; }
        public decimal ValorTotalProdutosPontos { get; set; }
        public decimal ValorTotalProdutosComSubsidio { get; set; }
        public decimal ValorTotalProdutosComSubsidioPontos { get; set; }
        public decimal ValorTotalPedido { get; set; }
        public decimal ValorTotalPedidoPontos { get; set; }
        public decimal ValorTotalPedidoComSubsidio { get; set; }
        public decimal ValorTotalPedidoComSubsidioPontos { get; set; }
        public List<ProductsCart> Produtos { get; set; }
        public string TipoFrete { get; set; }

        /// <summary>
        /// Vem do cliente que utiliza esse serviço
        /// </summary>
        public string ProtocoloCliente { get; set; }
    }

    public class ProductsCart
    {
        public string? Nome { get; set; }
        public int? IdSku { get; set; }
        public string? CodigoSku { get; set; }
        public string PrevisaoEntrega { get; set; }
        public int? Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? ValorUnitarioProdutoDe { get; set; }
        public decimal? ValorUnitarioProdutoDePontos { get; set; }
        public decimal? ValorUnitarioProduto { get; set; }
        public decimal? ValorUnitarioProdutoPontos { get; set; }
        public decimal? ValorUnitarioProdutoComSubsidio { get; set; }
        public decimal? ValorUnitarioProdutoComSubsidioPontos { get; set; }
        public decimal? ValorTotalProdutos { get; set; }
        public decimal? ValorTotalProdutosPontos { get; set; }
        public decimal? ValorTotalProdutosComSubsidio { get; set; }
        public decimal? ValorTotal { get; set; }
        public decimal? ValorTotalFrete { get; set; }
        public decimal? ValorTotalProdutosComSubsidioPontos { get; set; }
        public decimal? ValorFrete { get; set; }
        public decimal? ValorFretePontos { get; set; }
        public decimal? ValorTotalImpostos { get; set; }
        public decimal? ValorTotalImpostosPontos { get; set; }
        /// <summary>
        /// Valor que está sendo subsidiado. Ex: Produto custa R$ 100,00 e o subsídio é de R$ 20,00
        /// </summary>
        public decimal? ValorSubsidio { get; set; }
        public decimal? ValorSubsidioPontos { get; set; }
        public bool? Sucesso { get; set; }
        /// <summary>
        /// Informa se existe alguma mensagem (MensagemDeErro) que deve ser exibidada para o usuário. Não é um erro no produto.
        /// </summary>
        public bool? Warning { get; set; }
        //public Enums.TipoDeErro TipoDeErro { get; set; }
        //public Enums.CodigoErro CodigoDoErro { get; set; }
        public string? MensagemDeErro { get; set; }
        public string? MensagemDeErroEmPontos { get; set; }
        public List<ProductsCart>? ProdutoKitFilho { get; set; }
        public decimal? ValorUnitarioPontosDe { get; set; }
    }


    #endregion Response
}
