using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using corsosharp.DTOs;
using corsosharp.Services;
using System.Security.Claims;

namespace corsosharp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    //preferenze
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    // POST /api/auth/login
    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email) || string.IsNullOrWhiteSpace(dto.Password))
            return BadRequest(new { Message = "Email e password sono obbligatori" });

        var result = await _authService.LoginAsync(dto);

        if (result == null)
            return Unauthorized(new { Message = "Email o password non validi" });

        return Ok(result);
    }

    [HttpPost("logout")]
    public async Task<ActionResult> Logout()
    {
        await _authService.LogoutAsync();
        return Ok("Logout effettuato con successo");
    }

    // DEBUG: qui ho fatto i cleims  informazioni salvate nel token
    [Authorize]
    [HttpGet("me")]
    public ActionResult GetCurrentUser()
    {
        var claims = User.Claims.Select(c => new { c.Type, c.Value }).ToList();
        var role = User.FindFirst(ClaimTypes.Role)?.Value ?? "NESSUN RUOLO";
        var isAdmin = User.IsInRole("Admin");
        var isUser = User.IsInRole("User");

        return Ok(new
        {
            Role = role,
            IsAdmin = isAdmin,
            IsUser = isUser,
            AllClaims = claims
        });
    }
}

