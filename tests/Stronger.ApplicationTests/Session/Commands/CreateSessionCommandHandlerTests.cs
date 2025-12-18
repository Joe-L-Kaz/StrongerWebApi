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

namespace Company.TestProject1;

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
}
