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

        /// <summary>
        /// Get all Task that are bound to a specific user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> List<CustomTask> </returns>   

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

        /// <summary>
        /// Get all Reminders that are bound to a specific user id
        /// </summary>
        /// <param name="id"></param>
        /// <returns> true </returns>   

        //public void GetCustomRemindersUser(int id)
        //{
        //    try
        //    {
        //        string sql = @"SELECT * FROM Reminder WHERE UserId = @id";
        //        cmd = new MySqlCommand(sql, conn);
        //        cmd.Prepare();
        //        cmd.Parameters.AddWithValue("@id", id);

        //        var reslut = cmd.ExecuteReaderAsync();
        //    }
        //    catch (Exception e)
        //    {
        //        throw e;
        //    }
        //}

        /// <summary>
        /// Checks if the login and password are a valid combination
        /// </summary>
        /// <param name="user"></param>
        /// <returns> userid </returns> 

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
                while (result.Read())
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

        /// <summary>
        /// Gets all the project Id that are bound to a specific userId
        /// </summary>
        /// <param name="id"></param>
        /// <returns> List<int> </returns> 

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

        /// <summary>
        /// Get a specific project based on a projectId
        /// </summary>
        /// <param name="id"></param>
        /// <returns> Task<Kanbanproject> </returns> 

        public async Task<KanbanProject> GetProjectsByProjectId(int id)
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
                return kanbanProjects[0];
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// Get all Tasks that are bound to a specific projectid
        /// </summary>
        /// <param name="id"></param>
        /// <returns> List<TaskItem> </returns> 

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
                            result.GetInt32("ID"),
                            result.GetString("Name"),
                            result.GetString("Description"),
                            result.GetString("Deadline"),
                            result.GetInt32("ProjectId"),
                            result.GetInt32("UserId"),
                            result.GetInt32("LastEditedByUserId"),
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

        /// <summary>
        /// Get all Tasks that are bound to a specific projectid
        /// </summary>
        /// <param name="id"></param>
        /// <returns> List<User> </returns> 

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


        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="user"></param>
        /// <returns> bool </returns>   
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

        /// <summary>
        /// Creates a row that binds a user to a project
        /// </summary>
        /// <param name="project"></param>
        /// <returns> bool </returns>       

        public bool InsertUserInGroup(KanbanProject project)
        {
            try
            {
                string sql = @"INSERT INTO projectgroup (`ProjectId`, `Userid`) VALUES (@projectId, @usserId);";

                conn.Open();
                cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@name", User.GetUserId());
                cmd.Parameters.AddWithValue("@Password", project.ID);

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

        /// <summary>
        /// Create a new Task
        /// </summary>
        /// <param name="item"></param>
        /// <returns> bool </returns> 

        public bool InsertKanbanTask(TaskItem item)
        {
            try
            {
                string sql = @"INSERT INTO Task (Name, Description, Activity, ProjectId, Priority, UserId, Deadline, LastEditedByUserId, ItemCreated) VALUES (@name, @description, @activity, @projectid, @priority, @userid, @deadline, @lasteditedbyuserid, @ItemCreated); ";
                conn.Open();
                cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@name", item.Name);
                cmd.Parameters.AddWithValue("@description", item.Description);
                cmd.Parameters.AddWithValue("@activity", item.Activity.ToString());
                cmd.Parameters.AddWithValue("@deadline", item.Deadline);
                cmd.Parameters.AddWithValue("@projectid", item.ProjectId);
                cmd.Parameters.AddWithValue("@userid", item.UserId);
                cmd.Parameters.AddWithValue("@priority", item.itemPriority.ToString());
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


        /// <summary>
        /// Create a new kanban project
        /// </summary>
        /// <param name="project"></param>
        /// <returns> bool </returns>
        
        public bool InsertProject(KanbanProject project)
        {
            try
            {
                string sql = @"INSERT INTO Project(`Name`, `description`, `CreatedByUserId`, `Created`, `deadline`) VALUES (@name, @description, @createdbyuserid, @created, @deadline);";
                conn.Open();
                cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@name", project.Name);
                cmd.Parameters.AddWithValue("@description", project.Description);
                cmd.Parameters.AddWithValue("@createdbyuserid", project.CreatedByUserId);
                cmd.Parameters.AddWithValue("@created", project.Created);
                cmd.Parameters.AddWithValue("@deadline", project.Deadline);

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

        /// <summary>
        /// Updates the activity/priority of a task
        /// </summary>
        /// <param name="task"></param>
        /// <returns> bool </returns>

        public bool UpdateTask(TaskItem task)
        {
            try
            {
                string sql = @"UPDATE `task` set `Priority` = @priority , `Activity` = @activity , `LastEditedByUserId` = @LastEditedByUserId WHERE `id` = @id";
                conn.Open();
                cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", task.ID);
                cmd.Parameters.AddWithValue("@priority", task.itemPriority.ToString());
                cmd.Parameters.AddWithValue("@activity", task.Activity.ToString());
                cmd.Parameters.AddWithValue("@LastEditedByUserId", task.LastEditedByUserId);

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
        /// Delete statements
        ////


        /// <summary>
        /// Deletes a project
        /// </summary>
        /// <param name="project"></param>
        /// <returns> bool </returns>

        public bool DeleteProject(KanbanProject project)
        {
            try
            {
                string sql = @"DELETE FROM `project` WHERE ID = @id";
                conn.Open();

                cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", project.ID);

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


        /// <summary>
        /// Deletes a Task
        /// </summary>
        /// <param name="task"></param>
        /// <returns> bool </returns>

        public bool DeleteTask(TaskItem task)
        {
            try
            {
                string sql = @"DELETE FROM `task` WHERE ID = @id";
                conn.Open();

                cmd = new MySqlCommand(sql, conn);

                cmd.Parameters.AddWithValue("@id", task.ID);

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

    }
}
