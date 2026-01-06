namespace Shopping.Web.Pages
{
    public class CheckoutModel : PageModel
    {
        [BindProperty]
        public BasketCheckoutModel Order { get; set; } = default!;

        public ShoppingCartModel Cart { get; set; } = default;

        private readonly ILogger<CheckoutModel> _logger;
        private readonly IBasketService _basketService;

        public CheckoutModel(ILogger<CheckoutModel> logger, IBasketService basketService)
        {
            _logger = logger;
            _basketService = basketService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Cart = await _basketService.LoadUserBasket();

            return Page();
        }

        public async Task<IActionResult> OnPostCheckOutAsync()
        {
            _logger.LogInformation("Checkout button clicked");

            Cart = await _basketService.LoadUserBasket();

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Order.CustomerId = new Guid("3407c916-16d6-4330-ac19-0f966bd75362");
            Order.UserName = Cart.UserName;
            Order.TotalPrice = Cart.TotalPrice;

            await _basketService.CheckoutBasket(new CheckoutBasketRequest(Order));

            return RedirectToPage("Confirmation", "OrderSubmitted");
        }
    }
}
