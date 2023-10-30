using Catalog.Api.Contracts.DTOs;
using Ms.Api.Utilities.Models;
using System.Linq.Expressions;

namespace Catalog.Api.Contracts.Repositories
{
    public interface IMainCategoryRepository
    {
        Task<IEnumerable<MainCategory>> List();
        Task<IEnumerable<SelectItem<string>>> ListAsOptions();
        Task<MainCategory> Get(string id);
        Task<MainCategory> GetByName(string name);
        Task Create(MainCategory Category);
        Task Create(IEnumerable<MainCategory> categories);
        Task<bool> Update(MainCategory Category);
        Task<bool> Delete(string id);
        IEnumerable<MainCategory> Find(Expression<Func<MainCategory, bool>> filter);
    }
}
