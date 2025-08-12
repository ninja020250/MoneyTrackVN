using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MoneyTrack.Api.Filters;
using MoneyTrack.Api.Middleware;
using MoneyTrack.Application;
using MoneyTrack.Infrastructure;
using MoneyTrack.Persistence;
using Scalar.AspNetCore;

namespace MoneyTrack.Api;

public static class StartupExtensions
{
    public static WebApplication ConfigurationService(this WebApplicationBuilder builder)
    {
        // if (!builder.Environment.IsDevelopment())
        // {
        //     builder.Configuration
        //         .AddEnvironmentVariables();
        // }

        builder.Configuration.AddEnvironmentVariables();
        builder.Services.AddApplicationServices();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddPersistenceServices(builder.Configuration);
        builder.Services.AddControllers();

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]!)),
                    ValidateIssuer = false,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
                    ValidateAudience = false,
                    ValidAudience = builder.Configuration["JwtSettings:Audience"],
                };
            });

        // Add OpenAPI services
        builder.Services.AddOpenApi();
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter your JWT token below (no 'Bearer' prefix needed)"
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
        });

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin",
                policy => policy.AllowAnyOrigin() // Allow frontend
                    .AllowAnyMethod()
                    .AllowAnyHeader());
        });

        builder.Services.Configure<Dictionary<string, int>>(
            builder.Configuration.GetSection("ApiLimits"));
        builder.Services.AddScoped<ApiUsageLimitFilter>();
        // builder.Services.AddControllers(options =>
        // {
        //     options.Filters.Add<ApiUsageLimitFilter>();
        // }); -> uncommment if want to check api usage for all controller.
        return builder.Build();
    }

    public static WebApplication ConfigurationPipeline(this WebApplication app)
    {
        app.UseCors();
        app.UseHttpsRedirection();

        // Add Authentication and Authorization middleware
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        app.UseCustomExceptionHandler();

        // // Configure the HTTP request pipeline.
        // if (app.Environment.IsDevelopment())
        // {
        //     app.MapScalarApiReference();
        //     app.MapOpenApi();
        //     app.UseSwagger();
        //     app.UseSwaggerUI();
        // }
        if (!app.Environment.IsDevelopment())
        {
            var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
            app.Urls.Add($"http://0.0.0.0:{port}");
        }


        app.MapOpenApi();
        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }

    public static async Task ResetDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        try
        {
            var context = scope.ServiceProvider.GetService<MoneyTrackDbContext>();
            if (context != null)
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.MigrateAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            // TODO: add logging later on
        }
    }
}