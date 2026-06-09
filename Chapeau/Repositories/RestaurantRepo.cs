using Chapeau.Models.Enums;
using Chapeau.Repositories.Interfaces;
using Chapeau.ViewModels;
using Microsoft.Data.SqlClient;

namespace Chapeau.Repositories
{
    public class RestaurantRepo:IRestaurantRepo
    {
        
        private readonly string? _connectionString;
        public RestaurantRepo(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("ChapeauDatabase");

        }

        public List<RestaurantViewModel> GetRestaurantOverview(OrderStatus? status)
        {
            List<RestaurantViewModel> orders = new List<RestaurantViewModel>();
            using SqlConnection connection = new SqlConnection(_connectionString);

            string query = @"
        SELECT 
            co.order_ID,
            b.booth_num,
            co.status
        FROM Orders co
        INNER JOIN booth b
            ON co.booth_ID = b.booth_id
        WHERE (@Status IS NULL OR co.status = @Status)
        ORDER BY b.booth_num;
    ";

            SqlCommand command = new SqlCommand(query, connection);
            if (status == null)
            {
                command.Parameters.AddWithValue("@Status", DBNull.Value);
            }
            else
            {
                command.Parameters.AddWithValue("@Status", (int)status.Value);
            }

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                RestaurantViewModel order = new RestaurantViewModel
                {
                    OrderId = (int)reader["order_ID"],
                    TableNumber = (int)reader["booth_num"],
                    Status = (OrderStatus)(int)reader["status"]
                };

                orders.Add(order);
            }

            return orders;
        }
    }
}
