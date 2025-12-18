
using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Stronger.Api.Extensions;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Application.Responses.User;
using Stronger.Application.Extensions;
using Stronger.Application.UseCases.User.Commands;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.ApplicationTests.User.Commands;

[TestClass]
public class CreateNewUserCommandHandlerTests
{
    Mock<IRepositoryManager> _repo;
    Mock<IPasswordService> _passwordService;
    Mock<IMapper> _mapper;
    IRequestHandler<CreateNewUserCommand, Response<CreateNewUserResponse>> _handler;

    public CreateNewUserCommandHandlerTests()
    {
        _repo = new Mock<IRepositoryManager>();
        _passwordService = new Mock<IPasswordService>();
        _mapper = new Mock<IMapper>();

        IServiceCollection services = new ServiceCollection();
        services
            .AddApiLayer()
            .AddApplicationLayer();

        services.AddSingleton(_repo.Object);
        services.AddSingleton(_passwordService.Object);
        services.AddSingleton(_mapper.Object);

        var provider = services.BuildServiceProvider();

        _handler = provider.GetRequiredService<IRequestHandler<CreateNewUserCommand, Response<CreateNewUserResponse>>>();
    }

    [TestMethod]
    public async Task TestHandle_RequestNull_Returns400()
    {
        // Act
        Response<CreateNewUserResponse> response = await _handler.Handle(null!, CancellationToken.None);

        // Assert
        Assert.AreEqual(400, response.StatusCode);
        Assert.AreEqual("Request cannot be null", response.Error!.Message);
    }

    [TestMethod]
    public async Task TestHandle_EmailAlreadyExists_Returns409()
    {
        // Arrange
        CreateNewUserCommand cmd = new(
            "Fake",
            "Namington",
            DateOnly.FromDateTime(DateTime.Now),
            "Fake@Email.com",
            "Password_1"
        );

        _mapper
            .Setup(m => m.Map<UserEntity>(It.IsAny<CreateNewUserCommand>()))
            .Returns(() => new UserEntity());

        _repo
            .Setup(r => r.Users)
            .Returns(() =>
            {
                Mock<IUserRepository> repo = new();

                repo
                    .Setup(r => r.GetByEmailAsync(It.IsAny<String>(), CancellationToken.None))
                    .ReturnsAsync(new UserEntity());

                return repo.Object;
            });

        // Act
        Response<CreateNewUserResponse> response = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.AreEqual(409, response.StatusCode);
        Assert.AreEqual("Email already in use", response.Error!.Message);
    }

    [TestMethod]
    public async Task TestHandle_PasswordInvalid_Returns400()
    {
        // Arrange
        CreateNewUserCommand cmd = new(
            "Fake",
            "Namington",
            DateOnly.FromDateTime(DateTime.Now),
            "Fake@Email.com",
            "Password_1"
        );

        _mapper
            .Setup(m => m.Map<UserEntity>(It.IsAny<CreateNewUserCommand>()))
            .Returns(() => new UserEntity());

        _repo
            .Setup(r => r.Users)
            .Returns(() =>
            {
                Mock<IUserRepository> repo = new();

                repo
                    .Setup(r => r.GetByEmailAsync(It.IsAny<String>(), CancellationToken.None))
                    .ReturnsAsync(() => null);

                return repo.Object;
            });

        _passwordService
            .Setup(p => p.Validate(It.IsAny<String>()))
            .Returns(false);

        // Act
        Response<CreateNewUserResponse> response = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.AreEqual(400, response.StatusCode);
        Assert.AreEqual("Password does not meet security requirements", response.Error!.Message);
    }

    [TestMethod]
    public async Task TestHandle_Valid_Returns201()
    {
        // Arrange
        CreateNewUserCommand cmd = new(
            "Fake",
            "Namington",
            DateOnly.FromDateTime(DateTime.Now),
            "Fake@Email.com",
            "Password_1"
        );

        _mapper
            .Setup(m => m.Map<UserEntity>(It.IsAny<CreateNewUserCommand>()))
            .Returns(() => new UserEntity());

        _repo
            .Setup(r => r.Users)
            .Returns(() =>
            {
                Mock<IUserRepository> repo = new();

                repo
                    .Setup(r => r.GetByEmailAsync(It.IsAny<String>(), CancellationToken.None))
                    .ReturnsAsync(() => null);

                return repo.Object;
            });

        _passwordService
            .Setup(p => p.Validate(It.IsAny<String>()))
            .Returns(true);

        // Act
        Response<CreateNewUserResponse> response = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.AreEqual(201, response.StatusCode);
        Assert.IsNotNull(response.Content);
    }
}
