using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Stronger.Api.Extensions;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Responses.Exercise;
using Stronger.Application.UseCases;
using Stronger.Application.UseCases.Exercise.Queries;
using Stronger.Domain.Entities;
using Stronger.Domain.Responses;

namespace Stronger.ApplicationTests.Exercise.Queries;

[TestClass]
public class RetrieveExerciseQueryHandlerTests
{
    private Mock<IRepositoryManager> _repo;
    private IRequestHandler<RetrieveExerciseQuery, Response<RetrieveExerciseResponse>> _handler;
    public RetrieveExerciseQueryHandlerTests()
    {
        _repo = new Mock<IRepositoryManager>();
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton(_repo.Object);

        services
            .AddApiLayer()
            .AddApplicationLayer();

        var provider = services.BuildServiceProvider();
        _handler = provider.GetRequiredService<IRequestHandler<RetrieveExerciseQuery, Response<RetrieveExerciseResponse>>>();
    }

    [TestMethod]
    public async Task TestHandle_RequestNull_Returns400()
    {
        // Act
        Response<RetrieveExerciseResponse> response = await _handler.Handle(null!, CancellationToken.None);

        // Assert
        Assert.AreEqual(400, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_ExerciseNotFound_Returns404()
    {
        // Arrange 
        _repo
            .Setup(r => r.Exercises)
            .Returns(() =>
            {
                Mock<IExerciseRepository> mock = new Mock<IExerciseRepository>();
                mock
                    .Setup(m => m.GetByIdAsync(It.IsAny<long>(), CancellationToken.None))
                    .ReturnsAsync(() => null);

                return mock.Object;
            });

        Response<RetrieveExerciseResponse> response =
            await _handler.Handle(
                new RetrieveExerciseQuery(1),
                CancellationToken.None
            );

        Assert.AreEqual(404, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_ExerciseFound_Returns200()
    {
        // Arrange 
        _repo
            .Setup(r => r.Exercises)
            .Returns(() =>
            {
                Mock<IExerciseRepository> mock = new Mock<IExerciseRepository>();
                mock
                    .Setup(m => m.GetByIdAsync(It.IsAny<long>(), CancellationToken.None))
                    .ReturnsAsync(new ExerciseEntity());

                return mock.Object;
            });

        Response<RetrieveExerciseResponse> response =
            await _handler.Handle(
                new RetrieveExerciseQuery(1),
                CancellationToken.None
            );

        Assert.AreEqual(200, response.StatusCode);
    }
}
