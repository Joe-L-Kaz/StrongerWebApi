using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Stronger.Api.Extensions;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Application.Responses.WorkoutPlan;
using Stronger.Application.UseCases;
using Stronger.Application.UseCases.WorkoutPlan.Queries;
using Stronger.Domain.Responses;

namespace Stronger.ApplicationTests.WorkoutPlan.Queries;

[TestClass]
public class RetrieveWorkoutQueryHandlerTest
{
    private readonly IRequestHandler<RetrieveWorkoutPlanQuery, Response<RetrieveWorkoutPlanResponse>> _handler;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IRepositoryManager> _repo;
    private readonly Mock<IClaimsService> _claims;

    public RetrieveWorkoutQueryHandlerTest()
    {
        _mapper = new();
        _repo = new();
        _claims = new();

        IServiceCollection services = new ServiceCollection();

        services
            .AddApiLayer()
            .AddApplicationLayer();

        services.AddSingleton(_mapper.Object);
        services.AddSingleton(_repo.Object);
        services.AddSingleton(_claims.Object);

        IServiceProvider provider = services.BuildServiceProvider();

        _handler = provider.GetRequiredService<IRequestHandler<RetrieveWorkoutPlanQuery, Response<RetrieveWorkoutPlanResponse>>>();
    }
    [TestMethod]
    public async Task TestHandle_RequestNull_Returns400()
    {
        // Act
        Response<RetrieveWorkoutPlanResponse> response = await _handler.Handle(null!, CancellationToken.None);

        // Assert
        Assert.AreEqual(400, response.StatusCode);
    }
}
