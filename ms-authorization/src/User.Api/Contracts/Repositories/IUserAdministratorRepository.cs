using Ms.Api.Utilities.Models;
using User.Api.Contracts.DTOs;
using User.Api.Extensions;

namespace User.Api.Contracts.Repositories
{
    public interface IUserAdministratorRepository
    {
        Task<QueryPage<IEnumerable<DTOs.UserAdministrator>>> List(int page = 0, int pageSize = 10, bool? status = null, string? q = null, int? userType = null, string? clientId = null);
        Task<IEnumerable<DTOs.UserAdministrator>> List();
        Task<UserInfo> GetUserInfo(string login, string system, Cryptography cryptography);
        Task<Contracts.DTOs.UserAdministrator> GetByUserType(string id, int userType);
        Task<DTOs.UserAdministrator> Get(string id);
        Task Create(DTOs.UserAdministrator User);
        Task<bool> Update(DTOs.UserAdministrator User);
        Task<bool> Delete(string userId);
        Task<DTOs.UserAdministrator> GetUserByEmail(string email, string systemId);
        Task<bool> UpdatePassword(string userId, string password);
    }
}
