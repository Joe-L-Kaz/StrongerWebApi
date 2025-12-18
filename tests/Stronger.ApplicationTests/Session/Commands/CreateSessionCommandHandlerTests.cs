using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Stronger.Api.Extensions;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Application.Extensions;
using Stronger.Domain.Responses;
using Stronger.Application.UseCases.Session;
using Stronger.Application.Responses.Session;
using Stronger.Domain.Common;
using Stronger.Domain.Entities;

namespace Stronger.ApplicationTests.UseCases.Session.Commands;

[TestClass]
public class CreateSessionCommandHandlerTests
{
    private readonly IRequestHandler<CreateSessionCommand, Response<CreateSessionResponse>> _handler;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IRepositoryManager> _repo;
    private readonly Mock<IClaimsService> _claims;

    public CreateSessionCommandHandlerTests()
    {
        _mapper = new Mock<IMapper>();
        _repo = new Mock<IRepositoryManager>();
        _claims = new Mock<IClaimsService>();

        IServiceCollection services = new ServiceCollection();

        services
            .AddApiLayer()
            .AddApplicationLayer();

        services.AddSingleton(_mapper.Object);
        services.AddSingleton(_repo.Object);
        services.AddSingleton(_claims.Object);

        IServiceProvider provider = services.BuildServiceProvider();
        _handler = provider.GetRequiredService<IRequestHandler<CreateSessionCommand, Response<CreateSessionResponse>>>();
    }


    [TestMethod]
    public async Task TestHandle_RequestNull_Returns400()
    {
         // Act
        Response<CreateSessionResponse> response = await _handler.Handle(null!, CancellationToken.None);

        // Assert
        Assert.AreEqual(400, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_SessionDataNull_Returns400()
    {
        // Arrange
        CreateSessionCommand cmd = new(
            null!
        );

        // Act
        Response<CreateSessionResponse> response = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.AreEqual(400, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_ExercisesEmpty_Returns400()
    {
        // Arrange
        CreateSessionCommand cmd = new(
            new SessionData()
        );

        // Act
        Response<CreateSessionResponse> response = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.AreEqual(400, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_DatabaseError_Returns500()
    {
        // Arrange
        _mapper
            .Setup(m => m.Map<SessionEntity>(It.IsAny<CreateSessionCommand>()))
            .Returns(new SessionEntity());

        _claims
            .Setup(c => c.UserId)
            .Returns("80ec654c-cc1c-4aa1-98cb-be2bde0f8c16");

        _repo
            .Setup(r => r.Sessions)
            .Returns(() =>
            {
                Mock<ISessionRepository> sessions = new();
                sessions
                    .Setup(s => s.AddAsync(It.IsAny<SessionEntity>(), CancellationToken.None));
                
                return sessions.Object;
            });

        _repo
            .Setup(r => r.SaveChangesAsync(CancellationToken.None))
            .ThrowsAsync(new Exception());

        CreateSessionCommand cmd = new(
            new SessionData()
            {
                Exercises = new List<Stronger.Domain.Common.Exercise>
                {
                    new Domain.Common.Exercise()
                }
            }
        );

        // Act
        Response<CreateSessionResponse> response = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.AreEqual(500, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_Valid_Returns201()
    {
        // Arrange
        _mapper
            .Setup(m => m.Map<SessionEntity>(It.IsAny<CreateSessionCommand>()))
            .Returns(new SessionEntity());

        _claims
            .Setup(c => c.UserId)
            .Returns("80ec654c-cc1c-4aa1-98cb-be2bde0f8c16");

        _repo
            .Setup(r => r.Sessions)
            .Returns(() =>
            {
                Mock<ISessionRepository> sessions = new();
                sessions
                    .Setup(s => s.AddAsync(It.IsAny<SessionEntity>(), CancellationToken.None));
                
                return sessions.Object;
            });

        _repo
            .Setup(r => r.SaveChangesAsync(CancellationToken.None));

        CreateSessionCommand cmd = new(
            new SessionData()
            {
                Exercises = new List<Stronger.Domain.Common.Exercise>
                {
                    new Domain.Common.Exercise()
                }
            }
        );

        // Act
        Response<CreateSessionResponse> response = await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.AreEqual(201, response.StatusCode);
    }
}
