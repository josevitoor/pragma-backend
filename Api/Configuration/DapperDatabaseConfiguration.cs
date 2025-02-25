using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Data;
using TCE.Base.Dapper;

namespace Application.Configuration;
/// <summary>
/// setup geral para os contextos -> bancos utilizados na aplicação
/// </summary>w
public static class DapperDatabaseConfiguration
{

    /// <summary>
    /// resolve as dependências de banco na aplicação
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection ConfigureDapper(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDapper(options => options.ConnectionString = configuration["ConnectionStrings:BdAutomationTCE_uAutomationTCE_Config"]);

        services.AddTransient<IDbConnection>((sp) => new SqlConnection(configuration["ConnectionStrings:BdAutomationTCE_uAutomationTCE_Config"]));

        return services;
    }
}
