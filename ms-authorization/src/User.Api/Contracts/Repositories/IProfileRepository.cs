using Ms.Api.Utilities.Models;
using User.Api.Contracts.DTOs;

namespace User.Api.Contracts.Repositories
{
    public interface IProfileRepository
    {
        Task<IEnumerable<SelectItem<string>>> ListAsOptions(string systemId);
        Task<IEnumerable<Profile>> List();
        Task<Profile> Get(string id);
        Task<Profile> GetByName(string name);
        Task Create(Profile Profile);
        Task<bool> UpdateProfile(Profile Profile);
        Task<bool> Delete(string id);
    }
}
