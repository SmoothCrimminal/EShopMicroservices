using BuildingBlocks.Messaging.Events;
using MassTransit;
using Ordering.Application.Orders.Commands.CreateOrder;

namespace Ordering.Application.Orders.EventHandlers.Integration
{
    public class BasketCheckoutEventHandler : IConsumer<BasketCheckoutEvent>
    {
        private readonly ILogger<BasketCheckoutEventHandler> _logger;
        private readonly ISender _sender;

        public BasketCheckoutEventHandler(ILogger<BasketCheckoutEventHandler> logger, ISender sender)
        {
            _logger = logger;
            _sender = sender;
        }

        public async Task Consume(ConsumeContext<BasketCheckoutEvent> context)
        {
            _logger.LogInformation("Integration Event handled: {IntegrationEvent}", context.Message.GetType().Name);

            var command = MapToCreateOrderCommand(context.Message);
            await _sender.Send(command);
        }

        private CreateOrderCommand MapToCreateOrderCommand(BasketCheckoutEvent message)
        {
            var addressDto = new AddressDto(message.FirstName, message.LastName, message.EmailAddress, message.AddressLine, message.Country, message.State, message.ZipCode);
            var paymentDto = new PaymentDto(message.CardName, message.CardNumber, message.Expiration, message.CVV, message.PaymentMethod);
            var orderId = Guid.NewGuid();

            var orderDto = new OrderDto(
                orderId,
                message.CustomerId,
                message.UserName,
                addressDto,
                addressDto,
                paymentDto,
                Ordering.Domain.Enums.OrderStatus.Pending,
                [new OrderItemDto(orderId, new Guid("d9688ba6-15b0-4296-ae23-ef9081e91610"), 2, 500), new OrderItemDto(orderId, new Guid("110403e0-1c1e-45df-968e-7500c3bce0c6"), 1, 400)]);

            return new CreateOrderCommand(orderDto);
        }
    }
}
