using Ms.Api.Utilities.Models;
using PlatformConfiguration.Api.Contracts.DTOs;

namespace PlatformConfiguration.Api.Contracts.Repositories
{
    public interface IClientRepository
    {
        Task<IEnumerable<SelectItem<string>>> ListAsOptions();
        Task<QueryPage<IEnumerable<Client>>> List(int page = 0, int pageSize = 10, bool? status = null, string? q = null);
        Task<IEnumerable<Client>> List();
        public Task<Client> Get(string id);
        public Task Create(Client obj);
        public Task<bool> Update(Client obj);
        public Task<bool> Delete(string id);
    }
}