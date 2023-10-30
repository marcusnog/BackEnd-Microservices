using Ms.Api.Utilities.Models;
using Ms.Campaign.Connector.Contracts.DTO.Campaign;
using Ms.Campaign.Connector.Contracts.DTO.Request;

namespace Ms.Campaign.Connector.Contracts.Repositories
{
    public interface IRedisRepository
    {
        Task<DefaultResponse<ChocolateUserData>> InsertUserRedis(UserRedisRequest request);
    }
}
