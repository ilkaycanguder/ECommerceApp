using ECommerceApp.Product.Application.Interfaces;
using ECommerceApp.Product.Domain.Interfaces;
using ECommerceApp.Product.Infrastructure.Persistence.Context;
using ECommerceApp.Product.Infrastructure.Persistence.Repositories;
using ECommerceApp.Product.Infrastructure.Services.Cache;
using ECommerceApp.Product.Infrastructure.Services.Messaging;
using ECommerceApp.Shared.Abstractions;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace ECommerceApp.Product.Infrastructure.DependencyInjection
{
    public static class InfrastructureServiceRegistration
    {
        public static IServiceCollection AddInfrastructureServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // EF Core
            services.AddDbContext<ProductDbContext>(options =>
                options.UseSqlServer(
                    configuration.GetConnectionString("ProductDb"),
                    b => b.MigrationsAssembly(typeof(ProductDbContext).Assembly.FullName)));

            // Redis
            services.AddSingleton<IConnectionMultiplexer>(sp =>
                ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")!));

            services.AddScoped<ICacheService, RedisCacheService>();

            // Repositories
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // MassTransit + RabbitMQ
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]!);
                        h.Password(configuration["RabbitMQ:Password"]!);
                    });
                });
            });

            services.AddScoped<IEventPublisher, EventPublisher>();

            return services;
        }
    }
}
