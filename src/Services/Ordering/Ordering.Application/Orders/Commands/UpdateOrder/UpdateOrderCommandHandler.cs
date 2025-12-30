namespace Ordering.Application.Orders.Commands.UpdateOrder
{
    public class UpdateOrderCommandHandler : ICommandHandler<UpdateOrderCommand, UpdateOrderResult>
    {
        private readonly IApplicationDbContext _dbContext;

        public UpdateOrderCommandHandler(IApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UpdateOrderResult> Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
        {
            var orderId = OrderId.Of(command.Order.Id);
            var order = await _dbContext.Orders.FindAsync([orderId], cancellationToken);
            if (order is null)
                throw new OrderNotFoundException(command.Order.Id);

            UpdateOrderWithNewValues(order, command.Order);

            _dbContext.Orders.Update(order);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return new UpdateOrderResult(true);
        }

        private void UpdateOrderWithNewValues(Order order, OrderDto orderDto)
        {
            var shippingAddress = Address.Of(orderDto.ShippingAddress.FirstName, orderDto.ShippingAddress.LastName, orderDto.ShippingAddress.EmailAddress,
              orderDto.ShippingAddress.AddressLine, orderDto.ShippingAddress.Country, orderDto.ShippingAddress.State, orderDto.ShippingAddress.ZipCode);

            var billingAddress = Address.Of(orderDto.BillingAddress.FirstName, orderDto.BillingAddress.LastName, orderDto.BillingAddress.EmailAddress,
                orderDto.BillingAddress.AddressLine, orderDto.BillingAddress.Country, orderDto.BillingAddress.State, orderDto.BillingAddress.ZipCode);

            var updatedPayment = Payment.Of(orderDto.Payment.CardName, orderDto.Payment.CardNumber, orderDto.Payment.Expiration, orderDto.Payment.Cvv, orderDto.Payment.PaymentMethod);

            order.Update(OrderName.Of(orderDto.OrderName), shippingAddress, billingAddress, updatedPayment, orderDto.OrderStatus);
        }
    }
}
