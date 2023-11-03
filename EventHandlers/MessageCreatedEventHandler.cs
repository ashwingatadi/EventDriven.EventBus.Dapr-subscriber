using EventDriven.EventBus.Abstractions;

namespace subscriber.EventHandlers
{
    public class MessageCreatedEventHandler : IntegrationEventHandler<MessageCreatedEvent>
    {
        private readonly ILogger<MessageCreatedEventHandler> logger;

        public MessageCreatedEventHandler(ILogger<MessageCreatedEventHandler> logger)
        {
            this.logger = logger;
        }

        public override async Task HandleAsync(MessageCreatedEvent @event)
        {
            this.logger.LogInformation("Received at subscriber " + @event.message);
        }
    }
}
