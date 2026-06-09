using System.Data;
using Chapeau.Models;
using Chapeau.Models.Enums;
using Chapeau.Repositories.Interfaces;
using Microsoft.Data.SqlClient;

namespace Chapeau.Repositories
{
    public class MenuItemRepository : IMenuItemRepository
    {
        private readonly string _connectionString;

        public MenuItemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ChapeauDatabase");
        }

        public List<MenuItem> GetAllMenuItems()
        {
            List<MenuItem> menuItems = new();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT mi.item_id, mi.item_name, mi.price, mi.menu_id, mi.item_description, mi.vat, mi.stock,
                                m.menu_name, mi.category
                                FROM menu_item mi
                                JOIN menu m ON mi.menu_id = m.menu_id";

                SqlCommand cmd = new SqlCommand(query, conn);

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            menuItems.Add(ReadMenuItem(reader));
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Something went wrong with the database", ex);
                }
            }

            return menuItems;
        }

        public MenuItem? GetMenuItem(int itemId)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT mi.item_id, mi.item_name, mi.price, mi.menu_id, mi.item_description, mi.vat,
                                mi.stock, m.menu_name, mi.category
                                FROM menu_item mi
                                JOIN menu m ON mi.menu_id = m.menu_id
                                WHERE mi.item_id = @ItemId";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@ItemId", itemId);

                try
                {
                    conn.Open();

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return ReadMenuItem(reader);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Something went wrong with the database", ex);
                }
            }

            return null;
        }

        private MenuItem ReadMenuItem(SqlDataReader reader)
        {
            return new MenuItem
            {
                Id = (int)reader["item_id"],
                Name = reader["item_name"].ToString(),
                Price = (decimal)reader["price"],

                MenuId = new Menu
                {
                    Id = (int)reader["menu_id"],
                    Name = reader["menu_name"].ToString()
                },

                Description = reader["item_description"].ToString(),
                Vat = (decimal)reader["vat"],
                Stock = (int)reader["stock"],
                MenuName = reader["menu_name"].ToString(),

                Course = (CourseType)(int)reader["category"]
            };
        }

        public List<MenuItem> GetMenuItemsByFilter(string? menuName, int? category)
        {
            List<MenuItem> menuItems = new();
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                string query = @"SELECT mi.item_id, mi.item_name, mi.price, mi.menu_id, mi.item_description, mi.vat, mi.stock,
                        m.menu_name, mi.category
                        FROM menu_item mi
                        JOIN menu m ON mi.menu_id = m.menu_id
                        WHERE (@MenuName IS NULL OR m.menu_name = @MenuName)
                        AND (@Category IS NULL OR mi.category = @Category)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.Add("@MenuName", SqlDbType.NVarChar).Value = (object?)menuName ?? DBNull.Value;

                    cmd.Parameters.Add("@Category", SqlDbType.Int).Value = category.HasValue ? (object)category.Value : DBNull.Value;

                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            menuItems.Add(ReadMenuItem(reader));
                        }
                    }
                }
            }
            return menuItems;
        }
    }
}
