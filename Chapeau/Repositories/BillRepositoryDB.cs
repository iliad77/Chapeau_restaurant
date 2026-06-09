using Chapeau.Models;
using Chapeau.Models.Enums;
using Chapeau.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace Chapeau.Repositories
{
    public class BillRepositoryDB : IBillRepository
    {
        private readonly string _connectionString;
        public BillRepositoryDB(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ChapeauDatabase");
        }
        public Bill? AddBill(Bill bill)
        {
            Bill finalBill = null;
            {
                string query = @"
                INSERT INTO bill 
                (order_id, date_time, alcoholic_VAT, nonAlcoholic_VAT, tip_amount, total_price, bill_status, comment) 
                VALUES 
                (@OrderId, @DateTime, @AlcoholicVat, @NonAlcoholicVat, @TipAmount, @TotalPrice, @BillStatus, @Comment)";

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    SqlCommand command = new SqlCommand(query, connection);

                    command.Parameters.AddWithValue("@OrderId", finalBill.Order.Id);
                    command.Parameters.AddWithValue("@DateTime", finalBill.Date_Time);
                    command.Parameters.AddWithValue("@AlcoholicVat", finalBill.Alcholic_vat);
                    command.Parameters.AddWithValue("@NonAlcoholicVat", finalBill.nonAlcohol_vat);
                    command.Parameters.AddWithValue("@TipAmount", finalBill.Tip_Amount);
                    command.Parameters.AddWithValue("@TotalPrice", finalBill.Totall_price);
                    command.Parameters.AddWithValue("@BillStatus", finalBill.Status);

                    if (string.IsNullOrEmpty(finalBill.Comment))
                    {
                        command.Parameters.AddWithValue("@Comment", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@Comment", finalBill.Comment);
                    }

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery(); 
                    }
                    catch (SqlException ex)
                    {
                        throw new Exception("Could not save the bill to the database.", ex);
                    }
                }

                return finalBill;
            }
        }

        public void DeleteBill(int id)
        {
            throw new NotImplementedException();
        }

        public Bill? GetBillById(int id)
        {
            throw new NotImplementedException();
        }

        
        public List<Bill> GetBills()
        {

            List<Bill> bills = new List<Bill>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT bill_id, date_time, alcoholic_VAT, nonAlcoholic_VAT, tip_amount,total_price,bill_status,comment,order_id " +
                               "FROM bill";

                SqlCommand command = new SqlCommand(query, connection);
                command.Connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Bill bill = ReadBill(reader);
                    bills.Add(bill);
                }
                reader.Close();
            }
            return bills;

        }

        public void UpdateBill(Bill user)
        {
            throw new NotImplementedException();
        }

        //private string SQLqueries()
        //{
        //    return @"
        //SELECT oi.quantity, mi.item_name, mi.price, mi.vat, oi.menu_item_ID
        //FROM Order_Item oi
        //JOIN menu_item mi ON oi.menu_item_ID = mi.item_id
        //WHERE oi.order_ID = @OrderId";
        //}


        //public List<OrderItem> GetItemsForOrder(int orderId)
        //{
        //    List<OrderItem> items = new List<OrderItem>();
        //    string query = SQLqueries();
        //    using (SqlConnection connection = new SqlConnection(_connectionString))
        //    {
        //        SqlCommand command = new SqlCommand(query, connection);
        //        command.Parameters.AddWithValue("@OrderId", orderId);
        //        connection.Open();

        //        using (SqlDataReader reader = command.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {

        //                items.Add(EachBillitem(reader));
        //            }
        //        }
        //    }
        //    return items;
        //}

        //OrderItem EachBillitem(SqlDataReader reader)
        //{
        //    OrderItem item = new OrderItem
        //    {
        //        Quantity = reader.GetInt32(0),
        //        MenuItem = new MenuItem
        //        {
        //            Name = reader.GetString(1),
        //            Price = (decimal)reader.GetDecimal(2),
        //            Vat = (int)reader.GetDecimal(3),
        //            Id = reader.GetInt32(4)
        //        }
        //    };
        //    return item;
        //}
        public void MarkOrderAsFinished(Order order)
        {
            string query = "UPDATE Orders SET status = @Status WHERE order_ID = @OrderId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Status", (int)OrderStatus.Finished);
                command.Parameters.AddWithValue("@OrderId", order.Id);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                        throw new Exception("No order was found to update.");
                }
                catch (SqlException ex)
                {
                    throw new Exception("Failed to update the order status.", ex);
                }
            }
        }
        public void FreeUpBooth(Booth booth)
        {
            string query = "UPDATE booth SET booth_status = 0 WHERE booth_Id = @BoothId";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@BoothId", booth.booth_Id);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Failed to update the booth status.", ex);
                }
            }
        }

        private Bill ReadBill(SqlDataReader reader)
        {
            int Id = (int)reader["bill_id"];

            DateTime dateTime = (DateTime)reader["date_time"];
            decimal alcholic_vat = (decimal)reader["alcoholic_VAT"];
            decimal nonalcholic_vat = (decimal)reader["nonAlcoholic_VAT"];
            float _Tip_Amount = (float)reader["tip_amount"];
            decimal _totallA_price = (decimal)reader["total_price"];
            string _Status = (string)reader["bill_status"];
            string Comment = (string)reader["comment"];
            Order Order = null;
            if (reader["order_id"] != DBNull.Value)
            {
                Order.Id = (int)reader["order_id"];
            }

            return new Bill(Id, Order, dateTime, alcholic_vat, nonalcholic_vat, _Tip_Amount, _totallA_price, _Status, Comment);
        }

    }
}
