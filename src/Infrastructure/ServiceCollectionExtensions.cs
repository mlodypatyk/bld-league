using BldLeague.Application.Abstractions.Repositories;
using BldLeague.Application.Common;
using BldLeague.Infrastructure.Context;
using BldLeague.Infrastructure.HostedServices;
using BldLeague.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BldLeague.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBldLeagueInfrastructure(this IServiceCollection services,
        string connectionString, IConfiguration configuration)
    {
        services.AddDbContextFactory<AppDbContext>(
            options => options.UseNpgsql(connectionString, b => b.MigrationsHistoryTable("__ef_migrations_history")).UseSnakeCaseNamingConvention());

        services.AddScoped<IUnitOfWork, IUnitOfWork>(
            provider => new UnitOfWork(provider.GetRequiredService<IDbContextFactory<AppDbContext>>()));

        services.Configure<RoundFinalizationOptions>(configuration.GetSection(RoundFinalizationOptions.SectionName));
        services.AddSingleton<RoundClock>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RoundFinalizationOptions>>().Value;
            TimeZoneInfo tz;
            try
            {
                tz = TimeZoneInfo.FindSystemTimeZoneById(options.TimeZoneId);
            }
            catch (TimeZoneNotFoundException)
            {
                tz = TimeZoneInfo.Utc;
            }
            return new RoundClock(tz);
        });
        services.AddHostedService<RoundStandingsRefreshBackgroundService>();

        return services;
    }
}
