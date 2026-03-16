using ECommerceApp.Product.Application.Behaviors;
using ECommerceApp.Product.Application.Mappings;
using FluentValidation;
using MediatR;

namespace ECommerceApp.Product.API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(
                    typeof(ECommerceApp.Product.Application.Features.Products.Commands.CreateProduct.CreateProductCommand).Assembly));

            services.AddValidatorsFromAssembly(
                typeof(ECommerceApp.Product.Application.Features.Products.Commands.CreateProduct.CreateProductCommandValidator).Assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<ProductMappingProfile>();
            });

            return services;
        }
    }
}
