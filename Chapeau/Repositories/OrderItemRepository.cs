using Chapeau.Models;
using Chapeau.Models.Enums;
using Chapeau.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace Chapeau.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly string _connectionString;

        public OrderItemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ChapeauDatabase");
        }

        public OrderItem? GetById(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT oi.order_item_ID, oi.quantity, oi.comment, oi.status,
                        mi.item_id, mi.item_name, mi.price, mi.item_description, mi.vat, mi.stock, mi.category
                    FROM Order_Item oi
                    JOIN menu_item mi ON oi.menu_item_ID = mi.item_id
                    WHERE oi.order_item_ID = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return ReadOrderItem(reader);
                    }
                }
            }

            throw new Exception($"Order item with ID {id} truly does not exist in the database.");
        }
        public void Update(OrderItem item)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    UPDATE Order_Item
                    SET status = @status
                    WHERE order_item_ID = @id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@id", item.Id);
                cmd.Parameters.AddWithValue("@status", (int)item.Status);

                conn.Open();

                cmd.ExecuteNonQuery();
            }
        }
        private OrderItem ReadOrderItem(SqlDataReader reader)
        {
            MenuItem menuItem = new MenuItem
            {
                Id = (int)reader["item_id"],
                Name = (string)reader["item_name"],
                Price = (decimal)reader["price"],
                Description = reader["item_description"].ToString(),
                Vat = (decimal)reader["vat"],
                Stock = (int)reader["stock"],
                Course = (CourseType)(int)reader["category"]
            };

            return new OrderItem
            {
                Id = (int)reader["order_item_ID"],
                Quantity = (int)reader["quantity"],
                MenuItem = menuItem,
                Comment = reader["comment"]?.ToString(),
                Status = (ItemStatus)(int)reader["status"]
            };
        }

        //..................................

        public void Add(int orderId, int menuItemId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Order_Item
                                (order_ID, menu_item_ID, quantity)
                                VALUES
                                (@orderId, @menuItemId, 1)";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@orderId", orderId);
                cmd.Parameters.AddWithValue("@menuItemId", menuItemId);

                try
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Database error while adding order item", ex);
                }
            }
        }
        public void IncreaseQuantity(int orderItemId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Order_Item
                                SET quantity = quantity + 1
                                WHERE order_item_ID = @orderItemId";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@orderItemId", orderItemId);

                try
                {
                    conn.Open();

                    int rowsAffected = cmd.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception("Order item not found");
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Database error while updating quantity", ex);
                }
            }
        }
        public List<OrderItem> GetByOrderId(int orderId)
        {
            List<OrderItem> items = new List<OrderItem>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT oi.order_item_ID,
                           oi.quantity,
                           mi.item_id,
                           mi.item_name,
                           mi.price,
                           mi.item_description,
                           mi.vat,
                           mi.stock,
                           mi.category
                    FROM Order_Item oi
                    JOIN menu_item mi
                        ON oi.menu_item_ID = mi.item_id
                    WHERE oi.order_ID = @orderId";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@orderId", orderId);

                try
                {
                    conn.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        OrderItem item = ReadOrderItem(reader);
                        items.Add(item);
                    }

                    reader.Close();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Database error loading order items", ex);
                }
                catch (Exception ex)
                {
                    throw new Exception("Unexpected error loading order items", ex);
                }
            }

            return items;
        }
    }
}