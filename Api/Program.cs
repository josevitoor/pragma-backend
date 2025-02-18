using Application.Configurations;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Serilog;
using System;
using TCE.Base.Logger;

namespace Application
{
    /// <summary>
    /// Classe principal de execução da aplicação
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Método main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            if (args is not null)
            {
                try
                {
                    Log.Logger = new LoggerConfiguration().Enrich.FromLogContext().WriteTo.Console().CreateLogger();
                    Log.Information("Inicializando API");
                    BuildWebHost(args).Run();
                }
                catch (Exception ex)
                {
                    Log.Fatal(ex, "Aplicação terminou de forma inesperada.");
                }
                finally
                {
                    Log.CloseAndFlush();
                }
            }

        }

        /// <summary>
        /// Builda a aplicação
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args) =>
          WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(AppConfigurationExtensions.ConfigureSettingsFiles)
                .UseSerilog(AppLoggerExtensions.ConfigureSerilogLoggers)
                .UseStartup<Startup>()
                .Build();

    }
}
