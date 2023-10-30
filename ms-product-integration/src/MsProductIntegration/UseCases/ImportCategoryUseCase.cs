using MsProductIntegration.Contracts.Repositories;

namespace MsProductIntegration.UseCases
{
    public class ImportCategoryUseCase
    {
        readonly ICategoryRepository _categoryRepository;
        public ImportCategoryUseCase(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public Task Import()
        {
            // TODO: check if category exists
            // if exists
                // TODO: if SHA256 checksum == current SHA256 checksum
                    // do nothing
                // else
                    // update
            // else
                // create
            throw new NotImplementedException();
        }
    }
}
