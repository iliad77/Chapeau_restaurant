using Chapeau.Models.Enums;

namespace Chapeau.ViewModels
{
    public class RestaurantViewModel
    {
        public int OrderId { get; set; }
        public int TableNumber { get; set; }
        public OrderStatus Status { get; set; }
    }
}
