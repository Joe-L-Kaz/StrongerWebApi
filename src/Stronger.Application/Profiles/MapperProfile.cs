using System;
using System.Text.Json;
using AutoMapper;
using Stronger.Application.Responses.Exercise;
using Stronger.Application.Responses.Session;
using Stronger.Application.Responses.WorkoutPlan;
using Stronger.Application.UseCases.Exercise;
using Stronger.Application.UseCases.Session;
using Stronger.Application.UseCases.User.Commands;
using Stronger.Application.UseCases.WorkoutPlan.Commands;
using Stronger.Domain.Common;
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

        // Session
        this.CreateMap<CreateSessionCommand, SessionEntity>()
            .ForMember(
                dest => dest.SessionData,
                opt => opt.MapFrom(src => JsonSerializer.Serialize(
                    src.SessionData,
                    (JsonSerializerOptions?)null
                ))
            );
        
        this.CreateMap<SessionEntity, RetrieveSessionResponse>()
            .ForMember(
                dest => dest.SessionData,
                opt => opt.MapFrom(src => JsonSerializer.Deserialize<SessionData>(
                    src.SessionData,
                    (JsonSerializerOptions?)null
                ))
            );
    }
}
