using System;
using System.Diagnostics;
using Chapeau.Models;
using Chapeau.Models.Enums;
using Chapeau.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace Chapeau.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly string _connectionString;

        public OrderRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ChapeauDatabase");
        }
        public List<Order> GetActiveFoodOrders()
        {
            List<Order> orders = new List<Order>();
            Order currentOrder = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        o.order_ID, o.staff_ID, o.booth_ID, o.dateTime, o.status AS order_status,
                        oi.order_item_ID, oi.quantity, oi.comment, oi.status AS item_status,
                        mi.item_id, mi.item_name, mi.price, mi.item_description, mi.vat, mi.stock, mi.category
                    FROM [Orders] o
                    INNER JOIN [Order_Item] oi ON o.order_ID = oi.order_ID
                    INNER JOIN [menu_item] mi ON oi.menu_item_ID = mi.item_id
                    WHERE o.status = 0 AND mi.category < 3
                    ORDER BY o.order_ID, o.dateTime DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int orderId = (int)reader["order_ID"];

                            if (currentOrder == null || currentOrder.Id != orderId)
                            {
                                currentOrder = ReadOrder(reader);
                                orders.Add(currentOrder);
                            }

                            currentOrder.OrderItems.Add(ReadOrderItem(reader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error reading active food orders", ex);
                }
            }
            return orders;
        }

        public List<Order> GetFinishedFoodOrdersToday()
        {
            List<Order> orders = new List<Order>();
            Order currentOrder = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT 
                o.order_ID, o.staff_ID, o.booth_ID, o.dateTime, o.status AS order_status,
                oi.order_item_ID, oi.quantity, oi.comment, oi.status AS item_status,
                mi.item_id, mi.item_name, mi.price, mi.item_description, mi.vat, mi.stock, mi.category
            FROM [Orders] o
            INNER JOIN [Order_Item] oi ON o.order_ID = oi.order_ID
            INNER JOIN [menu_item] mi ON oi.menu_item_ID = mi.item_id
            WHERE o.status = 1 AND mi.category < 3 AND CAST(o.dateTime AS DATE) = CAST(GETDATE() AS DATE)
            ORDER BY o.order_ID, o.dateTime DESC"; 

                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int orderId = (int)reader["order_ID"];
                            if (currentOrder == null || currentOrder.Id != orderId)
                            {
                                currentOrder = ReadOrder(reader);
                                orders.Add(currentOrder);
                            }
                            currentOrder.OrderItems.Add(ReadOrderItem(reader));
                        }
                    }
                }
                catch (Exception ex) { throw new Exception("Error reading finished food orders", ex); }
            }
            return orders;
        }

        public List<Order> GetActiveDrinkOrders()
        {
            List<Order> orders = new List<Order>();
            Order currentOrder = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT 
                        o.order_ID, o.staff_ID, o.booth_ID, o.dateTime, o.status AS order_status,
                        oi.order_item_ID, oi.quantity, oi.comment, oi.status AS item_status,
                        mi.item_id, mi.item_name, mi.price, mi.item_description, mi.vat, mi.stock, mi.category
                    FROM [Orders] o
                    INNER JOIN [Order_Item] oi ON o.order_ID = oi.order_ID
                    INNER JOIN [menu_item] mi ON oi.menu_item_ID = mi.item_id
                    WHERE o.status = 0 AND mi.category = 3
                    ORDER BY o.order_ID, o.dateTime DESC";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int orderId = (int)reader["order_ID"];

                            if (currentOrder == null || currentOrder.Id != orderId)
                            {
                                currentOrder = ReadOrder(reader);
                                orders.Add(currentOrder);
                            }

                            currentOrder.OrderItems.Add(ReadOrderItem(reader));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error reading active drink orders", ex);
                }
            }
            return orders;
        }

        public List<Order> GetFinishedDrinkOrdersToday()
        {
            List<Order> orders = new List<Order>();
            Order currentOrder = null;

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"
            SELECT 
                o.order_ID, o.staff_ID, o.booth_ID, o.dateTime, o.status AS order_status,
                oi.order_item_ID, oi.quantity, oi.comment, oi.status AS item_status,
                mi.item_id, mi.item_name, mi.price, mi.item_description, mi.vat, mi.stock, mi.category
            FROM [Orders] o
            INNER JOIN [Order_Item] oi ON o.order_ID = oi.order_ID
            INNER JOIN [menu_item] mi ON oi.menu_item_ID = mi.item_id
            WHERE o.status = 1 AND mi.category = 3 AND CAST(o.dateTime AS DATE) = CAST(GETDATE() AS DATE)
            ORDER BY o.order_ID, o.dateTime DESC";

                SqlCommand cmd = new SqlCommand(query, conn);
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int orderId = (int)reader["order_ID"];
                            if (currentOrder == null || currentOrder.Id != orderId)
                            {
                                currentOrder = ReadOrder(reader);
                                orders.Add(currentOrder);
                            }
                            currentOrder.OrderItems.Add(ReadOrderItem(reader));
                        }
                    }
                }
                catch (Exception ex) { throw new Exception("Error reading finished drink orders", ex); }
            }
            return orders;
        }

        public Order? GetById(int id)
        {
            Order? order = null;
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT order_ID, staff_ID, booth_ID, dateTime, status AS order_status FROM Orders WHERE order_ID = @order_ID";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@order_ID", id);
                try
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) order = ReadOrder(reader);
                        else throw new Exception($"Order with ID {id} not found");
                    }
                }
                catch (Exception ex) { throw new Exception("Unexpected error in GetById", ex); }
            }
            return order;
        }

        public void Update(Order order)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"UPDATE Orders SET staff_ID = @staff_ID, booth_ID = @booth_ID, dateTime = @dateTime, status = @status WHERE order_ID = @order_ID";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@order_ID", order.Id);
                cmd.Parameters.AddWithValue("@staff_ID", order.Staff);
                cmd.Parameters.AddWithValue("@booth_ID", order.Booth);
                cmd.Parameters.AddWithValue("@dateTime", order.OrderTime);
                cmd.Parameters.AddWithValue("@status", (int)order.Status);

                try
                {
                    cmd.Connection.Open();
                    int nrOfRowsAffected = cmd.ExecuteNonQuery();
                    if (nrOfRowsAffected == 0)
                        throw new Exception("No records updated!");
                }
                catch (Exception ex)
                {
                    throw new Exception("Unexpected error in Update", ex);
                }
            }
        }

        private Order ReadOrder(SqlDataReader reader)
        {
            return new Order(
                (int)reader["order_ID"],
                new User { Id = (int)reader["staff_ID"] },
                new Booth { booth_Id = (int)reader["booth_ID"] },
                (DateTime)reader["dateTime"],
                (OrderStatus)(int)reader["order_status"]
            );
        }

        private OrderItem ReadOrderItem(SqlDataReader reader)
        {
            return new OrderItem
            {
                Id = (int)reader["order_item_ID"],
                Quantity = (int)reader["quantity"],
                Status = (ItemStatus)(int)reader["item_status"],
                Comment = (string)reader["comment"].ToString(),
                MenuItem = new MenuItem
                {
                    Course = (CourseType)(int)reader["category"],
                    Name = (string)reader["item_name"],
                    Price = (decimal)reader["price"]
                }
            };
        }

        //...................................
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

        public int Create(int staffId, int boothId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"INSERT INTO Orders (staff_ID, booth_ID, dateTime, status)
                                OUTPUT INSERTED.order_ID
                                VALUES (@staffId, @boothId, @dateTime, @status)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@staffId", staffId);
                    cmd.Parameters.AddWithValue("@boothId", boothId);
                    cmd.Parameters.AddWithValue("@dateTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@status", (int)OrderStatus.Ordered);

                    try
                    {
                        conn.Open();
                        int newId = (int)cmd.ExecuteScalar();
                        return newId;
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Database error while creating order", ex);
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Unexpected error while creating order", ex);
                    }
                }
            }
        }
        public Order GetActiveOrderForTable(Booth table)
        {
            int tableid = table.booth_Id;
            Order activeOrder = null;

            string query = @"
        SELECT order_ID, staff_ID, booth_ID, dateTime, status 
        FROM Orders 
        WHERE booth_ID = @TableId AND status = 1 
        ORDER BY dateTime DESC";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TableId", tableid);

                connection.Open();
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        activeOrder = ReadOrder(reader);
                    }
                }
            }
            return activeOrder;
        }

        public Order GetActiveOrderWithItems(Booth table)
        {
            Order activeOrder = null;

            
            string query = @"
        SELECT 
            o.order_ID, o.staff_ID, o.booth_ID, o.dateTime, o.status,
            oi.quantity, 
            mi.item_id, mi.item_name, mi.price, mi.vat
        FROM Orders o
        JOIN Order_Item oi ON o.order_ID = oi.order_ID
        JOIN menu_item mi ON oi.menu_item_ID = mi.item_id
        WHERE o.booth_ID = @BoothId AND o.status = 1"; 

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BoothId", table.booth_Id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {

                        if (activeOrder == null)
                        {
                            User staff = new User { Id = (int)reader["staff_ID"] };
                            Booth orderBooth = new Booth { booth_Id = (int)reader["booth_ID"] };
                            DateTime dateTime = (DateTime)reader["dateTime"];
                            OrderStatus status = (OrderStatus)(int)reader["status"];

                            activeOrder = new Order((int)reader["order_ID"], staff, orderBooth, dateTime, status);
                        }


                        MenuItem menuItem = new MenuItem
                        {
                            Id = (int)reader["item_id"],
                            Name = (string)reader["item_name"],
                            Price = (decimal)reader["price"],
                            Vat = (int)reader["vat"]
                        };

                        OrderItem item = new OrderItem
                        {
                            Quantity = (int)reader["quantity"],
                            MenuItem = menuItem
                        };


                        activeOrder.OrderItems.Add(item);
                    }
                }
            }

            return activeOrder;
        }
        

    }
}