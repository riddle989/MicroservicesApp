using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Data.Interfaces;
using Catalog.API.Repositories;
using Catalog.API.Repositories.Interfaces;
using Catalog.API.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Catalog.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            //Get the "CatalogDatabaseSettings" section from appsettings.json and bind it against the "CatalogDatabaseSettings.cs" class
            services.Configure<CatalogDatabaseSettings>(Configuration.GetSection(nameof(CatalogDatabaseSettings)));

            //This specefies that when we require "ICatalogDatabaseSettings" the IOC container will provide us
            // the catalogdatabasesettings, which will configured previously
            services.AddSingleton<ICatalogDatabaseSettings>(sp => {
                return sp.GetRequiredService<IOptions<CatalogDatabaseSettings>>().Value;
            });

            services.AddTransient<ICatalogContext, CatalogContext>();

            services.AddTransient<IProductRepository, ProductRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }
}
