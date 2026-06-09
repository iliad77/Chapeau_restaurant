using Chapeau.Models.Enums;

namespace Chapeau.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public Order? Order { get; set; }
        public MenuItem MenuItem { get; set; }
        public int Quantity { get; set; }
        public string? Comment { get; set; }
        public ItemStatus Status { get; set; } = ItemStatus.Ordered;
        public OrderItem()
        {
            //...
        }
        public OrderItem(int id, Order? order, MenuItem menuItem, int quantity, string? comment, ItemStatus status)
        {
            Id = id;
            Order = order;
            MenuItem = menuItem;
            Quantity = quantity;
            Comment = comment;
            Status = status;
        }
        public void NextStatus()
        {
            if (Status == ItemStatus.Ordered)
            {
                Status = ItemStatus.BeingPrepared;
            }
            else if (Status == ItemStatus.BeingPrepared)
            {
                Status = ItemStatus.ReadyToBeServed;
            }
        }
    }
}