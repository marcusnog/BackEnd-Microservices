using Ms.Api.Utilities.Enum;

namespace Ms.Api.Utilities.DTO.Request
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
        //public List<CalcShippingRequest_Product>? Produtos { get; set; }

        public IEnumerable<CalcShippingRequest_Store> Shops { get; set; }
    }

    //public class CalcShippingRequest_Product
    //{
    //    public string Codigo { get; set; }
    //    public int Quantidade { get; set; }
    //    public decimal? ValorUnitario { get; set; }
    //}

    public class CalcShippingRequest_Store
    {
        public Enums.Store Store { get; set; }

        public List<Generic_CalcShipping_Product_Request> Produtos { get; set; }
    }
}
