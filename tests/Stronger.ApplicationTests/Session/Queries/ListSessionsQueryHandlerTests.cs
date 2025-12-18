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
using Stronger.Application.UseCases.Session.Query;
using System.Linq.Expressions;

namespace Stronger.ApplicationTests.Sessions.Queries;

[TestClass]
public class ListSessionsQueryHandlerTests
{
    private readonly IRequestHandler<ListSessionsQuery, Response<List<RetrieveSessionResponse>>> _handler;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IRepositoryManager> _repo;
    private readonly Mock<IClaimsService> _claims;

    public ListSessionsQueryHandlerTests()
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
        _handler = provider.GetRequiredService<IRequestHandler<ListSessionsQuery, Response<List<RetrieveSessionResponse>>>>();
    }
    [TestMethod]
    public async Task TestHandle_RequestNull_Returns400()
    {
         // Act
        Response<List<RetrieveSessionResponse>> response = await _handler.Handle(null!, CancellationToken.None);

        // Assert
        Assert.AreEqual(400, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_Valid_Returns200()
    {
        // Arrange
        _claims
            .Setup(c => c.UserId)
            .Returns("80ec654c-cc1c-4aa1-98cb-be2bde0f8c16");

        _repo
            .Setup(r => r.Sessions)
            .Returns(() =>
            {
                Mock<ISessionRepository> sessions = new();
                sessions
                    .Setup(s => s.ListAsync(
                        It.IsAny<Expression<System.Func<Stronger.Domain.Entities.SessionEntity, bool>>>(),
                        CancellationToken.None
                    ))
                    .ReturnsAsync(new List<SessionEntity>());

                return sessions.Object;
                
            });

        _mapper
            .Setup(m => m.Map<RetrieveSessionResponse>(It.IsAny<SessionEntity>()))
            .Returns(new RetrieveSessionResponse());

        // Act
        ListSessionsQuery query = new();
        Response<List<RetrieveSessionResponse>> response = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.AreEqual(200, response.StatusCode);        
    }
}
