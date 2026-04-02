using lu1_graphics_secure_communication_api.Controllers;
using lu1_graphics_secure_communication_api.Models.Dto;
using lu1_graphics_secure_communication_api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace lu1_graphics_secure_communication_api.Tests.Controllers;

[TestClass]
public class AccountControllerTests
{
    private Mock<IAccountService> _accountServiceMock = null!;
    private AccountController _controller = null!;

    [TestInitialize]
    public void Setup()
    {
        _accountServiceMock = new Mock<IAccountService>();
        _controller = new AccountController(_accountServiceMock.Object);
    }

    [TestMethod]
    public async Task Register_WithValidRequest_ShouldReturnOkWithUserDto()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe",
            Age = 25
        };

        var userDto = new UserDto
        {
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Age = registerDto.Age
        };

        _accountServiceMock.Setup(x => x.RegisterAsync(registerDto))
            .ReturnsAsync(userDto);

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        Assert.IsNotNull(okResult.Value);
        Assert.IsInstanceOfType(okResult.Value, typeof(UserDto));
        var returnedUser = (UserDto)okResult.Value;
        Assert.AreEqual(userDto.Email, returnedUser.Email);
        _accountServiceMock.Verify(x => x.RegisterAsync(registerDto), Times.Once);
    }

    [TestMethod]
    public async Task Register_WithTreatmentDetails_ShouldReturnOkWithUserDto()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "patient@example.com",
            Password = "Password123!",
            FirstName = "Patient",
            LastName = "User",
            Age = 10,
            DoctorName = "Dr. Smith",
            TreatmentDetails = "Therapy",
            TreatmentDate = new DateOnly(2024, 6, 1)
        };

        var userDto = new UserDto
        {
            Email = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Age = registerDto.Age,
            DoctorName = registerDto.DoctorName,
            TreatmentDetails = registerDto.TreatmentDetails,
            TreatmentDate = registerDto.TreatmentDate
        };

        _accountServiceMock.Setup(x => x.RegisterAsync(registerDto))
            .ReturnsAsync(userDto);

        // Act
        var result = await _controller.Register(registerDto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        var returnedUser = (UserDto)okResult.Value!;
        Assert.AreEqual("Dr. Smith", returnedUser.DoctorName);
    }

    [TestMethod]
    public async Task Me_WithAuthenticatedUser_ShouldReturnOkWithUserDto()
    {
        // Arrange
        var userId = "user123";
        var userDto = new UserDto
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Age = 25
        };

        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Email, userDto.Email)
        }, "TestAuthentication"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = claims
            }
        };

        _accountServiceMock.Setup(x => x.GetCurrentUserAsync(It.IsAny<ClaimsPrincipal>()))
            .ReturnsAsync(userDto);

        // Act
        var result = await _controller.Me();

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        Assert.IsNotNull(okResult.Value);
        Assert.IsInstanceOfType(okResult.Value, typeof(UserDto));
        var returnedUser = (UserDto)okResult.Value;
        Assert.AreEqual(userDto.Email, returnedUser.Email);
        Assert.AreEqual(userDto.Email, returnedUser.Email);
        _accountServiceMock.Verify(x => x.GetCurrentUserAsync(It.IsAny<ClaimsPrincipal>()), Times.Once);
    }

    [TestMethod]
    public async Task Me_CallsAccountServiceWithCorrectPrincipal()
    {
        // Arrange
        var userId = "user123";
        var userDto = new UserDto
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Age = 25
        };

        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }, "TestAuthentication"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = claims
            }
        };

        ClaimsPrincipal? capturedPrincipal = null;
        _accountServiceMock.Setup(x => x.GetCurrentUserAsync(It.IsAny<ClaimsPrincipal>()))
            .Callback<ClaimsPrincipal>(p => capturedPrincipal = p)
            .ReturnsAsync(userDto);

        // Act
        await _controller.Me();

        // Assert
        Assert.IsNotNull(capturedPrincipal);
        Assert.AreEqual(claims, capturedPrincipal);
    }

    [TestMethod]
    public async Task UpdateCurrentLevel_WithValidRequest_ShouldReturnOkWithUpdatedUser()
    {
        // Arrange
        var userId = "user123";
        var newLevel = 5;
        var valueDto = new ValueDto<int> { Value = newLevel };

        var updatedUserDto = new UserDto
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Age = 25,
            CurrentLevel = newLevel
        };

        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }, "TestAuthentication"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = claims
            }
        };

        _accountServiceMock.Setup(x => x.UpdateCurrentLevel(It.IsAny<ClaimsPrincipal>(), newLevel))
            .ReturnsAsync(updatedUserDto);

        // Act
        var result = await _controller.UpdateCurrentLevel(valueDto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        Assert.IsNotNull(okResult.Value);
        Assert.IsInstanceOfType(okResult.Value, typeof(UserDto));
        var returnedUser = (UserDto)okResult.Value;
        Assert.AreEqual(newLevel, returnedUser.CurrentLevel);
        _accountServiceMock.Verify(x => x.UpdateCurrentLevel(It.IsAny<ClaimsPrincipal>(), newLevel), Times.Once);
    }

    [TestMethod]
    public async Task UpdateCurrentLevel_CallsServiceWithCorrectPrincipalAndLevel()
    {
        // Arrange
        var userId = "user123";
        var newLevel = 10;
        var valueDto = new ValueDto<int> { Value = newLevel };

        var updatedUserDto = new UserDto
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Age = 25,
            CurrentLevel = newLevel
        };

        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }, "TestAuthentication"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = claims
            }
        };

        ClaimsPrincipal? capturedPrincipal = null;
        int capturedLevel = 0;
        _accountServiceMock.Setup(x => x.UpdateCurrentLevel(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>()))
            .Callback<ClaimsPrincipal, int>((p, l) =>
            {
                capturedPrincipal = p;
                capturedLevel = l;
            })
            .ReturnsAsync(updatedUserDto);

        // Act
        await _controller.UpdateCurrentLevel(valueDto);

        // Assert
        Assert.IsNotNull(capturedPrincipal);
        Assert.AreEqual(claims, capturedPrincipal);
        Assert.AreEqual(newLevel, capturedLevel);
    }

    [TestMethod]
    public async Task UpdateCurrentLevel_WithLevelOne_ShouldUpdateToOne()
    {
        // Arrange
        var valueDto = new ValueDto<int> { Value = 1 };

        var updatedUserDto = new UserDto
        {
            Email = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Age = 25,
            CurrentLevel = 1
        };

        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "user123")
        }, "TestAuthentication"));

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new Microsoft.AspNetCore.Http.DefaultHttpContext
            {
                User = claims
            }
        };

        _accountServiceMock.Setup(x => x.UpdateCurrentLevel(It.IsAny<ClaimsPrincipal>(), 1))
            .ReturnsAsync(updatedUserDto);

        // Act
        var result = await _controller.UpdateCurrentLevel(valueDto);

        // Assert
        Assert.IsInstanceOfType(result, typeof(OkObjectResult));
        var okResult = (OkObjectResult)result;
        var returnedUser = (UserDto)okResult.Value!;
        Assert.AreEqual(1, returnedUser.CurrentLevel);
    }
}
