using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using SchoolRowingApp.Application.Common.Interfaces;
using SchoolRowingApp.Domain.Athletes;
using SchoolRowingApp.Domain.Constants;
using SchoolRowingApp.Domain.Payments;
using SchoolRowingApp.Domain.SharedKernel;
using SchoolRowingApp.Infrastructure.Data;
using SchoolRowingApp.Infrastructure.Data.Interceptors;
using SchoolRowingApp.Infrastructure.Identity;
using SchoolRowingApp.Infrastructure.Repositories;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        Guard.Against.Null(connectionString, message: "Connection string 'DefaultConnection' not found.");

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());

#if (UseSQLite)
            options.UseSqlite(connectionString);
#else
            //options.UseSqlServer(connectionString);
            options.UseNpgsql(
                                connectionString,
                                npgsqlOptions =>
                                {
                                    npgsqlOptions.EnableRetryOnFailure(); // Автоматические повторы при разрывах
                                    npgsqlOptions.CommandTimeout(30);      // Таймаут 30 секунд
                                }
                              );
#endif
        });
        services.AddScoped<IAthleteRepository, AthleteRepository>();
        services.AddScoped<IPayerRepository, PayerRepository>();
        services.AddScoped<IAthletePayerRepository, AthletePayerRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();


        services.AddScoped<ApplicationDbContextInitialiser>();

#if (UseApiOnly)
        services.AddAuthentication()
            .AddBearerToken(IdentityConstants.BearerScheme);

        services.AddAuthorizationBuilder();

        services
            .AddIdentityCore<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddApiEndpoints();
#else
        services
            .AddDefaultIdentity<ApplicationUser>()
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
#endif

        services.AddSingleton(TimeProvider.System);
        services.AddTransient<IIdentityService, IdentityService>();

        services.AddAuthorization(options =>
            options.AddPolicy(Policies.CanPurge, policy => policy.RequireRole(Roles.Administrator)));

        return services;
    }
}
