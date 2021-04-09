using Chia.NET.Clients;
using ChiaPool.Authentication;
using ChiaPool.Configuration;
using ChiaPool.Hubs;
using ChiaPool.Models;
using Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Reflection;

namespace ChiaPool
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
            services.AddApplication(Configuration, Assembly.GetAssembly(typeof(WalletClient)));

            services.AddDbContext<MinerContext>((provider, options) =>
            {
                var dbOptions = provider.GetRequiredService<DatabaseOption>();
                string connectionString = dbOptions.ConnectionString;
                options.UseNpgsql(connectionString);
            });

            services.AddAuthentication()
                .AddScheme<MinerAuthenticationOptions, MinerAuthenticationHandler>("Miner", null)
                .AddScheme<PlotterAuthenticationOptions, PlotterAuthenticationHandler>("Plotter", null);

            services.AddSignalR()
                .AddJsonProtocol();
            services.AddSingleton<PlotterHub>();
            services.AddSingleton<MinerHub>();
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

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<PlotterHub>("/PHub");
                endpoints.MapHub<MinerHub>("/MHub");
                endpoints.MapControllers();
            });
        }
    }
}
