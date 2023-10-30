using Ms.Api.Utilities.Models;
using User.Api.Contracts.DTO.Request;
using User.Api.Contracts.DTOs;

namespace User.Api.Connector.Contracts.Repositories
{
    public interface IRedisRepository
    {
        Task<DefaultResponse<ChocolateUserData>> InsertUserRedis(UserRedisRequest request);
    }
}
