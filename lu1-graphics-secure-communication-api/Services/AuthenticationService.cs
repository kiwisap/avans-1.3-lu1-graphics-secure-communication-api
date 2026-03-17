using lu1_graphics_secure_communication_api.Services.Interfaces;
using System.Security.Claims;

namespace lu1_graphics_secure_communication_api.Services;

public class AuthenticationService(IHttpContextAccessor httpContextAccessor) : IAuthenticationService
{
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public string? GetCurrentAuthenticatedUserId()
    {
        // Returns the user id of the currently authenticated user, or null if there is no user authenticated.
        return _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!;
    }
}
