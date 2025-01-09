using FreshCart.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.Interfaces
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllAsync();
        Task<CategoryDto> GetByIdAsync(int id);
        Task<IEnumerable<ProductDto>> GetProductsByCategoryIdAsync(int categoryId);
        Task<CategoryDto> CreateAsync(CategoryCreateDto createDto);
        Task<CategoryDto> UpdateAsync(int id, CategoryCreateDto updateDto);
        Task DeleteAsync(int id);
    }
}
