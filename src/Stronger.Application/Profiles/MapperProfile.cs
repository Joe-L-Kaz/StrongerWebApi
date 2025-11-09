using System;
using AutoMapper;
using Stronger.Application.Responses.Exercise;
using Stronger.Application.Responses.WorkoutPlan;
using Stronger.Application.UseCases.Exercise;
using Stronger.Application.UseCases.User.Commands;
using Stronger.Application.UseCases.WorkoutPlan.Commands;
using Stronger.Domain.Entities;

namespace Stronger.Application.Profiles;

internal class MapperProfile : Profile
{
    public MapperProfile()
    {
        // User
        this.CreateMap<CreateNewUserCommand, UserEntity>();

        // Exercise
        this.CreateMap<CreateExerciseCommand, ExerciseEntity>();
        this.CreateMap<ExerciseEntity, RetrieveExerciseResponse>();

        // Workout plan 
        this.CreateMap<CreateWorkoutPlanCommand, WorkoutPlanEntity>();
        this.CreateMap<WorkoutPlanEntity, RetrieveWorkoutPlanResponse>();
    }
}
