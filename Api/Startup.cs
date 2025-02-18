using Application.AutoMapper;
using Application.Configuration;
using Application.Configurations;
using AutoMapper;
//utilizacao do audit
using Audit.Core;
using Audit.EntityFramework;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Routing;
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

using AppContext = Domain.AutomationContext;
using Configuration = Audit.Core.Configuration;
using System;

namespace Application
{
    /// <summary>
    /// Inicalização e setup
    /// </summary>
    public class Startup
    {
        private readonly IConfiguration _configuration;

        private string nomeAplicacao;

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
            this.nomeAplicacao = _configuration.GetSection("ApplicationInfo:Name").ToString();

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

            // Audit.Core.Configuration
            //     .Setup()
            //     .UseSqlServer(options =>
            //         options
            //             .ConnectionString(_configuration["ConnectionStrings:BdAudit_uBdAudit_config"])
            //             .TableName("SisPFA_Audit")
            //             .IdColumnName("IdAuditJson")
            //             .CustomColumn("NomeTabela", auditEvent => auditEvent.GetEntityFrameworkEvent().Entries.FirstOrDefault()?.Table)
            //             .CustomColumn("Dados", auditEvent => auditEvent.ToJson()));

            //  Audit.Core.Configuration.AuditDisabled = Convert.ToBoolean(_configuration.GetSection("ApplicationInfo:AuditDisabled").Value);

            /*           Audit.Core.Configuration
                           .Setup()
                           .UseSqlServer(options =>
                               options
                                   .ConnectionString(_configuration["ConnectionStrings:BdcAudit_uBDC_Config"])
                                   .TableName("BDEscola_Audit")
                                   .IdColumnName("IdAuditJson")
                                   .CustomColumn("NomeTabela", auditEvent => auditEvent.GetEntityFrameworkEvent().Entries.FirstOrDefault()?.Table)
                                   .CustomColumn("Dados", auditEvent => auditEvent.ToJson()));

                       Audit.Core.Configuration.AuditDisabled = Convert.ToBoolean(_configuration.GetSection("ApplicationInfo:AuditDisabled").Value);
           */
            services.ConfigureDapper(_configuration);

            services.ResolveDependencies(_configuration);

            ConfigureInfra.Configurar(services, _configuration);

            DapperServiceCollection.AddDapper(services, options => options.ConnectionString = _configuration["ConnectionStrings:BdPlanoFiscalizacaoAnual_uPfa_Config"]);


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

            // Use o middleware Prometheus
            app.UseMetricServer();
            app.UseHttpMetrics();

            AppConfiguration.ConfigureApp(app, env, loggerFactory, _configuration, "SingleVersion");

        }
    }
}
