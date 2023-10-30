using Ms.Api.Utilities.DTO.Response;

namespace Ms.Api.Utilities.DTO.Request
{
    public class ViaVarejo_PedidoB2BRequest : BaseRequest_Pedido
    {
        //public ViaVarejo_ProdutoDadosPedidoB2B[] Produtos { get; set; }
        public List<ViaVarejo_ProdutoDadosPedidoB2B> Produtos { get; set; }

        public ViaVarejo_EnderecoEntregaDadosPedidoB2B EnderecoEntrega { get; set; }
        public ViaVarejo_DestinatarioDadosPedidoB2B Destinatario { get; set; }
        public int Campanha { get; set; }
        public string CNPJ { get; set; }
        //public long PedidoParceiro { get; set; }
        public string IdPedidoMktplc { get; set; }
        public string SenhaAtendimento { get; set; }
        public string Apolice { get; set; }
        public System.Nullable<int> Administrador { get; set; }
        public string ParametrosExtras { get; set; }
        //public decimal ValorFrete { get; set; }
        public System.Nullable<bool> AguardarConfirmacao { get; set; }
        public System.Nullable<bool> OptantePeloSimples { get; set; }
        public bool PossuiPagtoComplementar { get; set; }
        //public ViaVarejo_DadosPagamentoComplementar[] PagtosComplementares { get; set; }
        public List<ViaVarejo_DadosPagamentoComplementar> PagtosComplementares { get; set; }

        public ViaVarejo_PedidoDadosEntrega DadosEntrega { get; set; }
        public ViaVarejo_EnderecoCobrancaDadosPedidoB2B EnderecoCobranca { get; set; }
        public System.Nullable<int> IdEnderecoLojaFisica { get; set; }
        public System.Nullable<int> IdEntregaTipo { get; set; }
        public System.Nullable<int> IdUnidadeNegocio { get; set; }
        public decimal ValorTotalPedido { get; set; }
        public decimal ValorTotalComplementar { get; set; }
        public decimal ValorTotalComplementarComJuros { get; set; }

        public List<ViaVarejo_IntermediadoresFinanceiros> IntermediadoresFinanceiros { get; set; }
    }

    //public class PedidoDadosEntrega
    //{
    //    public string PrevisaoDeEntrega { get; set; }
    //    public System.Nullable<decimal> ValorFrete { get; set; }
    //    public System.Nullable<int> IdEntregaTipo { get; set; }
    //    public System.Nullable<int> IdEnderecoLojaFisica { get; set; }
    //    public System.Nullable<int> IdUnidadeNegocio { get; set; }
    //}

    public class ViaVarejo_ProdutoDadosPedidoB2B
    {
        public System.Nullable<int> IdLojista { get; set; }
        public int Codigo { get; set; }
        public int Quantidade { get; set; }
        public int Premio { get; set; }
        public System.Nullable<decimal> PrecoVenda { get; set; }
    }
    public class ViaVarejo_DadosPagamentoComplementar
    {
        public int IdFormaPagamento { get; set; }
        public ViaVarejo_DadosCartaoCreditoDTO DadosCartaoCredito { get; set; }
        public ViaVarejo_DadosCartaoCreditoValidacaoDTO DadosCartaoCreditoValidacao { get; set; }
        public decimal ValorComplementarComJuros { get; set; }
        public decimal ValorComplementar { get; set; }
    }
    public class ViaVarejo_DadosCartaoCreditoDTO
    {
        public string Nome { get; set; }
        public string Numero { get; set; }
        public string CodigoVerificador { get; set; }
        public string ValidadeAno { get; set; }
        public string ValidadeMes { get; set; }
        public string ValidadeAnoMes { get; set; }
        public int QuantidadeParcelas { get; set; }
    }
    public class ViaVarejo_DadosCartaoCreditoValidacaoDTO
    {
        public string Nome { get; set; }
        public string NumeroMascarado { get; set; }
        public string QtCaracteresCodigoVerificador { get; set; }
        public string ValidadeAno { get; set; }
        public string ValidadeMes { get; set; }
    }
    public class ViaVarejo_DestinatarioDadosPedidoB2B
    {
        //public string Nome { get; set; }
        public string CpfCnpj { get; set; }
        //public string InscricaoEstadual { get; set; }
        //public string Email { get; set; }
    }
    public class ViaVarejo_EnderecoPedidoB2B : BaseRequest_EnderecoPedido
    {
        //public string CEP { get; set; }
        //public string Estado { get; set; }
        //public string Logradouro { get; set; }
        //public string Cidade { get; set; }
        public int Numero { get; set; }
        //public string Referencia { get; set; }
        //public string Bairro { get; set; }
        //public string Complemento { get; set; }
        //public string Telefone { get; set; }
        //public string Telefone2 { get; set; }
        //public string Telefone3 { get; set; }
    }

