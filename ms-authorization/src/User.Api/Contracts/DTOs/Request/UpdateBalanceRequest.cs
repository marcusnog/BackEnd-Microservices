namespace User.Api.Contracts.DTOs.Request
{
    public class UpdateBalanceRequest
    {
        public string UserId { get; set; }
        public string Cpf { get; set; }
        public decimal Balance { get; set; }
    }
}
