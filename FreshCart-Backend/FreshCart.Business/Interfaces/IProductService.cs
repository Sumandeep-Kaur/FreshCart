using FreshCart.Business.DTOs;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetAllAsync();
        Task<ProductDto> GetByIdAsync(int id);
        Task<ProductDto> CreateAsync(ProductCreateDto createDto);
        Task<ProductDto> UpdateAsync(int id, ProductCreateDto updateDto);
        Task DeleteAsync(int id);
        Task<IEnumerable<ProductDto>> SearchAsync(string searchTerm);
    }
}
