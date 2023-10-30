using Microsoft.Extensions.Caching.Distributed;
using Ms.Api.Utilities.Models;
using Newtonsoft.Json;
using System.Text;
using User.Api.Connector.Contracts.Repositories;
using User.Api.Contracts.DTO.Request;
using User.Api.Contracts.DTOs;

namespace User.Api.Connector.Repositories
{
    public class RedisRepository : IRedisRepository
    {
        private readonly IDistributedCache _redisCache;

        public RedisRepository(IDistributedCache redisCache)
        {
            _redisCache = redisCache ?? throw new ArgumentNullException(nameof(redisCache));
        }

        public async Task<DefaultResponse<ChocolateUserData>> InsertUserRedis(UserRedisRequest request)
        {
            var response = new DefaultResponse<ChocolateUserData>();

            try
            {
                var cacheKey = request.IdUser;
                string serializedUsersRedis;
                ChocolateUserData userChocolate = new();

                //Primeiro verificar se existe o registro no redis de acordo com a chave 
                var userRedis = await _redisCache.GetAsync(cacheKey);

                if (userRedis != null)
                {
                    serializedUsersRedis = Encoding.UTF8.GetString(userRedis);
                    userChocolate = JsonConvert.DeserializeObject<ChocolateUserData>(serializedUsersRedis);
                    userChocolate.saldo = request.UserChocolate.saldo;
                }
                else
                {
                    userChocolate = request.UserChocolate;
                    serializedUsersRedis = JsonConvert.SerializeObject(userChocolate);
                    userRedis = Encoding.UTF8.GetBytes(serializedUsersRedis);
                }

                await _redisCache.SetAsync(cacheKey, userRedis,
                       options: new DistributedCacheEntryOptions().SetAbsoluteExpiration(DateTime.Now.AddHours(24)).SetSlidingExpiration(TimeSpan.FromHours(24)));

                response.Success = true;
                return response;
            }
            catch (Exception ex)
            {
                response.Success = false;
                throw ex;
            }
        }
    }
}
