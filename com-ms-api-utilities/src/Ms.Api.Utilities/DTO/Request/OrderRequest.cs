using Ms.Api.Utilities.Enum;
using System.Runtime.Serialization;
using static Ms.Api.Utilities.Enum.Enums;

namespace Ms.Api.Utilities.DTO.Request
{
    //public class OrderRequest : BaseRequest
    //{
    //    public OrderRequest()
    //    {
    //        //if (this.Fornecedor == Utilities.Enums.Fornecedor.Giftty)
    //        //    this.pedidoParceiro = new Random().RandomLong(0, 9999999999); //todo verificar
    //        //else if (this.Fornecedor == Utilities.Enums.Fornecedor.ITLog)
    //        //    this.pedidoParceiro = new Random().Next(1000000, 9999999); //WS da ItLog é INT em vez de LONG
    //        //else
    //        //    this.pedidoParceiro = new Random().RandomLong(0, 999999999999999);
    //    }

    //    private long pedidoParceiro { get; set; }

    //    internal long PedidoParceiro
    //    {
    //        get
    //        {
    //            return pedidoParceiro;
    //        }
    //    } //length 15

    //    [DataMember]
    //    public decimal ValorFrete { get; set; }
    //    [DataMember]
    //    public decimal ValorFreteOriginal { get; set; }

    //    [DataMember]
    //    public Request_DeliveryAddress Destinatario { get; set; }

    //    [DataMember]
    //    public BaseRequest_RecipientOrder EnderecoEntrega { get; set; }

    //    [DataMember]
    //    public List<ProductOrderRequest> Produtos { get; set; }

    //    //[DataMember]
    //    //public int? OperadorTeleMarketingId { get; set; }

    //    //[DataMember]
    //    //public string CodigoResgateAqui { get; set; }

    //    //public string TipoFrete { get; set; }
    //    public DadosPagamentoPedido DadosPagamento { get; set; }
    //}

    //public class DadosPagamentoPedido
    //{
    //    public decimal ValorPontos { get; set; }
    //    public decimal ValorComplemento { get; set; }
    //    public string NomeBandeira { get; set; }
    //    public int QuantidadeParcelas { get; set; }
    //    public string NSU { get; set; }
    //}

    ////public class EnderecoEntrega
    ////{
    ////    public string Bairro { get; set; }
    ////    public string Cidade { get; set; }
    ////    public string CEP { get; set; }
    ////    public string Complemento { get; set; }
    ////    public string Estado { get; set; }
    ////    public string Logradouro { get; set; }
    ////    public string Numero { get; set; }
    ////    public string Referencia { get; set; }
    ////    public string Telefone { get; set; }
    ////    public string Telefone1 { get; set; }
    ////    public string Telefone2 { get; set; }
    ////    public string Telefone3 { get; set; }
    ////    public string NomeResponsavel { get; set; }
    ////    /* Ghadeer */
    ////    public bool? RetirarNoEvento { get; set; }
    ////    public bool? RetirarNoTrabalho { get; set; }
    ////}
    //public class Request_DeliveryAddress : BaseRequest_DeliveryAddress
    //{
    //}

    ////public class DestinatarioPedido
    ////{
    ////    public string CPFCNPJ { get; set; }
    ////    public string Email { get; set; }
    ////    public string Nome { get; set; }
    ////    public string InscricaoEstadual { get; set; }
    ////    public string CodigoClienteCitibank { get; set; }

    ////    public string Telefone { get; set; }
    ////    public string Telefone1 { get; set; }

    ////    public bool? AceitaReceberContatoUnicef { get; set; }
    ////}

    //public class Request_RecipientOrder : BaseRequest_RecipientOrder
    //{
    //    public string CPFCNPJ { get; set; }
    //    public string Email { get; set; }
    //    public string Nome { get; set; }
    //    public string InscricaoEstadual { get; set; }
    //    public string CodigoClienteCitibank { get; set; }

    //    public string Telefone { get; set; }
    //    public string Telefone1 { get; set; }

