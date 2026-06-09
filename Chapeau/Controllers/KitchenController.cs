using System.Diagnostics;
using Chapeau.Models;
using Chapeau.Models.Enums;
using Chapeau.Repositories;
using Chapeau.Repositories.Interfaces;
using Chapeau.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau.Controllers
{
    public class KitchenController : Controller
    {
        private readonly IOrderService _orderService;

        public KitchenController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.IsFinished = false;
            ViewBag.IsBar = false;
            var orders = _orderService.GetActiveFoodOrders();
            return View("Index", orders);
        }

        [HttpGet]
        public IActionResult FinishedOrders()
        {
            ViewBag.IsFinished = true;
            ViewBag.IsBar = false;
            var orders = _orderService.GetFinishedFoodOrdersToday();
            return View("Index", orders);
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

        [HttpPost]
        public IActionResult ChangeCourseStatus(int orderId, CourseType courseType)
        {
            _orderService.MoveCourseToNextStatus(orderId, courseType);
            return RedirectToAction("Index");
        }
    }
}
