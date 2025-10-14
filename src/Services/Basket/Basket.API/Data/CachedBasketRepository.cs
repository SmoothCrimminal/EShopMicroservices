using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Basket.API.Data
{
    public class CachedBasketRepository : IBasketRepository
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IDistributedCache _cache;

        public CachedBasketRepository(IBasketRepository basketRepository, IDistributedCache cache)
        {
            _basketRepository = basketRepository;
            _cache = cache;
        }

        public async Task<ShoppingCart> GetBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            var cachedBasket = await _cache.GetStringAsync(userName, cancellationToken);
            if (!string.IsNullOrWhiteSpace(cachedBasket))
                return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket) ?? throw new BasketNotFoundException(userName);

            var basket = await _basketRepository.GetBasketAsync(userName, cancellationToken);
            await _cache.SetStringAsync(userName, JsonSerializer.Serialize(basket), cancellationToken);

            return basket;
        }

        public async Task<ShoppingCart> StoreBasketAsync(ShoppingCart cart, CancellationToken cancellationToken = default)
        {
            await _basketRepository.StoreBasketAsync(cart, cancellationToken);
            await _cache.SetStringAsync(cart.UserName, JsonSerializer.Serialize(cart), cancellationToken);

            return cart;
        }

        public async Task<bool> DeleteBasketAsync(string userName, CancellationToken cancellationToken = default)
        {
            await _basketRepository.DeleteBasketAsync(userName, cancellationToken);
            await _cache.RemoveAsync(userName, cancellationToken);

            return true;
        }
    }
}
