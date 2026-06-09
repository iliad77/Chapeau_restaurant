using Chapeau.Models.Enums;

namespace Chapeau.Models
{
    public class User
    {
        public int Id { get; set; }
        public string First_name { get; set; }

        public string Last_name { get; set; }

        public string Email { get; set; }

        public string PhoneNum { get; set; }

        public string Username { get; set; }
        public string Password { get; set; }

        public  StaffRole Role{ get; set; }

        public bool Status { get; set; } = true;

        public User() { }

        public User(int id, string first_name, string last_name,string email, string phoneNum, string username, string password, StaffRole role)
        {
            this.Id = id;
            this.First_name = first_name;
            this.Last_name = last_name;
            this.Email = email;
            this.PhoneNum = phoneNum;
            this.Username = username;
            this.Password = password;
            this.Role = role;
        }



    }
}
