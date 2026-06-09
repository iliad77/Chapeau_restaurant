using Chapeau.Models;

namespace Chapeau.ViewModels
{
    public class OrderViewModel
    {
        public int OrderId { get; set; }
        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public List<OrderItem> CurrentOrderItems { get; set; } = new List<OrderItem>();
    }
}
