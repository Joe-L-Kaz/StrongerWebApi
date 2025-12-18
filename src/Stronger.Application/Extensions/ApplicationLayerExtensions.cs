using System.Reflection;

using Microsoft.Extensions.DependencyInjection;
using Stronger.Application.Profiles;

namespace Stronger.Application.Extensions;

public static class ApplicationLayerExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()))
            .AddAutoMapper(cfg => {}, typeof(MapperProfile));
        return services;
    }
}
