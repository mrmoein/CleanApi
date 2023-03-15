using CleanApi.API.Filters;
using CleanApi.API.Services;
using CleanApi.Application.Common.Interfaces;
using CleanApi.Infrastructure.Persistence;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace CleanApi.API;

public static class ConfigureServices
{
    public static IServiceCollection AddApiServices(this IServiceCollection services)
    {
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddSingleton<ICurrentUserService, CurrentUserService>();

        services.AddHttpContextAccessor();

        // services.Configure<RouteOptions>(options =>
        // {
        //     // options.LowercaseUrls = true; 
        //     options.LowercaseQueryStrings = true;
        // });

        services.AddHealthChecks()
            .AddDbContextCheck<ApplicationDbContext>();

        services.AddControllers(options =>
            options.Filters.Add<ApiExceptionFilterAttribute>());

        services.AddFluentValidationClientsideAdapters();

        // services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CleanApi API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string>()
                    }
                });

                string swaggerPath = Path.Combine(AppContext.BaseDirectory, "CleanApi.API.xml");
                c.IncludeXmlComments(swaggerPath);
            }
        );

        // Customise default API behaviour
        services.Configure<ApiBehaviorOptions>(options =>
            options.SuppressModelStateInvalidFilter = true);

        return services;
    }
}