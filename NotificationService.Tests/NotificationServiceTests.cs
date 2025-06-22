using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Moq;
using NotificationService;

namespace NotificationService.Tests;

public class NotificationServiceTests
{
    private readonly Mock<ILogger<Function1>> _loggerMock;
    private readonly Function1 _function;

    public NotificationServiceTests()
    {
        _loggerMock = new Mock<ILogger<Function1>>();
        _function = new Function1();
    }

    [Fact]
    public async Task Run_WithValidHttpRequest_ShouldReturnOkResponse()
    {
        // Arrange
        var context = new Mock<FunctionContext>();
        var request = new Mock<HttpRequest>();
        
        // Mock the request body
        var requestBody = "test request body";
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(requestBody));
        request.Setup(r => r.Body).Returns(stream);

        // Act
        var result = await _function.Run(request.Object, context.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Welcome to Azure Functions!", okResult.Value);
    }

    [Fact]
    public async Task Run_WithEmptyRequest_ShouldStillReturnOkResponse()
    {
        // Arrange
        var context = new Mock<FunctionContext>();
        var request = new Mock<HttpRequest>();
        
        var emptyStream = new MemoryStream();
        request.Setup(r => r.Body).Returns(emptyStream);

        // Act
        var result = await _function.Run(request.Object, context.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Welcome to Azure Functions!", okResult.Value);
    }

    [Theory]
    [InlineData("POST")]
    [InlineData("GET")]
    [InlineData("PUT")]
    [InlineData("DELETE")]
    public async Task Run_WithDifferentHttpMethods_ShouldReturnOkResponse(string httpMethod)
    {
        // Arrange
        var context = new Mock<FunctionContext>();
        var request = new Mock<HttpRequest>();
        
        request.SetupGet(r => r.Method).Returns(httpMethod);
        var stream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes("test"));
        request.Setup(r => r.Body).Returns(stream);

        // Act
        var result = await _function.Run(request.Object, context.Object);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Welcome to Azure Functions!", okResult.Value);
    }

    [Fact]
    public void KafkaMessage_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var message = new KafkaMessage
        {
            Id = 1,
            Topic = "test-topic",
            Message = "test message",
            Timestamp = DateTime.UtcNow
        };

        // Assert
        Assert.Equal(1, message.Id);
        Assert.Equal("test-topic", message.Topic);
        Assert.Equal("test message", message.Message);
        Assert.True(message.Timestamp <= DateTime.UtcNow);
    }

    [Fact]
    public void LoginNotificationEmail_Creation_ShouldSetPropertiesCorrectly()
    {
        // Arrange & Act
        var notification = new LoginNotificationEmail
        {
            To = "test@example.com",
            Subject = "Login Notification",
            Body = "You have successfully logged in"
        };

        // Assert
        Assert.Equal("test@example.com", notification.To);
        Assert.Equal("Login Notification", notification.Subject);
        Assert.Equal("You have successfully logged in", notification.Body);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void KafkaMessage_WithInvalidTopic_ShouldAllowButIndicateInvalidState(string invalidTopic)
    {
        // Arrange & Act
        var message = new KafkaMessage
        {
            Topic = invalidTopic,
            Message = "valid message"
        };

        // Assert
        Assert.Equal(invalidTopic, message.Topic);
        Assert.True(string.IsNullOrWhiteSpace(message.Topic));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void LoginNotificationEmail_WithInvalidEmail_ShouldAllowButIndicateInvalidState(string invalidEmail)
    {
        // Arrange & Act
        var notification = new LoginNotificationEmail
        {
            To = invalidEmail,
            Subject = "Test Subject",
            Body = "Test Body"
        };

        // Assert
        Assert.Equal(invalidEmail, notification.To);
        Assert.True(string.IsNullOrWhiteSpace(notification.To));
    }

    [Fact]
    public void KafkaMessage_DefaultValues_ShouldBeSet()
    {
        // Arrange & Act
        var message = new KafkaMessage();

        // Assert
        Assert.Equal(0, message.Id);
        Assert.Null(message.Topic);
        Assert.Null(message.Message);
        Assert.Equal(default(DateTime), message.Timestamp);
    }

    [Fact]
    public void LoginNotificationEmail_DefaultValues_ShouldBeSet()
    {
        // Arrange & Act
        var notification = new LoginNotificationEmail();

        // Assert
        Assert.Null(notification.To);
        Assert.Null(notification.Subject);
        Assert.Null(notification.Body);
    }
} 