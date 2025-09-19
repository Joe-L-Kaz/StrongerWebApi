using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Stronger.Api.Extensions;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Application.Responses.WorkoutPlan;
using Stronger.Application.UseCases;
using Stronger.Application.UseCases.WorkoutPlan.Commands;
using Stronger.Domain.Responses;

namespace Stronger.ApplicationTests.UseCases.WorkoutPlan.Commands;

[TestClass]
public class CreateWorkoutPlanCommandHandlerTests
{
    private readonly IRequestHandler<CreateWorkoutPlanCommand, Response<CreateWorkoutPlanResponse>> _handler;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IRepositoryManager> _repo;
    private readonly Mock<IClaimsService> _claims;

    public CreateWorkoutPlanCommandHandlerTests()
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
        _handler = provider.GetRequiredService<IRequestHandler<CreateWorkoutPlanCommand, Response<CreateWorkoutPlanResponse>>>();
    }

    [TestMethod]
    public async Task TestHandle_RequestNull_Returns400()
    {
        // Act
        Response<CreateWorkoutPlanResponse> response = await _handler.Handle(null!, CancellationToken.None);

        // Assert
        Assert.AreEqual(400, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_NoAssignedExercises_Returns400()
    {
        // Arrange
        CreateWorkoutPlanCommand cmd = new CreateWorkoutPlanCommand(
            Name: "Test",
            AssociatedExercises: new List<long>()
        );

        // Act
        Response<CreateWorkoutPlanResponse> response = await _handler.Handle(null!, CancellationToken.None);

        // Assert
        Assert.AreEqual(400, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_ExerciseDoesNotExist_Returns404()
    {
        // Assert
        
    }
}
