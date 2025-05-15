﻿using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PWAApi.ApiService.Authentication.DataTransferObjects;
using PWAApi.ApiService.Authentication.Services;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterDTO registerDTO)
    {
        var result = await _authService.RegisterUserAsync(registerDTO);

        return Ok(result);
    }

    [AllowAnonymous]
    [HttpGet("email-available")]
    public async Task<IActionResult> EmailAvailable(string email)
    {
        var result = await _authService.EmailAvailableAsync(email);

        //True if the Email is available. Otherwise it's false
        return Ok(result);
    }

    [AllowAnonymous]
    [HttpPost("exchange-google-token")]
    public async Task<IActionResult> ExchangeGoogleToken([FromBody] ExchangeTokenRequest request)
    {
        if(request.IdToken == null || string.IsNullOrWhiteSpace(request.IdToken))
        {
            return BadRequest("Google credential missing from request");
        }

        var jwt = await _authService.GoogleLoginAsync(request.IdToken);
        return Ok(new { jwt });
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO loginDTO)
    {
        if (loginDTO == null)
        {
            return BadRequest("Invalid user");
        }

        var jwt = await _authService.LoginAsync(loginDTO);
        
        return Ok(new { jwt });
    }

    [Authorize]
    [HttpGet("GetUser")]
    public async Task<IActionResult> GetUser()
    {
        var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userID))
        {
            return Unauthorized();
        }

        try
        {
            var user = await _authService.GetUser(userID);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }
}