using Moq;
using User.Application.Services;
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
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        _repositoryMock.Setup(r => r.AddUserAsync(userCreateDto))
                      .ReturnsAsync(userCreateDto);

        // Act
        var result = await _userService.Add(userCreateDto);

        // Assert
        Assert.Equal(userCreateDto, result);
        _repositoryMock.Verify(r => r.AddUserAsync(userCreateDto), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<UserResponseDTO>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
            new() { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" }
        };

        _repositoryMock.Setup(r => r.GetUserAsync())
                      .ReturnsAsync(users);

        // Act
        var result = await _userService.GetAllAsync();

        // Assert
        Assert.Equal(users, result);
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
            FirstName = "John",
            LastName = "Doe",
            Email = "john@example.com"
        };

        _repositoryMock.Setup(r => r.GetUserByIdAsync(userId))
                      .ReturnsAsync(user);

        // Act
        var result = await _userService.GetAsync(userId);

        // Assert
        Assert.Equal(user, result);
        _repositoryMock.Verify(r => r.GetUserByIdAsync(userId), Times.Once);
    }

    [Fact]
    public async Task Update_ShouldCallRepositoryUpdateUser()
    {
        // Arrange
        var userUpdateDto = new UserUpdateDTO
        {
            Id = 1,
            FirstName = "John Updated",
            LastName = "Doe Updated",
            Email = "johnupdated@example.com"
        };

        _repositoryMock.Setup(r => r.UpdateUserAsync(userUpdateDto))
                      .ReturnsAsync(userUpdateDto);

        // Act
        var result = await _userService.Update(userUpdateDto);

        // Assert
        Assert.Equal(userUpdateDto, result);
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
            Email = email,
            PasswordHash = "hashedpassword"
        };

        _repositoryMock.Setup(r => r.GetUserLoginAsync(email))
                      .ReturnsAsync(userLogin);

        // Act
        var result = await _userService.GetLoginAsync(email);

        // Assert
        Assert.Equal(userLogin, result);
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