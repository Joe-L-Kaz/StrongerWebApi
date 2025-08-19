using System.Reflection;

namespace Stronger.Api.Extensions;

public static class ApiLayerExtensions
{
    public static IServiceCollection AddApiLayer(this IServiceCollection services)
    {
        services
            .AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        return services;
    }
}
