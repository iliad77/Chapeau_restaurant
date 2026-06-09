using Chapeau.Models;

namespace Chapeau.Repositories.Interfaces
{
    public interface IMenuItemRepository
    {
        List<MenuItem> GetAllMenuItems();
        MenuItem? GetMenuItem(int itemId);
        List<MenuItem> GetMenuItemsByFilter(string? menuName, int? category);
    }
}
