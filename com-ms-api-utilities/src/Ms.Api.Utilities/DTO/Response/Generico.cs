namespace Ms.Api.Utilities.DTO.Response
{
    public class Generic_ResultOrder
    {
        public Boolean Sucesso { get; set; }
        public String? CodigoPedidoNoParceiro { get; set; }
        public String? Mensagem { get; set; }

    }

    public class Generic_ResultOrderConsult
    {
        public Boolean Sucesso { get; set; }
        public String? Status { get; set; }
        public String? Mensagem { get; set; }

    }


    #region Calculo CEP

    //public class Generic_CalcShipping_Response
    //{
    //    public decimal ValorFrete { get; set; }

    //    public decimal ValorFretePontos { get; set; }

    //    public decimal ValorImpostos { get; set; }

    //    public decimal ValorImpostosPontos { get; set; }

    //    public decimal ValorTotalProdutos { get; set; }

    //    public decimal ValorTotalProdutosPontos { get; set; }

    //    public decimal ValorTotalProdutosComSubsidio { get; set; }

    //    public decimal ValorTotalProdutosComSubsidioPontos { get; set; }

    //    public decimal ValorTotalPedido { get; set; }

    //    public decimal ValorTotalPedidoPontos { get; set; }

    //    public decimal ValorTotalPedidoComSubsidio { get; set; }

    //    public decimal ValorTotalPedidoComSubsidioPontos { get; set; }

    //    //public List<ProductsCart> Produtos { get; set; }
    //    public string TipoFrete { get; set; }

    //    /// <summary>
    //    /// Vem do cliente que utiliza esse serviço
    //    /// </summary>
    //    //public string ProtocoloCliente { get; set; }

    //    public List<Generic_CalcShipping_Product_Response> ListaProduto { get; set; }
    //}

    //public class Generic_CalcShipping_Product_Response
    //{
    //    //public string Nome { get; set; }
    //    public string CodigoSku { get; set; }
    //    public string PrevisaoEntrega { get; set; }

    //    public int Quantidade { get; set; }

    //    //public decimal ValorUnitarioProdutoDe { get; set; }
    //    //public decimal ValorUnitarioProdutoDePontos { get; set; }


    //    public decimal ValorUnitarioProduto { get; set; }
    //    public decimal ValorUnitarioProdutoPontos { get; set; }

    //    //public decimal ValorUnitarioProdutoComSubsidio { get; set; }
    //    //public decimal ValorUnitarioProdutoComSubsidioPontos { get; set; }


    //    public decimal ValorTotalProdutos { get; set; }
    //    public decimal ValorTotalProdutosPontos { get; set; }

    //    //public decimal ValorTotalProdutosComSubsidio { get; set; }
    //    //public decimal ValorTotalProdutosComSubsidioPontos { get; set; }

    //    public decimal ValorFrete { get; set; }
    //    public decimal ValorFretePontos { get; set; }

    //    //public decimal ValorTotalImpostos { get; set; }
    //    //public decimal ValorTotalImpostosPontos { get; set; }

    //    /// <summary>
    //    /// Valor que está sendo subsidiado. Ex: Produto custa R$ 100,00 e o subsídio é de R$ 20,00
    //    /// </summary>
    //    //public decimal ValorSubsidio { get; set; }
    //    //public decimal ValorSubsidioPontos { get; set; }

    //    //public bool Sucesso { get; set; }

    //    /// <summary>
    //    /// Informa se existe alguma mensagem (MensagemDeErro) que deve ser exibidada para o usuário. Não é um erro no produto.
    //    /// </summary>
    //    //public bool Warning { get; set; }

    //    //public Enums.TipoDeErro TipoDeErro { get; set; }
    //    //public Enums.CodigoErro CodigoDoErro { get; set; }
    //    //public string MensagemDeErro { get; set; }
    //    //public string MensagemDeErroEmPontos { get; set; }
    //    //public List<ProdutosCarrinhoDTO> ProdutoKitFilho { get; set; }

    //    //public decimal ValorUnitarioPontosDe { get; set; }
    //}

    #endregion Calculo CEP
}
