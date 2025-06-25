using Moq;
using User.Application.Services;
using User.Domain.Models.Entities;
using User.Domain.Models.Response;
using User.Domain.Repositories;

namespace User.Application.Tests.Services;

public class UserServiceTests
{
    private readonly Mock<IRepository> _repositoryMock;
    private readonly UserService _userService;

    public UserServiceTests()
    {
        _repositoryMock = new Mock<IRepository>();
        _userService = new UserService(_repositoryMock.Object);
    }

    [Fact]
    public async Task Add_ShouldCallRepositoryAddUser()
    {
        // Arrange
        var userCreateDto = new UserCreateDTO
        {
            Username = "johndoe",
            Email = "john@example.com",
            PasswordHash = "hashedpassword"
        };

        _repositoryMock.Setup(r => r.AddUserAsync(userCreateDto))
                      .ReturnsAsync(userCreateDto);

        // Act
        var result = await _userService.Add(userCreateDto);

        // Assert
        Assert.Equal(userCreateDto.Username, result.Username);
        Assert.Equal(userCreateDto.Email, result.Email);
        Assert.Equal(userCreateDto.PasswordHash, result.PasswordHash);
        _repositoryMock.Verify(r => r.AddUserAsync(userCreateDto), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<UserResponseDTO>
        {
            new() { Id = 1, Username = "johndoe", Email = "john@example.com", CreatedAt = DateTime.UtcNow, Roles = new List<Role>() },
            new() { Id = 2, Username = "janesmith", Email = "jane@example.com", CreatedAt = DateTime.UtcNow, Roles = new List<Role>() }
        };

        _repositoryMock.Setup(r => r.GetUserAsync())
                      .ReturnsAsync(users);

        // Act
        var result = await _userService.GetAllAsync();

        // Assert
        Assert.Equal(users.Count, result.Count);
        Assert.Equal(users[0].Id, result[0].Id);
        Assert.Equal(users[1].Id, result[1].Id);
        _repositoryMock.Verify(r => r.GetUserAsync(), Times.Once);
    }

    [Fact]
    public async Task GetAsync_WithValidId_ShouldReturnUser()
    {
        // Arrange
        var userId = 1;
        var user = new UserResponseDTO
        {
            Id = userId,
            Username = "johndoe",
            Email = "john@example.com",
            CreatedAt = DateTime.UtcNow,
            Roles = new List<Role>()
        };

        _repositoryMock.Setup(r => r.GetUserByIdAsync(userId))
                      .ReturnsAsync(user);

        // Act
        var result = await _userService.GetAsync(userId);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(user.Id, result.Id);
        Assert.Equal(user.Username, result.Username);
        Assert.Equal(user.Email, result.Email);
        _repositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldCallRepositoryUpdateUser()
    {
        // Arrange
        var userUpdateDto = new UserUpdateDTO
        {
            Id = 1,
            Username = "johnupdated",
            Email = "johnupdated@example.com",
            Roles = new List<Role>()
        };

        _repositoryMock.Setup(r => r.UpdateUserAsync(userUpdateDto))
                      .ReturnsAsync(userUpdateDto);

        // Act
        var result = await _userService.Update(userUpdateDto);

        // Assert
        Assert.Equal(userUpdateDto.Id, result.Id);
        Assert.Equal(userUpdateDto.Username, result.Username);
        Assert.Equal(userUpdateDto.Email, result.Email);
        _repositoryMock.Verify(r => r.UpdateUserAsync(userUpdateDto), Times.Once);
    }

    [Fact]
    public async Task Delete_ShouldCallRepositoryDeleteUser()
    {
        // Arrange
        var userId = 1;
        _repositoryMock.Setup(r => r.DeleteUserAsync(userId))
                      .Returns(Task.CompletedTask);

        // Act
        await _userService.Delete(userId);

        // Assert
        _repositoryMock.Verify(r => r.DeleteUserAsync(userId), Times.Once);
    }

    [Fact]
    public async Task GetLoginAsync_ShouldReturnUserLoginInfo()
    {
        // Arrange
        var email = "test@example.com";
        var userLogin = new UserLoginDTO
        {
            Id = 1,
            Email = email,
            PasswordHash = "hashedpassword",
            Roles = new List<Role>()
        };

        _repositoryMock.Setup(r => r.GetUserLoginAsync(email))
                      .ReturnsAsync(userLogin);

        // Act
        var result = await _userService.GetLoginAsync(email);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userLogin.Id, result.Id);
        Assert.Equal(userLogin.Email, result.Email);
        Assert.Equal(userLogin.PasswordHash, result.PasswordHash);
        _repositoryMock.Verify(r => r.GetUserLoginAsync(email), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999)]
    public async Task GetAsync_WithInvalidId_ShouldStillCallRepository(int invalidId)
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetUserByIdAsync(invalidId))
                      .ReturnsAsync((UserResponseDTO)null);

        // Act
        var result = await _userService.GetAsync(invalidId);

        // Assert
        Assert.Null(result);
        _repositoryMock.Verify(r => r.GetUserByIdAsync(invalidId), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task GetLoginAsync_WithInvalidEmail_ShouldStillCallRepository(string invalidEmail)
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetUserLoginAsync(invalidEmail))
                      .ReturnsAsync((UserLoginDTO)null);

        // Act
        var result = await _userService.GetLoginAsync(invalidEmail);

        // Assert
        Assert.Null(result);
        _repositoryMock.Verify(r => r.GetUserLoginAsync(invalidEmail), Times.Once);
    }
} 