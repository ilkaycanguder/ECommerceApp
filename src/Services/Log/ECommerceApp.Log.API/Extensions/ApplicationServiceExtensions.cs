using ECommerceApp.Log.Application.Behaviors;
using FluentValidation;
using MediatR;

namespace ECommerceApp.Log.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(ECommerceApp.Log.Application.Features.Logs.Commands.CreateLog.CreateLogCommand).Assembly));

            services.AddValidatorsFromAssembly(
                typeof(ECommerceApp.Log.Application.Features.Logs.Commands.CreateLog.CreateLogCommandValidator).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            return services;
        }
    }
}
