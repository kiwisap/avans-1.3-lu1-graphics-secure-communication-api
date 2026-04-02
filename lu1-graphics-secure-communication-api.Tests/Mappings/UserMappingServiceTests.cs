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
            Age = 25,
            IsChild = false
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
        Assert.AreEqual(registerDto.IsChild, user.IsChild);
    }

    [TestMethod]
    public void RegisterDtoToUser_WithChildFields_ShouldMapAllFields()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Email = "child@example.com",
            Password = "Password123!",
            FirstName = "Child",
            LastName = "User",
            Age = 10,
            IsChild = true,
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
        Assert.IsTrue(user.IsChild);
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
            IsChild = false,
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
            Age = 25,
            IsChild = false
        };

        // Act
        var userDto = _mappingService.UserToUserDto(user);

        // Assert
        Assert.IsNotNull(userDto);
        Assert.AreEqual(user.Id, userDto.Id);
        Assert.AreEqual(user.Email, userDto.Email);
        Assert.AreEqual(user.FirstName, userDto.FirstName);
        Assert.AreEqual(user.LastName, userDto.LastName);
        Assert.AreEqual(user.Age, userDto.Age);
        Assert.AreEqual(user.IsChild, userDto.IsChild);
    }

    [TestMethod]
    public void UserToUserDto_WithAllFields_ShouldMapAllFields()
    {
        // Arrange
        var user = new User
        {
            Id = "child123",
            Email = "child@example.com",
            UserName = "child@example.com",
            FirstName = "Child",
            LastName = "User",
            Age = 10,
            IsChild = true,
            DoctorName = "Dr. Smith",
            TreatmentDetails = "Therapy sessions",
            TreatmentDate = new DateOnly(2024, 6, 1)
        };

        // Act
        var userDto = _mappingService.UserToUserDto(user);

        // Assert
        Assert.IsNotNull(userDto);
        Assert.AreEqual(user.Id, userDto.Id);
        Assert.AreEqual(user.DoctorName, userDto.DoctorName);
        Assert.AreEqual(user.TreatmentDetails, userDto.TreatmentDetails);
        Assert.AreEqual(user.TreatmentDate, userDto.TreatmentDate);
        Assert.IsTrue(userDto.IsChild);
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
            IsChild = false,
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
}
