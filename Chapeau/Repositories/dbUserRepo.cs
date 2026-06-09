using Chapeau.Models;
using Chapeau.Models.Enums;
using Chapeau.Repositories.Interfaces;
using Chapeau.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;
using System.Security.Cryptography;
using System.Text;


namespace Chapeau.Repositories
{
    public class dbUserRepo : IStaffRepository 
    {
        private readonly string? _connectionString;
        private readonly IStaffService? _staffService;

        public dbUserRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ChapeauDatabase");
            
        }

        private User ReadUser(SqlDataReader reader)
        {
            int id = (int)reader["id"];
            string first_name = (string)reader["first_name"];
            string last_name = (string)reader["last_name"];
            string phone = (string)reader["phone_num"];
            string username = (string)reader["username"];
            string password = (string)reader["password"];
            StaffRole role = Enum.Parse<StaffRole>((string)reader["role"]);
            string email = (string)reader["email"];

            return new User(id, first_name, last_name, email, phone, username, password, role);
        }

        public List<User> GetAllStaff()
        {

            List<User> result = new List<User>();

            try
            { 

                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM staff WHERE status=1";
                    SqlCommand cmd = new SqlCommand(query, connection);

                    cmd.Connection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read()) 
                    {
                        result.Add(ReadUser(reader));

                        
                    }
                    reader.Close();

                }
                return result;
            }
            catch (Exception ex) { Console.WriteLine($"error :{ex.Message}"); }
            return result;
            
        }

        public User? GetbyCredentials(string username, string hashedPassword)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = @"
                    SELECT id, first_name, last_name, email, phone_num, username, password, role, status
                    FROM staff
                    WHERE username = @username AND password = @password;
                ";

                SqlCommand cmd = new SqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@username", username);
                cmd.Parameters.AddWithValue("@password", hashedPassword);

                connection.Open();

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    return ReadUser(reader);
                }

                return null;
            }
        }

        public User GetOneStaff(int id)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "SELECT * FROM staff WHERE id = @id";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@id", id);

                    connection.Open();

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        return ReadUser(reader);
                    }
                }

            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            return new User();
        }

        
            
        

        public void CreateStaff(User user)
        {
            try
            {
                using(SqlConnection connection = new SqlConnection(_connectionString))
                {
                    
                    string query = "INSERT INTO staff" +
                        " (first_name , last_name , phone_num , username, password, role, email)" +
                        "VALUES (@firstName,@lastName,@phoneNum,@username,@password,@role,@email)" +
                        "SELECT SCOPE_IDENTITY();";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@firstName", user.First_name);
                    cmd.Parameters.AddWithValue("@lastName", user.Last_name);
                    cmd.Parameters.AddWithValue("@phoneNum", user.PhoneNum);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@role", user.Role.ToString());
                    cmd.Parameters.AddWithValue("@email", user.Email);

                    cmd.Connection.Open ();
                    user.Id = Convert.ToInt32(cmd.ExecuteScalar());



                }
            }
            catch(Exception ex) { Console.WriteLine(ex.ToString()); }

        }

        public void UpdateStaff(User user)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = "UPDATE staff SET first_name = @firstName , last_name = @lastName" +
                        ", phone_num = @phoneNum , username = @username , password = @password , role = @role" +
                        " WHERE id = @staffID";

                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@staffID", user.Id);
                    cmd.Parameters.AddWithValue("@firstName", user.First_name);
                    cmd.Parameters.AddWithValue("@lastName", user.Last_name);
                    cmd.Parameters.AddWithValue("@phoneNum", user.PhoneNum);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@password", user.Password);
                    cmd.Parameters.AddWithValue("@role", user.Role.ToString());

                    cmd.Connection.Open();
                    int effectedRow = cmd.ExecuteNonQuery();
                    if (effectedRow > 0) { Console.WriteLine("user has been updated"); }

                }
            }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }

        }

        public void DeleteStaff(int id) 
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_connectionString))
                {
                    string query = $"DELETE FROM staff WHERE id = @ID";
                    SqlCommand cmd = new SqlCommand(query, connection);
                    cmd.Parameters.AddWithValue("@ID", id);

                    cmd.Connection.Open();

                    int effectedrow = cmd.ExecuteNonQuery();
                    if (effectedrow > 0) Console.WriteLine("staff deleted");

                }
            }
            catch(Exception ex) { ex.ToString(); }

        }


    }
}
