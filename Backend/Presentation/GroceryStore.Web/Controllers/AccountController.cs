using GroceryStore.Data.Dto;
using GroceryStore.Data.EntityModels;
using GroceryStore.Services.Repositories.UserRepository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryStore.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AccountController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] User model)
        {
            var userExists = await _userRepository.Find(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });

            var result = await _userRepository.Register(model);

            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User Registration failed. Please check user details and try again." });

            return Ok(new { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User user)
        {
            var userExists = await _userRepository.Find(user.Email);

            if (userExists != null)
            {
                var result = await _userRepository.Login(user);
                if (result.Succeeded)
                {
                    var userInfo = new UserInfoDto
                    {
                        Id = userExists.Id,
                        Name = userExists.UserName,
                        isAdmin = await _userRepository.CheckUserRole(userExists)
                    };
                    return Ok(new { userInfo, Status = "Success", loginSuccessful = true });
                } 
                else
                {
                    return Unauthorized(new { Status = "Unsuccess", Message = "Login Unsuccesful, Invalid Credentials!" });
                }
            }
            else
                return Unauthorized( new { Status = "Unsuccess", Message = "Login Unsuccesful, User Not Found!" });
        }

    }
}
