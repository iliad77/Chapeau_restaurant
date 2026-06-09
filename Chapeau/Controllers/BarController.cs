using Chapeau.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau.Controllers
{
    public class BarController : Controller
    {
        private readonly IOrderService _orderService;

        public BarController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.IsFinished = false;
            ViewBag.IsBar = true;
            var orders = _orderService.GetActiveDrinkOrders();
            return View(orders);
        }

        [HttpGet]
        public IActionResult FinishedOrders()
        {
            ViewBag.IsFinished = true;
            ViewBag.IsBar = true;
            var orders = _orderService.GetFinishedDrinkOrdersToday();
            return View(orders);
        }

        [HttpPost]
        public IActionResult ChangeItemStatus(int orderItemId)
        {
            _orderService.MoveItemToNextStatus(orderItemId);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult ChangeOrderStatus(int orderId)
        {
            _orderService.MoveOrderToNextStatus(orderId);
            return RedirectToAction("Index");
        }
    }
}
