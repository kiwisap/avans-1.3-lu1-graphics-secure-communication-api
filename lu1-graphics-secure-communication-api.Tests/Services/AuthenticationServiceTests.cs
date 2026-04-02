using lu1_graphics_secure_communication_api.Services;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace lu1_graphics_secure_communication_api.Tests.Services;

[TestClass]
public class AuthenticationServiceTests
{
    private Mock<IHttpContextAccessor> _httpContextAccessorMock = null!;
    private AuthenticationService _authenticationService = null!;

    [TestInitialize]
    public void Setup()
    {
        _httpContextAccessorMock = new Mock<IHttpContextAccessor>();
    }

    [TestMethod]
    public void GetCurrentAuthenticatedUserId_WithAuthenticatedUser_ShouldReturnUserId()
    {
        // Arrange
        var userId = "user123";
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, "test@example.com")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthentication");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
        _authenticationService = new AuthenticationService(_httpContextAccessorMock.Object);

        // Act
        var result = _authenticationService.GetCurrentAuthenticatedUserId();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(userId, result);
    }

    [TestMethod]
    public void GetCurrentAuthenticatedUserId_WithNoAuthenticatedUser_ShouldReturnNull()
    {
        // Arrange
        var claims = new List<Claim>();
        var identity = new ClaimsIdentity(claims);
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
        _authenticationService = new AuthenticationService(_httpContextAccessorMock.Object);

        // Act
        var result = _authenticationService.GetCurrentAuthenticatedUserId();

        // Assert
        Assert.IsNull(result);
    }

    [TestMethod]
    public void GetCurrentAuthenticatedUserId_WithDifferentClaimTypes_ShouldReturnNull()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Name, "John Doe")
        };
        var identity = new ClaimsIdentity(claims, "TestAuthentication");
        var claimsPrincipal = new ClaimsPrincipal(identity);

        var httpContext = new DefaultHttpContext
        {
            User = claimsPrincipal
        };

        _httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);
        _authenticationService = new AuthenticationService(_httpContextAccessorMock.Object);

        // Act
        var result = _authenticationService.GetCurrentAuthenticatedUserId();

        // Assert
        Assert.IsNull(result);
    }
}
