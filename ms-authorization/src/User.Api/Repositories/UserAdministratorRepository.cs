using User.Api.Contracts.DTOs;
using User.Api.Contracts.Repositories;
using MongoDB.Driver;
using User.Api.Extensions;
using System.Text;
using Ms.Api.Utilities.Models;
using MongoDB.Bson;
using System.Text.RegularExpressions;
using User.Api.Contracts.UseCases;

namespace User.Api.Repositories
{
    public class UserAdministratorRepository : IUserAdministratorRepository
    {
        private readonly IAuthContext _context;
        readonly IAccountUseCase _accountUseCase;

        public UserAdministratorRepository(IAuthContext context, IAccountUseCase accountUseCase)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _accountUseCase = accountUseCase ?? throw new ArgumentNullException(nameof(accountUseCase));
        }

        public async Task<IEnumerable<Contracts.DTOs.UserAdministrator>> List()
        {
            return await _context
                            .UserAdministrator
                            .Find(p => true)
                            .ToListAsync();
        }

        public async Task<QueryPage<IEnumerable<Contracts.DTOs.UserAdministrator>>> List(int page = 0, int pageSize = 10, bool? status = null, string? q = null, int? userType = null, string? clientId = null)
        {
            var filter = Builders<Contracts.DTOs.UserAdministrator>.Filter.Empty;
            if (status != null)
                filter &= Builders<Contracts.DTOs.UserAdministrator>.Filter.Eq(x => x.Active, status);
            if (q != null)
                filter &= Builders<Contracts.DTOs.UserAdministrator>.Filter.Regex(x => x.Email, new BsonRegularExpression(new Regex(q, RegexOptions.IgnoreCase)));
            if (userType != null)
                filter &= Builders<Contracts.DTOs.UserAdministrator>.Filter.Eq(x => x.UserType, userType);
            if (clientId != null)
                filter &= Builders<Contracts.DTOs.UserAdministrator>.Filter.Eq(x => x.ClientId, clientId);

            return await GetPagerResultAsync(page, pageSize, _context.UserAdministrator, filter);
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
                            .UserAdministrator
                            .Find(p => p.Login == login && p.IdSystem == s.Id && p.Active)
                            .FirstOrDefaultAsync();

            UserParticipant participant = null;
            if (user == null)
                participant = await _context
                            .UserParticipant
                            .Find(p => p.Login == login && p.IdSystem == s.Id && p.Active)
                            .FirstOrDefaultAsync();

            if (user == null && participant == null) return null;

            var idProfile = user?.IdProfile ?? participant?.IdProfile;
            var profile = await _context
                            .Profiles
                            .Find(p => p.Id == idProfile && p.IdSystem == s.Id)
                            .FirstOrDefaultAsync();

            var account = await _accountUseCase.GetAccount(user.Id, null);

            return new UserInfo
            {
                ClientId = user?.ClientId,
                UserId = user?.Id ?? participant?.Id,
                ClientSecrets = new string[] { cryptography.Decrypt(user?.Password ?? participant?.Password)?.Sha256() },
                Claims = new List<ClientClaim>()
                {
                    new ClientClaim(){
                        Type = "id",
                        Value = user?.Id ?? participant?.Id
                    },
                    new ClientClaim(){
                        Type = "profile",
                        Value = profile?.Name ?? ""
                    },
                    new ClientClaim(){
                        Type = "email",
                        Value = user?.Email ?? participant?.Email
                    },
                    new ClientClaim(){
                        Type = "nickname",
                        Value = user?.Nickname
                                ?? user?.Login
                                ?? participant?.Nickname
                                ?? participant?.Login
                    },
                    new ClientClaim()
                    {
                        Type = "abilities",
                        Value = Newtonsoft.Json.JsonConvert.SerializeObject(profile?.Abilities)
                    },
                    new ClientClaim(){
                        Type = "campaign",
                        Value = participant?.CampaignId ?? ""
                    },
                    new ClientClaim(){
                        Type = "account",
                        Value = account?.Id ?? ""
                    },
                    //new ClientClaim(){
                    //    Type = "acceptedConsentTerm",
                    //    Value = user?.AcceptedConsentTerm.ToString() ?? ""
                    //}
                }
            };
        }

        public async Task<Contracts.DTOs.UserAdministrator> GetByUserType(string id, int userType)
        {
            return await _context
                           .UserAdministrator
                           .Find(p => p.Id == id && p.UserType == userType)
                           .FirstOrDefaultAsync();
        }

        public async Task<Contracts.DTOs.UserAdministrator> Get(string id)
        {
            return await _context
                           .UserAdministrator
                           .Find(p => p.Id == id)
                           .FirstOrDefaultAsync();
        }

        public async Task Create(Contracts.DTOs.UserAdministrator User)
        {
            await _context.UserAdministrator.InsertOneAsync(User);
        }

        public async Task<bool> Update(Contracts.DTOs.UserAdministrator User)
        {
            var updateResult = await _context
                                        .UserAdministrator
                                        .ReplaceOneAsync(filter: g => g.Id == User.Id, replacement: User);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }

        public async Task<bool> Delete(string userId)
        {
            FilterDefinition<Contracts.DTOs.UserAdministrator> filter = Builders<Contracts.DTOs.UserAdministrator>.Filter.Eq(p => p.Id, userId);

            DeleteResult deleteResult = await _context
                                                .UserAdministrator
                                                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<Contracts.DTOs.UserAdministrator> GetUserByEmail(string email, string systemId)
        {
            return await _context
                            .UserAdministrator
                            .Find(p => p.IdSystem == systemId && p.Email == email)
                            .FirstOrDefaultAsync();
        }

        public async Task<bool> UpdatePassword(string userId, string password)
        {
            var user = await Get(userId);
            if (user == null) return false;

            user.Password = password;

            var updateResult = await _context
                                        .UserAdministrator
                                        .ReplaceOneAsync(filter: g => g.Id == user.Id, replacement: user);

            return updateResult.IsAcknowledged
                    && updateResult.ModifiedCount > 0;
        }
    }
}
