using PWAApi.ApiService.Authentication.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using Azure.Core;
using System.Net.Http;
using System.Text.Json;
using CsvHelper.Configuration;
using PWAApi.ApiService.Authentication.DataTransferObjects;

namespace PWAApi.ApiService.Authentication.Services
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenService _tokenService;
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public AuthService(IHttpClientFactory httpClientFactory,
            IConfiguration config,
            TokenService tokenService,
            UserManager<ApplicationUser> userManager)
        {
            _config = config;
            _httpClient = httpClientFactory.CreateClient();
            _userManager = userManager;
            _tokenService = tokenService;
        }

        public async Task<string> GoogleLoginAsync(string token)
        {
            var googleURI = _config["Google:URL"] ?? throw new ConfigurationException("Google Oauth2 URL is missing!");
            var googleResponse = await _httpClient.GetAsync($"{googleURI}/tokeninfo?id_token={token}");

            if (!googleResponse.IsSuccessStatusCode)
                throw new ApplicationException("Failed to log in with Google.");

            var payload = JsonSerializer.Deserialize<GoogleUserDTO>(await googleResponse.Content.ReadAsStringAsync());

            if (payload == null || string.IsNullOrEmpty(payload.email))
                throw new ApplicationException("Failed to process response from Google.");

            // Create or find user
            var user = await _userManager.FindByEmailAsync(payload.email);
            if (user == null)
            {
                user = new ApplicationUser
                {
                    UserName = payload.email,
                    Email = payload.email,
                    Name = payload.name,
                    ProviderId = payload.sub,
                    Provider = "Google",
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(user);
                if (!result.Succeeded)
                {
                    throw new ApplicationException(string.Join(", ", result.Errors.Select(p => p.Description)));
                }
            }
            else
            {
                //We found a user. Make sure it was a user that was previously logged in with Google auth, and not an Adjutum user with a gmail email
                if (user.Provider != "Google" || string.IsNullOrWhiteSpace(user.ProviderId))
                {
                    //Not sure what we want to say here without giving away too much information?
                    throw new ApplicationException("A user with this email already exists in our system.");
                }
            }

            return await _tokenService.GenerateJwtToken(user);
        }

        public async Task<bool> RegisterUserAsync(RegisterDTO registerDTO)
        {
            if (registerDTO.Password != registerDTO.ConfirmPassword)
                throw new ValidationException("Passwords do not match");

            if( !await EmailAvailableAsync(registerDTO.Email) )
                throw new ValidationException("Email is already taken");

            ApplicationUser user = new ApplicationUser()
            {
                Email = registerDTO.Email,
                UserName = registerDTO.Email,
                EmailConfirmed = false,
                Name = registerDTO.Name,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password);

            if (!result.Succeeded)
                throw new ApplicationException(string.Join(", ", result.Errors.Select(p => p.Description)));

            //TODO: We'd need to consider sending an email to the user to make them confirm their account creation before we make it active.

            return true;
        }

        public async Task<bool> EmailAvailableAsync(string email)
        {
            if(string.IsNullOrWhiteSpace(email))
                throw new ValidationException("Email is empty");

            var user = await _userManager.FindByEmailAsync(email);

            //Email already linked to a user! This will include any users who logged in using Google auth.
            if (user != null)
                return false;

            return true;
        }

        public async Task<string> LoginAsync(LoginDTO loginDTO)
        {
            if (string.IsNullOrWhiteSpace(loginDTO.Email) || string.IsNullOrWhiteSpace(loginDTO.Password))
                throw new ValidationException("Email or password is missing");

            var appUser = await _userManager.FindByEmailAsync(loginDTO.Email);

            if (appUser == null)
                throw new ValidationException("Login information does not match our records.");

            //If ProviderId is not null, this user was created via Google login authentication. These users cannot log in via our normal
            //login screen, they must use the Google auth route.
            if (appUser.ProviderId != null)
                throw new ValidationException("Login information does not match our records.");

            //KEF: to revisit. We won't lock them out for now if their email isn't confirmed, but we should
            //put a mechanism in place to force verification
            //if (!appUser.EmailConfirmed)
            //    throw new ValidationException("Email has not yet been confirmed. Please check your email before attempting to log in.");

            var isValid = await _userManager.CheckPasswordAsync(appUser, loginDTO.Password);

            if (!isValid)
                //Using the same generic error as above. Don't want to give away too much information on what is/isn't correct about their login
                throw new ValidationException("Login information does not match our records.");

            
            return await _tokenService.GenerateJwtToken(appUser);
        }

    }
}
