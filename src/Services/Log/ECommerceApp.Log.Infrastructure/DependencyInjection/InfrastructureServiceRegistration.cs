using ECommerceApp.Log.Application.Interfaces;
using ECommerceApp.Log.Infrastructure.Consumers;
using ECommerceApp.Log.Infrastructure.Persistence.Context;
using ECommerceApp.Log.Infrastructure.Persistence.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceApp.Log.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // EF Core
            services.AddDbContext<LogDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("LogDb"),
                    b => b.MigrationsAssembly(typeof(LogDbContext).Assembly.FullName)));

            // Repository
            services.AddScoped<ILogRepository, LogRepository>();

            // MassTransit + RabbitMQ
            services.AddMassTransit(x =>
            {
                x.AddConsumer<ProductAddedEventConsumer>();
                x.AddConsumer<ProductUpdatedEventConsumer>();
                x.AddConsumer<UserRegisteredEventConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]!);
                        h.Password(configuration["RabbitMQ:Password"]!);
                    });

                    cfg.ConfigureEndpoints(context);
                });
            });

            return services;
        }
    }
}
