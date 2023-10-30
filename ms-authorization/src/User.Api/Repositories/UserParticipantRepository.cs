using MongoDB.Bson;
using MongoDB.Driver;
using Ms.Api.Utilities.Models;
using System.Text.RegularExpressions;
using User.Api.Connector.Contracts.Repositories;
using User.Api.Contracts.DTO.Request;
using User.Api.Contracts.DTOs;
using User.Api.Contracts.Repositories;
using User.Api.Extensions;

namespace User.Api.Repositories
{
    public class UserParticipantRepository : IUserParticipantRepository
    {
        private readonly IAuthContext _context;
        private readonly IRedisRepository _redisRepository;

        public UserParticipantRepository(IAuthContext context, IRedisRepository redisRepository )
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _redisRepository = redisRepository ?? throw new ArgumentNullException(nameof(redisRepository));
        }

        public async Task<IEnumerable<Contracts.DTOs.UserParticipant>> List()
        {
            return await _context
                            .UserParticipant
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<QueryPage<IEnumerable<Contracts.DTOs.UserParticipant>>> List(string? campaignId = null, int page = 0, int pageSize = 10, bool? status = null, string? q = null, int? userType = null)
        {
            var filter = Builders<Contracts.DTOs.UserParticipant>.Filter.Empty;
            if (status != null)
                filter &= Builders<Contracts.DTOs.UserParticipant>.Filter.Eq(x => x.Active, status);
            if (q != null)
                filter &= Builders<Contracts.DTOs.UserParticipant>.Filter.Regex(x => x.Email, new BsonRegularExpression(new Regex(q, RegexOptions.IgnoreCase)));
            if (userType != null)
                filter &= Builders<Contracts.DTOs.UserParticipant>.Filter.Eq(x => x.UserType, userType);
            if (!string.IsNullOrEmpty(campaignId))
                filter &= Builders<Contracts.DTOs.UserParticipant>.Filter.Eq(x => x.CampaignId, campaignId);

            return await GetPagerResultAsync(page, pageSize, _context.UserParticipant, filter);
        }

        private static async Task<QueryPage<IEnumerable<T>>> GetPagerResultAsync<T>(int page, int pageSize, IMongoCollection<T> collection, FilterDefinition<T>? filter = null)
        {
            // count facet, aggregation stage of count
            var countFacet = AggregateFacet.Create("countFacet",
                PipelineDefinition<T, AggregateCountResult>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Count<T>()
                }));

            // data facet, we’ll use this to sort the data and do the skip and limiting of the results for the paging.
            var dataFacet = AggregateFacet.Create("dataFacet",
                PipelineDefinition<T, T>.Create(new[]
                {
                    PipelineStageDefinitionBuilder.Skip<T>((page - 1) * pageSize),
                    PipelineStageDefinitionBuilder.Limit<T>(pageSize),
                }));

            filter ??= Builders<T>.Filter.Empty;

            var aggregation = await collection.Aggregate()
                .Match(filter)
                .Facet(countFacet, dataFacet)
                .ToListAsync();

            var count = aggregation.First()
                .Facets.First(x => x.Name == "countFacet")
                .Output<AggregateCountResult>()
                ?.FirstOrDefault()
                ?.Count ?? 0;

            var data = aggregation.First()
                .Facets.First(x => x.Name == "dataFacet")
                .Output<T>();

            return new QueryPage<IEnumerable<T>>
            {
                TotalItems = count,
                PageSize = pageSize,
                Page = page,
                Items = data
            };
        }

        public async Task<UserInfo?> GetUserInfo(string login, string system, Cryptography cryptography)
        {
            var s = await _context
                            .Systems
                            .Find(p => p.Name.ToLower() == system.ToLower())
                            .FirstOrDefaultAsync();
            if (s == null) return null;

            var user = await _context
                            .UserParticipant
                            .Find(p => p.Login == login && p.IdSystem == s.Id && p.Active)
                            .FirstOrDefaultAsync();

            if (user == null) return null;


            //var profile = await _context
            //                .Profiles
            //                .Find(p => p.Id == user.IdProfile && p.IdSystem == s.Id)
            //                .FirstOrDefaultAsync();

            //user.Profile = profile;
            //user.System = s;

            var userRedis = new UserRedisRequest();
            userRedis.IdUser = user.Id;
            userRedis.UserChocolate = new Contracts.DTOs.ChocolateUserData()
            {
                id = user.Id,
                email = user.Email,
                nome = user.Nickname,
                telefone = user.Phone,
                userId = user.Id,
                celular = user.Phone,
                internCampaign = true
            };

            var account =  await _context.Account.Find(x => x.UserId == user.Id).FirstOrDefaultAsync();

            userRedis.UserChocolate.saldo = account?.Balance ?? 0;

            var returnRedis = _redisRepository.InsertUserRedis(userRedis).Result;

            if (!returnRedis.Success)
                throw new ArgumentException(returnRedis.Message);

            return new UserInfo
            {
                ClientId = null,
                UserId = user.Id,
                ClientSecrets = new string[] { cryptography.Decrypt(user.Password).Sha256() },
                Claims = new List<ClientClaim>()
                {
                    new ClientClaim(){
                        Type = "userId",
                        Value = user.Id
                    },
                    new ClientClaim()
                    {
                        Type = "campaign",
                        Value = user.CampaignId
                    }
                }
            };
        }

        public async Task<Contracts.DTOs.UserParticipant> GetByUserType(string id, int userType)
        {
            return await _context
                           .UserParticipant
                           .Find(p => p.Id == id && p.UserType == userType)
                           .FirstOrDefaultAsync();
        }

        public async Task<Contracts.DTOs.UserParticipant> Get(string id)
        {
            return await _context
                           .UserParticipant
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Contracts.DTOs.UserParticipant User)
        {
            await _context.UserParticipant.InsertOneAsync(User);
        }

        public async Task<bool> Update(Contracts.DTOs.UserParticipant User)
        {
            var updateResult = await _context
                                        .UserParticipant
                                        .ReplaceOneAsync(filter: g => g.Id == User.Id, replacement: User);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string userId)
        {
            FilterDefinition<Contracts.DTOs.UserParticipant> filter = Builders<Contracts.DTOs.UserParticipant>.Filter.Eq(p => p.Id, userId);

            DeleteResult deleteResult = await _context
                                                .UserParticipant
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<Contracts.DTOs.UserParticipant> GetUserByEmail(string email, string systemId)
        {
            return await _context
                            .UserParticipant
                            .Find(p => p.IdSystem == systemId && p.Email == email)
                            .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatePassword(string userId, string password)
        {
            var user = await Get(userId);
            if (user == null) return false;

            user.Password = password;

            var updateResult = await _context
                                        .UserParticipant
                                        .ReplaceOneAsync(filter: g => g.Id == user.Id, replacement: user);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
