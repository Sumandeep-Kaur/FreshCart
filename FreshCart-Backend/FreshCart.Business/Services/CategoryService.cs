using AutoMapper;
using FreshCart.Business.DTOs;
using FreshCart.Business.Interfaces;
using FreshCart.Data.Entities;
using FreshCart.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IAzureStorageService _storageService;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IAzureStorageService storageService,
            ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _storageService = storageService;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync()
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetAllAsync();

                return _mapper.Map<IEnumerable<CategoryDto>>(categories);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all categories");
                throw new ApplicationException("Error occurred while retrieving categories", ex);
            }
        }

        public async Task<CategoryDto> GetByIdAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.Categories.Query()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with ID {id} not found");
                }

                return _mapper.Map<CategoryDto>(category);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving category with ID {CategoryId}", id);
                throw new ApplicationException($"Error occurred while retrieving category with ID {id}", ex);
            }
        }

        public async Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(int categoryId)
        {
            try
            {
                var category = await _unitOfWork.Categories.Query()
                    .Include(c => c.Products)
                    .FirstOrDefaultAsync(c => c.Id == categoryId);

                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with ID {categoryId} not found");
                }

                if (!category.Products.Any())
                {
                    throw new KeyNotFoundException($"No products found for category ID {categoryId}");
                }

                return _mapper.Map<IEnumerable<ProductDto>>(category.Products);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving products for category ID {CategoryId}", categoryId);
                throw new ApplicationException($"Error occurred while retrieving products for category ID {categoryId}", ex);
            }
        }

        public async Task<CategoryDto> CreateAsync(CategoryCreateDto createDto)
        {
            try
            {
                var category = _mapper.Map<Category>(createDto);

                if (createDto.Image != null)
                {
                    category.ImageUrl = await _storageService.UploadCategoryImageAsync(createDto.Image);
                }

                await _unitOfWork.Categories.AddAsync(category);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<CategoryDto>(category);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating category");
                throw new ApplicationException("Error occurred while creating category", ex);
            }
        }

        public async Task<CategoryDto> UpdateAsync(int id, CategoryCreateDto updateDto)
        {
            try
            {
                var category = await _unitOfWork.Categories.Query()
                    .FirstOrDefaultAsync(c => c.Id == id);

                if (category == null)
                    throw new KeyNotFoundException($"Category with ID {id} not found");

                if (updateDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(category.ImageUrl))
                    {
                        await _storageService.DeleteCategoryImageAsync(category.ImageUrl);
                    }
                    category.ImageUrl = await _storageService.UploadCategoryImageAsync(updateDto.Image);
                }

                _mapper.Map(updateDto, category);
                await _unitOfWork.Categories.UpdateAsync(category);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<CategoryDto>(category);
            }
            catch (KeyNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating category with ID {CategoryId}", id);
                throw new ApplicationException($"Error occurred while updating category with ID {id}", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var category = await _unitOfWork.Categories.GetByIdAsync(id);
                if (category == null)
                {
                    throw new KeyNotFoundException($"Category with ID {id} not found");
                }

                var hasProducts = await _unitOfWork.Products.Query()
                    .AnyAsync(p => p.CategoryId == id);

                if (hasProducts)
                {
                    throw new InvalidOperationException("Cannot delete category as it contains products");
                }

                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    await _storageService.DeleteCategoryImageAsync(category.ImageUrl);
                }

                await _unitOfWork.Categories.DeleteAsync(id);
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
                _logger.LogError(ex, "Error occurred while deleting category with ID {CategoryId}", id);
                throw new ApplicationException($"Error occurred while deleting category with ID {id}", ex);
            }
        }
    }
}
