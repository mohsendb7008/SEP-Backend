using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SEP_Backend.User;

namespace SEP_Backend;

public static class AppBuilderBootstrap
{
    public static void Run(WebApplicationBuilder builder)
    {
        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        builder.Host.ConfigureContainer<ContainerBuilder>(c => c.RegisterModule<AppModule>());
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        AddAuthentication(builder);
        AddAuthorization(builder);
        AddSwagger(builder);
        AddCors(builder);
    }

    private static void AddAuthentication(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(authOptions =>
            {
                authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOptions =>
            {
                var jwtConfig = new JwtConfig(builder.Configuration);
                jwtOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtConfig.GetIssuer(),
                    ValidAudience = jwtConfig.GetAudience(),
                    IssuerSigningKey = jwtConfig.GetSigningKey()
                };
            });
    }

    private static void AddAuthorization(WebApplicationBuilder builder)
    {
        builder.Services.AddAuthorizationBuilder()
            .AddPolicy("CSO", policy =>
                policy.RequireRole("CustomerServiceOfficer", "SeniorCustomerServiceOfficer", "FinancialManager")
            )
            .AddPolicy("Task", policy =>
                policy.RequireRole("ProductionManager", "ServiceManager")
            )
            .AddPolicy("Admin", policy =>
                policy.RequireRole("AdministrationManager", "FinancialManager", "ProductionManager", "ServiceManager",
                    "MarketingManager", "VicePresident")
            );
    }

    private static void AddSwagger(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(swaggerOptions =>
        {
            swaggerOptions.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header
            });
            swaggerOptions.AddSecurityRequirement(new OpenApiSecurityRequirement
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
                    []
                }
            });
        });
    }

    private static void AddCors(WebApplicationBuilder builder)
    {
        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}