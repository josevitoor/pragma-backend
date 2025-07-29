using Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Application.Configuration;
/// <summary>
/// setup geral para os contextos -> bancos utilizados na aplicação
/// </summary>
public static class ContextDatabaseConfiguration
{

    /// <summary>
    /// resolve as dependências de banco na aplicação
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureContext(this IServiceCollection services, IConfiguration configuration)
    {
        if (configuration.GetValue<string>("ApplicationInfo:Environment") == "workstation")
        {
            services.AddDbContextPool<PragmaContext>(options =>
                   options.UseSqlServer(configuration["ConnectionStrings:BdPragmaTCE_uPragmaTCE_Config"],
                    sqlServerOptions => sqlServerOptions
                            .UseNetTopologySuite()
                            .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                            .EnableSensitiveDataLogging()
                            .EnableDetailedErrors()
                            .LogTo(Log.Logger.Information, LogLevel.Information, null));
        }
        else
        {
            services.AddDbContextPool<PragmaContext>(options =>
                options.UseSqlServer(configuration["ConnectionStrings:BdPragmaTCE_uPragmaTCE_Config"],
                sqlServerOptions => sqlServerOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));
        }

        return services;
    }
}
