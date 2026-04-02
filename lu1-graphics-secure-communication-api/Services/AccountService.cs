using lu1_graphics_secure_communication_api.Exceptions;
using lu1_graphics_secure_communication_api.Mappings.Interfaces;
using lu1_graphics_secure_communication_api.Models.Dto;
using lu1_graphics_secure_communication_api.Models.Entities;
using lu1_graphics_secure_communication_api.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace lu1_graphics_secure_communication_api.Services;

public class AccountService : IAccountService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;

    private readonly IUserMappingService _userMappingService;

    public AccountService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        IUserMappingService userMappingService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _userMappingService = userMappingService;
    }

    public async Task<UserDto> RegisterAsync(RegisterDto request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new BadRequestException("Gebruiker met dit e-mailadres bestaat al.");
        }

        var user = _userMappingService.RegisterDtoToUser(request);

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
        {
            throw new ValidationException(result.Errors
                .GroupBy(e => e.Code)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.Description).ToArray()
                ));
        }

        return _userMappingService.UserToUserDto(user);
    }

    public async Task<UserDto> LoginAsync(LoginDto request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new BadRequestException("Ongeldige inloggegevens.");
        }

        var result = await _signInManager.PasswordSignInAsync(
            user.UserName!,
            request.Password,
            isPersistent: true,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new BadRequestException("Ongeldige inloggegevens.");
        }

        return _userMappingService.UserToUserDto(user);
    }

    public async Task<UserDto> GetCurrentUserAsync(ClaimsPrincipal principal)
    {
        var user = await _userManager.GetUserAsync(principal);
        if (user == null)
        {
            throw new NotFoundException("Gebruiker niet gevonden.");
        }

        return _userMappingService.UserToUserDto(user);
    }
}