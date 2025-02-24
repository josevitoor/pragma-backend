using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Net.Http.Headers;

namespace Application.Configuration;
/// <summary>
/// class estática para a configuração de clientes http
/// </summary>
public static class HttpClientConfiguration
{
    /// <summary>
    /// Realiza o setup das configurações de HttpClient
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection ResolveHttpClientConfigurations(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddHttpClient("TCEAPICore", client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("TCEAPICore:BaseURL"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", configuration.GetValue<string>("TCEAPICore:Token"));
        });

        services.AddHttpClient("APIProcessos", client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("APIProcessos:BaseURL"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", configuration.GetValue<string>("APIProcessos:Token"));
        });

        services.AddHttpClient("Robocop", client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("RoboCop:BaseURL"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", configuration.GetValue<string>("RoboCop:Token"));
        });

        services.AddHttpClient("TCEApi", client =>
        {
            var httpContextAccessor = services.BuildServiceProvider().GetService<IHttpContextAccessor>();

            StringValues _tokenInfo = "";

            if (httpContextAccessor.HttpContext != null)
            {
                httpContextAccessor.HttpContext.Request.Headers.TryGetValue("Authorization", out _tokenInfo);
            }

            if (_tokenInfo != "") client.DefaultRequestHeaders.Add("Authorization", _tokenInfo.ToString());

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var baseURL = configuration.GetSection("TCEApi:BaseURL").Value;
            client.BaseAddress = new Uri(baseURL);
        });

        services.AddHttpClient("AssinadorAPI", client =>
        {
            client.BaseAddress = new Uri(configuration.GetValue<string>("AssinadorAPI:BaseURL"));
        });

        return services;
    }
}
