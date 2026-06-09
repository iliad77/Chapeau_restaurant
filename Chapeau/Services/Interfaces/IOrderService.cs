using Chapeau.Models;
using Chapeau.Models.Enums;

namespace Chapeau.Services.Interfaces
{
    public interface IOrderService
    {
        // For Kitchen
        List<Order> GetActiveFoodOrders();
        List<Order> GetFinishedFoodOrdersToday();
        // For Bar
        List<Order> GetActiveDrinkOrders();
        List<Order> GetFinishedDrinkOrdersToday();
        // Methods for managing order status
        void MoveOrderToNextStatus(int orderId);
        void MoveCourseToNextStatus(int orderId, CourseType courseType);
        void MoveItemToNextStatus(int orderItemId);
        //...........................................
        int StartOrder(int staffId, int boothId);
        void AddItem(int orderId, int menuItemId);
    }
}
