using Microsoft.Data.SqlClient;
using Chapeau.Models;

namespace Chapeau.Repositories.Interfaces
{
    public interface IStaffRepository
    {
        List<User> GetAllStaff();

        User GetOneStaff(int id);
        void CreateStaff(User user);
        void UpdateStaff(User user);

        void DeleteStaff(int id);

        User GetbyCredentials(string username, string password);

    }
}
