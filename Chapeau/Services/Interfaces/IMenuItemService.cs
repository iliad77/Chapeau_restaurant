using Chapeau.Models;

public interface IMenuItemService
{
    List<MenuItem> GetAllMenuItems();
    MenuItem? GetMenuItem(int id);
    List<MenuItem> GetMenuItemsByFilter(string? menuName, int? category);
}