using FluentValidation;
using GameServerSP.Application.Application;
using GameServerSP.Application.Extentions;
using GameServerSP.Application.Options;
using GameServerSP.Application.Services.AuthValidation;
using GameServerSP.Application.Services.WebSocketManagement.Connections;
using GameServerSP.Application.Services.WebSocketManagement.Handlers;
using GameServerSP.Domain.Repositories;
using GameServerSP.Infrastructure.Data;
using GameServerSP.Infrastructure.Extentions;
using GameServerSP.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System.Reflection;
using WebSocketServer;

namespace GameServerSP.Application
{

    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IConnectionManager, ConnectionManager>();
            services.AddScoped<IWebSocketHandler, WebSocketHandler>();

            services.AddTransient<IDeviceRepository, DeviceRepository>();
            services.AddScoped<IPlayerRepository, PlayerRepository>();
            services.AddScoped<IAuthenticationValidationService, AuthenticationValidationService>();
            services.AddScoped(typeof(IValidationService<,>), typeof(ValidationService<,>));

            services.Configure<WebSocketConnectionOptions>(_configuration.GetSection(nameof(WebSocketConnectionOptions)));
            services.AddSqliteDb<GameServerContext>(_configuration);
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
            services.AddValidatorsFromAssemblyContaining<Startup>();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(_configuration)
                .CreateLogger();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsDevelopment())
            {
                using (var scope = app.ApplicationServices.CreateScope())
                {
                    var db = scope.ServiceProvider.GetRequiredService<GameServerContext>();

                    if (db.Database.ProviderName != "Microsoft.EntityFrameworkCore.InMemory")
                    {
                        db.Database.Migrate();
                    }
                }
            }

            app.UseWebSockets();
            app.MapWebSocketConnections("/gameServer");
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("-- GameServerSP --");
            });
        }
    }
}
