using Chia.NET.Clients;
using ChiaMiningManager.Configuration;
using ChiaMiningManager.Models;
using ChiaMiningManager.Services;
using Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Net.Http;
using System.Reflection;

namespace ChiaMiningManager
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
            services.AddApplication(Configuration, Assembly.GetExecutingAssembly());
            services.AddSingleton<HttpClient>();
            services.AddSingleton<HarvesterClient>();
            services.AddScoped<PlotManager>();

            services.AddDbContext<ConfigurationContext>((provider, options) =>
            {
                var authOptions = provider.GetRequiredService<AuthOption>();
                options.UseSqlite(@$"Data Source=data.db");
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        { 
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
