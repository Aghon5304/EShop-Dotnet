using Microsoft.EntityFrameworkCore;
using Moq;
using User.Application.Producer;
using User.Application.Services;
using User.Domain.Exceptions.Login;
using User.Domain.Models.Entities;
using User.Domain.Models.Response;
using User.Domain.Repositories;

namespace User.Application.Tests.Services;

public class LoginServiceTests
{
    private readonly Mock<IJwtTokenService> _jwtTokenServiceMock;
    private readonly Mock<IKafkaProducer> _kafkaProducerMock;
    private readonly Mock<IRepository> _repositoryMock;
    private readonly Mock<DataContext> _contextMock;
    private readonly LoginService _loginService;

    public LoginServiceTests()
    {
        _jwtTokenServiceMock = new Mock<IJwtTokenService>();
        _kafkaProducerMock = new Mock<IKafkaProducer>();
        _repositoryMock = new Mock<IRepository>();
        var options = new DbContextOptionsBuilder<DataContext>().Options;
        _contextMock = new Mock<DataContext>(options);
        _loginService = new LoginService(
            _jwtTokenServiceMock.Object,
            _kafkaProducerMock.Object,
            _repositoryMock.Object,
            _contextMock.Object);
    }

    [Fact]
    public async Task Login_WithValidCredentials_ShouldReturnToken()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";
        var userLogin = new UserLoginDTO
        {
            Email = email,
            PasswordHash = password,
            Roles = new List<Role> { new() { Name = "User" } }
        };
        var expectedToken = "jwt-token-123";

        _repositoryMock.Setup(r => r.GetUserLoginAsync(email))
                      .ReturnsAsync(userLogin);
        _jwtTokenServiceMock.Setup(j => j.GenerateToken(userLogin.Id, It.IsAny<List<string>>()))
                           .Returns(expectedToken);
        _kafkaProducerMock.Setup(k => k.SendMessageAsync("after-login-email-topic", email))
                         .Returns(Task.CompletedTask);

        // Act
        var result = await _loginService.Login(email, password);

        // Assert
        Assert.Equal(expectedToken, result);
        _repositoryMock.Verify(r => r.GetUserLoginAsync(email), Times.Once);
        _jwtTokenServiceMock.Verify(j => j.GenerateToken(userLogin.Id, It.Is<List<string>>(roles => roles.Contains("User"))), Times.Once);
        _kafkaProducerMock.Verify(k => k.SendMessageAsync("after-login-email-topic", email), Times.Once);
    }

    [Fact]
    public async Task Login_WithNonExistentUser_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var email = "nonexistent@example.com";
        var password = "password123";

        _repositoryMock.Setup(r => r.GetUserLoginAsync(email))
                      .ReturnsAsync((UserLoginDTO?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => _loginService.Login(email, password));
        _repositoryMock.Verify(r => r.GetUserLoginAsync(email), Times.Once);
        _jwtTokenServiceMock.Verify(j => j.GenerateToken(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        _kafkaProducerMock.Verify(k => k.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Login_WithWrongPassword_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var email = "test@example.com";
        var correctPassword = "password123";
        var wrongPassword = "wrongpassword";
        var userLogin = new UserLoginDTO
        {
            Email = email,
            PasswordHash = correctPassword,
            Roles = new List<Role> { new() { Name = "User" } }
        };

        _repositoryMock.Setup(r => r.GetUserLoginAsync(email))
                      .ReturnsAsync(userLogin);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => _loginService.Login(email, wrongPassword));
        _repositoryMock.Verify(r => r.GetUserLoginAsync(email), Times.Once);
        _jwtTokenServiceMock.Verify(j => j.GenerateToken(It.IsAny<int>(), It.IsAny<List<string>>()), Times.Never);
        _kafkaProducerMock.Verify(k => k.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public async Task Login_WithWrongEmail_ShouldThrowInvalidCredentialsException()
    {
        // Arrange
        var correctEmail = "test@example.com";
        var wrongEmail = "wrong@example.com";
        var password = "password123";
        var userLogin = new UserLoginDTO
        {
            Email = correctEmail,
            PasswordHash = password,
            Roles = new List<Role> { new() { Name = "User" } }
        };

        _repositoryMock.Setup(r => r.GetUserLoginAsync(wrongEmail))
                      .ReturnsAsync(userLogin);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => _loginService.Login(wrongEmail, password));
    }

    [Fact]
    public async Task Login_WithMultipleRoles_ShouldIncludeAllRoles()
    {
        // Arrange
        var email = "admin@example.com";
        var password = "adminpass";
        var userLogin = new UserLoginDTO
        {
            Email = email,
            PasswordHash = password,
            Roles = new List<Role> 
            { 
                new() { Name = "Admin" },
                new() { Name = "User" },
                new() { Name = "Manager" }
            }
        };
        var expectedToken = "admin-jwt-token";

        _repositoryMock.Setup(r => r.GetUserLoginAsync(email))
                      .ReturnsAsync(userLogin);
        _jwtTokenServiceMock.Setup(j => j.GenerateToken(userLogin.Id, It.IsAny<List<string>>()))
                           .Returns(expectedToken);

        // Act
        var result = await _loginService.Login(email, password);

        // Assert
        Assert.Equal(expectedToken, result);
        _jwtTokenServiceMock.Verify(j => j.GenerateToken(userLogin.Id, 
            It.Is<List<string>>(roles => 
                roles.Contains("Admin") && 
                roles.Contains("User") && 
                roles.Contains("Manager") &&
                roles.Count == 3)), Times.Once);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public async Task Login_WithInvalidEmail_ShouldThrowInvalidCredentialsException(string invalidEmail)
    {
        // Arrange
        var password = "password123";
        _repositoryMock.Setup(r => r.GetUserLoginAsync(invalidEmail))
                      .ReturnsAsync((UserLoginDTO?)null);

        // Act & Assert
        await Assert.ThrowsAsync<InvalidCredentialsException>(() => _loginService.Login(invalidEmail, password));
    }
}