    //    public bool? AceitaReceberContatoUnicef { get; set; }
    //}

    //public class ProductOrderRequest
    //{
    //    /// <summary>
    //    /// SKU
    //    /// </summary>
    //    public string Codigo { get; set; }

    //    /// <summary>
    //    /// Quantidade do produto a ser pedido
    //    /// </summary>
    //    public int Quantidade { get; set; }

    //    /// <summary>
    //    /// Valor do preço de venda do produto a ser conferido. Valida o valor e abortará o pedido caso haja diferença.
    //    /// </summary>
    //    public decimal PrecoVenda { get; set; }

    //    /// <summary>
    //    /// Nome do produto
    //    /// </summary>
    //    public string Nome { get; set; }

    //    /// <summary>
    //    /// Imagem principal do produto
    //    /// </summary>
    //    //public string UrlImagem { get; set; }

    //    public decimal ValorSubsidio { get; set; }

    //}




    public class OrderRequest : BaseRequest
    {
        ////public string CampaignId { get; set; }
        public decimal ConversationFactor { get; set; }
        public decimal TotalOrderAmount { get; set; }
        public decimal TotalOrderAmountPoints { get; set; }
        public decimal TotalOrderAmountCurrency { get; set; }
        public string PaymentMethodId { get; set; }
        public IEnumerable<Request_OrderStore> Shops { get; set; }
        public Request_OrderRecipient Recipient { get; set; }
        public Request_OrderDeliveryAddress DeliveryAddress { get; set; }
        public Request_OrderPaymentData PaymentData { get; set; }
        public Boolean ControlPointInternal { get; set; }
        public TypeRequest Type { get; set; }
    }

    public class Request_OrderPaymentData
    {
        public decimal ValorPontos { get; set; }
        public decimal ValorComplemento { get; set; }
        public string NomeBandeira { get; set; }
        public int QuantidadeParcelas { get; set; }
        public string NSU { get; set; }
        public string NumeroCartao { get; set; }
        public string CodigoSegurancaCartao { get; set; }
        public string AnoValidade { get; set; }
        public string MesValidade { get; set; }
        public string NomeTitular { get; set; }

    }

    public class Request_OrderStore
    {
        public Enums.Store Store { get; set; }
        public string OrderStoreId { get; set; }
        public string OrderCode { get; set; }
        public decimal ShippingValue { get; set; }
        public bool Confirmed { get; set; }
        public string StoreId { get; set; }
        public decimal TotalValueProducts { get; set; }
        public double DateDeliveryForecast { get; set; }

        public IEnumerable<Request_OrderProduct> Products { get; set; }
    }

    public class Request_OrderProduct
    {
        public string OrderProductId { get; set; }
        public string CodeSku { get; set; }
        public string CodeProduct { get; set; }

        public int Quantity { get; set; }
        public double DateDeliveryForecast { get; set; }


        public decimal ValueUnitary { get; set; }
        public decimal ValueUnitaryPoints { get; set; }

        public decimal TotalOrderAmountCurrency { get; set; }

        public string StatusDelivery { get; set; }

        public double DateBilling { get; set; }
    }

    public class Request_OrderRecipient
    {
        public string OrderRecipientId { get; set; }
        public string CPFCNPJ { get; set; }

        public string Email { get; set; }
        public string Name { get; set; }
        public string StateRegistration { get; set; }
        public bool Active { get; set; }
        public double DateCreation { get; set; }
        public double DateDisabled { get; set; }

    }

    public class Request_OrderDeliveryAddress
    {
        public string OrderDeliveryAddressId { get; set; }
        public string District { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Complement { get; set; }
        public string State { get; set; }
        public string PublicPlace { get; set; }
        public string Number { get; set; }
        public string Reference { get; set; }
        public string Telephone { get; set; }
        //public string Telephone1 { get; set; }
        public string Telephone2 { get; set; }
        public string Telephone3 { get; set; }
        public bool Active { get; set; }
        public double DateCreation { get; set; }
        public double DateDisabled { get; set; }

    }


}
