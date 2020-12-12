using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.DTO.Order;
using WebStore.Domain.ViewModels;
using WebStore.Services.Interfaces;

namespace WebStore.Controllers
{
	public class CartController : Controller
    {
        private readonly ICartService _cartService;
		private readonly ILogger<CartController> _logger;

		public CartController(ICartService CartService, ILogger<CartController> logger)
		{
			_cartService = CartService;
			_logger = logger;
		}

		public IActionResult Details()
		{
			return View(new CartOrderViewModel { Cart = _cartService.TransformFromCart() });
		}

		public IActionResult AddToCart(int id)
        {
            _cartService.AddToCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult DecrementFromCart(int id)
        {
            _cartService.DecrementFromCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult RemoveFromCart(int id)
        {
            _cartService.RemoveFromCart(id);
            return RedirectToAction(nameof(Details));
        }

        public IActionResult Clear()
        {
            _cartService.Clear();
            return RedirectToAction(nameof(Details));
        }

        [Authorize]
        public async Task<IActionResult> CheckOut(OrderViewModel OrderModel, [FromServices] IOrderService OrderService)
        {
            using (_logger.BeginScope("Check out order from {0}", OrderModel.Name))
            {
                if (!ModelState.IsValid)
                    return View(nameof(Details), new CartOrderViewModel
                    {
                        Cart = _cartService.TransformFromCart(),
                        Order = OrderModel
                    });
                var orderCreateModel = new CreateOrderModel()
                {
                    Order = OrderModel,
                    Items = _cartService.TransformFromCart().Items.Select(x => new OrderItemDTO
                    {
                        Id = x.Product.Id,
                        Price = x.Product.Price,
                        Quantity = x.Quantity
                    }).ToList()
                };

                var order = await OrderService.CreateOrder(User.Identity.Name, orderCreateModel);
                if (order != null)
				{
                    _logger.LogInformation("Order from {0} registered with id {1}", order.Name,order.Id);
                }

                _cartService.Clear();

                return RedirectToAction(nameof(OrderConfirmed), new { id = order.Id });
            }
        }

        public IActionResult OrderConfirmed(int id)
        {
            ViewBag.OrderId = id;
            return View();
        }
    }
}
