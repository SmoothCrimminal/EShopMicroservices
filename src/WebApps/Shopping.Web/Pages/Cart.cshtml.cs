namespace Shopping.Web.Pages
{
    public class CartModel : PageModel
    {
        public ShoppingCartModel Cart { get; set; } = new ShoppingCartModel();

        private readonly ILogger<CartModel> _logger;
        private readonly IBasketService _basketService;

        public CartModel(ILogger<CartModel> logger, IBasketService basketService)
        {
            _logger = logger;
            _basketService = basketService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _basketService.LoadUserBasket();

            return Page();
        }

        public async Task<IActionResult> OnPostRemoveFromCartAsync(Guid productId)
        {
            _logger.LogInformation("Remove from cart button clicked");
            Cart = await _basketService.LoadUserBasket();

            Cart.Items.RemoveAll(x => x.ProductId == productId);

            await _basketService.StoreBasket(new StoreBasketRequest(Cart));

            return RedirectToPage();
        }
    }
}
