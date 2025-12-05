namespace Ordering.Infrastructure.Data.Extensions
{
    internal class InitialData
    {
        public static IEnumerable<Customer> Customers =>
            new List<Customer>
            {
                Customer.Create(CustomerId.Of(new Guid("b4b51fdb-5a2a-4f96-a88b-c96319dbc668")), "mehmet", "mehmet@example.com"),
                Customer.Create(CustomerId.Of(new Guid("3407c916-16d6-4330-ac19-0f966bd75362")), "john", "john@example.com")
            };

        public static IEnumerable<Product> Products =>
            new List<Product>
            {
                Product.Create(ProductId.Of(new Guid("d9688ba6-15b0-4296-ae23-ef9081e91610")), "Iphone X", 500),
                Product.Create(ProductId.Of(new Guid("110403e0-1c1e-45df-968e-7500c3bce0c6")), "Samsung 10", 400),
                Product.Create(ProductId.Of(new Guid("3fecf932-31ad-46b5-9834-4414548fda84")), "Huawei Plus", 650),
                Product.Create(ProductId.Of(new Guid("032282b4-f955-4ab2-9a9f-5197dffbf6ef")), "Xiaomi Mi", 450)
            };

        public static IEnumerable<Order> OrdersWithItems
        {
            get
            {
                var address1 = Address.Of("mehmet", "ozkaya", "mehmet@example.com", "Bahcelievler No:4", "Turkey", "Istanbul", "38050");
                var address2 = Address.Of("john", "doe", "john@example.com", "Brodway No:1", "England", "Nottingham", "08050");

                var payment1 = Payment.Of("mehmet", "55555555554444", "12/28", "355", 1);
                var payment2 = Payment.Of("john", "8885555554444", "06/30", "222", 1);

                var order1 = Order.Create(
                    OrderId.Of(Guid.NewGuid()),
                    CustomerId.Of(new Guid("b4b51fdb-5a2a-4f96-a88b-c96319dbc668")),
                    OrderName.Of("ORD_1"),
                    address1,
                    address1,
                    payment1);

                order1.Add(ProductId.Of(new Guid("d9688ba6-15b0-4296-ae23-ef9081e91610")), 2, 500);
                order1.Add(ProductId.Of(new Guid("110403e0-1c1e-45df-968e-7500c3bce0c6")), 1, 400);

                var order2 = Order.Create(
                    OrderId.Of(Guid.NewGuid()),
                    CustomerId.Of(new Guid("3407c916-16d6-4330-ac19-0f966bd75362")),
                    OrderName.Of("ORD_2"),
                    address2,
                    address2,
                    payment2);

                order2.Add(ProductId.Of(new Guid("3fecf932-31ad-46b5-9834-4414548fda84")), 1, 650);
                order2.Add(ProductId.Of(new Guid("032282b4-f955-4ab2-9a9f-5197dffbf6ef")), 2, 450);

                return new List<Order> { order1, order2 };
            }
        }
    }
}
