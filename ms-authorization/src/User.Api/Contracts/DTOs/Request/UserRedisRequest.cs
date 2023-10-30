using User.Api.Contracts.DTOs;

namespace User.Api.Contracts.DTO.Request
{
    public class UserRedisRequest
    {
        public string IdUser { get; set; }
        public ChocolateUserData UserChocolate { get; set; }
    }
}
