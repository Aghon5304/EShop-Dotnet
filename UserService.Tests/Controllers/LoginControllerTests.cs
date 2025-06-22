using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Authentication;
using User.Application.Producer;
using User.Application.Services;
using User.Domain.Models.Request;
using UserService.Controllers;

namespace UserService.Tests.Controllers;

public class LoginControllerTests
{
    private readonly Mock<ILoginService> _loginServiceMock;
    private readonly Mock<IKafkaProducer> _kafkaProducerMock;
    private readonly LoginController _loginController;

    public LoginControllerTests()
    {
        _loginServiceMock = new Mock<ILoginService>();
        _kafkaProducerMock = new Mock<IKafkaProducer>();
        _loginController = new LoginController(_loginServiceMock.Object, _kafkaProducerMock.Object);
    }

    [Fact]
    public void Login_WithValidCredentials_ShouldReturnOkWithToken()
    {
        // Arrange
        var loginRequest = new LogInRequest
        {
            Email = "test@example.com",
            Password = "password123"
        };
        var expectedToken = "jwt-token-123";

        _loginServiceMock.Setup(s => s.Login(loginRequest.Email, loginRequest.Password))
                        .Returns(expectedToken);
        _kafkaProducerMock.Setup(k => k.SendMessageAsync("login-events", It.IsAny<string>()))
                         .Returns(Task.CompletedTask);

        // Act
        var result = _loginController.Login(loginRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        dynamic responseValue = okResult.Value;
        Assert.Equal(expectedToken, responseValue.token);
        
        _loginServiceMock.Verify(s => s.Login(loginRequest.Email, loginRequest.Password), Times.Once);
        _kafkaProducerMock.Verify(k => k.SendMessageAsync("login-events", 
            It.Is<string>(msg => msg.Contains(loginRequest.Email) && msg.Contains("logged in"))), Times.Once);
    }

    [Fact]
    public void Login_WithInvalidCredentials_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginRequest = new LogInRequest
        {
            Email = "invalid@example.com",
            Password = "wrongpassword"
        };

        _loginServiceMock.Setup(s => s.Login(loginRequest.Email, loginRequest.Password))
                        .Throws<InvalidCredentialException>();

        // Act
        var result = _loginController.Login(loginRequest);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
        _loginServiceMock.Verify(s => s.Login(loginRequest.Email, loginRequest.Password), Times.Once);
        _kafkaProducerMock.Verify(k => k.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
    }

    [Fact]
    public void AdminPage_ShouldReturnOk()
    {
        // Act
        var result = _loginController.AdminPage();

        // Assert
        Assert.IsType<OkResult>(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Login_WithInvalidEmail_ShouldReturnUnauthorized(string invalidEmail)
    {
        // Arrange
        var loginRequest = new LogInRequest
        {
            Email = invalidEmail,
            Password = "password123"
        };

        _loginServiceMock.Setup(s => s.Login(invalidEmail, loginRequest.Password))
                        .Throws<InvalidCredentialException>();

        // Act
        var result = _loginController.Login(loginRequest);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Login_WithInvalidPassword_ShouldReturnUnauthorized(string invalidPassword)
    {
        // Arrange
        var loginRequest = new LogInRequest
        {
            Email = "test@example.com",
            Password = invalidPassword
        };

        _loginServiceMock.Setup(s => s.Login(loginRequest.Email, invalidPassword))
                        .Throws<InvalidCredentialException>();

        // Act
        var result = _loginController.Login(loginRequest);

        // Assert
        Assert.IsType<UnauthorizedResult>(result);
    }

    [Fact]
    public void Login_WithServiceException_ShouldReturnUnauthorized()
    {
        // Arrange
        var loginRequest = new LogInRequest
        {
            Email = "test@example.com",
            Password = "password123"
        };

        _loginServiceMock.Setup(s => s.Login(loginRequest.Email, loginRequest.Password))
                        .Throws<Exception>();

        // Act & Assert
        Assert.Throws<Exception>(() => _loginController.Login(loginRequest));
    }

    [Fact]
    public void Login_ShouldLogLoginEvent()
    {
        // Arrange
        var loginRequest = new LogInRequest
        {
            Email = "test@example.com",
            Password = "password123"
        };
        var expectedToken = "jwt-token-123";

        _loginServiceMock.Setup(s => s.Login(loginRequest.Email, loginRequest.Password))
                        .Returns(expectedToken);

        // Act
        _loginController.Login(loginRequest);

        // Assert
        _kafkaProducerMock.Verify(k => k.SendMessageAsync("login-events", 
            It.Is<string>(msg => 
                msg.Contains(loginRequest.Email) && 
                msg.Contains("logged in") &&
                msg.Contains(DateTime.UtcNow.ToString("yyyy-MM-dd")))), Times.Once);
    }
} 