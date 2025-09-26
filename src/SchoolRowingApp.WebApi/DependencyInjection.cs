using Azure.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.OpenApi.Models; // Добавь это
using SchoolRowingApp.Application.Common.Interfaces;
using SchoolRowingApp.Infrastructure.Data;
using SchoolRowingApp.Web.Services;
using System.Reflection;

#if (UseApiOnly)
using System.Text; // Для JWT
using Microsoft.IdentityModel.Tokens; // Для JWT
#endif

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddWebServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddScoped<IUser, CurrentUser>();

        services.AddHttpContextAccessor();

        services.AddHealthChecks()
            //.AddDbContextCheck<ApplicationDbContext>()
            ;

        services.AddExceptionHandler<CustomExceptionHandler>();

        // Убери AddRazorPages() — тебе не нужно
        // services.AddRazorPages();

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        services.AddEndpointsApiExplorer();

        // 🔥 Замени AddOpenApiDocument на AddSwaggerGen
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "SchoolRowingApp API", Version = "v1" });
            // Получаем путь к XML
            // ранее   xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            // документация на дто генерируется в проекте Application
            // а {Assembly.GetExecutingAssembly().GetName().Name}.xml даёт SchoolRowingApp.WebApi.xml
            // прибъём гвоздём путь к файлу $"SchoolRowingApp.Application.xml"; 

            var xmlFile = $"SchoolRowingApp.Application.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

            // Добавляем документацию в Swagger
            options.IncludeXmlComments(xmlPath);

            // Включаем аннотации для отображения комментариев к свойствам
            options.EnableAnnotations();

#if (UseApiOnly)
            // Добавляем JWT Bearer авторизацию
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter token: Bearer {token}",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
#endif
        });

        return services;
    }


}