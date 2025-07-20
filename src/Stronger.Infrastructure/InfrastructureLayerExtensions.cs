using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Stronger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Stronger.Application.Common.Interfaces;

namespace Stronger.Infrastructure;

public static class InfrastructureLayerExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<StrongerDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("MySqlConnection"), ServerVersion.AutoDetect(configuration.GetConnectionString("MySqlConnection")));
            })
            .AddScoped<IStrongerDbContext>(sp => sp.GetRequiredService<StrongerDbContext>());

        return services;
    }

}
