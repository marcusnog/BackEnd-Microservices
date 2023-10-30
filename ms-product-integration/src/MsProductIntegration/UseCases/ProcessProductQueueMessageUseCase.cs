using Platform.Events.Core.Contracts;
using MsProductIntegration.Contracts.DTOs;
using MsProductIntegration.Contracts.Repositories;
using MsProductIntegration.Extensions;
using Newtonsoft.Json;
using Platform.Events.Core.Contracts.Product;
using MsProductIntegration.Contracts.Enum;
using MsProductIntegration.Contracts.UseCases;
using MsProductIntegration.Contracts.Exceptions;
using MongoDB.Bson;
using Microsoft.Extensions.Logging;

namespace MsProductIntegration.UseCases
{
    public class ProcessProductQueueMessageUseCase : IProcessProductQueueMessageUseCase
    {
        readonly IProductRepository _productRepository;
        readonly IProductSkuBillRepository _productSkuBillRepository;
        readonly IProductSkuPreDefinedPriceRepository _productSkuPreDefinedPriceRepository;
        readonly IProductSkuRangePriceRepository _productSkuRangePriceRepository;
        readonly ILogger<ProcessProductQueueMessageUseCase> _logger;

        public ProcessProductQueueMessageUseCase(IProductRepository productRepository, 
            IProductSkuBillRepository productSkuBillRepository,
            IProductSkuPreDefinedPriceRepository productSkuPreDefinedPriceRepository, 
            IProductSkuRangePriceRepository productSkuRangePriceRepository,
            ILogger<ProcessProductQueueMessageUseCase> logger)
        {
            _productRepository = productRepository;
            _productSkuBillRepository = productSkuBillRepository;
            _productSkuPreDefinedPriceRepository = productSkuPreDefinedPriceRepository;
            _productSkuRangePriceRepository = productSkuRangePriceRepository;
            _logger = logger;
        }

        public async Task Process(string message)
        {
            try
            {
                Func<string, Task> processFunc = null;

                var eventBase = JsonConvert.DeserializeObject<EventBaseDTO>(message);
                switch (eventBase?.ApiVersion)
                {
                    case "CreateProduct/v1":
                        processFunc = CreateProductV1;
                        break;
                    case "UpdateProduct/v1":
                        processFunc = UpdateProductV1;
                        break;
                    case "CreateProductSkuPreDefinedPrice/v1":
                        processFunc = CreateProductSkuPreDefinedPriceV1;
                        break;
                    case "UpdateProductSkuPreDefinedPrice/v1":
                        processFunc = UpdateProductSkuPreDefinedPriceV1;
                        break;
                    case "CreateProductSkuRangePrice/v1":
                        processFunc = CreateProductSkuRangePriceV1;
                        break;
                    case "UpdateProductSkuRangePrice/v1":
                        processFunc = UpdateProductSkuRangePriceV1;
                        break;
                    case "CreateProductSkuBill/v1":
                        processFunc = CreateProductSkuBillV1;
                        break;
                    case "UpdateProductSkuBill/v1":
                        processFunc = UpdateProductSkuBillV1;
                        break;
                    case "CreateSkuPreDefinedPrice/v1":
                        processFunc = CreateSkuPreDefinedPriceV1;
                        break;
                    case "UpdateSkuPreDefinedPrice/v1":
                        processFunc = UpdateSkuPreDefinedPriceV1;
                        break;
                    case "CreateSkuRangePrice/v1":
                        processFunc = CreateSkuRangePriceV1;
                        break;
                    case "UpdateSkuRangePrice/v1":
                        processFunc = UpdateSkuRangePriceV1;
                        break;
                    case "CreateSkuBill/v1":
                        processFunc = CreateSkuBillV1;
                        break;
                    case "UpdateSkuBill/v1":
                        processFunc = UpdateSkuBillV1;
                        break;
                    case "AvailabilitySku/v1":
                        processFunc = AvailabilitySkuV1;
                        break;
                    case "AvailabilitySkuPreDefinedPrice/v1":
                        processFunc = AvailabilitySkuPreDefinedPriceV1;
                        break;
                    case "AvailabilitySkuRangePrice/v1":
                        processFunc = AvailabilitySkuRangePriceV1;
                        break;
                    case "AvailabilitySkuBill/v1":
                        processFunc = AvailabilitySkuBillV1;
                        break;
                }

                if (processFunc == null)
                    throw new InvalidEventOperationException();

                await processFunc(message);
            }
            catch (InvalidEventOperationException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new InvalidEventOperationException("Couldn't handle event message", e);
            }

        }

