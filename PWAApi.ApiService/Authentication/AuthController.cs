using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PWAApi.ApiService.Authentication;
using PWAApi.ApiService.Authentication.Models;
using System.Text.Json;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;
    private AuthService _authService;

    public AuthController(IConfiguration configuration,
                          AuthService authService)
    {
        _config = configuration;
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("exchange-google-token")]
    public async Task<IActionResult> ExchangeGoogleToken([FromBody] ExchangeTokenRequest request)
    {
        using var httpClient = new HttpClient();
        var googleURI = _config["Google:URL"] ?? throw new Exception("Oauth2 URL is missing!");
        var googleResponse = await httpClient.GetAsync($"{googleURI}/tokeninfo?id_token={request.IdToken}");

        if (!googleResponse.IsSuccessStatusCode)
            return BadRequest("Invalid Google ID Token.");

        var payload = JsonSerializer.Deserialize<GoogleUserDTO>(await googleResponse.Content.ReadAsStringAsync());

        if (payload == null || string.IsNullOrEmpty(payload.email))
            return BadRequest("Failed to validate Google token.");

        // Create or find user
        var user = await _authService.GetUserByEmail(payload.email);
        if (user == null)
        {
            user = await _authService.AddUser(payload);
        }

        var jwt = _authService.GenerateJwtToken(user);
        return Ok(new { jwt });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
    {
        if (userDTO == null)
        {
            return BadRequest("Invalid user");
        }

        if (string.IsNullOrEmpty(userDTO.Email))
        {
            return BadRequest("Login credentials invalid");
        }

        var user = await _authService.GetUserByEmail(userDTO.Email);
        if (user == null)
        {
            return Unauthorized("User not found");
        }

        var jwt = _authService.GenerateJwtToken(user);
        return Ok(new { jwt });
    }
}