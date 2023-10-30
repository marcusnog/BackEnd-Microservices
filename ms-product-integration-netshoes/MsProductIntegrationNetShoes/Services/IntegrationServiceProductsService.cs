using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MsProductIntegrationNetShoes.Contracts.DTOs;
using MsProductIntegrationNetShoes.Contracts.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MsProductIntegrationNetShoes.Services
{
    public class IntegrationServiceProductsService : IIntegrationServiceProductsService
    {
        readonly IConfiguration _configuration;
        readonly ILogger<IntegrationServiceProductsService> _logger;
        public readonly string _urlProductsDb;
        public readonly string _urlCategoryDb;
        public readonly string _store;
        public readonly string _typeProductSku;

        public IntegrationServiceProductsService(IConfiguration configuration, ILogger<IntegrationServiceProductsService> logger)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));


            _store = _configuration.GetValue<string>("StoreId");
            _typeProductSku = _configuration.GetValue<string>("TypeProductSku");
            _urlProductsDb = String.Format(_configuration.GetValue<string>("DatabaseProducts"), _store, _typeProductSku);
            _urlCategoryDb = _configuration.GetValue<string>("DatabaseCategory");
        }
        public async Task<Dictionary<string, ProductsDB>> GetProductsDB()
        {
            try
            {
                HttpClient client = new();

                client.Timeout = TimeSpan.FromMinutes(5);
                var response = await client.GetAsync($"{_urlProductsDb}");
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception("unexpected return", new Exception(content));

                return JsonConvert.DeserializeObject<Dictionary<string, ProductsDB>>(content);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying get products from Database");
                throw;
            }
        }

        public async Task<IEnumerable<CategoryDB>> GetCategoriesDB()
        {
            try
            {
                HttpClient client = new();

                var response = await client.GetAsync($"{_urlCategoryDb}");
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                    throw new Exception("unexpected return", new Exception(content));

                List<CategoryDB> lstCategoriesComplete = new();
                var lstLevel1 = JsonConvert.DeserializeObject<IEnumerable<CategoryDB>>(content);

                if (lstLevel1 != null && lstLevel1.Any())
                {
                    lstCategoriesComplete.AddRange(lstLevel1);

                    foreach (var level1 in lstLevel1)
                    {
                        if (level1.Children != null && level1.Children.Any())
                            lstCategoriesComplete.AddRange(level1.Children);

                        foreach (var level2 in level1.Children)
                        {
                            if (level2.Children != null && level2.Children.Any())
                                lstCategoriesComplete.AddRange(level2.Children);
                        }
                    }
                }

                return lstCategoriesComplete.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error when trying get categories from Database");
                throw;
            }
        }
    }
}
