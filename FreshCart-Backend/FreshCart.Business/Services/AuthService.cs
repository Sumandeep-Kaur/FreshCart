using AutoMapper;
using FreshCart.Business.DTOs;
using FreshCart.Business.Interfaces;
using FreshCart.Data.Entities;
using FreshCart.Data.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace FreshCart.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<AuthResponse> LoginAsync(LoginRequest request)
        {
            var user = await _unitOfWork.Users.Query()
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null || !VerifyPasswordHash(request.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                Token = token,
                Name = user.Name,
                Role = user.Role
            };
        }

        public async Task<AuthResponse> RegisterAsync(RegisterRequest request)
        {
            if (await _unitOfWork.Users.Query().AnyAsync(u => u.Email == request.Email))
            {
                throw new InvalidOperationException("Email already exists");
            }

            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                PasswordHash = HashPassword(request.Password),
                Role = "User"
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.SaveChangesAsync();

            var token = GenerateJwtToken(user);

            return new AuthResponse
            {
                Token = token,
                Name = user.Name,
                Role = user.Role
            };
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPasswordHash(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }

        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("name", user.Name),
                new Claim("email", user.Email),
                new Claim("role", user.Role),
                new Claim("userId", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
