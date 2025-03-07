﻿using Contoso.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace Contoso.API.Infrastructure;

public static class HostingExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration config)
    {
        services
            .AddControllers();

        services
            // Api routing
            .AddEndpointsApiExplorer()
            // Enable Cors
            .AddCors(options => options
                .AddDefaultPolicy(policy => policy
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowAnyOrigin()
                )
            )
            .AddSwaggerGen();

        services.AddContosoEntities(dbOptionsBuilder =>
        {
            var dir = new DirectoryInfo(Path.Combine(".\\", "../data")).FullName;
            dbOptionsBuilder
                .UseSqlite($"DataSource={dir}\\contoso.db", sqliteBuilder => sqliteBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery))
                .EnableDetailedErrors()
                .EnableSensitiveDataLogging();
        });

        return services;
    }

    public static WebApplication ConfigureApp(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseHttpsRedirection();

        app.UseCors();
        app.MapControllers();

        return app;
    }
}