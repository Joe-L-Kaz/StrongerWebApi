using System.Linq.Expressions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
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
public class ListExercisesQueryHandlerTests
{
    private Mock<IRepositoryManager> _repo;
    private IRequestHandler<ListExercisesQuery, Response<IEnumerable<RetrieveExerciseResponse>>> _handler;
    public ListExercisesQueryHandlerTests()
    {
        _repo = new Mock<IRepositoryManager>();
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton(_repo.Object);

        services
            .AddApiLayer()
            .AddApplicationLayer()
            .AddLogging();

        var provider = services.BuildServiceProvider();
        _handler = provider.GetRequiredService<IRequestHandler<ListExercisesQuery, Response<IEnumerable<RetrieveExerciseResponse>>>>();
    }

    [TestMethod]
    public async Task TestHandle_RequestNull_Returns400()
    {
        // Act
        Response<IEnumerable<RetrieveExerciseResponse>> response = await _handler.Handle(null!, CancellationToken.None);

        // Assert
        Assert.AreEqual(400, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_ExercisesFound_Returns200()
    {
        // Arrange
        ListExercisesQuery query = new ListExercisesQuery(
            null,
            null,
            null,
            null,
            null
        );

        _repo
            .Setup(r => r.Exercises)
            .Returns(() =>
            {
                Mock<IExerciseRepository> exercises = new Mock<IExerciseRepository>();
                exercises
                    .Setup(e => e.ListAsync(
                        It.IsAny<Expression<Func<ExerciseEntity, bool>>?>(),
                        CancellationToken.None
                    ))
                    .ReturnsAsync(new List<ExerciseEntity>());

                return exercises.Object;
            });

        // Act
        Response<IEnumerable<RetrieveExerciseResponse>> response = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.AreEqual(200, response.StatusCode);
    }
}
