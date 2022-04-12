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
        public static int ID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string DateOfBirth { get; set; }

        public User(string name, string password, string email, string dateOfBirth)
        {
            Name = name;
            Password = password;
            Email = email;
            DateOfBirth = dateOfBirth;
        }

        public User(int id, string name, string email, string dateOfBirth)
        {
            ID = id;
            Name = Name;
            Email = email;
            DateOfBirth = dateOfBirth; 
        }

        public User(string name, string password)
        {
            Name = name;
            Password = password;
        }

        public static int GetUserId()
        {
            return ID;
        }

        public void SetUserId(int id)
        {
            ID = id;
        }
    }
}
