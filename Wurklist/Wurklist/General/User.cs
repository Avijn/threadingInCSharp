using Windows.System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;

namespace Wurklist.General
{
    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }

        public User(string name, string password, string email, DateTime dateOfBirth)
        {
            Name = name;
            Password = password;
            Email = email;
            DateOfBirth = dateOfBirth;
        }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }
    }
}
