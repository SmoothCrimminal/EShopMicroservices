namespace Ordering.Application.Extensions
{
    public static class OrderExtensions
    {
        public static IEnumerable<OrderDto> ToOrderDtoList(this IEnumerable<Order> orders)
        {
            return orders.Select(o => new OrderDto(
                o.Id.Value,
                o.CustomerId.Value,
                o.OrderName.Value,
                new AddressDto(
                    o.ShippingAddress.FirstName,
                    o.ShippingAddress.LastName,
                    o.ShippingAddress.EmailAddress,
                    o.ShippingAddress.AddressLine,
                    o.ShippingAddress.Country,
                    o.ShippingAddress.State,
                    o.ShippingAddress.ZipCode
                    ),
                new AddressDto(
                    o.BillingAddress.FirstName,
                    o.BillingAddress.LastName,
                    o.BillingAddress.EmailAddress,
                    o.BillingAddress.AddressLine,
                    o.BillingAddress.Country,
                    o.BillingAddress.State,
                    o.BillingAddress.ZipCode
                    ),
                new PaymentDto(
                    o.Payment.CardName,
                    o.Payment.CardNumber,
                    o.Payment.Expiration,
                    o.Payment.CVV,
                    o.Payment.PaymentMethod
                ),
                o.Status,
                o.OrderItems.Select(oi => new OrderItemDto(oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price)).ToList()));
        }

        public static OrderDto ToOrderDto(this Order order)
        {
            return new OrderDto(
                order.Id.Value,
                order.CustomerId.Value,
                order.OrderName.Value,
                new AddressDto(
                    order.ShippingAddress.FirstName,
                    order.ShippingAddress.LastName,
                    order.ShippingAddress.EmailAddress,
                    order.ShippingAddress.AddressLine,
                    order.ShippingAddress.Country,
                    order.ShippingAddress.State,
                    order.ShippingAddress.ZipCode
                    ),
                new AddressDto(
                    order.BillingAddress.FirstName,
                    order.BillingAddress.LastName,
                    order.BillingAddress.EmailAddress,
                    order.BillingAddress.AddressLine,
                    order.BillingAddress.Country,
                    order.BillingAddress.State,
                    order.BillingAddress.ZipCode
                    ),
                new PaymentDto(
                    order.Payment.CardName,
                    order.Payment.CardNumber,
                    order.Payment.Expiration,
                    order.Payment.CVV,
                    order.Payment.PaymentMethod
                ),
                order.Status,
                order.OrderItems.Select(oi => new OrderItemDto(oi.OrderId.Value, oi.ProductId.Value, oi.Quantity, oi.Price)).ToList());
        }
    }
}
