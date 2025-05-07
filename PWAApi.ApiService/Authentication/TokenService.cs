using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PWAApi.ApiService.Authentication.Models;
using API.Models;
using AutoMapper;
using PWAApi.ApiService.Middleware;

namespace PWAApi.ApiService.Authentication
{
    public class TokenService
    {
        private readonly IConfiguration _config;
        private IMapper _mapper;

        public TokenService(IConfiguration config, 
            IMapper mapper)
        {
            _config = config;
            _mapper = mapper;
        }

        public string GenerateJwtToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            if (key == null)
            {
                throw new ConfigurationException("Jwt:Key missing");
            }

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
