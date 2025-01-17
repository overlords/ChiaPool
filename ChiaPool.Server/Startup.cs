using Chia.NET.Clients;
using ChiaPool.Authentication;
using ChiaPool.Configuration;
using ChiaPool.Extensions;
using ChiaPool.Hubs;
using ChiaPool.Models;
using Common.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
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

            services.AddAuthentication("Miner")
                .AddScheme<MinerAuthenticationOptions, MinerAuthenticationHandler>("Miner", null)
                .AddScheme<PlotterAuthenticationOptions, PlotterAuthenticationHandler>("Plotter", null)
                .AddScheme<BasicAuthOptions, BasicAuthHandler>("Basic", null);

            services.AddSignalR()
                .AddJsonProtocol();

            services.AddControllers();

            if (Configuration.IsSwaggerEnabled())
            {
                services.AddSwaggerGen(c =>
                {
                    var apiInfo = new OpenApiInfo()
                    {
                        Title = "ChiaPool",
                        Description = "The web api of the crypto mining pool software ChiaPool",
                        Version = "v0.9",
                    };

                    c.SwaggerDoc("v0.9", apiInfo);
                });
            }

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (Configuration.IsSwaggerEnabled())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v0.9/swagger.json", "ChiaPool API v0.9");
                });
            }

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<PlotterHub>("/PHub", x => x.Transports = HttpTransportType.WebSockets);
                endpoints.MapHub<MinerHub>("/MHub", x => x.Transports = HttpTransportType.WebSockets);
                endpoints.MapControllers();
            });
        }
    }
}
