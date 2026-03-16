using ECommerceApp.Contracts.Events.Auth;
using ECommerceApp.Log.Application.DTOs;
using ECommerceApp.Log.Application.Interfaces;
using MassTransit;

namespace ECommerceApp.Log.Infrastructure.Consumers
{
    public class UserRegisteredEventConsumer : IConsumer<UserRegisteredEvent>
    {
        private readonly ILogRepository _logRepository;

        public UserRegisteredEventConsumer(ILogRepository logRepository)
        {
            _logRepository = logRepository;
        }

        public async Task Consume(ConsumeContext<UserRegisteredEvent> context)
        {
            await _logRepository.AddAsync(new CreateLogDto
            {
                Level = "INFO",
                Message = $"User registered: {context.Message.FullName} with email {context.Message.Email}",
                Source = "Auth.Service"
            });
        }
    }
}
