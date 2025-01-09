using FreshCart.Business.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.Interfaces
{
    public interface IUserService
    {
        Task<UserDto> GetByIdAsync(int id);
        Task<UserDto> GetByEmailAsync(string email);
        Task<UserDto> UpdateAsync(int id, UserDto userDto);
        Task DeleteAsync(int id);
    }
}
