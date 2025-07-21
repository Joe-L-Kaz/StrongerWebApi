using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Stronger.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Stronger.Application.Common.Interfaces;
using Stronger.Infrastructure.Services;

namespace Stronger.Infrastructure;

public static class InfrastructureLayerExtensions
{
    public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddDbContext<StrongerDbContext>(options =>
            {
                options.UseMySql(configuration.GetConnectionString("MySql")!, ServerVersion.AutoDetect(configuration.GetConnectionString("MySql")));
            })
            .AddScoped<IStrongerDbContext>(sp => sp.GetRequiredService<StrongerDbContext>())
            .AddScoped<IPasswordService, PasswordService>();

        return services;
    }

}
