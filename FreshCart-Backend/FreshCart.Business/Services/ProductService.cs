using AutoMapper;
using Azure.Storage.Blobs;
using FreshCart.Business.DTOs;
using FreshCart.Business.Interfaces;
using FreshCart.Data.Entities;
using FreshCart.Data.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAzureStorageService _storageService;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAzureStorageService storageService,
            ILogger<ProductService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storageService = storageService;
            _logger = logger;
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            try
            {
                var products = await _unitOfWork.Products.Query()
                    .Include(p => p.Category)
                    .Include(p => p.Reviews)
                        .ThenInclude(r => r.User)
                    .OrderBy(p => p.Name)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all products");
                throw new ApplicationException("Error occurred while retrieving products", ex);
            }
        }

        public async Task<ProductDto> GetByIdAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.Products.Query()
                    .Include(p => p.Category)
                    .Include(p => p.Reviews)
                        .ThenInclude(r => r.User)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {id} not found");
                }

                return _mapper.Map<ProductDto>(product);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving product with ID {ProductId}", id);
                throw new ApplicationException($"Error occurred while retrieving product with ID {id}", ex);
            }
        }

        public async Task<ProductDto> CreateAsync(ProductCreateDto createDto)
        {
            try
            {
                await ValidateProductAsync(createDto);

                var product = _mapper.Map<Product>(createDto);

                if (createDto.Image != null)
                {
                    product.ImageUrl = await _storageService.UploadProductImageAsync(createDto.Image);
                }

                await _unitOfWork.Products.AddAsync(product);
                await _unitOfWork.SaveChangesAsync();

                return await GetByIdAsync(product.Id);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating product");
                throw new ApplicationException("Error occurred while creating product", ex);
            }
        }

        public async Task<ProductDto> UpdateAsync(int id, ProductCreateDto updateDto)
        {
            try
            {
                var product = await _unitOfWork.Products.Query()
            .FirstOrDefaultAsync(p => p.Id == id);

                if (product == null)
                    throw new KeyNotFoundException($"Product with ID {id} not found");

                await ValidateProductAsync(updateDto);

                if (updateDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(product.ImageUrl))
                    {
                        await _storageService.DeleteProductImageAsync(product.ImageUrl);
                    }
                    product.ImageUrl = await _storageService.UploadProductImageAsync(updateDto.Image);
                }

                _mapper.Map(updateDto, product);
                await _unitOfWork.Products.UpdateAsync(product);
                await _unitOfWork.SaveChangesAsync();

                return await GetByIdAsync(id);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating product with ID {ProductId}", id);
                throw new ApplicationException($"Error occurred while updating product with ID {id}", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var product = await _unitOfWork.Products.GetByIdAsync(id);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {id} not found");
                }

                // Check if product is in any active carts
                var activeCartItems = await _unitOfWork.CartItems.Query()
                    .AnyAsync(ci => ci.ProductId == id);

                if (activeCartItems)
                {
                    throw new InvalidOperationException("Cannot delete product as it exists in active shopping carts");
                }

                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    await _storageService.DeleteProductImageAsync(product.ImageUrl);
                }

                await _unitOfWork.Products.DeleteAsync(id);
                await _unitOfWork.SaveChangesAsync();
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting product with ID {ProductId}", id);
                throw new ApplicationException($"Error occurred while deleting product with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<ProductDto>> SearchAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    throw new ValidationException("Search term cannot be empty");
                }

                var products = await _unitOfWork.Products.Query()
                    .Include(p => p.Category)
                    .Where(p => p.Name.Contains(searchTerm) ||
                               p.Description.Contains(searchTerm) ||
                               p.Category.Name.Contains(searchTerm))
                    .OrderBy(p => p.Name)
                    .ToListAsync();

                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching products with term {SearchTerm}", searchTerm);
                throw new ApplicationException("Error occurred while searching products", ex);
            }
        }

        private async Task ValidateProductAsync(ProductCreateDto productDto)
        {
            var category = await _unitOfWork.Categories
                .GetByIdAsync(productDto.CategoryId);

            if (category == null)
                throw new InvalidOperationException($"Category with ID {productDto.CategoryId} does not exist");

            if (productDto.DiscountPercentage.HasValue &&
                (productDto.DiscountPercentage < 0 || productDto.DiscountPercentage > 100))
                throw new ValidationException("Discount percentage must be between 0 and 100");
        }
    }
}
