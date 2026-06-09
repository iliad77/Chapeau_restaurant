using Chapeau.Repositories.Interfaces;
using Chapeau.Services;
using Chapeau.Services.Interfaces;
using Chapeau.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau.Controllers
{
    public class OrderController : Controller
    {
        private readonly IMenuItemService _menuItemService;
        private readonly IOrderService _orderService;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderController(IOrderService orderService, IMenuItemService menuItemService, IOrderItemRepository orderItemRepository)
        {
            _orderService = orderService;
            _menuItemService = menuItemService;
            _orderItemRepository = orderItemRepository;
        }

        [HttpPost]
        public IActionResult StartOrder(int staffId, int boothId)
        {
            int orderId = _orderService.StartOrder(staffId, boothId);

            return RedirectToAction("EditOrder", new { id = orderId });
        }

        public IActionResult EditOrder(int id)
        {
            var menuItems = _menuItemService.GetAllMenuItems();
            var currentItems = _orderItemRepository.GetByOrderId(id);

            var viewModel = new OrderViewModel
            {
                OrderId = id,
                MenuItems = menuItems,
                CurrentOrderItems = currentItems
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult AddItem(int orderId, int menuItemId)
        {
            _orderService.AddItem(orderId, menuItemId);

            return RedirectToAction("EditOrder", new { id = orderId });
        }
    }
}