using Chapeau.Services;
using Microsoft.AspNetCore.Mvc;

namespace Chapeau.Controllers
{
    public class MenuController : Controller
    {
        private readonly MenuItemService _menuItemService;

        public MenuController(MenuItemService menuItemService)
        {
            _menuItemService = menuItemService;
        }

        public IActionResult Index(string? menuName, int? category)
        {
            var items = _menuItemService.GetMenuItemsByFilter(menuName, category);

            return View(items);
        }
    }
}
