using Chapeau.Models;
using Chapeau.Repositories;
using System.Text;
namespace Chapeau.Service.Interface

{
    public interface IStaffService
    {
        List<User> GetAllStaff();

        User GetbyCredentials(string username, string password);

        User GetOneStaff(int id);
        void CreateStaff(User user);
        void UpdateStaff(User user);

        void DeleteStaff(int id);

        
        
            
        
    }
}
