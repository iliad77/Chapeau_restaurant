using Chapeau.Models;
using Chapeau.Models.Enums;
using Chapeau.Repositories.Interfaces;
using Chapeau.Services.Interfaces;

namespace Chapeau.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderItemRepository _orderItemRepository;

        public OrderService(IOrderRepository orderRepository, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
        }

        public List<Order> GetActiveFoodOrders()
        {
            return _orderRepository.GetActiveFoodOrders();
        }

        public List<Order> GetFinishedFoodOrdersToday()
        {
            return _orderRepository.GetFinishedFoodOrdersToday();
        }

        public List<Order> GetActiveDrinkOrders()
        {
            return _orderRepository.GetActiveDrinkOrders();
        }

        public List<Order> GetFinishedDrinkOrdersToday()
        {
            return _orderRepository.GetFinishedDrinkOrdersToday();
        }

        public void MoveItemToNextStatus(int orderItemId)
        {
            OrderItem item = _orderItemRepository.GetById(orderItemId);
            if (item != null)
            {
                item.NextStatus(); 
                _orderItemRepository.Update(item);
            }
        }

        public void MoveOrderToNextStatus(int orderId)
        {
            Order order = _orderRepository.GetById(orderId);

            if (order != null && order.OrderItems != null)
            {
                foreach (var item in order.OrderItems)
                {
                    item.Status = ItemStatus.ReadyToBeServed;
                    _orderItemRepository.Update(item);
                }

                order.Status = OrderStatus.Finished;
                _orderRepository.Update(order);
            }
        }

        public void MoveCourseToNextStatus(int orderId, CourseType courseType)
        {
            Order order = _orderRepository.GetById(orderId);

            if (order != null && order.OrderItems != null)
            {
                var courseItems = order.OrderItems.Where(i => i.MenuItem.Course == courseType);

                foreach (var item in courseItems)
                {
                    item.NextStatus();
                    _orderItemRepository.Update(item);
                }
            }
        }
        //.............................

        public int StartOrder(int staffId, int boothId)
        {
            return _orderRepository.Create(staffId, boothId);
        }

        public void AddItem(int orderId, int menuItemId)
        {
            var items = _orderItemRepository.GetByOrderId(orderId);

            var existing = items.FirstOrDefault(i => i.MenuItem.Id == menuItemId);

            if (existing != null)
            {
                _orderItemRepository.IncreaseQuantity(existing.Id);
            }
            else
            {
                _orderItemRepository.Add(orderId, menuItemId);
            }
        }
    }
}
