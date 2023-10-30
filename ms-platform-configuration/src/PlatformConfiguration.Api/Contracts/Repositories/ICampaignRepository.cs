using Ms.Api.Utilities.Models;
using PlatformConfiguration.Api.Contracts.DTOs;

namespace PlatformConfiguration.Api.Contracts.Repositories
{
    public interface ICampaignRepository
    {
        Task<QueryPage<IEnumerable<Campaign>>> List(int page = 0, int pageSize = 10, bool? status = null, string? q = null);
        public Task<Campaign> Get(string id);
        public Task<Campaign> GetByName(string name);
        public Task Create(Campaign obj);
        public Task<bool> Update(Campaign obj);
        public Task<bool> Delete(string id);
        Task<IEnumerable<SelectItem<string>>> ListAsOptions(string clientId);
    }
}