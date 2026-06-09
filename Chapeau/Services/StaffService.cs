using Chapeau.Models;
using Chapeau.Repositories.Interfaces;
using Chapeau.Service.Interface;
using Chapeau.Services.Interfaces;
using Microsoft.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
namespace Chapeau.Services
{
    public class StaffService: IStaffService
    {
        private IStaffRepository _staffRepo;
        public StaffService(IStaffRepository staffRepository) 
        {
            _staffRepo = staffRepository;
        }
        private readonly string? _connectionString;

        
        public List<User> GetAllStaff()
        {
            return _staffRepo.GetAllStaff();
        }

        private string HashPass(string password)
        {
            using (SHA256 SHA = SHA256.Create())
            {
                byte[] hash = SHA.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hash);
            }
        }

       

        public User? GetbyCredentials(string username, string password)
        {
            string hashedPassword = HashPass(password);
            return _staffRepo.GetbyCredentials(username, hashedPassword);
        }
         

        public User GetOneStaff(int id)
        {
            return _staffRepo.GetOneStaff(id);
        }
        public void CreateStaff(User user)
        {
            user.Password = HashPass(user.Password);
            _staffRepo.CreateStaff(user);
        }
        public void UpdateStaff(User user)
        {
            _staffRepo.UpdateStaff(user);
        }

        public void DeleteStaff(int id)
        {
            _staffRepo.DeleteStaff(id);
        }


    }
}
