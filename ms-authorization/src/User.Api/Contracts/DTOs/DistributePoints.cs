namespace User.Api.Contracts.DTOs
{
    public class DistributePoints
    {
        public string Cpf { get; set; }
        public string ParticipantName { get; set; }
        public decimal Points { get; set; }
        public string? Observation { get; set; }
    }
}
