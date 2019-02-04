﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SettingsContracts;
using SettingsContracts.DatabaseModels;
using SettingsProject.Managers;
using SettingsProject.Managers.Interfaces;
using SettingsResources.DatabaseRepositories;
using Swashbuckle.AspNetCore.Swagger;

namespace SettingsProject
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
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
                
            });

            // Dependency Injection
            services.AddSingleton(typeof(IRepository<Grandparent>), typeof(GrandparentRepository<Grandparent>));
            services.AddSingleton(typeof(IRepository<Parent>), typeof(GrandparentRepository<Parent>));
            services.AddSingleton(typeof(IRepository<Child>), typeof(GrandparentRepository<Child>));
            services.AddSingleton(typeof(IRepository<Grandchild>), typeof(GrandparentRepository<Grandchild>));
            services.AddSingleton<IGrandparentManager, GrandparentManager>();
            services.AddSingleton<IParentManager, ParentManager>();
            services.AddSingleton<IChildManager, ChildManager>();
            services.AddSingleton<IGrandchildManager, GrandchildManager>();
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Settings Project V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
