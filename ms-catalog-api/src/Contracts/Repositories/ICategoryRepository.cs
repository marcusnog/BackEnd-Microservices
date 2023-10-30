using Catalog.Api.Contracts.DTOs;
using Ms.Api.Utilities.Models;
using System.Linq.Expressions;

namespace Catalog.Api.Contracts.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> List();
        Task<IEnumerable<SelectItem<string>>> ListAsOptions();
        Task<Category> Get(string id);
        Task<Category> GetByName(string name);
        Task Create(Category Category);
        Task Create(IEnumerable<Category> categories);
        Task<bool> Update(Category Category);
        Task<bool> Delete(string id);
        IEnumerable<Category> Find(Expression<Func<Category, bool>> filter);
    }
}
