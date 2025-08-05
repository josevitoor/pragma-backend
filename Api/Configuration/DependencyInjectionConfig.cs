using AutoMapper;
using Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services;
using TCE.Base.UnitOfWork;

namespace Application.Configurations;
/// <summary>
/// Container de inejeção de dependências a serem utilizadas no projeto
/// </summary>
public static class DependencyInjectionConfig
{
    /// <summary>
    /// Container de injeção de dependência a serem utilzizadas no projeto
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection ResolveDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton(configuration);
        services.AddTransient<IConfiguracaoCaminhosService, ConfiguracaoCaminhosService>();
        services.AddTransient<IUnitOfWork, UnitOfWork<PragmaContext>>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddAutoMapper(typeof(IMapper), typeof(Mapper));
        services.AddTransient<IInformationService, InformationService>();
        services.AddTransient<IGenerateService, GenerateService>();
        services.AddTransient<IConfiguracaoConexaoBancoService, ConfiguracaoConexaoBancoService>();

        return services;
    }
}
