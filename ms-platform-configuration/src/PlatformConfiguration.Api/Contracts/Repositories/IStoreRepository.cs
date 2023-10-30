using Ms.Api.Utilities.Models;
using PlatformConfiguration.Api.Contracts.DTOs;
using System.Linq.Expressions;

namespace PlatformConfiguration.Api.Contracts.Repositories
{
    public interface IStoreRepository
    {
        public Task<Store> Get(string id);
        public Task Create(Store obj);
        public Task<bool> Update(Store obj);
        public Task<bool> Delete(string id);
        public IEnumerable<Store> Find(Expression<Func<Store, bool>> filter);
        Task<IEnumerable<SelectItem<string>>> ListAsOptions();
        Task<IEnumerable<Store>> ListStoresGiftty();
    }
}