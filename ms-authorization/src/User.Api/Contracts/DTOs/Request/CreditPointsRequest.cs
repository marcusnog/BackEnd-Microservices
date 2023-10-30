namespace User.Api.Contracts.DTOs.Request
{
    public class CreditPointsRequest
    {
        public string AccountId { get; set; }
        public string Cpf { get; set; }
        public decimal Value { get; set; }
        public string? Description { get; set; }
    }
}
