namespace corsosharp.Services;
using corsosharp.DTOs;
public interface IAuthService
{
  
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    Task LogoutAsync();
}