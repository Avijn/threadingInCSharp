using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Wurklist.General;
using Wurklist.Kanban;
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
            //connectionString = @"server=10.110.110.121;database=Wurklist;user id=arjan;password=YecGaa";
            connectionString = @"server=127.0.0.1;database=Test;user id = root;";
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
                        result.GetInt16("Priority"),
                        result.GetInt16("LastEditedByUserId"),
                        result.GetString("ItemCreated")
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

        public int CheckLogin(User user)
        {
            try
            {
                string sql = @"SELECT Id FROM User WHERE Name = @Username AND Password = @Password";
                //string sql = @"SELECT name, password FROM user WHERE name = @Username AND password = @Password;";
                int userId = 0;
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@Username", user.Name);
                cmd.Parameters.AddWithValue("@Password", user.Password);
                cmd.Prepare();

                MySqlDataReader result = cmd.ExecuteReader();
                while(result.Read())
                { 
                    userId = result.GetInt32("Id");
                }
                conn.Close();
                return userId;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public List<int> GetProjectIdsByUserId(int id)
        {
            try
            {
                List<int> projectIDs = new List<int>();
                conn.Open();
                //string sql = @"SELECT ProjectId FROM group WHERE UserId = @UserId";
                //string sql = @"SELECT ProjectId FROM projectGroup WHERE UserId = @UserId";
                string sql = @"SELECT ProjectId FROM projectgroup WHERE UserId = @UserId";
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@UserId", id);
                cmd.Prepare();

                MySqlDataReader result = cmd.ExecuteReader();

                while (result.Read())
                {
                    if (result.HasRows)
                    {
                        projectIDs.Add(result.GetInt32("ProjectId"));
                    }
                }
                conn.Close();
                return projectIDs;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<KanbanProject>> GetProjectsByProjectId(int id)
        {
            try
            {
                List<KanbanProject> kanbanProjects = new List<KanbanProject>();
                conn.Open();

                string projectsQuery = @"SELECT * FROM Project WHERE ID = @id;";
                cmd = new MySqlCommand(projectsQuery, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Prepare();

                MySqlDataReader result = cmd.ExecuteReader();

                while (await result.ReadAsync())
                {
                    if (result.HasRows)
                    {
                        kanbanProjects.Add(new KanbanProject(
                            result.GetInt32("Id"),
                            result.GetString("Name"),
                            result.GetString("Description"),
                            result.GetInt32("CreatedByUserId"),
                            result.GetString("Created"),
                            result.GetString("Deadline")
                        ));
                    }
                }

                conn.Close();

                return kanbanProjects;
            }
            catch(Exception)
            {
                throw;
            }
        }
    

        public List<TaskItem> GetKanbanItemsByProjectId(int id)
        {
            try
            {
                List<TaskItem> customTasks = new List<TaskItem>();
                string sql = @"SELECT * FROM Task WHERE ProjectId = @ProjectId";
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ProjectId", id);
                cmd.Prepare();

                MySqlDataReader result = cmd.ExecuteReader();

                while (result.Read())
                {
                    if (result.HasRows)
                    {
                        customTasks.Add(new TaskItem(
                            result.GetString("Name"),
                            result.GetString("Description"),
                            result.GetString("Deadline"),
                            result.GetInt16("ProjectId"),
                            result.GetInt16("UserId"),
                            result.GetInt16("LastEditedByUserId"),
                            result.GetString("ItemCreated")
                        ));
                    }
                }
                conn.Close();
                return customTasks;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<User> GetUsersByProjectId(int id)
        {
            try
            {
                List<User> users = new List<User>();
                string sql = @"SELECT ID, Name, Email, DateOfBirth from User u, projectgroup g WHERE u.id = g.userid AND g.ProjectId = @ProjectId";
                conn.Open();
                cmd = new MySqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("@ProjectId", id);
                cmd.Prepare();

                MySqlDataReader result = cmd.ExecuteReader();

                while (result.Read())
                {
                    if (result.HasRows)
                    {
                        users.Add(new User(
                            result.GetInt32("ID"),
                            result.GetString("Name"),
                            result.GetString("Email"),
                            result.GetString("DateOfBirth")
                        ));
                    }
                }
                conn.Close();
                return users;
            }
            catch (Exception)
            {
                throw;
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

        /*
         * @params TaskItem is a task that is made either in the kanbanboard or in the agenda
         */
        public bool InsertKanbanTask(TaskItem item)
        {
            try
            {
                string sql = @"INSERT INTO Task ('Name', 'Description', 'Activity', 'Deadline', 'ProjectId', 'UserId', 'Priority', 'LastEditedByUserId', 'ItemCreated') VALUES (@name, @description, @activity, @deadline, @projectid, @userid, @priority, @lasteditedbyuserid, @ItemCreated,); ";
                conn.Open();
                cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@name", item.Name);
                cmd.Parameters.AddWithValue("@description", item.Description);
                cmd.Parameters.AddWithValue("@activity", item.Activity);
                cmd.Parameters.AddWithValue("@deadline", item.Activity);
                cmd.Parameters.AddWithValue("@projectid", item.ProjectId);
                cmd.Parameters.AddWithValue("@userid", item.ProjectId);
                cmd.Parameters.AddWithValue("@priority", item.Deadline);
                cmd.Parameters.AddWithValue("@lasteditedbyuserid", item.LastEditedByUserId);
                cmd.Parameters.AddWithValue("@ItemCreated", item.Created);

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

        public bool InsertProject(KanbanProject project)
        {
            try
            {
                string sql = @"INSERT INTO Project('Name', 'deadline') VALUES (@name, @deadline);";
                conn.Open();
                cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@name", project.Name);


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
