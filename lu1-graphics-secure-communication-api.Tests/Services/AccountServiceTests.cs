using lu1_graphics_secure_communication_api.Exceptions;
using lu1_graphics_secure_communication_api.Mappings.Interfaces;
using lu1_graphics_secure_communication_api.Models.Dto;
using lu1_graphics_secure_communication_api.Models.Entities;
using lu1_graphics_secure_communication_api.Services;
using Microsoft.AspNetCore.Identity;
using Moq;
using System.Security.Claims;

namespace lu1_graphics_secure_communication_api.Tests.Services;

[TestClass]
public class AccountServiceTests
{
    private Mock<UserManager<User>> _userManagerMock = null!;
    private Mock<SignInManager<User>> _signInManagerMock = null!;
    private Mock<IUserMappingService> _userMappingServiceMock = null!;
    private AccountService _accountService = null!;

    [TestInitialize]
    public void Setup()
    {
        var userStoreMock = new Mock<IUserStore<User>>();
        _userManagerMock = new Mock<UserManager<User>>(
            userStoreMock.Object, null, null, null, null, null, null, null, null);

        var contextAccessorMock = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
        var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<User>>();
        _signInManagerMock = new Mock<SignInManager<User>>(
            _userManagerMock.Object,
            contextAccessorMock.Object,
            claimsFactoryMock.Object,
            null, null, null, null);

        _userMappingServiceMock = new Mock<IUserMappingService>();

        _accountService = new AccountService(
            _userManagerMock.Object,
            _signInManagerMock.Object,
            _userMappingServiceMock.Object);
    }

    [TestMethod]
    public async Task RegisterAsync_WithNewUser_ShouldRegisterSuccessfully()
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

        var user = new User
        {
            Email = registerDto.Email,
            UserName = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Age = registerDto.Age
        };

        var userDto = new UserDto
        {
            Id = "user123",
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Age = user.Age
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync((User?)null);
        _userMappingServiceMock.Setup(x => x.RegisterDtoToUser(registerDto))
            .Returns(user);
        _userManagerMock.Setup(x => x.CreateAsync(user, registerDto.Password))
            .ReturnsAsync(IdentityResult.Success);
        _userMappingServiceMock.Setup(x => x.UserToUserDto(user))
            .Returns(userDto);

        // Act
        var result = await _accountService.RegisterAsync(registerDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(userDto.Email, result.Email);
        Assert.AreEqual(userDto.FirstName, result.FirstName);
        Assert.AreEqual(userDto.LastName, result.LastName);
        _userManagerMock.Verify(x => x.CreateAsync(user, registerDto.Password), Times.Once);
    }

    [TestMethod]
    public async Task RegisterAsync_WithExistingEmail_ShouldThrowBadRequestException()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "existing@example.com",
            Password = "Password123!",
            FirstName = "Jane",
            LastName = "Doe",
            Age = 30
        };

        var existingUser = new User
        {
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync(existingUser);

        // Act & Assert
        BadRequestException? exception = null;
        try
        {
            await _accountService.RegisterAsync(registerDto);
            Assert.Fail("Expected BadRequestException was not thrown");
        }
        catch (BadRequestException ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);
        Assert.AreEqual("Gebruiker met dit e-mailadres bestaat al.", exception.Message);
        _userManagerMock.Verify(x => x.CreateAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Never);
    }

    [TestMethod]
    public async Task RegisterAsync_WithInvalidPassword_ShouldThrowValidationException()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "weak",
            FirstName = "John",
            LastName = "Doe",
            Age = 25
        };

        var user = new User
        {
            Email = registerDto.Email,
            UserName = registerDto.Email
        };

        var errors = new[]
        {
            new IdentityError { Code = "PasswordTooShort", Description = "Password is too short" },
            new IdentityError { Code = "PasswordRequiresDigit", Description = "Password requires a digit" }
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync((User?)null);
        _userMappingServiceMock.Setup(x => x.RegisterDtoToUser(registerDto))
            .Returns(user);
        _userManagerMock.Setup(x => x.CreateAsync(user, registerDto.Password))
            .ReturnsAsync(IdentityResult.Failed(errors));

        // Act & Assert
        ValidationException? exception = null;
        try
        {
            await _accountService.RegisterAsync(registerDto);
            Assert.Fail("Expected ValidationException was not thrown");
        }
        catch (ValidationException ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);
        Assert.IsNotNull(exception.Errors);
        Assert.IsTrue(exception.Errors.ContainsKey("PasswordTooShort"));
        Assert.IsTrue(exception.Errors.ContainsKey("PasswordRequiresDigit"));
    }

    [TestMethod]
    public async Task RegisterAsync_WithTreatmentDetails_ShouldRegisterWithAllFields()
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

        var user = new User
        {
            Email = registerDto.Email,
            UserName = registerDto.Email,
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Age = registerDto.Age,
            DoctorName = registerDto.DoctorName,
            TreatmentDetails = registerDto.TreatmentDetails,
            TreatmentDate = registerDto.TreatmentDate
        };

        var userDto = new UserDto
        {
            Id = "patient123",
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Age = user.Age,
            DoctorName = user.DoctorName,
            TreatmentDetails = user.TreatmentDetails,
            TreatmentDate = user.TreatmentDate
        };

        _userManagerMock.Setup(x => x.FindByEmailAsync(registerDto.Email))
            .ReturnsAsync((User?)null);
        _userMappingServiceMock.Setup(x => x.RegisterDtoToUser(registerDto))
            .Returns(user);
        _userManagerMock.Setup(x => x.CreateAsync(user, registerDto.Password))
            .ReturnsAsync(IdentityResult.Success);
        _userMappingServiceMock.Setup(x => x.UserToUserDto(user))
            .Returns(userDto);

        // Act
        var result = await _accountService.RegisterAsync(registerDto);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Dr. Smith", result.DoctorName);
        Assert.AreEqual("Therapy", result.TreatmentDetails);
    }

    [TestMethod]
    public async Task GetCurrentUserAsync_WithValidUser_ShouldReturnUserDto()
    {
        // Arrange
        var user = new User
        {
            Id = "user123",
            Email = "test@example.com",
            UserName = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Age = 25
        };

        var userDto = new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Age = user.Age
        };

        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Email, user.Email)
        }));

        _userManagerMock.Setup(x => x.GetUserAsync(claims))
            .ReturnsAsync(user);
        _userMappingServiceMock.Setup(x => x.UserToUserDto(user))
            .Returns(userDto);

        // Act
        var result = await _accountService.GetCurrentUserAsync(claims);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(userDto.Id, result.Id);
        Assert.AreEqual(userDto.Email, result.Email);
        Assert.AreEqual(userDto.FirstName, result.FirstName);
    }

    [TestMethod]
    public async Task GetCurrentUserAsync_WithInvalidUser_ShouldThrowNotFoundException()
    {
        // Arrange
        var claims = new ClaimsPrincipal(new ClaimsIdentity(new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "invalid-user-id")
        }));

        _userManagerMock.Setup(x => x.GetUserAsync(claims))
            .ReturnsAsync((User?)null);

        // Act & Assert
        NotFoundException? exception = null;
        try
        {
            await _accountService.GetCurrentUserAsync(claims);
            Assert.Fail("Expected NotFoundException was not thrown");
        }
        catch (NotFoundException ex)
        {
            exception = ex;
        }

        Assert.IsNotNull(exception);
        Assert.AreEqual("Gebruiker niet gevonden.", exception.Message);
    }
}
