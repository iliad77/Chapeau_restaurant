using System.Diagnostics;
using Chapeau.Models;

namespace Chapeau.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        public Order GetActiveOrderWithItems(Booth table);

        // For kitchen
        List<Order> GetActiveFoodOrders();
        List<Order> GetFinishedFoodOrdersToday();
        // For Bar
        List<Order> GetActiveDrinkOrders();
        List<Order> GetFinishedDrinkOrdersToday();

        Order? GetById(int id);
        void Update(Order order);
        //..................
        int Create(int staffId, int boothId);
    }
}
