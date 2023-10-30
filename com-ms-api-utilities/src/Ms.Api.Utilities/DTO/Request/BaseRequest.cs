using Ms.Api.Utilities.Enum;

namespace Ms.Api.Utilities.DTO.Request
{
    public class BaseRequest_Pedido
    {
        public long PedidoParceiro { get; set; }
        public decimal? ValorFrete { get; set; }
    }

    public class BaseRequest_EnderecoPedido
    {
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Estado { get; set; }
        public string? CEP { get; set; }
        public string? Referencia { get; set; }
        public string? Complemento { get; set; }
        public string? Logradouro { get; set; }
        public string? Telefone { get; set; }
        public string? Telefone2 { get; set; }
        public string? Telefone3 { get; set; }
    }

    public class BaseRequest_Destinatario
    {
        public string? InscricaoEstadual { get; set; }
        public string? Email { get; set; }
        public string? Nome { get; set; }
    }


    #region Order Generico

    public class BaseRequest
    {
        public BaseRequest()
        {
            this.Data = DateTime.Now;
            //this.ValidarFornecedor = true;
        }

        public int ClienteId { get; set; }
        //public int CampanhaId { get; set; }
        public string CodigoCampanhaFornecedor { get; set; }
        public decimal FatorConversao { get; set; }

        public string CNPJ { get; set; }

        internal DateTime Data { get; set; }

        public string LoginCliente { get; set; }
        public string SenhaCliente { get; set; }
        public string ChaveCampanha { get; set; }

        //public int Loja { get; set; }

        public Enums.Provider Provider { get; set; }
        public Enums.Campaign Campaign { get; set; }
        //public Enums.Store Store { get; set; }

    }

    public class BaseRequest_DeliveryAddress
    {
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string CEP { get; set; }
        public string Complemento { get; set; }
        public string Estado { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Referencia { get; set; }
        public string Telefone { get; set; }
        public string Telefone1 { get; set; }
        public string Telefone2 { get; set; }
        public string Telefone3 { get; set; }
        public string NomeResponsavel { get; set; }
        /* Ghadeer */
        public bool? RetirarNoEvento { get; set; }
        public bool? RetirarNoTrabalho { get; set; }
    }

    public class BaseRequest_RecipientOrder
    {
        public string CPFCNPJ { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string InscricaoEstadual { get; set; }
        public string CodigoClienteCitibank { get; set; }

        public string Telefone { get; set; }
        public string Telefone1 { get; set; }

        public bool? AceitaReceberContatoUnicef { get; set; }
    }

    #endregion Order Generico

}
