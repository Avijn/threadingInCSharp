using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Wurklist.General;
using Wurklist.DataBase;
using Wurklist.Models;
using Wurklist.Kanban;

namespace Wurklist.login
{
    public class Login
    {
        private readonly DBCalls _dbcalls;

        public Login()
        {
            _dbcalls = new DBCalls();
        }

        public async Task<int> TryLoginAsync(User user)
        {
            try
            {
                user.Password = EncryptPassword(user.Password);
                int userId = _dbcalls.CheckLogin(user);
                user.SetUserId(userId);


                //test
                List<int> ids = _dbcalls.GetProjectIdsByUserId(userId);
                List<List<KanbanProject>> allProjectsFromUser = new List<List<KanbanProject>>(); 

                foreach(int id in ids)
                {
                    allProjectsFromUser.Add(await _dbcalls.GetProjectsByProjectId(id));
                }
                //



                return userId;
            }
            catch(Exception)
            {
                throw;
            }
        }

        public bool Register(User user)
        {
            user.Password = EncryptPassword(user.Password);
            return _dbcalls.InsertUser(user);
        }

        public string EncryptPassword(string password)
        {
            byte[] data = System.Text.Encoding.ASCII.GetBytes(password);
            data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            return System.Text.Encoding.ASCII.GetString(data);
        }
    }
}
