namespace MsOrderApi.Contracts.DTOs
{
    public struct ED_ControleFluxo_Pedido
    {
        public Boolean EnviadoLoja;
        public Boolean SalvoBanco;
        public Boolean EnviadoPagarMe;
        public Boolean PontoReservado;
        public Boolean PontoEfetivado;
        public String PedidoReconheceID;
        public Boolean TransacaoFinanceira_Utilizada;
        public Boolean TransacaoBoleto_Utilizada;
        public Boolean TransacaoRecarga_Utilizada;

        public String idReservaPonto;
        public String idDebitoPonto;

    }
}
