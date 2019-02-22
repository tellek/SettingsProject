﻿using System;
using System.IO;
using System.Net;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using SettingsContracts;
using SettingsContracts.DatabaseModels;
using SettingsProject.Extensions;
using SettingsProject.Managers;
using SettingsResources.DatabaseRepositories;
using Swashbuckle.AspNetCore.Swagger;

namespace SettingsProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
            Configuration = configuration;
            StaticProps.DbConnectionString = configuration["ConnectionStrings:PostgresDb"];
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMemoryCache();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "Topher's Settings Project",
                    Description = "A simple example ASP.NET Core Web API"
                });
                //Locate the XML file being generated by ASP.NET...
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

                //... and tell Swagger to use those XML comments.
                c.IncludeXmlComments(xmlPath);
            });
            
            // Dependency Injection
            services.AddSingleton(typeof(IDbRepository<Grandparent>), typeof(DbRepository<Grandparent>));
            services.AddSingleton(typeof(IDbRepository<Parent>), typeof(DbRepository<Parent>));
            services.AddSingleton(typeof(IDbRepository<Child>), typeof(DbRepository<Child>));
            services.AddSingleton(typeof(IDbRepository<Grandchild>), typeof(DbRepository<Grandchild>));
            services.AddSingleton(typeof(IDbRepository<UserAuth>), typeof(DbRepository<UserAuth>));
            services.AddSingleton<IAuthManager, AuthManager>();
            services.AddSingleton(typeof(ISettingsManager<Grandparent>), typeof(SettingsManager<Grandparent>));
            services.AddSingleton(typeof(ISettingsManager<Parent>), typeof(SettingsManager<Parent>));
            services.AddSingleton(typeof(ISettingsManager<Child>), typeof(SettingsManager<Child>));
            services.AddSingleton(typeof(ISettingsManager<Grandchild>), typeof(SettingsManager<Grandchild>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMiddleware<ExceptionMiddleware>();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Settings Project V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseHttpsRedirection();
            app.UseMvc();

            Log.Information("API configured, running, and ready for use!");
        }
    }
}
