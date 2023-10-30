using Ms.Api.Utilities.Models;
using User.Api.Contracts.DTOs;
using User.Api.Extensions;

namespace User.Api.Contracts.Repositories
{
    public interface IUserParticipantRepository
    {
        Task<QueryPage<IEnumerable<DTOs.UserParticipant>>> List(string? campaignId = null, int page = 0, int pageSize = 10, bool? status = null, string? q = null, int? userType = null);
        Task<IEnumerable<DTOs.UserParticipant>> List();
        Task<UserInfo> GetUserInfo(string login, string system, Cryptography cryptography);
        Task<Contracts.DTOs.UserParticipant> GetByUserType(string id, int userType);
        Task<DTOs.UserParticipant> Get(string id);
        Task Create(DTOs.UserParticipant User);
        Task<bool> Update(DTOs.UserParticipant User);
        Task<bool> Delete(string userId);
        Task<DTOs.UserParticipant> GetUserByEmail(string email, string systemId);
        Task<bool> UpdatePassword(string userId, string password);
    }
}
