namespace Ms.Campaign.Connector.Contracts.DTO.Campaign
{
    public class ChocolateDefaultResponse<T>
    {
        public T dados { get; set; }
        public bool sucesso { get; set; }
        public string mensagem { get; set; }
    }
    public class ChocolateErrorResponse
    {
        public int status { get; set; }
        public string error { get; set; }
        public int statusCode { get; set; }
        public string message { get; set; }
    }
    public class ChocolateUserData
    {
        public string id { get; set; }
        public string userId { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public string dataNascimento { get; set; }
        public string email { get; set; }
        public string telefone { get; set; }
        public string celular { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string complemento { get; set; }
        public string bairro { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string cep { get; set; }
        public decimal saldo { get; set; }
        public string reserva { get; set; }
        public string extratoId { get; set; }
        public bool? internCampaign { get; set; }
    }
}