    public class ViaVarejo_EnderecoEntregaDadosPedidoB2B : ViaVarejo_EnderecoPedidoB2B { }
    public class ViaVarejo_EnderecoCobrancaDadosPedidoB2B : ViaVarejo_EnderecoPedidoB2B { }


    public class ViaVarejo_IntermediadoresFinanceiros
    {
        /// <summary>
        /// Indica o código da forma de pagamento
        /// à vista = 0
        /// a prazo = 1
        /// </summary>
        public int formaPagamento { get; set; }
        /// <summary>
        /// indica o código do meio de pagamento, sempre com dois dígitos.
        /// Valores possíveis: 
        /// 01=Dinheiro 
        /// 02=Cheque 
        /// 03=Cartão de Crédito 
        /// 04=Cartão de Débito 
        /// 05=Crédito Loja
        /// 10=Vale Alimentação 
        /// 11=Vale Refeição 
        /// 12=Vale Presente 
        /// 13=Vale Combustível 
        /// 15=Boleto Bancário 
        /// 16=Depósito Bancário 
        /// 17=Pagamento Instantâneo(PIX) 
        /// 18=Transferência bancária, Carteira Digital 
        /// 19=Programa de fidelidade, Cashback, Crédito Virtual 
        /// 90=Sem pagamento
        /// </summary>
        public string meioPagamento { get; set; }
        /// <summary>
        /// Valor do pagamento de determinado intermediado
        /// </summary>
        public decimal valorPagamento { get; set; }
        /// <summary>
        /// 1= Pagamento integrado com o sistema de automação da empresa (Ex.: equipamento  TEF, Comércio Eletrônico)
        /// 2= Pagamento não integrado com o sistema de automação da empresa(Ex.:  equipamento POS)
        /// </summary>
        public int tipoIntegracaoPagamento { get; set; }
        /// <summary>
        /// CNPJ do intermediador composto por 18 caracteres (CNPJ formatado com caracteres especiais)
        /// </summary>
        public string cnpjIntermediador { get; set; }
        /// <summary>
        /// Descrição da razão social do intermediador, composto por até 50 caracteres
        /// </summary>
        public string razaoSocialIntermediador { get; set; }
        /// <summary>
        /// Código da bandeira da operadora do cartão, composto por 2 dígitos. 
        /// Valores possíveis: Vide tabela.
        /// </summary>
        public string bandeiraOperadoraCartao { get; set; }
        /// <summary>
        /// NSU gerado conforme padrão de até 10 dígitos
        /// Valores possíveis: 0000000001 e possibilita a impressão de 9.999.999.999
        /// </summary>
        public string numAutorizacaoCartao { get; set; }
    }

    #region CalcShipping

    //public class CalcShippingRequest
    //{
    //    //public string CEP { get; set; }
    //    //public string? CNPJ { get; set; }
    //    //public int IdCampanha { get; set; }
    //    //public List<ProductBasketRequest> Produtos { get; set; }

    //    public string IdUser { get; set; }
    //    public string CEP { get; set; }
    //    public string? CNPJ { get; set; }
    //    public string? CPFParticipante { get; set; }
    //    public string? NumeroParticipante { get; set; }
    //    public decimal? FatorConversao { get; set; }
    //    public int? IdCampanha { get; set; }
    //    public string Store { get; set; }
    //    public List<ProductBasketRequest>? Produtos { get; set; }
    //}

    //public class ProductBasketRequest
    //{
    //    public int Codigo { get; set; }
    //    //public string Codigo { get; set; }
    //    public int Quantidade { get; set; }

    //    public decimal? ValorUnitario { get; set; }
    //}

    #endregion CalcShipping
}
