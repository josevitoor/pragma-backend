using Application.AutoMapper;
using Application.Configuration;
using Application.Configurations;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using TCE.Base;
using TCE.Base.Dapper;
using Prometheus;

namespace Application;
/// <summary>
/// Inicalização e setup
/// </summary>
public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IHostEnvironment env)
    {
        _configuration = ConfigureTce.BuildConfiguration(env);

        ConfigureMap.Configure();

    }

    /// <summary>
    /// Configuração geral dos serviços e recursos utilizados na aplicação
    /// </summary>
    /// <param name="services"></param>
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddApiVersioning(o =>
        {
            o.ReportApiVersions = true;
            o.AssumeDefaultVersionWhenUnspecified = true;
        });

        services.Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

        services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

        IMapper mapper = ConfigureMap.Configure();

        services.AddSingleton(mapper);

        services.AddResponseCompression(options =>
        {
            options.Providers.Add<BrotliCompressionProvider>();
            options.Providers.Add<GzipCompressionProvider>();
            options.MimeTypes =
                ResponseCompressionDefaults.MimeTypes.Concat(
                    new[] { "image/svg+xml" });
            options.EnableForHttps = true;
        });

        services.AddCors(o => o.AddPolicy(name: "AllowAll", builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        }));

        services.ConfigureContext(_configuration);

        services.ConfigureDapper(_configuration);

        services.ResolveDependencies(_configuration);

        ConfigureInfra.Configurar(services, _configuration);

        DapperServiceCollection.AddDapper(services, options => options.ConnectionString = _configuration["ConnectionStrings:BdPragmaTCE_uPragmaTCE_Config"]);


        services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
            options.JsonSerializerOptions.PropertyNamingPolicy = null;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });

        services.AddControllers().AddNewtonsoftJson(options =>
          options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
        );

        services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });


        services.AddRazorPages();

        services.ResolveHttpClientConfigurations(_configuration);

    }

    /// <summary>
    /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    /// </summary>
    /// <param name="app"></param>
    /// <param name="env"></param>
    /// <param name="loggerFactory"></param>
    /// <param name="configuration"></param>
    public void Configure(
        IApplicationBuilder app,
        IWebHostEnvironment env,
        ILoggerFactory loggerFactory,
        IConfiguration configuration)
    {
        var path = Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), "Upload"));

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        app.UseStaticFiles(new StaticFileOptions()
        {
            FileProvider = new PhysicalFileProvider(path),
            RequestPath = new PathString("/Upload")
        });

        app.UseResponseCompression();

        app.UseHttpsRedirection();

        app.UseMetricServer();
        app.UseHttpMetrics();

        AppConfiguration.ConfigureApp(app, env, loggerFactory, _configuration, "SingleVersion");

    }
}
