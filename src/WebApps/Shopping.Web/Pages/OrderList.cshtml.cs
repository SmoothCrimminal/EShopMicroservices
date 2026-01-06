namespace Shopping.Web.Pages
{
    public class OrderListModel : PageModel
    {
        public IEnumerable<OrderModel> Orders { get; set; } = [];

        private readonly ILogger<OrderListModel> _logger;
        private readonly IOrderingService _orderingService;

        public OrderListModel(ILogger<OrderListModel> logger, IOrderingService orderingService)
        {
            _logger = logger;
            _orderingService = orderingService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var customerId = new Guid("3407c916-16d6-4330-ac19-0f966bd75362");

            var response = await _orderingService.GetOrdersByCustomer(customerId);
            Orders = response.Orders;

            return Page();
        }
    }
}
