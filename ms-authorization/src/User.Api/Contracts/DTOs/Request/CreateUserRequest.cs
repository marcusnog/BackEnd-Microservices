namespace User.Api.Contracts.DTOs.Request
{
    public class CreateUserRequest
    {
        public string? Nickname { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string Password { get; set; }
        public string? CNPJ { get; set; }
        public string? Cliente { get; set; }
        public string? ClientId { get; set; }
        public bool? NewClientUser { get; set; }
    }
}
