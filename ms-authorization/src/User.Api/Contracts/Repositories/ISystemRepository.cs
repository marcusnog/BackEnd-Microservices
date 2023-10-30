using Ms.Api.Utilities.Models;
using System.Linq.Expressions;
using User.Api.Contracts.DTOs;

namespace User.Api.Contracts.Repositories
{
    public interface ISystemRepository
    {
        Task<IEnumerable<SelectItem<string>>> ListAsOptions();
        Task<IEnumerable<Contracts.DTOs.System>> List();
        Task<Contracts.DTOs.System> Find(Expression<Func<Contracts.DTOs.System, bool>> filter);
        Task<Contracts.DTOs.System> FindByName(string name);
        Task Create(Contracts.DTOs.System System);
        Task<bool> UpdateSystem(Contracts.DTOs.System System);
        Task<bool> Delete(string id);
        Task<Contracts.DTOs.System> Get(string id);
        Task<Contracts.DTOs.System> GetByName(string name);
    }
}
