using GroceryStore.Data.EntityModels;
using GroceryStore.Services.DataAccess;
using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;

namespace GroceryStore.Services.Repositories.UserRepository
{
    public class UserRepository : IUserRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public UserRepository(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public async Task<IdentityResult> Register(User userModel)
        {
            var user = new IdentityUser()
            {
                UserName = userModel.Name,
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber,
            };
            var result = await _userManager.CreateAsync(user, userModel.Password);
            return result;
        }

        public async Task<SignInResult> Login(User model)
        {
            var user = await this.Find(model.Email);
            var result = await _signInManager.PasswordSignInAsync(user.UserName, model.Password, false, false);
            return result;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<IdentityUser> Find(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<bool> CheckUserRole(IdentityUser user)
        {
            return await _userManager.IsInRoleAsync(user, "Admin");
        }
    }
}
