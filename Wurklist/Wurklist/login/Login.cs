﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Xamarin.Forms;
using Wurklist.General;
using Wurklist.DataBase;
using Wurklist.Models;

namespace Wurklist.login
{
    public class Login
    {
        private readonly DBCalls _dbcalls;

        public Login()
        {
            _dbcalls = new DBCalls();
        }

        public bool TryLogin(User user)
        {
            //byte[] data = System.Text.Encoding.ASCII.GetBytes(user.Password);
            //data = new System.Security.Cryptography.SHA256Managed().ComputeHash(data);
            //String hash = System.Text.Encoding.ASCII.GetString(data);
            user.Password = EncryptPassword(user.Password);
            //_dbcalls.GetCustomTasksUser(1);
            //List<CustomTask> customstasks = _dbcalls.getCustomTasksUser(1);
            if(_dbcalls.CheckLogin(user))
            {
                return true;
            }
            return false;
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
