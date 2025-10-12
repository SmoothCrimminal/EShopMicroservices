namespace Basket.API.Data
{
    public class BasketRepository : IBasketRepository
    {
        private readonly IDocumentSession _documentSession;

        public BasketRepository(IDocumentSession documentSession)
        {
            _documentSession = documentSession;
        }

        public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            var basket = await _documentSession.LoadAsync<ShoppingCart>(userName, cancellationToken);
            return basket ?? throw new BasketNotFoundException(userName);
        }

        public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            _documentSession.Store(cart);
            await _documentSession.SaveChangesAsync(cancellationToken);
            return cart;
        }

        public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            _documentSession.Delete<ShoppingCart>(userName);
            await _documentSession.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
