using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Stronger.Api.Extensions;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Common.Interfaces;
using Stronger.Application.Responses.WorkoutPlan;
using Stronger.Application.Extensions;
using Stronger.Application.UseCases.WorkoutPlan.Commands;
using Stronger.Domain.Entities;
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
        // Act
        Response<CreateWorkoutPlanResponse> response = await _handler.Handle(null!, CancellationToken.None);

        // Assert
        Assert.AreEqual(400, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_ExerciseDoesNotExist_Returns404()
    {
        // Arrange
        CreateWorkoutPlanCommand cmd = new(
            "Plan Name",
            new List<long> { 1, 2, 3 }
        );

        _repo
            .Setup(r => r.Exercises)
            .Returns(() =>
            {
                Mock<IExerciseRepository> repo = new();

                repo
                    .Setup(e => e.ListAsync(null, CancellationToken.None))
                    .ReturnsAsync(new List<ExerciseEntity>()
                    {
                        new ExerciseEntity
                        {
                            Id = 4
                        }
                    });

                return repo.Object;
            });

        Response<CreateWorkoutPlanResponse> response =
            await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.AreEqual(404, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_Success_Returns201()
    {
        // Arrange
        CreateWorkoutPlanCommand cmd = new(
            "Plan Name",
            new List<long> { 1 }
        );

        _repo
            .Setup(r => r.Exercises)
            .Returns(() =>
            {
                Mock<IExerciseRepository> repo = new();

                repo
                    .Setup(e => e.ListAsync(null, CancellationToken.None))
                    .ReturnsAsync(new List<ExerciseEntity>()
                    {
                        new ExerciseEntity
                        {
                            Id = 1
                        }
                    });

                return repo.Object;
            });

        _claims
            .Setup(c => c.UserId)
            .Returns(Guid.NewGuid().ToString());

        _mapper
            .Setup(m => m.Map<WorkoutPlanEntity>(It.IsAny<CreateWorkoutPlanCommand>()))
            .Returns(new WorkoutPlanEntity());

        _repo
            .Setup(r => r.WorkoutPlans)
            .Returns(() =>
            {
                Mock<IWorkoutPlanRepository> repo = new();

                repo
                    .Setup(r => r.GetNextIdAsync())
                    .ReturnsAsync(1);

                return repo.Object;
            });
        
        _repo
            .Setup(r => r.WorkoutPlanExercises)
            .Returns(() =>
            {
                Mock<IWorkoutPlanExerciseRepository> repo = new();

                repo
                    .Setup( r => r.AddAsync(It.IsAny<WorkoutPlanExerciseEntity>(), CancellationToken.None));

                return repo.Object;
            });

        Response<CreateWorkoutPlanResponse> response =
            await _handler.Handle(cmd, CancellationToken.None);

        // Assert
        Assert.AreEqual(201, response.StatusCode);
    }
}
