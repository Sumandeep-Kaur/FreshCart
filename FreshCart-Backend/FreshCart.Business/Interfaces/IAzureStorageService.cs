using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.Interfaces
{
    public interface IAzureStorageService
    {
        Task<string> UploadProductImageAsync(IFormFile file);
        Task DeleteProductImageAsync(string imageUrl);
        Task<string> UploadCategoryImageAsync(IFormFile file);
        Task DeleteCategoryImageAsync(string imageUrl);
    }
}
