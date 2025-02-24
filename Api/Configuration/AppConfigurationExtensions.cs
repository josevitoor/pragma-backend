using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Application.Configurations;
/// <summary>
/// Configuração dos arquivos de serilogs
/// </summary>
public static class AppConfigurationExtensions
{
    /// <summary>
    /// Configuração de serilogs
    /// </summary>
    /// <param name="context"></param>
    /// <param name="builder"></param>
    public static void ConfigureSettingsFiles(WebHostBuilderContext context, IConfigurationBuilder builder)
    {
        var env = context.HostingEnvironment;

        var parentFolder = Directory.GetParent(env.ContentRootPath);
        var connectStringsPath = Path.GetFullPath(Path.Combine(parentFolder.ToString(), @"..\_ConnectionString"));

        if (env.IsDevelopment())
        {
            connectStringsPath = Directory.GetCurrentDirectory();
        }

        var connectStringsFilepath = Path.Combine(connectStringsPath, "connectStrings.json");
        builder.SetBasePath(env.ContentRootPath)
            .AddJsonFile(connectStringsFilepath, optional: false, reloadOnChange: true)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables();
    }
}
