using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Stronger.Api.Extensions;
using Stronger.Application.Abstractions.Repositories;
using Stronger.Application.Responses.Exercise;
using Stronger.Application.Extensions;
using Stronger.Application.UseCases.Exercise;
using Stronger.Domain.Entities;
using Stronger.Domain.Enums;
using Stronger.Domain.Responses;

namespace Stronger.ApplicationTests.Exercise.Commands;

[TestClass]
public class CreateExerciseCommandHandlerTests
{
    private Mock<IRepositoryManager> _repo;
    private Mock<IMapper> _mapper;
    private IRequestHandler<CreateExerciseCommand, Response<CreateExerciseResponse>> _handler;

    public CreateExerciseCommandHandlerTests()
    {
        _repo = new Mock<IRepositoryManager>();
        _mapper = new Mock<IMapper>();

        IServiceCollection services = new ServiceCollection();
        services
            .AddApiLayer()
            .AddApplicationLayer();

        services.AddSingleton(_repo.Object);
        services.AddSingleton(_mapper.Object);

        var provider = services.BuildServiceProvider();

        _handler = provider.GetRequiredService<IRequestHandler<CreateExerciseCommand, Response<CreateExerciseResponse>>>();
    }

    [TestMethod]
    public async Task TestHandle_RequestNull_Returns400()
    {
        // Act
        Response<CreateExerciseResponse> response = await _handler.Handle(null!, CancellationToken.None);

        //Assert
        Assert.AreEqual(400, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_ExerciseExists_Returns409()
    {
        // Arrange
        CreateExerciseCommand cmd = new CreateExerciseCommand(
            "",
            "",
            "",
            MuscleGroup.Back,
            MuscleGroup.Bicep,
            ExerciseType.Strength,
            ForceType.Pull
        );

        _repo
            .Setup(r => r.Exercises)
            .Returns(() =>
            {
                Mock<IExerciseRepository> repository = new Mock<IExerciseRepository>();
                repository
                    .Setup(r => r.AnyAsync(It.IsAny<String>(), CancellationToken.None))
                    .ReturnsAsync(true);

                return repository.Object;
            });

        Response<CreateExerciseResponse> response = await _handler.Handle(cmd, CancellationToken.None);

        Assert.AreEqual(409, response.StatusCode);
    }

    [TestMethod]
    public async Task TestHandle_Valid_Returns201()
    {
        // Arrange
        CreateExerciseCommand cmd = new CreateExerciseCommand(
            "",
            "",
            "",
            MuscleGroup.Back,
            MuscleGroup.Bicep,
            ExerciseType.Strength,
            ForceType.Pull
        );

        _repo
            .Setup(r => r.Exercises)
            .Returns(() =>
            {
                Mock<IExerciseRepository> repository = new Mock<IExerciseRepository>();
                repository
                    .Setup(r => r.AnyAsync(It.IsAny<String>(), CancellationToken.None))
                    .ReturnsAsync(false);

                return repository.Object;
            });

        _mapper
            .Setup(m => m.Map<ExerciseEntity>(cmd))
            .Returns(new ExerciseEntity());

        Response<CreateExerciseResponse> response = await _handler.Handle(cmd, CancellationToken.None);

        Assert.AreEqual(201, response.StatusCode);
    }
}
