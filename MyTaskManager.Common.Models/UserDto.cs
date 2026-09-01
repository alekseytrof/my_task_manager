using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyTaskManager.Common.Models
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public DateTime LastLoginDate { get; set; }
        public byte[] Photo { get; set; }
        public UserStatus Status { get; set; }

        public UserDto(string fname, string lname, string email, string password,
            UserStatus status, string phone)
        {
            FirstName = fname;
            LastName = lname;
            Email = email;
            Password = password;
            Status = status;
            Phone = phone;
            RegistrationDate = DateTime.Now;
        }

        public UserDto() { }
    }
}
