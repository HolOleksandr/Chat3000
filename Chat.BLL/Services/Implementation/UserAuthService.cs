using AutoMapper;
using Chat.BLL.Exceptions;
using Chat.BLL.Services.Interfaces;
using Chat.DAL.Entities;
using Chat.BLL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Chat.BLL.Models.Requests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Chat.BLL.Services.Implementation
{
    public class UserAuthService : IUserAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _environment;
        private readonly IMapper _mapper;
        private User? _user;

        public UserAuthService(
            UserManager<User> userManager, 
            IConfiguration configuration, 
            IMapper mapper,
            IWebHostEnvironment environment) 
        {
            _userManager = userManager;
            _configuration = configuration;
            _mapper = mapper;
            _environment = environment;
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
            if (user == null)
                throw new ChatException("User is not exist");
            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            return result;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var key = GetJwtKey();
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
                issuer: GetJwtIssuer(),
                audience: GetJwtAudience(),
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials);
            return token;
        }

        private string GetJwtKey()
        {
            if (_environment.IsDevelopment())
            {
                return _configuration["Jwt:Key"];
            }
            else
            {
                return _configuration["ProdJwt:Key"];
            }
        }

        private string GetJwtIssuer()
        {
            if (_environment.IsDevelopment())
            {
                return _configuration["Jwt:Issuer"];
            }
            else
            {
                return _configuration["ProdJwt:Issuer"];
            }
        }

        private string GetJwtAudience()
        {
            if (_environment.IsDevelopment())
            {
                return _configuration["Jwt:Audience"];
            }
            else
            {
                return _configuration["ProdJwt:Audience"];
            }
        }
    }
}
