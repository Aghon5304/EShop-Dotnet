using Microsoft.AspNetCore.Mvc;
using Moq;
using User.Application.Services;
using User.Domain.Models.Response;
using UserService.Controllers;

namespace UserService.Tests.Controllers;

public class UserControllerTests
{
    private readonly Mock<IUserService> _userServiceMock;
    private readonly UserController _userController;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _userController = new UserController(_userServiceMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnAllUsers()
    {
        // Arrange
        var users = new List<UserResponseDTO>
        {
            new() { Id = 1, FirstName = "John", LastName = "Doe", Email = "john@example.com" },
            new() { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane@example.com" }
        };
        _userServiceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(users);

        // Act
        var result = await _userController.Get();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(users, okResult.Value);
    }

    [Fact]
    public async Task GetById_WithValidId_ShouldReturnUser()
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
        _userServiceMock.Setup(s => s.GetAsync(userId)).ReturnsAsync(user);

        // Act
        var result = await _userController.Get(userId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(user, okResult.Value);
    }

    [Fact]
    public async Task GetById_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var userId = 999;
        _userServiceMock.Setup(s => s.GetAsync(userId)).ReturnsAsync((UserResponseDTO)null);

        // Act
        var result = await _userController.Get(userId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Post_WithValidUser_ShouldCreateUser()
    {
        // Arrange
        var userCreateDto = new UserCreateDTO
        {
            FirstName = "New",
            LastName = "User",
            Email = "newuser@example.com"
        };
        _userServiceMock.Setup(s => s.Add(userCreateDto)).ReturnsAsync(userCreateDto);

        // Act
        var result = await _userController.Post(userCreateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(userCreateDto, okResult.Value);
        _userServiceMock.Verify(s => s.Add(userCreateDto), Times.Once);
    }

    [Fact]
    public async Task Put_WithValidUser_ShouldUpdateUser()
    {
        // Arrange
        var userId = 1;
        var userUpdateDto = new UserUpdateDTO
        {
            Id = userId,
            FirstName = "Updated",
            LastName = "User",
            Email = "updated@example.com"
        };
        _userServiceMock.Setup(s => s.Update(userUpdateDto)).ReturnsAsync(userUpdateDto);

        // Act
        var result = await _userController.Put(userId, userUpdateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(userUpdateDto, okResult.Value);
        _userServiceMock.Verify(s => s.Update(userUpdateDto), Times.Once);
    }

    [Fact]  
    public async Task Delete_WithValidId_ShouldDeleteUser()
    {
        // Arrange
        var userId = 1;
        _userServiceMock.Setup(s => s.Delete(userId)).Returns(Task.CompletedTask);

        // Act
        var result = await _userController.Delete(userId);

        // Assert
        Assert.IsType<OkResult>(result);
        _userServiceMock.Verify(s => s.Delete(userId), Times.Once);
    }

    [Fact]
    public async Task Put_WithMismatchedId_ShouldStillUpdateUser()
    {
        // Arrange
        var urlId = 1;
        var userUpdateDto = new UserUpdateDTO
        {
            Id = 2, // Different from URL
            FirstName = "Updated",
            LastName = "User",
            Email = "updated@example.com"
        };
        _userServiceMock.Setup(s => s.Update(userUpdateDto)).ReturnsAsync(userUpdateDto);

        // Act
        var result = await _userController.Put(urlId, userUpdateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        _userServiceMock.Verify(s => s.Update(userUpdateDto), Times.Once);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999)]
    public async Task GetById_WithInvalidIds_ShouldReturnNotFound(int invalidId)
    {
        // Arrange
        _userServiceMock.Setup(s => s.GetAsync(invalidId)).ReturnsAsync((UserResponseDTO)null);

        // Act
        var result = await _userController.Get(invalidId);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-999)]
    public async Task Delete_WithInvalidIds_ShouldStillCallService(int invalidId)
    {
        // Arrange
        _userServiceMock.Setup(s => s.Delete(invalidId)).Returns(Task.CompletedTask);

        // Act
        var result = await _userController.Delete(invalidId);

        // Assert
        Assert.IsType<OkResult>(result);
        _userServiceMock.Verify(s => s.Delete(invalidId), Times.Once);
    }
} 