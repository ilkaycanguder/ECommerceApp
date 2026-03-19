using FluentValidation;
using MediatR;

namespace ECommerceApp.Product.Application.Behaviors
{
    /// <summary>
    /// Validation Pipeline Behavior — Decorator Pattern implementasyonu.
    /// 
    /// Pipeline Behavior: MediatR pipeline'ına eklenen bu behavior,
    /// her Command veya Query handler'dan ÖNCE otomatik olarak çalışır.
    /// 
    /// Open/Closed Principle: Handler'lar değiştirilmeden yeni validation
    /// kuralları eklenebilir. Her Command için ayrı Validator sınıfı yazılır.
    /// 
    /// Single Responsibility: Validation sorumluluğu handler'lardan alınmış,
    /// bu sınıfa verilmiştir. Her sınıf tek bir sorumluluğa sahiptir.
    /// </summary>
    public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
      where TRequest : IRequest<TResponse>
    {
        private readonly IEnumerable<IValidator<TRequest>> _validators;

        public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
        {
            _validators = validators;
        }

        public async Task<TResponse> Handle(
            TRequest request,
            RequestHandlerDelegate<TResponse> next,
            CancellationToken cancellationToken)
        {
            if (!_validators.Any())
                return await next();

            var context = new ValidationContext<TRequest>(request);

            var errors = _validators
                .Select(v => v.Validate(context))
                .SelectMany(r => r.Errors)
                .Where(f => f is not null)
                .ToList();

            if (errors.Count != 0)
                throw new ValidationException(errors);

            return await next();
        }
    }
}
