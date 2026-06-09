using Chapeau.Models;
using Chapeau.Repositories.Interfaces;

namespace Chapeau.Services
{
    public class MenuItemService : IMenuItemService
    {
        private readonly IMenuItemRepository _menuItemRepository;

        public MenuItemService(IMenuItemRepository menuItemRepository)
        {
            _menuItemRepository = menuItemRepository;
        }

        public List<MenuItem> GetAllMenuItems()
        {
            return _menuItemRepository.GetAllMenuItems();
        }

        public MenuItem? GetMenuItem(int id)
        {
            return _menuItemRepository.GetMenuItem(id);
        }

        public List<MenuItem> GetMenuItemsByFilter(string? menuName, int? category)
        {
            return _menuItemRepository.GetMenuItemsByFilter(menuName, category);
        }
    }
}
