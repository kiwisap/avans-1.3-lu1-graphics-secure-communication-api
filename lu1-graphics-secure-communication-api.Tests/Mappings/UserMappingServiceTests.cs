using lu1_graphics_secure_communication_api.Mappings;
using lu1_graphics_secure_communication_api.Models.Dto;
using lu1_graphics_secure_communication_api.Models.Entities;

namespace lu1_graphics_secure_communication_api.Tests.Mappings;

[TestClass]
public class UserMappingServiceTests
{
    private UserMappingService _mappingService = null!;

    [TestInitialize]
    public void Setup()
    {
        _mappingService = new UserMappingService();
    }

    [TestMethod]
    public void RegisterDtoToUser_WithValidDto_ShouldMapCorrectly()
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

        // Act
        var user = _mappingService.RegisterDtoToUser(registerDto);

        // Assert
        Assert.IsNotNull(user);
        Assert.AreEqual(registerDto.Email, user.Email);
        Assert.AreEqual(registerDto.Email, user.UserName);
        Assert.AreEqual(registerDto.FirstName, user.FirstName);
        Assert.AreEqual(registerDto.LastName, user.LastName);
        Assert.AreEqual(registerDto.Age, user.Age);
    }

    [TestMethod]
    public void RegisterDtoToUser_WithTreatmentFields_ShouldMapAllFields()
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
            TreatmentDetails = "Therapy sessions",
            TreatmentDate = new DateOnly(2024, 6, 1)
        };

        // Act
        var user = _mappingService.RegisterDtoToUser(registerDto);

        // Assert
        Assert.IsNotNull(user);
        Assert.AreEqual(registerDto.DoctorName, user.DoctorName);
        Assert.AreEqual(registerDto.TreatmentDetails, user.TreatmentDetails);
        Assert.AreEqual(registerDto.TreatmentDate, user.TreatmentDate);
    }

    [TestMethod]
    public void RegisterDtoToUser_WithNullOptionalFields_ShouldMapToNull()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "test@example.com",
            Password = "Password123!",
            FirstName = "John",
            LastName = "Doe",
            Age = 30,
            DoctorName = null,
            TreatmentDetails = null,
            TreatmentDate = null
        };

        // Act
        var user = _mappingService.RegisterDtoToUser(registerDto);

        // Assert
        Assert.IsNotNull(user);
        Assert.IsNull(user.DoctorName);
        Assert.IsNull(user.TreatmentDetails);
        Assert.IsNull(user.TreatmentDate);
    }

    [TestMethod]
    public void UserToUserDto_WithValidUser_ShouldMapCorrectly()
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

        // Act
        var userDto = _mappingService.UserToUserDto(user);

        // Assert
        Assert.IsNotNull(userDto);
        Assert.AreEqual(user.Email, userDto.Email);
        Assert.AreEqual(user.FirstName, userDto.FirstName);
        Assert.AreEqual(user.LastName, userDto.LastName);
        Assert.AreEqual(user.Age, userDto.Age);
    }

    [TestMethod]
    public void UserToUserDto_WithAllFields_ShouldMapAllFields()
    {
        // Arrange
        var user = new User
        {
            Id = "patient123",
            Email = "patient@example.com",
            UserName = "patient@example.com",
            FirstName = "Patient",
            LastName = "User",
            Age = 10,
            DoctorName = "Dr. Smith",
            TreatmentDetails = "Therapy sessions",
            TreatmentDate = new DateOnly(2024, 6, 1)
        };

        // Act
        var userDto = _mappingService.UserToUserDto(user);

        // Assert
        Assert.IsNotNull(userDto);
        Assert.AreEqual(user.DoctorName, userDto.DoctorName);
        Assert.AreEqual(user.TreatmentDetails, userDto.TreatmentDetails);
        Assert.AreEqual(user.TreatmentDate, userDto.TreatmentDate);
    }

    [TestMethod]
    public void UserToUserDto_WithNullOptionalFields_ShouldMapToNull()
    {
        // Arrange
        var user = new User
        {
            Id = "user123",
            Email = "test@example.com",
            UserName = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Age = 30,
            DoctorName = null,
            TreatmentDetails = null,
            TreatmentDate = null
        };

        // Act
        var userDto = _mappingService.UserToUserDto(user);

        // Assert
        Assert.IsNotNull(userDto);
        Assert.IsNull(userDto.DoctorName);
        Assert.IsNull(userDto.TreatmentDetails);
        Assert.IsNull(userDto.TreatmentDate);
    }

    [TestMethod]
    public void UserToUserDto_WithCurrentLevel_ShouldMapCurrentLevel()
    {
        // Arrange
        var user = new User
        {
            Id = "user123",
            Email = "test@example.com",
            UserName = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Age = 25,
            CurrentLevel = 5
        };

        // Act
        var userDto = _mappingService.UserToUserDto(user);

        // Assert
        Assert.IsNotNull(userDto);
        Assert.AreEqual(5, userDto.CurrentLevel);
    }

    [TestMethod]
    public void UserToUserDto_WithDefaultCurrentLevel_ShouldMapToOne()
    {
        // Arrange
        var user = new User
        {
            Id = "user123",
            Email = "test@example.com",
            UserName = "test@example.com",
            FirstName = "John",
            LastName = "Doe",
            Age = 25,
            CurrentLevel = 1
        };

        // Act
        var userDto = _mappingService.UserToUserDto(user);

        // Assert
        Assert.IsNotNull(userDto);
        Assert.AreEqual(1, userDto.CurrentLevel);
    }
}
