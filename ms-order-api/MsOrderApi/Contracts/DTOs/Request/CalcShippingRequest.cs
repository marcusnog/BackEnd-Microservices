namespace MsOrderApi.Contracts.DTOs.Request
{
    public class CalcShippingRequest
    {
        public string IdUser { get; set; }
        public string CEP { get; set; }
        public string? CNPJ { get; set; }
        public string? CPFParticipante { get; set; }
        public string? NumeroParticipante { get; set; }
        public decimal? FatorConversao { get; set; }
        public int? IdCampanha { get; set; }
        public string Store { get; set; }
        public List<ProductBasketRequest>? Produtos { get; set; }
    }

    public class ProductBasketRequest
    {
        public string Codigo { get; set; }
        public int Quantidade { get; set; }
        public decimal? ValorUnitario { get; set; }
    }
}
