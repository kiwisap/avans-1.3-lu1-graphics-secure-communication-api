namespace lu1_graphics_secure_communication_api.Services.Interfaces;

public interface IAuthenticationService
{
    /// <summary>
    /// Returns the user name of the authenticated user
    /// </summary>
    string? GetCurrentAuthenticatedUserId();
}
