using Catalog.Api.Contracts.DTOs;
using Catalog.Api.Contracts.Repositories;
using System.Diagnostics;

namespace Catalog.Api.Contracts.Migrations
{
    public static class V3Migration
    {
        public static async Task Apply(ICategoryRepository categoryRepository, IMainCategoryRepository mainCategoryRepository, IProductSkuRepository productRepository, IDbVersionRepository dbVersionRepository)
        {
            var categories = await categoryRepository.List();

            var dictionary = new Dictionary<string, Dictionary<string, List<string>>>();

            #region level 1

            dictionary.Add("Ar e Ventilação", new Dictionary<string, List<string>>());
            dictionary.Add("Artigos para Festas", new Dictionary<string, List<string>>());
            dictionary.Add("Automotivo", new Dictionary<string, List<string>>());
            dictionary.Add("Bebês", new Dictionary<string, List<string>>());
            dictionary.Add("Beleza e Saúde", new Dictionary<string, List<string>>());
            dictionary.Add("Brinquedos", new Dictionary<string, List<string>>());
            dictionary.Add("Calçados", new Dictionary<string, List<string>>());
            dictionary.Add("Cama, Mesa e Banho", new Dictionary<string, List<string>>());
            dictionary.Add("Câmeras, Filmadoras e Drones", new Dictionary<string, List<string>>());
            dictionary.Add("Cartão Presente Virtual", new Dictionary<string, List<string>>());
            dictionary.Add("Casa e Construção", new Dictionary<string, List<string>>());
            dictionary.Add("DVDs e Blu-Ray", new Dictionary<string, List<string>>());
            dictionary.Add("Eletrodomésticos", new Dictionary<string, List<string>>());
            dictionary.Add("Eletroportáteis", new Dictionary<string, List<string>>());
            dictionary.Add("Esporte e Lazer", new Dictionary<string, List<string>>());
            dictionary.Add("Ferramentas", new Dictionary<string, List<string>>());
            dictionary.Add("Games", new Dictionary<string, List<string>>());
            dictionary.Add("Informática", new Dictionary<string, List<string>>());
            dictionary.Add("Instrumentos Musicais", new Dictionary<string, List<string>>());
            dictionary.Add("Malas e Mochilas", new Dictionary<string, List<string>>());
            dictionary.Add("Móveis", new Dictionary<string, List<string>>());
            dictionary.Add("Papelaria", new Dictionary<string, List<string>>());           
            dictionary.Add("Perfumaria e Cosméticos", new Dictionary<string, List<string>>());
            dictionary.Add("Relógios", new Dictionary<string, List<string>>());
            dictionary.Add("Tablets", new Dictionary<string, List<string>>());
            dictionary.Add("TV e Vídeo", new Dictionary<string, List<string>>());            
            dictionary.Add("Utilidades Domésticas", new Dictionary<string, List<string>>());

            #endregion

            var mainCategories = await mainCategoryRepository.List();
            foreach (var x in mainCategories)
            {
                await mainCategoryRepository.Delete(x.Id);
            }

            foreach (var item in categories)
            {
                if (dictionary.ContainsKey(item.Name))
                {
                    await mainCategoryRepository.Create(new MainCategory() 
                    {
                        Id = item.Id,
                        Code = item.Code,
                        Name = item.Name,
                        CreationDate = item.CreationDate,
                        DeletionDate = item.DeletionDate,
                        Active = item.Active,
                        Children = item.Children
                    });   
                }
            }
          
            await dbVersionRepository.SetVersion("1.1.4");
        }
    }
}
