using Chapeau.Models;

namespace Chapeau.Repositories.Interfaces
{
    public interface IOrderItemRepository
    {
        OrderItem? GetById(int id);
        void Update(OrderItem item);
        //.......................................
        List<OrderItem> GetByOrderId(int orderId);
        void Add(int orderId, int menuItemId);
        void IncreaseQuantity(int orderItemId);
    }
}
