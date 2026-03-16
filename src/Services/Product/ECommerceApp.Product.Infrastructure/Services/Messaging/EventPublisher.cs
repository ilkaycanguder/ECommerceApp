using ECommerceApp.Product.Application.Interfaces;
using MassTransit;

namespace ECommerceApp.Product.Infrastructure.Services.Messaging
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public EventPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default) where T : class
            => await _publishEndpoint.Publish(@event, cancellationToken);
    }
}
