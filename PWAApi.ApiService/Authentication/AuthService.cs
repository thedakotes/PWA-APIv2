using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using PWAApi.ApiService.Authentication.Models;
using API.Models;
using AutoMapper;

namespace PWAApi.ApiService.Authentication
{
    public class AuthService
    {
        private IAuthRepository _authRepo;
        private readonly IConfiguration _config;
        private IMapper _mapper;

        public AuthService(IAuthRepository authRepo, 
            IConfiguration config, 
            IMapper mapper)
        {
            _authRepo = authRepo;
            _config = config;
            _mapper = mapper;
        }

        public async Task<UserDTO?> GetUserByProviderID(string googleID)
        {
            var entity = await _authRepo.GetUserByProviderID(googleID);
            
            return entity != null ? _mapper.Map<UserDTO>(entity) : null;
        }

        public async Task<UserDTO> AddUser(GoogleUserDTO user)
        {
            var entity = _mapper.Map<ApplicationUser>(user);
            await _authRepo.AddAsync(entity);
            return _mapper.Map<UserDTO>(entity);
        }

        public async Task<UserDTO?> GetUserByEmail(string email)
        {
            var entity = await _authRepo.GetUserByEmail(email);
            return entity != null ? _mapper.Map<UserDTO>(entity) : null;
        }

        public string GenerateJwtToken(UserDTO user)
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
                throw new ArgumentNullException("Jwt:Key missing");
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
