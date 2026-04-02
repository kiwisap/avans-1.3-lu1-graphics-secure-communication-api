using lu1_graphics_secure_communication_api.Models.Dto;
using lu1_graphics_secure_communication_api.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace lu1_graphics_secure_communication_api.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        var user = await _accountService.RegisterAsync(request);
        return Ok(user);
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var user = await _accountService.GetCurrentUserAsync(User);
        return Ok(user);
    }
}