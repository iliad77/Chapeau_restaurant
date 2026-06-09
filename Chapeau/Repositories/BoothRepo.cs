using Chapeau.Models;
using Chapeau.Models.Enums;
using Chapeau.Repositories.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;

namespace Chapeau.Repositories

{
    public class BoothRepo:IBoothRepo
    {
        private readonly string? _connectionString;
        public BoothRepo(IConfiguration config) 
        {
            _connectionString = config.GetConnectionString("ChapeauDatabase");

        }

        private Booth BoothObjectCreator(SqlDataReader reader)
        {
            Booth booth = new Booth();
            booth.booth_Id = (int)reader["booth_id"];
            booth.booth_num = (int)reader["booth_num"];
            booth.seats = (int)reader["seats"];
            booth.booth_status = Enum.Parse<BoothStatus>((string)reader["booth_status"]);
            
            return booth;
        }

        public List<Booth> GetAllBooth()
        {
            try
            {
                List<Booth> allBooth = new List<Booth>();
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM booth;";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();


                    while (reader.Read())
                    {
                        allBooth.Add(BoothObjectCreator(reader));
                    }
                    cmd.Connection.Close();
                }
                return allBooth;
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"error :{ex.Message}");
                throw;
            }
            
        }

        public Booth GetBooth(int id)
        {
            try
            {
                using(SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM booth WHERE booth_id = @id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        return BoothObjectCreator(reader);
                    }


                }
            }
            catch (Exception ex) { Console.WriteLine($"error :{ex.Message}"); }
            return null;
        }

        public int AddBooth(Booth booth) 
        {
            try
            {
                
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = "INSERT INTO booth (booth_num, seats, booth_status) VALUES (@booth_num, @seats, @booth_status)" +
                        " SELECT SCOPE_IDENTITY();";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@booth_num", booth.booth_num);
                    cmd.Parameters.AddWithValue("@seats", booth.seats);
                    cmd.Parameters.AddWithValue("@booth_status", booth.booth_status.ToString());

                    cmd.Connection.Open();
                    int newId = Convert.ToInt32(cmd.ExecuteScalar());

                    return newId;

                }
            }
            catch (Exception ex) 
            { 
                Console.WriteLine($"error :{ex.Message}");
                return 0;
            }
        }

        public int UpdateBooth(Booth booth)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = @"
                UPDATE booth
                SET booth_num = @booth_num,
                    seats = @seats,
                    booth_status = @booth_status
                WHERE booth_ID = @booth_ID;
            ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@booth_ID", booth.booth_Id);
                    cmd.Parameters.AddWithValue("@booth_num", booth.booth_num);
                    cmd.Parameters.AddWithValue("@seats", booth.seats);
                    cmd.Parameters.AddWithValue("@booth_status", booth.booth_status.ToString());

                    conn.Open();

                    int result = cmd.ExecuteNonQuery();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
                return 0;
            }
        }

        public int DeleteBooth(int id)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(_connectionString))
                {
                    string query = @"
                DELETE FROM booth
                WHERE booth_ID = @id;
            ";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@id", id);

                    conn.Open();

                    int result = cmd.ExecuteNonQuery();

                    return result;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"error: {ex.Message}");
                return 0;
            }
        }

    }
}
