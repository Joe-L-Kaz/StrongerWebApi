using System;
using AutoMapper;
using Stronger.Application.UseCases.Exercise;
using Stronger.Application.UseCases.User.Commands;
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
    }
}
