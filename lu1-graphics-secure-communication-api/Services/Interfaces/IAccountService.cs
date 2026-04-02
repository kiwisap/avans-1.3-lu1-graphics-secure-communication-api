using lu1_graphics_secure_communication_api.Models.Dto;
using System.Security.Claims;

namespace lu1_graphics_secure_communication_api.Services.Interfaces;

public interface IAccountService
{
    Task<UserDto> RegisterAsync(RegisterDto request);

    Task<UserDto> GetCurrentUserAsync(ClaimsPrincipal principal);
}