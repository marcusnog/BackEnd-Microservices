using Ms.Campaign.Connector.Contracts.DTO.Campaign;

namespace Ms.Campaign.Connector.Contracts.DTO.Request
{
    public class UserRedisRequest
    {
        public string IdUser { get; set; }
        public ChocolateUserData UserChocolate { get; set; }
    }
}
