using Ms.Api.Utilities.DTO.Request;

namespace Ms.Api.Utilities.DTO.Response
{
    public class Giftty_RespostaPedido : Giftty_RespostaWebService
    {
        public Giftty_RespostaPedido() : base() { }
        public Giftty_RespostaPedido(Exception ex) : base(ex) { }

        public Giftty_Informacoes Informacoes { get; set; }

        public Giftty_RespostaProdutoPedido[] Produtos { get; set; }
    }
    public class Giftty_Informacoes
    {
        public int? codPedido { get; set; } //>9424819</codPedido>
        public string valorProdutos { get; set; } //>50.00</valorProdutos>
        public string valorFrete { get; set; } // />
        public string valorTotal { get; set; } //>50.00</valorTotal>        
    }
    public class Giftty_RespostaProdutoPedido
    {
        public string codigo { get; set; } //>576</codigo>
        public string valor { get; set; } //>50.00</valor>
        public string quantidade { get; set; } //>1</quantidade>
        public Giftty_Cartoes[] Cartoes { get; set; }
    }
    public class Giftty_Cartoes
    {
        public string codigo { get; set; } //>4391d778473dc17a9c6fe9eac51182080cd7023d3e6d67ab40ff45066c699863</codigo>
        public string pin { get; set; } //>e3d3a82174aa93a06545222515fedfb6</pin>
        public string qr_code { get; set; } //>e3d3a82174aa93a06545222515fedfb6</qr_code>
        public string link { get; set; } //>https://giftty.com.br/cea/cartao.php?8e7ee9b50b48be2b02231b9bd3509b7c43144bc3ffc2d4acd0e8e76acb929cddc9a8191b0de51fc14c7a998168b46a52</link>
    }



    public class Giftty_RespostaConsultarTracking : Giftty_RespostaWebService
    {
        public Giftty_RespostaConsultarTracking() : base() { }
        public Giftty_RespostaConsultarTracking(Exception ex) : base(ex) { }
        public Giftty_RespostaConsultarTracking(Giftty_RespostaWebService respostaApi)
        {
            this.erros = respostaApi.erros;
            this.detalhesErros = respostaApi.detalhesErros;
            this.mensagem = respostaApi.mensagem;
            this.statusCode = respostaApi.statusCode;
            this.excecao = respostaApi.excecao;
        }

        public Giftty_ItemTracking[] Itens { get; set; }
    }

    public class Giftty_ItemTracking
    {
        public string codPedido { get; set; }
        public string codItem { get; set; }
        public string nome { get; set; }
        public string codigoProduto { get; set; }

        /// <summary>
        /// Status de resposta: <br></br>
        ///     Aguardando confirmação de pagamento de boleto. <br></br>
        ///     Processando o pagamento.<br></br>
        ///     Pagamento efetuado e confirmado.<br></br>
        ///     Problema no processamento: confirmar endereço de entrega.<br></br>
        ///     Problema no processamento: aguardando documentação ou informação adicional.<br></br>
        ///     Pedido em rota de entrega.<br></br>
        ///     Pedido entregue.<br></br>
        ///     Pedido cancelado em dd/mm/yyyy.<br></br>
        ///     Pedido suspenso.<br></br>
        /// </summary>
        public string status { get; set; }
        public string dataPedido { get; set; }
        public string dataEmbarque { get; set; }
        public string dataRecebimento { get; set; }
        public string observacoes { get; set; }
        public string email { get; set; }
        public string rastreamento { get; set; }

    }

}
