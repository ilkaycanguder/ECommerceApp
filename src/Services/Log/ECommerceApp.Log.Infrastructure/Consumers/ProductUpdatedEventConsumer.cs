using ECommerceApp.Contracts.Events.Product;
using ECommerceApp.Log.Application.DTOs;
using ECommerceApp.Log.Application.Interfaces;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApp.Log.Infrastructure.Consumers
{
    public class ProductUpdatedEventConsumer : IConsumer<ProductUpdatedEvent>
    {
        private readonly ILogRepository _logRepository;

        public ProductUpdatedEventConsumer(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task Consume(ConsumeContext<ProductUpdatedEvent> context)
        {
            await _logRepository.AddAsync(new CreateLogDto
            {
                Level = "INFO",
                Message = $"Product updated: {context.Message.Name} with price {context.Message.Price}",
                Source = "Product.Service"
            });
        }
    }
}
