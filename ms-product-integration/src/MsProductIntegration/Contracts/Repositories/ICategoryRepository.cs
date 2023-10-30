using MsProductIntegration.Contracts.DTOs;

namespace MsProductIntegration.Contracts.Repositories
{
    public interface ICategoryRepository
    {
        Task<IEnumerable<Category>> List();
        Task<Category> Get(string id);
        Task Create(Category Category);
        Task<bool> Update(Category Category);
        Task<bool> Delete(string id);
    }
}
