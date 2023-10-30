using Catalog.Api.Contracts.DTOs;
using Catalog.Api.Contracts.Repositories;
using System.Diagnostics;

namespace Catalog.Api.Contracts.Migrations
{
    public static class V2Migration
    {
        public static async Task Apply(ICategoryRepository categoryRepository, IProductSkuRepository productRepository, IDbVersionRepository dbVersionRepository)
        {
            var dictionary = new Dictionary<string, Dictionary<string, List<string>>>();

            #region level 1

            dictionary.Add("Cartão Presente Virtual", new Dictionary<string, List<string>>());

            #endregion


            var categories = dictionary.Select(c1 => new Category(c1.Key)
            {
                Children = c1.Value.Select(c2 => new Category(c2.Key)
                {
                    Children = c2.Value.Select(c3 => new Category(c3) { Children = new Category[0] })?.ToArray() ?? Array.Empty<Category>()
                })?.ToArray() ?? Array.Empty<Category>()
            });

            var category = categoryRepository.GetByName("Cartão Presente").Result;

            if (category != null) 
            {
                category.Name = "Cartão Presente Físico";
                await categoryRepository.Update(category);
            }

            await categoryRepository.Create(categories);

            await dbVersionRepository.SetVersion("1.1.3");
        }
    }
}
