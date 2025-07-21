using System;
using AutoMapper;
using Stronger.Application.UseCases.User.Commands;
using Stronger.Domain.Entities;

namespace Stronger.Application.Profiles;

internal class MapperProfile : Profile
{
    public MapperProfile()
    {
        this.CreateMap<CreateNewUserCommand, UserEntity>()
            .ForMember(e => e.PasswordHash, opt => opt.MapFrom(cmd => cmd.Password));
    }
}
