using System.Linq.Expressions;
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
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.ApplicationTests.WorkoutPlan.Queries;

[TestClass]
public class RetrieveWorkoutPlanQueryHandlerTest
{
    private readonly IRequestHandler<RetrieveWorkoutPlanQuery, Response<RetrieveWorkoutPlanResponse>> _handler;
    private readonly Mock<IMapper> _mapper;
    private readonly Mock<IRepositoryManager> _repo;
    private readonly Mock<IClaimsService> _claims;

    public RetrieveWorkoutPlanQueryHandlerTest()
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

    [TestMethod]
    public async Task TestHandle_NoWorkoutPlans_Returns404()
    {
        // Arrange
        RetrieveWorkoutPlanQuery query = new(1);

        _repo
            .Setup(r => r.WorkoutPlans)
            .Returns(() =>
            {
                Mock<IWorkoutPlanRepository> repo = new();
                repo
                    .Setup(r => r.GetByIdAsync(It.IsAny<long>(), CancellationToken.None))
                    .ReturnsAsync(() => null);

                return repo.Object;
            });

        Response<RetrieveWorkoutPlanResponse> response = await _handler.Handle(query, CancellationToken.None);

        Assert.AreEqual(404, response.StatusCode);

    }

    [TestMethod]
    public async Task TestHandle_NoAssociatedExercises_Returns404()
    {
        // Arrange
        RetrieveWorkoutPlanQuery query = new(1);

        _repo
            .Setup(r => r.WorkoutPlans)
            .Returns(() =>
            {
                Mock<IWorkoutPlanRepository> repo = new();
                repo
                    .Setup(r => r.GetByIdAsync(It.IsAny<long>(), CancellationToken.None))
                    .ReturnsAsync(new WorkoutPlanEntity());

                return repo.Object;
            });

        _repo
            .Setup(r => r.WorkoutPlanExercises)
            .Returns(() =>
            {
                Mock<IWorkoutPlanExerciseRepository> repo = new();
                repo
                    .Setup(r => r.ListAsync(It.IsAny<Expression<Func<WorkoutPlanExerciseEntity, bool>>?>(), CancellationToken.None))
                    .ReturnsAsync(() => new List<WorkoutPlanExerciseEntity>());

                return repo.Object;
            });

        // Act
        Response<RetrieveWorkoutPlanResponse> response = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.AreEqual(404, response.StatusCode);
    }
}
