using FreshCart.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.Interfaces
{
    public interface ICartService
    {
        Task<CartDto> GetUserCartAsync(int userId);
        Task<CartDto> AddToCartAsync(int userId, int productId, int quantity);
        Task<CartDto> UpdateCartItemAsync(int userId, int cartItemId, int quantity);
        Task RemoveFromCartAsync(int userId, int cartItemId);
        Task ClearCartAsync(int userId);
    }
}
