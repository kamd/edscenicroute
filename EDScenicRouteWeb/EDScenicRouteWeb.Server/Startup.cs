// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;
using AspNetCoreRateLimit;
using EDScenicRouteWeb.Server.Data;
using EDScenicRouteWeb.Server.Data.DataUpdates.Database;
using EDScenicRouteWeb.Server.Repositories;
using EDScenicRouteWeb.Server.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace EDScenicRouteWeb.Server
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            // needed to store rate limit counters and ip rules
            services.AddMemoryCache();

            //load general configuration from appsettings.json
            services.Configure<IpRateLimitOptions>(Configuration.GetSection("IpRateLimiting"));

            //load ip rules from appsettings.json
            services.Configure<IpRateLimitPolicies>(Configuration.GetSection("IpRateLimitPolicies"));
            

            // inject counter and rules stores
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });

            services.AddScoped<IGalaxyService, GalaxyService>();
            services.AddScoped<IGalaxyRepository, DatabaseGalaxyRepository>();
            services.AddDbContext<GalacticSystemContext>(options => options.UseNpgsql(new Npgsql.NpgsqlConnectionStringBuilder()
            {
                Host = Environment.GetEnvironmentVariable("DBHOST"),
                Port = int.Parse(Environment.GetEnvironmentVariable("DBPORT")),
                Database = Environment.GetEnvironmentVariable("DBDATABASE"),
                Username = Environment.GetEnvironmentVariable("DBUSER"),
                Password = Environment.GetEnvironmentVariable("DBPASS"),
                Timeout = int.Parse(Environment.GetEnvironmentVariable("DBTIMEOUT"))
            }.ToString()));
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration => { configuration.RootPath = "edclientapp"; });
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
           // app.UseIpRateLimiting();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc(routes =>
            {
                routes.MapRoute(name: "default", template: "{controller}/{action}/{id?}");
            });

            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "edclientapp";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });

            var context = app.ApplicationServices.GetRequiredService<GalacticSystemContext>();
            context.Database.Migrate();

            var logger = app.ApplicationServices.GetRequiredService<ILogger<Startup>>();
            var config = app.ApplicationServices.GetRequiredService<IConfiguration>();
            ConsumePoisFromJsonFile(context, logger, config);
        }

        private void ConsumePoisFromJsonFile(GalacticSystemContext context, ILogger logger, IConfiguration config)
        {
            const string configKey = "POIUpdateDirectory";
            var updateDir = config.GetSection("GalaxyUpdates")[configKey];
            if (!Directory.Exists(updateDir))
            {
                logger.Log(LogLevel.Information, $"POI folder {updateDir} did not exist, skipping import.");
                return;
            }
            if (!string.IsNullOrEmpty(updateDir))
            {
                logger.Log(LogLevel.Information, $"Checking {configKey} {updateDir}...");
                foreach (var jsonFile in Directory.GetFiles(updateDir, "*.json"))
                {
                    DatabaseUpdater.UpdatePOIsFromFile(context, jsonFile, logger);
                    File.Delete(jsonFile);
                }
            }
        }
    }
}
