using System.Runtime.Serialization;

namespace Ms.Api.Utilities.DTO.Request
{
    //[DataContract]
    //public class ConsultarStatusPedidoRequest_Generico : BaseRequest
    //{
    //    [DataMember]
    //    public long CodigoPedido { get; set; }

    //    [DataMember]
    //    public long PedidoParceiro { get; set; }
    //}

    public class Generic_ConsultStatusOrderRequest : BaseRequest_Pedido
    {
        public string? CPF { get; set; }
        public string? CodigoPedido { get; set; }
        public string? CodigoCampanha { get; set; }
    }

    #region Calculo CEP

    public class Generic_CalcShipping_Request
    {
        public string CEP { get; set; }
        public List<Generic_CalcShipping_Product_Request> ListaProduto { get; set; }
        public string? CPF { get; set; }
        public decimal ValorTotal { get; set; }
        //public decimal? FatorConversao { get; set; }
    }

    public class Generic_CalcShipping_Product_Request
    {
        public string Codigo { get; set; } 
        public int Quantidade { get; set; }
        public decimal ValorUnitario { get; set; }
    }

    #endregion Calculo CEP

}
