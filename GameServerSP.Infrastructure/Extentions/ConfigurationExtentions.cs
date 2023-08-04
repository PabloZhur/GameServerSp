using GameServerSP.Domain.Exceptions;
using GameServerSP.Domain.Repositories.Base;
using GameServerSP.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GameServerSP.Infrastructure.Extentions;

public static class ConfigurationExtentions
{
    private const string ConnectionStringConfigurationName = "DbConnectionString";

    public static IServiceCollection AddSqliteDb<TDbContext>(this IServiceCollection services,
    IConfiguration configuration) where TDbContext : DbContext
    {
        var connectionString = configuration.GetSection(ConnectionStringConfigurationName).Value;

        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new ConfigurationNotFoundException($"Configuration for {ConnectionStringConfigurationName} has not been found.");
        }

        services.AddDbContext<TDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        services.AddScoped<IUnitOfWork, UnitOfWork<TDbContext>>();

        return services;
    }
}
