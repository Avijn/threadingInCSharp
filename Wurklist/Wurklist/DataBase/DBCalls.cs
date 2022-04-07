using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using Wurklist.General;
using Wurklist.Models;

namespace Wurklist.DataBase
{
    public class DBCalls
    {
        string connectionString;
        MySqlConnection conn;
        MySqlCommand cmd;

        public DBCalls()
        {
            connectionString = @"server=10.110.110.121;database=Wurklist;user id=arjan;password=YecGaa";
            //connectionString = @"server=127.0.0.1;database=Test;user id = root;";
            conn = new MySqlConnection(connectionString);
        }

        ////
        /// Select statements
        ////

        /*
         * https://stackoverflow.com/questions/11070434/using-prepared-statement-in-c-sharp-with-mysql
         * https://stackoverflow.com/questions/12408693/how-to-read-and-print-out-data-from-mysql-in-c-sharp
         */

        public List<CustomTask> GetCustomTasksUser(int id)
        {
            try
            {
                List<CustomTask> customTasks = new List<CustomTask>();
                string sql = @"SELECT * FROM task WHERE UserId = @id";

                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                MySqlDataReader result = cmd.ExecuteReader();

                while (result.Read())
                {
                    customTasks.Add(new CustomTask(
                        result.GetString("Name"),
                        result.GetString("Description"),
                        result.GetString("Activity"),
                        result.GetString("Deadline"),
                        result.GetInt16("ProjectId"),
                        result.GetInt16("UserId"),
                        result.GetInt16("Priority")
                    ));

                }
                conn.Close();
                return customTasks;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void GetCustomRemindersUser(int id)
        {
            try
            {
                string sql = @"SELECT * FROM Reminder WHERE UserId = @id";
                cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@id", id);

                var reslut = cmd.ExecuteReaderAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public bool CheckLogin(User user)
        {
            try
            {
                string sql = @"SELECT Name, Password FROM User WHERE Name = @Username AND Password = @Password";
                //string sql = @"SELECT name, password FROM user WHERE name = @Username AND password = @Password;";
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", user.Name);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Prepare();

                MySqlDataReader result = cmd.ExecuteReader();
                if(result.HasRows)
                {
                    conn.Close();
                    return true;
                }
                conn.Close();
                return false;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public void GetProjectByUserId(int id)
        {
            try
            {
                string sql = @"SELECT ProjectId FROM Group WHERE UserId = @UserId";
                cmd = new MySqlCommand(sql, conn);
                cmd.Prepare();
                cmd.Parameters.AddWithValue("@UserId", id);

                var result = cmd.ExecuteReaderAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        ////
        /// Insert statements
        ////

        public bool InsertUser(User user)
        {
            try
            {
                string sql = @"INSERT INTO User (`Name`, `Password`, `Email`, `DateOfBirth`) VALUES (@name, @Password, @Email, @Dateofbirth);";
                //string sql = @"INSERT INTO user (`Name`, `Password`, `Email`, `DateOfBirth`) VALUES (@name, @Password, @Email, @DateOfBirth);";

                conn.Open();
                cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@name", user.Name);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Parameters.AddWithValue("@Email", user.Email);
                cmd.Parameters.AddWithValue("@DateOfBirth", user.DateOfBirth);

                cmd.Prepare();
                cmd.ExecuteNonQuery();

                conn.Close();
                return true;
            }
            catch (Exception)
            {
                throw;
            }
              
        }

        ////
        /// Update statements
        ////


        ////
        /// Delete statements
        ////


    }
}
