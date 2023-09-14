using GroceryStore.Data.EntityModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryStore.Services.Repositories.UserRepository
{
    public interface IUserRepository
    {
        Task<IdentityResult> Register(User userModel);
        Task<SignInResult> Login(User loginModel);
        Task Logout();
        Task<IdentityUser> Find(string email);
        Task<bool> CheckUserRole(IdentityUser user);
    }
}
