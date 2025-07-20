using System;

using Microsoft.Extensions.DependencyInjection;

using MediatR;
using System.Reflection;


namespace Stronger.Application.UseCases;

public static class ApplicationLayerExtensions
{
    public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        return services;
    }

}
