using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Moq;
using Stronger.Api.Extensions;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Application.UseCases;
using Stronger.Application.UseCases.User.Commands;
using Stronger.Domain.Responses;
using Stronger.Domain.Entities;

namespace Stronger.ApplicationTests.User.Commands;

[TestClass]
public class AuthenticateUserCommandHandlerTests
{
    Mock<IRepositoryManager> _repo;
    Mock<ITokenService> _tokenService;
    Mock<IPasswordService> _passwordService;
    IRequestHandler<AuthenticateUserCommand, Response> _handler;

    public AuthenticateUserCommandHandlerTests()
    {
        _repo = new Mock<IRepositoryManager>();
        _tokenService = new Mock<ITokenService>();
        _passwordService = new Mock<IPasswordService>();

        IServiceCollection services = new ServiceCollection();
        services
            .AddApiLayer()
            .AddApplicationLayer();

        services.AddSingleton(_repo.Object);
        services.AddSingleton(_tokenService.Object);
        services.AddSingleton(_passwordService.Object);

        var provider = services.BuildServiceProvider();

        _handler = provider.GetRequiredService<IRequestHandler<AuthenticateUserCommand, Response>>();
    }

    [TestMethod]
    public async Task TestHandle_RequestNull_Returns400()
    {
        //Act
        Response response = await _handler.Handle(null!, CancellationToken.None);

        //Assert
        Assert.AreEqual(400, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_EmailNotFound_Returns401()
    {
        // Arrange
        AuthenticateUserCommand cmd = new(
            "Some@Email.com",
            "SomePassword"
        );

        _repo
            .Setup(u => u.Users)
            .Returns(() =>
            {
                Mock<IUserRepository> repo = new();

                repo
                    .Setup(u => u.GetByEmailAsync(It.IsAny<String>(), CancellationToken.None))
                    .ReturnsAsync(() => null!);

                return repo.Object;
            });

        // Act
        Response response = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.AreEqual(401, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_PasswordIncorrect_Returns401()
    {
        // Arrange
        AuthenticateUserCommand cmd = new(
            "Some@Email.com",
            "SomePassword"
        );

        _repo
            .Setup(u => u.Users)
            .Returns(() =>
            {
                Mock<IUserRepository> repo = new();

                repo
                    .Setup(u => u.GetByEmailAsync(It.IsAny<String>(), CancellationToken.None))
                    .ReturnsAsync(() => new UserEntity());

                return repo.Object;
            });

        _passwordService
            .Setup(p => p.Verify(It.IsAny<String>(), It.IsAny<String>()))
            .Returns(false);

        // Act
        Response response = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.AreEqual(401, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_CorrectCredentials_Returns200()
    {
        // Arrange
        AuthenticateUserCommand cmd = new(
            "Some@Email.com",
            "SomePassword"
        );

        _repo
            .Setup(u => u.Users)
            .Returns(() =>
            {
                Mock<IUserRepository> repo = new();

                repo
                    .Setup(u => u.GetByEmailAsync(It.IsAny<String>(), CancellationToken.None))
                    .ReturnsAsync(() => new UserEntity());

                return repo.Object;
            });

        _passwordService
            .Setup(p => p.Verify(It.IsAny<String>(), It.IsAny<String>()))
            .Returns(true);

        _tokenService
            .Setup(t => t.GenerateToken(
                It.IsAny<Guid>(),
                It.IsAny<String>(),
                It.IsAny<String>(),
                It.IsAny<DateOnly>(),
                It.IsAny<String>()
            ))
            .Returns(String.Empty);

        // Act
        Response response = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.AreEqual(200, response.StatusCode);
    }
}