using ECommerceApp.Auth.Application.Behaviors;
using ECommerceApp.Auth.Application.Features.Auth.Commands.Login;
using ECommerceApp.Auth.Application.Features.Auth.Commands.Register;
using ECommerceApp.Auth.Application.Mappings;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace ECommerceApp.Auth.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(LoginCommand).Assembly));

            services.AddValidatorsFromAssembly(
                typeof(RegisterCommandValidator).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<AuthMappingProfile>();
            });

            return services;
        }
    }
}
