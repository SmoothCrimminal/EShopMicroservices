namespace Shopping.Web.Pages
{
    public class ProductListModel : PageModel
    {
        public IEnumerable<string> CategoryList { get; set; } = [];
        public IEnumerable<ProductModel> ProductList { get; set; } = [];

        [BindProperty(SupportsGet = true)]
        public string SelectedCategory { get; set; } = default!;

        private readonly ILogger<ProductListModel> _logger;
        private readonly ICatalogService _catalogService;
        private readonly IBasketService _basketService;

        public ProductListModel(ILogger<ProductListModel> logger, ICatalogService catalogService, IBasketService basketService)
        {
            _logger = logger;
            _catalogService = catalogService;
            _basketService = basketService;
        }

        public async Task<IActionResult> OnGetAsync(string categoryName)
        {
            var response = await _catalogService.GetProducts();

            CategoryList = response.Products.SelectMany(p => p.Category).Distinct();

            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                ProductList = response.Products.Where(p => p.Category.Contains(categoryName));
                SelectedCategory = categoryName;
            }
            else
            {
                ProductList = response.Products;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAddToCartAsync(Guid productId)
        {
            _logger.LogInformation("Att to cart button clicked");

            var productResponse = await _catalogService.GetProduct(productId);
            var basket = await _basketService.LoadUserBasket();

            basket.Items.Add(new ShoppingCartItemModel
            {
                ProductId = productId,
                ProductName = productResponse.Product.Name,
                Price = productResponse.Product.Price,
                Quantity = 1,
                Color = "Black"
            });

            await _basketService.StoreBasket(new StoreBasketRequest(basket));

            return RedirectToPage("Cart");
        }
    }
}
