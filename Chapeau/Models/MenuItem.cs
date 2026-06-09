using Chapeau.Models.Enums;

namespace Chapeau.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public Menu MenuId { get; set; }
        public string Description { get; set; }
        public decimal Vat { get; set; }
        public int Stock { get; set; }
        public string MenuName { get; set; }
        public CourseType Course { get; set; }

        public MenuItem()
        {
            
        }

        public MenuItem(int id, string name, decimal price, Menu menuId, string description, decimal vat, int stock, string menuName, CourseType course)
        {
            Id = id;
            Name = name;
            Price = price;
            MenuId = menuId;
            Description = description;
            Vat = vat;
            Stock = stock;
            MenuName = menuName;
            Course = course;
        }
    }
}
