using Chapeau.Models.Enums;

namespace Chapeau.Models
{
    public class Order
    {
        public int Id { get; set; }
        public User Staff { get; set; } 
        public Booth Booth { get; set; }
        public DateTime OrderTime { get; set; }
        public OrderStatus Status { get; set; } = OrderStatus.Ordered;
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

        public Order(int id, User staff, Booth booth, DateTime orderTime, OrderStatus status)
        {
            Id = id;
            Staff = staff;
            Booth = booth;
            OrderTime = orderTime;
            Status = status;
        }
        public Bill CalculateBillTotals( )
        {
            Bill bill = new Bill();




            decimal foodSubTotal = 0;
            decimal alcoholSubTotal = 0;

            foreach (OrderItem item in OrderItems)
            {
                decimal itemTotal = (decimal)item.MenuItem.Price * item.Quantity;

                if (item.MenuItem.Vat == 9)
                {
                    foodSubTotal += itemTotal;
                }
                else if (item.MenuItem.Vat == 21)
                {
                    alcoholSubTotal += itemTotal;
                }
                else
                {
                    foodSubTotal += itemTotal;
                }
            }

            bill.nonAlcohol_vat = (foodSubTotal * (9m / 109m));
            bill.Alcholic_vat = (alcoholSubTotal * (21m / 121m));
            bill.Totall_price = (foodSubTotal + alcoholSubTotal);

            return bill;
        }

        public string GetWaitingTime()
        {
            TimeSpan diff = DateTime.Now - OrderTime;
            return $"{(int)diff.TotalMinutes} min {diff.Seconds} sec";
        }
    }

}
