namespace Ms.Api.Utilities.DTO.Request
{
    public class NetShoes_CreateOrderRequest : BaseRequest_Pedido
    {
        //public long PedidoParceiro { get; set; }
        //public decimal? ValorFrete { get; set; }
        public NetShoes_RecipientOrder Destinatario { get; set; }
        public NetShoes_DeliveryAddress EnderecoEntrega { get; set; }
        public List<NetShoes_ProductCreateOrder> Produtos { get; set; }
        public int? OperadorTeleMarketingId { get; set; }
        public string? CodigoResgateAqui { get; set; }
        public string? TipoFrete { get; set; }
        public string? Parceiro { get; set; }
    }

    public class NetShoes_RecipientOrder
    {
        public string? CPFCNPJ { get; set; }
        //public string? Email { get; set; }
        //public string? Nome { get; set; }
        //public string? InscricaoEstadual { get; set; }
        public string? CodigoClienteCitibank { get; set; }
        public string? Telefone { get; set; }
        public string? Telefone1 { get; set; }
        public bool? AceitaReceberContatoUnicef { get; set; }
    }

    public class NetShoes_DeliveryAddress : BaseRequest_EnderecoPedido
    {
        //public string? Bairro { get; set; }
        //public string? Cidade { get; set; }
        //public string? CEP { get; set; }
        //public string? Complemento { get; set; }
        //public string? Estado { get; set; }
        //public string? Logradouro { get; set; }
        public string? Numero { get; set; }
        //public string? Referencia { get; set; }
        //public string? Telefone { get; set; }
        public string? Telefone1 { get; set; }
        //public string? Telefone2 { get; set; }
        //public string? Telefone3 { get; set; }
        public string? NomeResponsavel { get; set; }
    }

    public class NetShoes_ProductCreateOrder
    {
        /// <summary>
        /// SKU
        /// </summary>
        public string? Codigo { get; set; }

        /// <summary>
        /// Quantidade do produto a ser pedido
        /// </summary>
        public int? Quantidade { get; set; }

        /// <summary>
        /// Valor do preço de venda do produto a ser conferido. Valida o valor e abortará o pedido caso haja diferença.
        /// </summary>
        public decimal? PrecoVenda { get; set; }

        /// <summary>
        /// Nome do produto
        /// </summary>
        public string? Nome { get; set; }

        /// <summary>
        /// Imagem principal do produto
        /// </summary>
        public string? UrlImagem { get; set; }

        public decimal? ValorSubsidio { get; set; }
    }
}