        async Task CreateProductV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<CreateProductEventV1>(message);

            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The creating message product is null");
                return;
            }

            // validation - if product exists -> exit process
            if ((await _productRepository.Find(new { StoreId = eventObj.StoreId, StoreItemCode = eventObj.Code })).Any())
            {
                _logger.LogError($"The product {eventObj.DisplayName} is not found to creating");
                return;
            }


            var product = new Product<Sku>()
            {
                Active = true,
                CategoryId = eventObj.CategoryId,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                Description = eventObj.Description,
                DisplayName = eventObj.DisplayName,
                Images = eventObj.Images?.Select(x => new ProductImage()
                {
                    Size = (ProductImageSize)x.Size,
                    Url = x.Url
                }),
                StoreId = eventObj.StoreId,
                StoreItemCode = eventObj.Code,
                Skus = new List<Sku>(),
                Hash = JsonConvert.SerializeObject(new
                {
                    eventObj.DisplayName,
                    eventObj.Description,
                    eventObj.CategoryId,
                    Active = true,
                    eventObj.Images
                }).Sha256()

            };
            await _productRepository.Create(product);

        }
        async Task UpdateProductV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<UpdateProductEventV1>(message);
            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The updating message product is null");
                return;
            }

            // validation - if product does not exists -> exit process
            if (!(await _productRepository.Find(new { StoreId = eventObj.StoreId, StoreItemCode = eventObj.Code })).Any())
            {
                _logger.LogError($"The product {eventObj.DisplayName} is not found to updating");
                return;
            }

            var product = new Product<Sku>()
            {
                Id = eventObj.Id,
                Active = eventObj.Active,
                CategoryId = eventObj.CategoryId,
                ModificationDate = DateTime.UtcNow.ToUnixTimestamp(),
                DeletionDate = !eventObj.Active ? DateTime.UtcNow.ToUnixTimestamp() : null,
                Description = eventObj.Description,
                DisplayName = eventObj.DisplayName,
                Images = eventObj.Images?.Select(x => new ProductImage()
                {
                    Size = (ProductImageSize)x.Size,
                    Url = x.Url
                }),
                StoreId = eventObj.StoreId,
                StoreItemCode = eventObj.Code,
                Hash = JsonConvert.SerializeObject(new
                {
                    eventObj.DisplayName,
                    eventObj.Description,
                    eventObj.CategoryId,
                    Active = true,
                    eventObj.Images
                }).Sha256()

            };
            await _productRepository.Update(product);
        }

        async Task CreateProductSkuPreDefinedPriceV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<CreateProductSkuPreDefinedPriceEventV1>(message);

            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The creating message product is null");
                return;
            }

            // validation - if product exists -> exit process
            if ((await _productSkuPreDefinedPriceRepository.Find(new { StoreId = eventObj.StoreId, StoreItemCode = eventObj.Code })).Any())
            {
                _logger.LogError($"The product {eventObj.DisplayName} is not found to creating");
                return;
            }

            var product = new Product<SkuPreDefinedPrice>()
            {
                Active = true,
                CategoryId = eventObj.CategoryId,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                Description = eventObj.Description,
                DisplayName = eventObj.DisplayName,
                Images = eventObj.Images?.Select(x => new ProductImage()
                {
                    Size = (ProductImageSize)x.Size,
                    Url = x.Url
                }),
                StoreId = eventObj.StoreId,
                StoreItemCode = eventObj.Code,
                Skus = new List<SkuPreDefinedPrice>(),
                Attributes = eventObj.Attributes,
                Hash = JsonConvert.SerializeObject(new
                {
                    eventObj.DisplayName,
                    eventObj.Description,
                    eventObj.CategoryId,
                    Active = true,
                    eventObj.Images,
                    eventObj.Attributes
                }).Sha256()

            };
            await _productSkuPreDefinedPriceRepository.Create(product);

        }
        async Task UpdateProductSkuPreDefinedPriceV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<UpdateProductSkuPreDefinedPriceEventV1>(message);
            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The updating message product is null");
                return;
            }

            // validation - if product does not exists -> exit process
            if (!(await _productSkuPreDefinedPriceRepository.Find(new { StoreId = eventObj.StoreId, StoreItemCode = eventObj.Code })).Any())
            {
                _logger.LogError($"The product {eventObj.DisplayName} is not found to updating");
                return;
            }

            var product = new Product<SkuPreDefinedPrice>()
            {
                Id = eventObj.Id,
                Active = eventObj.Active,
                CategoryId = eventObj.CategoryId,
                ModificationDate = DateTime.UtcNow.ToUnixTimestamp(),
                DeletionDate = !eventObj.Active ? DateTime.UtcNow.ToUnixTimestamp() : null,
                Description = eventObj.Description,
                DisplayName = eventObj.DisplayName,
                Images = eventObj.Images?.Select(x => new ProductImage()
                {
                    Size = (ProductImageSize)x.Size,
                    Url = x.Url
                }),
                StoreId = eventObj.StoreId,
                StoreItemCode = eventObj.Code,
                Attributes = eventObj.Attributes,
                Hash = JsonConvert.SerializeObject(new
                {
                    eventObj.DisplayName,
                    eventObj.Description,
                    eventObj.CategoryId,
                    Active = true,
                    eventObj.Images,
                    eventObj.Attributes
                }).Sha256()

            };
            await _productSkuPreDefinedPriceRepository.Update(product);
        }

        async Task CreateProductSkuRangePriceV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<CreateProductSkuRangePriceEventV1>(message);

            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The creating message product is null");
                return;
            }

            // validation - if product exists -> exit process
            if ((await _productSkuRangePriceRepository.Find(new { StoreId = eventObj.StoreId, StoreItemCode = eventObj.Code })).Any())
            {
                _logger.LogError($"The product {eventObj.DisplayName} is not found to creating");
                return;
            }

            var product = new Product<SkuRangePrice>()
            {
                Active = true,
                CategoryId = eventObj.CategoryId,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                Description = eventObj.Description,
                DisplayName = eventObj.DisplayName,
                Images = eventObj.Images?.Select(x => new ProductImage()
                {
                    Size = (ProductImageSize)x.Size,
                    Url = x.Url
                }),
                StoreId = eventObj.StoreId,
                StoreItemCode = eventObj.Code,
                Skus = new List<SkuRangePrice>(),
                Hash = JsonConvert.SerializeObject(new
                {
                    eventObj.DisplayName,
                    eventObj.Description,
                    eventObj.CategoryId,
                    Active = true,
                    eventObj.Images
                }).Sha256()

            };
            await _productSkuRangePriceRepository.Create(product);

        }
        async Task UpdateProductSkuRangePriceV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<UpdateProductSkuRangePriceEventV1>(message);
            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The updating message product is null");
                return;
            }

            // validation - if product does not exists -> exit process
            if (!(await _productSkuRangePriceRepository.Find(new { StoreId = eventObj.StoreId, StoreItemCode = eventObj.Code })).Any())
            {
                _logger.LogError($"The product {eventObj.DisplayName} is not found to updating");
                return;
            }

            var product = new Product<SkuRangePrice>()
            {
                Id = eventObj.Id,
                Active = eventObj.Active,
                CategoryId = eventObj.CategoryId,
                ModificationDate = DateTime.UtcNow.ToUnixTimestamp(),
                DeletionDate = !eventObj.Active ? DateTime.UtcNow.ToUnixTimestamp() : null,
                Description = eventObj.Description,
                DisplayName = eventObj.DisplayName,
                Images = eventObj.Images?.Select(x => new ProductImage()
                {
                    Size = (ProductImageSize)x.Size,
                    Url = x.Url
                }),
                StoreId = eventObj.StoreId,
                StoreItemCode = eventObj.Code,
                Hash = JsonConvert.SerializeObject(new
                {
                    eventObj.DisplayName,
                    eventObj.Description,
                    eventObj.CategoryId,
                    Active = true,
                    eventObj.Images
                }).Sha256()

            };
            await _productSkuRangePriceRepository.Update(product);
        }

        async Task CreateProductSkuBillV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<CreateProductSkuBillEventV1>(message);

            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The creating message product is null");
                return;
            }

            // validation - if product exists -> exit process
            if ((await _productSkuBillRepository.Find(new { StoreId = eventObj.StoreId, StoreItemCode = eventObj.Code })).Any())
            {
                _logger.LogError($"The product {eventObj.DisplayName} is not found to creating");
                return;
            }

            var product = new Product<SkuBill>()
            {
                Active = true,
                CategoryId = eventObj.CategoryId,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                Description = eventObj.Description,
                DisplayName = eventObj.DisplayName,
                Images = eventObj.Images?.Select(x => new ProductImage()
                {
                    Size = (ProductImageSize)x.Size,
                    Url = x.Url
                }),
                StoreId = eventObj.StoreId,
                StoreItemCode = eventObj.Code,
                Skus = new List<SkuBill>(),
                Hash = JsonConvert.SerializeObject(new
                {
                    eventObj.DisplayName,
                    eventObj.Description,
                    eventObj.CategoryId,
                    Active = true,
                    eventObj.Images
                }).Sha256()

            };
            await _productSkuBillRepository.Create(product);

        }
        async Task UpdateProductSkuBillV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<UpdateProductSkuBillEventV1>(message);
            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The updating message product is null");
                return;
            }

            // validation - if product does not exists -> exit process
            if (!(await _productSkuBillRepository.Find(new { StoreId = eventObj.StoreId, StoreItemCode = eventObj.Code })).Any())
            {
                _logger.LogError($"The product {eventObj.DisplayName} is not found to updating");
                return;
            }

            var product = new Product<SkuBill>()
            {
                Id = eventObj.Id,
                Active = eventObj.Active,
                CategoryId = eventObj.CategoryId,
                ModificationDate = DateTime.UtcNow.ToUnixTimestamp(),
                DeletionDate = !eventObj.Active ? DateTime.UtcNow.ToUnixTimestamp() : null,
                Description = eventObj.Description,
                DisplayName = eventObj.DisplayName,
                Images = eventObj.Images?.Select(x => new ProductImage()
                {
                    Size = (ProductImageSize)x.Size,
                    Url = x.Url
                }),
                StoreId = eventObj.StoreId,
                StoreItemCode = eventObj.Code,
                Hash = JsonConvert.SerializeObject(new
                {
                    eventObj.DisplayName,
                    eventObj.Description,
                    eventObj.CategoryId,
                    Active = true,
                    eventObj.Images
                }).Sha256()

            };
            await _productSkuBillRepository.Update(product);
        }

        async Task CreateSkuPreDefinedPriceV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<CreateSkuPreDefinedPriceEventV1>(message);

            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
                return;

            // validation - if product does not exists -> exit process
            var product = await _productSkuPreDefinedPriceRepository.Get(eventObj.ProductId);
            if (product == null)
                return;

            // validation - if sku exists -> exit process
            if (product?.Skus?.Any(x => x.StoreItemCode == eventObj.Code) == true)
                return;

            var sku = new SkuPreDefinedPrice()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Active = true,
                Attributes = eventObj.Attributes,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                ListPrice = eventObj.ListPrice,
                SalePrice = eventObj.SalePrice,
                StoreItemCode = eventObj.Code,
                Tags = eventObj.Tags,
                Availability = eventObj.Availability,
                Model = eventObj.Model,
                Hash = JsonConvert.SerializeObject(new
                {
                    eventObj.Code,
                    eventObj.ListPrice,
                    eventObj.SalePrice,
                    eventObj.Attributes,
                    eventObj.Tags,
                    Active = true,
                    eventObj.Availability,
                    eventObj.Model,
                }).Sha256()

            };
            await _productSkuPreDefinedPriceRepository.SetSkuPreDefinedPrice(eventObj.ProductId, sku, true);
        }
        async Task UpdateSkuPreDefinedPriceV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<UpdateSkuPreDefinedPriceEventV1>(message);

            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
                return;

            // validation - if product does not exists -> exit process
            if (_productSkuPreDefinedPriceRepository.Get(eventObj.ProductId) == null)
                return;

            // validation - if product does not exists -> exit process
            var product = await _productSkuPreDefinedPriceRepository.Get(eventObj.ProductId);
            if (product == null)
                return;

            // validation - if sku does not exists -> exit process
            if (product?.Skus?.Any(x => x.StoreItemCode == eventObj.Code) != true)
                return;

            var sku = new SkuPreDefinedPrice()
            {
                Id = eventObj.Id,
                Active = true,
                Attributes = eventObj.Attributes,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                ListPrice = eventObj.ListPrice,
                SalePrice = eventObj.SalePrice,
                StoreItemCode = eventObj.Code,
                Tags = eventObj.Tags,
                Availability = eventObj.Availability,
                Model= eventObj.Model,
                Hash = JsonConvert.SerializeObject(new
                {
                    eventObj.Code,
                    eventObj.ListPrice,
                    eventObj.SalePrice,
                    eventObj.Attributes,
                    eventObj.Tags,
                    Active = true,
                    eventObj.Availability,
                    eventObj.Model
                }).Sha256()

            };
            await _productSkuPreDefinedPriceRepository.SetSkuPreDefinedPrice(eventObj.ProductId, sku, false);
        }

        async Task CreateSkuRangePriceV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<CreateSkuRangePriceEventV1>(message);

            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The creating message sku is null");
                return;
            }

            // validation - if product does not exists -> exit process
            var product = await _productSkuRangePriceRepository.Get(eventObj.ProductId);
            if (product == null)
            {
                _logger.LogError($"The product is not found to creating sku");
                return;
            }

            // validation - if sku exists -> exit process
            if (product?.Skus?.Any(x => x.StoreItemCode == eventObj.Code) == true)
            {
                _logger.LogError($"The product sku is not found");
                return;
            }

            var sku = new SkuRangePrice()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Active = true,
                Attributes = eventObj.Attributes,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                MinPrice = eventObj.MinPrice,
                MaxPrice = eventObj.MaxPrice,
                StoreItemCode = eventObj.Code,
                Tags = eventObj.Tags,
                Availability = eventObj.Availability,
                Hash = JsonConvert.SerializeObject(new
                {
                    eventObj.Code,
                    eventObj.MinPrice,
                    eventObj.MaxPrice,
                    eventObj.Attributes,
                    eventObj.Tags,
                    Active = true,
                    eventObj.Availability
                }).Sha256()

            };
            await _productSkuRangePriceRepository.SetSkuRangePrice(eventObj.ProductId, sku, true);
        }
        async Task UpdateSkuRangePriceV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<UpdateSkuRangePriceEventV1>(message);

            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The updating message sku is null");
                return;
            }

            // validation - if product does not exists -> exit process
            if (_productSkuRangePriceRepository.Get(eventObj.ProductId) == null)
            {
                _logger.LogError($"The product is not found to updating sku");
                return;
            }

            // validation - if product does not exists -> exit process
            var product = await _productSkuRangePriceRepository.Get(eventObj.ProductId);
            if (product == null)
            {
                _logger.LogError($"The product is not found to updating sku");
                return;
            }

            // validation - if sku does not exists -> exit process
            if (product?.Skus?.Any(x => x.StoreItemCode == eventObj.Code) != true)
            {
                _logger.LogError($"The product sku is not found");
                return;
            }

            var sku = new SkuRangePrice()
            {
                Id = eventObj.Id,
                Active = true,
                Attributes = eventObj.Attributes,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                MinPrice = eventObj.MinPrice,
                MaxPrice = eventObj.MaxPrice,
                StoreItemCode = eventObj.Code,
                Tags = eventObj.Tags,
                Availability = eventObj.Availability,
                Hash = JsonConvert.SerializeObject(new
                {
                    eventObj.Code,
                    eventObj.MinPrice,
                    eventObj.MaxPrice,
                    eventObj.Attributes,
                    eventObj.Tags,
                    Active = true,
                    eventObj.Availability
                }).Sha256()

            };
            await _productSkuRangePriceRepository.SetSkuRangePrice(eventObj.ProductId, sku, false);
        }

        async Task CreateSkuBillV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<CreateSkuBillEventV1>(message);

            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The creating message sku is null");
                return;
            }

            // validation - if product does not exists -> exit process
            var product = await _productSkuBillRepository.Get(eventObj.ProductId);
            if (product == null)
            {
                _logger.LogError($"The product is not found to creating sku");
                return;
            }

            // validation - if sku exists -> exit process
            if (product?.Skus?.Any(x => x.StoreItemCode == eventObj.Code) == true)
            {
                _logger.LogError($"The product sku is not found");
                return;
            }

            var sku = new SkuBill()
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Active = true,
                Attributes = eventObj.Attributes,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                BarCode = eventObj.BarCode,
                TotalPrice = eventObj.TotalPrice,
                DueDate = eventObj.DueDate,
                Document = eventObj.Document,
                StoreItemCode = eventObj.Code,
                Tags = eventObj.Tags,
                Availability = eventObj.Availability,
                Hash = JsonConvert.SerializeObject(new
                {
                    eventObj.Code,
                    eventObj.BarCode,
                    eventObj.TotalPrice,
                    eventObj.DueDate,
                    eventObj.Document,
                    eventObj.Attributes,
                    eventObj.Tags,
                    Active = true,
                    eventObj.Availability
                }).Sha256()

            };
            await _productSkuBillRepository.SetSkuBill(eventObj.ProductId, sku, true);
        }
        async Task UpdateSkuBillV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<UpdateSkuBillEventV1>(message);

            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The updating message sku is null");
                return;
            }

            // validation - if product does not exists -> exit process
            if (_productSkuBillRepository.Get(eventObj.ProductId) == null)
            {
                _logger.LogError($"The product is not found to updating sku");
                return;
            }

            // validation - if product does not exists -> exit process
            var product = await _productSkuBillRepository.Get(eventObj.ProductId);
            if (product == null)
            {
                _logger.LogError($"The product is not found to updating sku");
                return;
            }

            // validation - if sku does not exists -> exit process
            if (product?.Skus?.Any(x => x.StoreItemCode == eventObj.Code) != true)
            {
                _logger.LogError($"The product sku is not found");
                return;
            }

            var sku = new SkuBill()
            {
                Id = eventObj.Id,
                Active = true,
                Attributes = eventObj.Attributes,
                CreationDate = DateTime.UtcNow.ToUnixTimestamp(),
                BarCode = eventObj.BarCode,
                TotalPrice = eventObj.TotalPrice,
                DueDate = eventObj.DueDate,
                Document = eventObj.Document,
                StoreItemCode = eventObj.Code,
                Tags = eventObj.Tags,
                Availability = eventObj.Availability,
                Hash = JsonConvert.SerializeObject(new
                {
                    eventObj.Code,
                    eventObj.BarCode,
                    eventObj.TotalPrice,
                    eventObj.DueDate,
                    eventObj.Document,
                    eventObj.Attributes,
                    eventObj.Tags,
                    Active = true,
                    eventObj.Availability
                }).Sha256()

            };
            await _productSkuBillRepository.SetSkuBill(eventObj.ProductId, sku, false);
        }

        async Task AvailabilitySkuV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<AvailabilitySkuEventV1>(message);

            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The updating message availability is null");
                return;
            }

            // validation - if product does not exists -> exit process
            if (_productRepository.Get(eventObj.ProductId) == null)
            {
                _logger.LogError($"The product is not found to updating availability");
                return;
            }

            // validation - if product does not exists -> exit process
            var product = await _productRepository.Get(eventObj.ProductId);
            if (product == null)
            {
                _logger.LogError($"The product is not found to updating availability");
                return;
            }

            // validation - if sku does not exists -> exit process
            var skuDb = product?.Skus?.FirstOrDefault(x => x.Id == eventObj.SkuId);
            if (skuDb is null)
            {
                _logger.LogError($"The product sku is not found to updating availability");
                return;
            }

            skuDb.Availability = eventObj.Availability;

            await _productRepository.SetSku(eventObj.ProductId, skuDb, false);
        }
        async Task AvailabilitySkuPreDefinedPriceV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<AvailabilitySkuPreDefinedPriceEventV1>(message);

            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The updating message availability is null");
                return;
            }

            // validation - if product does not exists -> exit process
            if (_productSkuPreDefinedPriceRepository.Get(eventObj.ProductId) == null)
            {
                _logger.LogError($"The product is not found to updating availability");
                return;
            }

            // validation - if product does not exists -> exit process
            var product = await _productSkuPreDefinedPriceRepository.Get(eventObj.ProductId);
            if (product == null)
            {
                _logger.LogError($"The product is not found to updating availability");
                return;
            }

            // validation - if sku does not exists -> exit process
            var skuDb = product?.Skus?.FirstOrDefault(x => x.Id == eventObj.SkuId);
            if (skuDb is null)
            {
                _logger.LogError($"The product sku is not found to updating availability");
                return;
            }

            skuDb.Availability = eventObj.Availability;

            await _productSkuPreDefinedPriceRepository.SetSkuPreDefinedPrice(eventObj.ProductId, skuDb, false);
        }
        async Task AvailabilitySkuRangePriceV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<AvailabilitySkuRangePriceEventV1>(message);

            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The updating message availability is null");
                return;
            }

            // validation - if product does not exists -> exit process
            if (_productSkuRangePriceRepository.Get(eventObj.ProductId) == null)
            {
                _logger.LogError($"The product is not found to updating availability");
                return;
            }

            // validation - if product does not exists -> exit process
            var product = await _productSkuRangePriceRepository.Get(eventObj.ProductId);
            if (product == null)
            {
                _logger.LogError($"The product is not found to updating availability");
                return;
            }

            // validation - if sku does not exists -> exit process
            var skuDb = product?.Skus?.FirstOrDefault(x => x.Id == eventObj.SkuId);
            if (skuDb is null)
            {
                _logger.LogError($"The product sku is not found to updating availability");
                return;
            }

            skuDb.Availability = eventObj.Availability;

            await _productSkuRangePriceRepository.SetSkuRangePrice(eventObj.ProductId, skuDb, false);
        }
        async Task AvailabilitySkuBillV1(string message)
        {
            var eventObj = JsonConvert.DeserializeObject<AvailabilitySkuBillEventV1>(message);

            // validation
            // validation - if message not recognized -> exit process
            if (eventObj == null)
            {
                _logger.LogError("The updating message availability is null");
                return;
            }

            // validation - if product does not exists -> exit process
            if (_productSkuBillRepository.Get(eventObj.ProductId) == null)
            {
                _logger.LogError($"The product is not found to updating availability");
                return;
            }

            // validation - if product does not exists -> exit process
            var product = await _productSkuBillRepository.Get(eventObj.ProductId);
            if (product == null)
            {
                _logger.LogError($"The product is not found to updating availability");
                return;
            }

            // validation - if sku does not exists -> exit process
            var skuDb = product?.Skus?.FirstOrDefault(x => x.Id == eventObj.SkuId);
            if (skuDb is null)
            {
                _logger.LogError($"The product sku is not found to updating availability");
                return;
            }

            skuDb.Availability = eventObj.Availability;

            await _productSkuBillRepository.SetSkuBill(eventObj.ProductId, skuDb, false);
        }
    }
}