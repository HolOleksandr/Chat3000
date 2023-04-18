using AutoMapper;
using Chat.BLL.Exceptions;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Chat.BLL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Chat.BLL.Services.Realizations
{
    public class UserAuthService : IUserAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private User? _user;

        public UserAuthService(
            UserManager<User> userManager, 
            IConfiguration configuration, 
            IMapper mapper) 
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
        }

        public async Task<string> CreateTokenAsync()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationModel userModel)
        {
            var user = _mapper.Map<User>(userModel);
            user.UserName = userModel.Email;
            var defaultRole = "User";
            if (userModel.Password == null)
                throw new ChatException("Password is empty");
            var result = await _userManager.CreateAsync(user, userModel.Password);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, defaultRole);

            return result;
        }

        public async Task<bool> ValidateUserAsync(UserLoginModel userModel)
        {
            _user = await _userManager.FindByNameAsync(userModel.Email);
            if (_user == null)
                return false;
            return await _userManager.CheckPasswordAsync(_user, userModel.Password);
        }

        public async Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            return result;
        }



        private SigningCredentials GetSigningCredentials()
        {
            var key = _configuration["Jwt:Key"];
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user!.Email),
                new Claim(ClaimTypes.NameIdentifier, _user.Id)
            };
            var roles = await _userManager.GetRolesAsync(_user);

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var expiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(
                _configuration["Jwt:ExpirationTimeInMinutes"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials);
            return token;
        }
    }
}
