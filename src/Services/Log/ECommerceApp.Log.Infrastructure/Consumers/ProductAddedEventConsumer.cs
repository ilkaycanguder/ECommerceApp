using ECommerceApp.Contracts.Events.Product;
using ECommerceApp.Log.Application.DTOs;
using ECommerceApp.Log.Application.Interfaces;
using MassTransit;

namespace ECommerceApp.Log.Infrastructure.Consumers
{
    /// <summary>
    /// Product Added Event Consumer — Observer Pattern implementasyonu.
    /// 
    /// Observer Pattern: Bu sınıf RabbitMQ üzerinden ProductAddedEvent'i dinler.
    /// Product servisi bu consumer'ı tanımaz, sadece event fırlatır.
    /// Loose coupling sağlanmış, servisler birbirinden bağımsızdır.
    /// 
    /// Event-Driven Architecture: MassTransit ile RabbitMQ entegrasyonu yapılmıştır.
    /// Asenkron mesaj işleme ile sistem ölçeklenebilirliği artırılmıştır.
    /// </summary>
    public class ProductAddedEventConsumer : IConsumer<ProductAddedEvent>
    {
        private readonly ILogRepository _logRepository;

        public ProductAddedEventConsumer(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task Consume(ConsumeContext<ProductAddedEvent> context)
        {
            await _logRepository.AddAsync(new CreateLogDto
            {
                Level = "INFO",
                Message = $"Product added: {context.Message.Name} with price {context.Message.Price}",
                Source = "Product.Service"
            });
        }
    }
}
