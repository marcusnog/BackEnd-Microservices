namespace MsProductIntegrationNovapontocom.Contracts.DTOs
{
    public class CategoryPartner
    {
        public int LojaId { get; set; }
        public int Id { get; set; }
        public bool Ativo { get; set; }
        public DateTime DataCadastro { get; set; }
        public int? DepartamentoId { get; set; }
        public int Level { get; set; }
        public string Nome { get; set; }
        public int? CategoriaPaiId { get; set; }

    }
}
